namespace OSU_player.Graphic
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    public class VideoDecoder : IDisposable
    {
        private IntPtr buffer;
        public int BufferSize;
        private FFmpeg.AVCodecContext codecCtx;
        private double currentDisplayTime;
        private Thread decodingThread;
        private FFmpeg.AVFormatContext formatContext;
        private byte[][] FrameBuffer;
        private double[] FrameBufferTimes;
        public double FrameDelay;
        private int frameFinished;
        private bool isDisposed;
        private double lastPts;
        private IntPtr packet;
        private IntPtr pCodec;
        private IntPtr pCodecCtx;
        private IntPtr pFormatCtx;
        private IntPtr pFrame;
        private IntPtr pFrameRGB;
        private const int PIXEL_FORMAT = 6;
        private int readCursor;
        private FFmpeg.AVStream stream;
        private GCHandle streamHandle;
        private bool videoOpened;
        private int videoStream;
        private int writeCursor;
        public VideoDecoder(int bufferSize)
        {
            this.BufferSize = bufferSize;
            this.FrameBufferTimes = new double[this.BufferSize];
            this.FrameBuffer = new byte[this.BufferSize][];
            FFmpeg.av_register_all();
            this.videoOpened = false;
        }
        private void Decode()
        {
            try
            {
                while (true)
                {
                    bool flag = false;
                    lock (this)
                    {
                        while (((this.writeCursor - this.readCursor) < this.BufferSize) && (FFmpeg.av_read_frame(this.pFormatCtx, this.packet) >= 0))
                        {
                            if (Marshal.ReadInt32(this.packet, 0x18) == this.videoStream)
                            {
                                double num = Marshal.ReadInt64(this.packet, 8);
                                IntPtr buf = Marshal.ReadIntPtr(this.packet, 0x10);
                                int num2 = Marshal.ReadInt32(this.packet, 20);
                                FFmpeg.avcodec_decode_video(this.pCodecCtx, this.pFrame, ref this.frameFinished, buf, num2);
                                if (((this.frameFinished != 0) && (Marshal.ReadIntPtr(this.packet, 0x10) != IntPtr.Zero)) && (FFmpeg.img_convert(this.pFrameRGB, 6, this.pFrame, (int)this.codecCtx.pix_fmt, this.codecCtx.width, this.codecCtx.height) == 0))
                                {
                                    Marshal.Copy(Marshal.ReadIntPtr(this.pFrameRGB), this.FrameBuffer[this.writeCursor % this.BufferSize], 0, this.FrameBuffer[this.writeCursor % this.BufferSize].Length);
                                    this.FrameBufferTimes[this.writeCursor % this.BufferSize] = ((num - this.stream.start_time) * this.FrameDelay) * 1000.0;
                                    this.writeCursor++;
                                    this.lastPts = num;
                                    flag = true;
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        Thread.Sleep(15);
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                using (StreamWriter writer = File.CreateText("video-debug.txt"))
                {
                    writer.WriteLine(exception.ToString());
                }
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        public void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                if (this.decodingThread != null)
                {
                    this.decodingThread.Abort();
                }
                try
                {
                    Marshal.FreeHGlobal(this.packet);
                    Marshal.FreeHGlobal(this.buffer);
                }
                catch { }
                try
                {
                    FFmpeg.av_free(this.pFrameRGB);
                    FFmpeg.av_free(this.pFrame);
                }
                catch { }
                try
                {
                    this.streamHandle.Free();
                }
                catch { }
                this.frameFinished = 0;
                try
                {
                      FFmpeg.avcodec_close(this.pCodecCtx);
                      FFmpeg.av_close_input_file(this.pFormatCtx);
                }
                catch { }
            }
        }
        ~VideoDecoder()
        {
            this.Dispose(false);
        }
        public byte[] GetFrame(int time)
        {
            while ((this.readCursor < (this.writeCursor - 1)) && (this.FrameBufferTimes[(this.readCursor + 1) % this.BufferSize] <= time))
            {
                this.readCursor++;
            }
            if (this.readCursor < this.writeCursor)
            {
                this.currentDisplayTime = this.FrameBufferTimes[this.readCursor % this.BufferSize];
                return this.FrameBuffer[this.readCursor % this.BufferSize];
            }
            return null;
        }
        public bool Open(string path)
        {
            FileStream inStream = File.OpenRead(path);
            this.OpenStream(inStream);
            return true;
        }
        public bool Open(byte[] bytes)
        {
            if ((bytes == null) || (bytes.Length == 0))
            {
                return false;
            }
            this.streamHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            IntPtr ptr = this.streamHandle.AddrOfPinnedObject();
            if (this.videoOpened)
            {
                return false;
            }
            this.videoOpened = true;
            string filename = string.Concat(new object[] { "memory:", ptr, "|", bytes.Length });
            if (FFmpeg.av_open_input_file(out this.pFormatCtx, filename, IntPtr.Zero, bytes.Length, IntPtr.Zero) != 0)
            {
                throw new Exception("Couldn't open input file");
            }
            if (FFmpeg.av_find_stream_info(this.pFormatCtx) < 0)
            {
                throw new Exception("Couldn't find stream info");
            }
            FFmpeg.dump_format(this.pFormatCtx, 0, filename, 0);
            this.formatContext = (FFmpeg.AVFormatContext)Marshal.PtrToStructure(this.pFormatCtx, typeof(FFmpeg.AVFormatContext));
            this.videoStream = -1;
            int num2 = this.formatContext.nb_streams;
            for (int i = 0; i < num2; i++)
            {
                FFmpeg.AVStream stream = (FFmpeg.AVStream)Marshal.PtrToStructure(this.formatContext.streams[i], typeof(FFmpeg.AVStream));
                FFmpeg.AVCodecContext context = (FFmpeg.AVCodecContext)Marshal.PtrToStructure(stream.codec, typeof(FFmpeg.AVCodecContext));
                if (context.codec_type == FFmpeg.CodecType.CODEC_TYPE_VIDEO)
                {
                    this.videoStream = i;
                    this.stream = stream;
                    this.codecCtx = context;
                    this.pCodecCtx = this.stream.codec;
                    break;
                }
            }
            if (this.videoStream == -1)
            {
                throw new Exception("couldn't find video stream");
            }
            this.FrameDelay = FFmpeg.av_q2d(this.stream.time_base);
            this.pCodec = FFmpeg.avcodec_find_decoder(this.codecCtx.codec_id);
            if (this.pCodec == IntPtr.Zero)
            {
                throw new Exception("couldn't find decoder");
            }
            if (FFmpeg.avcodec_open(this.pCodecCtx, this.pCodec) < 0)
            {
                throw new Exception("couldn't open codec");
            }
            this.pFrame = FFmpeg.avcodec_alloc_frame();
            this.pFrameRGB = FFmpeg.avcodec_alloc_frame();
            if (this.pFrameRGB == IntPtr.Zero)
            {
                throw new Exception("couldn't allocate RGB frame");
            }
            int cb = FFmpeg.avpicture_get_size(6, this.codecCtx.width, this.codecCtx.height);
            this.buffer = Marshal.AllocHGlobal(cb);
            FFmpeg.avpicture_fill(this.pFrameRGB, this.buffer, 6, this.codecCtx.width, this.codecCtx.height);
            this.packet = Marshal.AllocHGlobal(0x39);
            for (int j = 0; j < this.BufferSize; j++)
            {
                this.FrameBuffer[j] = new byte[(this.width * this.height) * 4];
            }
            this.decodingThread = new Thread(new ThreadStart(this.Decode));
            this.decodingThread.IsBackground = true;
            this.decodingThread.Start();
            return true;
        }
        public bool OpenStream(Stream inStream)
        {
            byte[] buffer = new byte[inStream.Length];
            inStream.Read(buffer, 0, (int)inStream.Length);
            return this.Open(buffer);
        }
        public void Seek(int time)
        {
            lock (this)
            {
                int flags = 0;
                double num2 = ((((double)time) / 1000.0) / this.FrameDelay) + this.stream.start_time;
                if (num2 < this.lastPts)
                {
                    flags = 1;
                }
                FFmpeg.av_seek_frame(this.pFormatCtx, this.videoStream, (long)num2, flags);
                this.readCursor = 0;
                this.writeCursor = 0;
            }
        }
        public double CurrentTime
        {
            get
            {
                return this.currentDisplayTime;
            }
        }
        public int height
        {
            get
            {
                return this.codecCtx.height;
            }
        }
        public double Length
        {
            get
            {
                long duration = this.stream.duration;
                if (duration < 0L)
                {
                    return 36000000.0;
                }
                return (duration * this.FrameDelay);
            }
        }
        private double startTimeMs
        {
            get
            {
                return ((0x3e8L * this.stream.start_time) * this.FrameDelay);
            }
        }
        public int width
        {
            get
            {
                return this.codecCtx.width;
            }
        }
        public delegate void EndOfFileHandler();
    }
}
