using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
// ReSharper disable All
namespace OSUplayer.Graphic
{
    public class VideoDecoder : IDisposable
    {
        public delegate void EndOfFileHandler();

        private const int PIXEL_FORMAT = 6;
        private readonly byte[][] FrameBuffer;
        private readonly double[] FrameBufferTimes;

        public int BufferSize;
        public double FrameDelay;
        private IntPtr buffer;
        private FFmpeg.AVCodecContext codecCtx;
        private double currentDisplayTime;
        private Thread decodingThread;
        private FFmpeg.AVFormatContext formatContext;
        private int frameFinished;
        private bool isDisposed;
        private double lastPts;
        private IntPtr pCodec;
        private IntPtr pCodecCtx;
        private IntPtr pFormatCtx;
        private IntPtr pFrame;
        private IntPtr pFrameRGB;
        private IntPtr packet;
        private int readCursor;
        private FFmpeg.AVStream stream;
        private GCHandle streamHandle;
        private bool videoOpened;
        private int videoStream;
        private int writeCursor;

        public VideoDecoder(int bufferSize)
        {
            BufferSize = bufferSize;
            FrameBufferTimes = new double[BufferSize];
            FrameBuffer = new byte[BufferSize][];
            FFmpeg.av_register_all();
            videoOpened = false;
        }

        public double CurrentTime
        {
            get { return currentDisplayTime; }
        }

        public int height
        {
            get { return codecCtx.height; }
        }

        public double Length
        {
            get
            {
                long duration = stream.duration;
                if (duration < 0L)
                {
                    return 36000000.0;
                }
                return (duration*FrameDelay);
            }
        }

        private double startTimeMs
        {
            get { return ((0x3e8L*stream.start_time)*FrameDelay); }
        }

        public int width
        {
            get { return codecCtx.width; }
        }

        public void Dispose()
        {
            Dispose(true);
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
                        while (((writeCursor - readCursor) < BufferSize) &&
                               (FFmpeg.av_read_frame(pFormatCtx, packet) >= 0))
                        {
                            if (Marshal.ReadInt32(packet, 0x18) == videoStream)
                            {
                                double num = Marshal.ReadInt64(packet, 8);
                                IntPtr buf = Marshal.ReadIntPtr(packet, 0x10);
                                int num2 = Marshal.ReadInt32(packet, 20);
                                FFmpeg.avcodec_decode_video(pCodecCtx, pFrame, ref frameFinished, buf, num2);
                                if (((frameFinished != 0) && (Marshal.ReadIntPtr(packet, 0x10) != IntPtr.Zero)) &&
                                    (FFmpeg.img_convert(pFrameRGB, 6, pFrame, (int) codecCtx.pix_fmt, codecCtx.width,
                                        codecCtx.height) == 0))
                                {
                                    Marshal.Copy(Marshal.ReadIntPtr(pFrameRGB), FrameBuffer[writeCursor%BufferSize], 0,
                                        FrameBuffer[writeCursor%BufferSize].Length);
                                    FrameBufferTimes[writeCursor%BufferSize] = ((num - stream.start_time)*FrameDelay)*
                                                                               1000.0;
                                    writeCursor++;
                                    lastPts = num;
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

        public void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                if (decodingThread != null)
                {
                    decodingThread.Abort();
                }
                try
                {
                    Marshal.FreeHGlobal(packet);
                    Marshal.FreeHGlobal(buffer);
                }
                catch
                {
                }
                try
                {
                    FFmpeg.av_free(pFrameRGB);
                    FFmpeg.av_free(pFrame);
                }
                catch
                {
                }
                try
                {
                    streamHandle.Free();
                }
                catch
                {
                }
                frameFinished = 0;
                try
                {
                    FFmpeg.avcodec_close(pCodecCtx);
                    FFmpeg.av_close_input_file(pFormatCtx);
                }
                catch
                {
                }
            }
        }

        ~VideoDecoder()
        {
            Dispose(false);
        }

        public byte[] GetFrame(int time)
        {
            while ((readCursor < (writeCursor - 1)) && (FrameBufferTimes[(readCursor + 1)%BufferSize] <= time))
            {
                readCursor++;
            }
            if (readCursor < writeCursor)
            {
                currentDisplayTime = FrameBufferTimes[readCursor%BufferSize];
                return FrameBuffer[readCursor%BufferSize];
            }
            return null;
        }

        public bool Open(string path)
        {
            FileStream inStream = File.OpenRead(path);
            OpenStream(inStream);
            return true;
        }

        public bool Open(byte[] bytes)
        {
            if ((bytes == null) || (bytes.Length == 0))
            {
                return false;
            }
            streamHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            IntPtr ptr = streamHandle.AddrOfPinnedObject();
            if (videoOpened)
            {
                return false;
            }
            videoOpened = true;
            string filename = string.Concat(new object[] {"memory:", ptr, "|", bytes.Length});
            if (FFmpeg.av_open_input_file(out pFormatCtx, filename, IntPtr.Zero, bytes.Length, IntPtr.Zero) != 0)
            {
                throw new Exception("Couldn't open input file");
            }
            if (FFmpeg.av_find_stream_info(pFormatCtx) < 0)
            {
                throw new Exception("Couldn't find stream info");
            }
            FFmpeg.dump_format(pFormatCtx, 0, filename, 0);
            formatContext = (FFmpeg.AVFormatContext) Marshal.PtrToStructure(pFormatCtx, typeof (FFmpeg.AVFormatContext));
            videoStream = -1;
            int num2 = formatContext.nb_streams;
            for (int i = 0; i < num2; i++)
            {
                var _stream =
                    (FFmpeg.AVStream) Marshal.PtrToStructure(formatContext.streams[i], typeof (FFmpeg.AVStream));
                var context =
                    (FFmpeg.AVCodecContext) Marshal.PtrToStructure(_stream.codec, typeof (FFmpeg.AVCodecContext));
                if (context.codec_type == FFmpeg.CodecType.CODEC_TYPE_VIDEO)
                {
                    videoStream = i;
                    this.stream = _stream;
                    codecCtx = context;
                    pCodecCtx = this.stream.codec;
                    break;
                }
            }
            if (videoStream == -1)
            {
                throw new Exception("couldn't find video stream");
            }
            FrameDelay = FFmpeg.av_q2d(stream.time_base);
            pCodec = FFmpeg.avcodec_find_decoder(codecCtx.codec_id);
            if (pCodec == IntPtr.Zero)
            {
                throw new Exception("couldn't find decoder");
            }
            if (FFmpeg.avcodec_open(pCodecCtx, pCodec) < 0)
            {
                throw new Exception("couldn't open codec");
            }
            pFrame = FFmpeg.avcodec_alloc_frame();
            pFrameRGB = FFmpeg.avcodec_alloc_frame();
            if (pFrameRGB == IntPtr.Zero)
            {
                throw new Exception("couldn't allocate RGB frame");
            }
            int cb = FFmpeg.avpicture_get_size(6, codecCtx.width, codecCtx.height);
            buffer = Marshal.AllocHGlobal(cb);
            FFmpeg.avpicture_fill(pFrameRGB, buffer, 6, codecCtx.width, codecCtx.height);
            packet = Marshal.AllocHGlobal(0x39);
            for (int j = 0; j < BufferSize; j++)
            {
                FrameBuffer[j] = new byte[(width*height)*4];
            }
            decodingThread = new Thread(Decode);
            decodingThread.IsBackground = true;
            decodingThread.Start();
            return true;
        }

        public bool OpenStream(Stream inStream)
        {
            var buffer = new byte[inStream.Length];
            inStream.Read(buffer, 0, (int) inStream.Length);
            return Open(buffer);
        }

        public void Seek(int time)
        {
            lock (this)
            {
                int flags = 0;
                double num2 = ((time/1000.0)/FrameDelay) + stream.start_time;
                if (num2 < lastPts)
                {
                    flags = 1;
                }
                FFmpeg.av_seek_frame(pFormatCtx, videoStream, (long) num2, flags);
                readCursor = 0;
                writeCursor = 0;
            }
        }
    }
}