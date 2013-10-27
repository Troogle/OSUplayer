using Microsoft.DirectX.AudioVideoPlayback;
using System;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace OSU_player
{
    /* public class Audiofiles
     {
         public string path;
         public Audiofiles(string filename)
         {
             Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
             path = filename;
         }

         public void Play(int volume)
         {

         }
         private int _stream = 0;
         private string _fileName = string.Empty;
         private SYNCPROC _sync = null;
         private int _updateInterval = 1;
         // 1ms
         private Un4seen.Bass.BASSTimer _updateTimer = null;
         private bool _isPaused = false;
         public void Audiofiles00(string fileName)
         {
             _fileName = fileName;
             _updateTimer = new Un4seen.Bass.BASSTimer(_updateInterval);
             _sync = new SYNCPROC(EndPosition);

             Bass.BASS_StreamFree(_stream);
             if (_fileName != string.Empty)
             {
                 BASSFlag flag = (BASSFlag)(BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                 _stream = Bass.BASS_StreamCreateFile(_fileName, 0, 0, flag);
             }
             if (_stream == 0)
             {

             }
         }

         public void Play(float volume__1 = 1.0F)
         {
             _updateTimer.Stop();
             if (_stream != 0 && Bass.BASS_ChannelPlay(_stream, true))
             {
                 _updateTimer.Start();
             }
             else
             {
                 throw (new Exception("Internal Error! " + Bass.BASS_ErrorGetCode()));
             }
             Volume = volume__1;
         }

         public void SeekSamplePosition(double position)
         {
             if (IsPlaying())
             {
                 Bass.BASS_ChannelSetPosition(_stream, Bass.BASS_ChannelSeconds2Bytes(_stream, position));
             }
         }

         public void PauseOrResume()
         {
             if (_isPaused)
             {
                 _updateTimer.Start();
                 Bass.BASS_ChannelPlay(_stream, false);
             }
             else
             {
                 _updateTimer.Stop();
                 Bass.BASS_ChannelPause(_stream);
             }
             _isPaused = !_isPaused;
         }

         public bool IsPlaying()
         {
             return Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING;
         }

         public void Stop(bool release)
         {
             _updateTimer.Stop();

             if (release)
             {
                 Bass.BASS_StreamFree(_stream);
                 _stream = 0;
             }
             else
             {
                 Bass.BASS_ChannelStop(_stream);
             }
         }

         private void EndPosition(int handle, int channel, int data, IntPtr user)
         {
             Bass.BASS_ChannelStop(channel);
         }

         public long ChannelGetLength
         {
             get
             {
                 return Bass.BASS_ChannelGetLength(_stream);
             }
         }

         public long ChannelGetPosition
         {
             get
             {
                 return Bass.BASS_ChannelGetPosition(_stream);
             }
         }

         public double Bytes2Second(long pos)
         {
             return Bass.BASS_ChannelBytes2Seconds(_stream, pos);
         }

         public void Dispose()
         {
             Release();
         }

         public static void Release()
         {
             Bass.BASS_Stop();
             // close bass
             Bass.BASS_Free();
         }

         public float Volume
         {
             get
             {
                 float vol = 1.0F;
                 Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
                 return vol;
             }
             set
             {
                 Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, value);
             }
         }

         public bool IsPaused
         {
             get
             {
                 return _isPaused;
             }
         }
         public Un4seen.Bass.BASSTimer UpdateTimer
         {
             get
             {
                 return _updateTimer;
             }
             set
             {
                 _updateTimer = value;
             }
         }
     }
     */
    public class Audiofiles
    {
        private Audio audiofile = new Audio(Core.defaultAudio);
        public double durnation { get { return audiofile.Duration; } }
        public double position { get { return audiofile.CurrentPosition; } }
        public bool isstopped { get { return audiofile.Stopped; } }
        public bool isplaying { get { return audiofile.Playing; } }
        public void init(string path)
        {
            audiofile.Open(path);
        }
        public void Play()
        {
            audiofile.Play();
        }
        public void Pause()
        {
            if (audiofile.Paused)
            {
                audiofile.Play();
            }
            else
            {
                audiofile.Pause();
            }
        }
        public void Stop()
        {
            audiofile.Stop();
        }
        public void seek(double time)
        {
            audiofile.SeekCurrentPosition(time * 10000000, SeekPositionFlags.AbsolutePositioning);
        }
        public void volume(int value)
        {
            audiofile.Volume = value;
        }
    }
    public class Videofiles
    {
        private Video videofile = new Video(Core.defaultBG);
        public double durnation { get { return videofile.Duration; } }
        public double position { get { return videofile.CurrentPosition; } }
        public bool isstopped { get { return videofile.Stopped; } }
        public void init(string path)
        {
            videofile.Open(path);
        }
        public void initbg(string path)
        {
            videofile.Owner = null;
            videofile.Stop();
            videofile.Dispose();
            videofile = new Video(path);
        }
        public void Play(System.Windows.Forms.Panel panel)
        {
            int height = 360;
            int width = 480;
            videofile.Owner = panel;
            panel.Width = width;
            panel.Height = height;
            videofile.Size = panel.Size;
            videofile.Play();
        }
        public void Pause()
        {
            try
            {
                if (videofile.Paused)
                {
                    videofile.Play();
                }
                else
                {
                    videofile.Pause();
                }
            }
            catch (Exception)
            {

            }


        }
        public void seek(double time)
        {
            videofile.SeekCurrentPosition(time * 10000000, SeekPositionFlags.AbsolutePositioning);
        }
        public void Stop()
        {
            videofile.Owner = null;
            videofile.Stop();
            videofile.Dispose();

        }
    }
    public class SoundEngine : IDisposable
    {
        private int channel = 0;
        private string Path = "";
        private SYNCPROC _sync = null;
        private int Interval = 1;
        private BASSTimer Timer = null;
        private bool isPaused = false;
        public SoundEngine()
        {
        }
        public double durnation
        {
            get
            {
                return Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));
            }
        }
        public double position
        {
            get
            {
                return Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetPosition(channel));
            }
        }
        //isstopped
        public bool isplaying { get { return !isPaused; } }
        //init
        public void Play(float volume = 1.0f)
        {
            Timer.Stop();
            if (channel != 0 && Bass.BASS_ChannelPlay(channel, true))
            {
                Timer.Start();
            }
            else
            {
                throw new Exception("Internal Error! " + Bass.BASS_ErrorGetCode());
            }
            Volume = volume;
        }
        public void Pause()
        {
            if (isPaused)
            {
                Timer.Start();
                Bass.BASS_ChannelPlay(channel, false);
            }
            else
            {
                Timer.Stop();
                Bass.BASS_ChannelPause(channel);
            }
            isPaused = !isPaused;
        }
        public void Stop(bool release)
        {
            Timer.Stop();

            if (release)
            {
                Bass.BASS_StreamFree(channel);
                channel = 0;
            }
            else
            {
                Bass.BASS_ChannelStop(channel);
            }
        }
        public void Seek(double time)
        {
            // if (IsPlaying())
            //  {
            Bass.BASS_ChannelSetPosition(channel, Bass.BASS_ChannelSeconds2Bytes(channel, time));
            //  }
        }
        public float Volume
        {
            get
            {
                float vol = 1.0f;
                Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
                return vol;
            }
            set
            {
                Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, value);
            }
        }

        public SoundEngine(string path)
        {
            Path = path;
            Timer = new BASSTimer(Interval);
            _sync = new SYNCPROC(EndPosition);

            Bass.BASS_StreamFree(channel);
            if (Path != String.Empty)
            {
                BASSFlag flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN;
                channel = Bass.BASS_StreamCreateFile(Path, 0, 0, flag);
            }
            if (channel == 0)
            {
                throw (new FormatException("Zero Ptr found while loading sample"));
            }
        }

        public bool IsPlaying()
        {
            return Bass.BASS_ChannelIsActive(channel) == BASSActive.BASS_ACTIVE_PLAYING;
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }

        private void EndPosition(int handle, int channel, int data, IntPtr user)
        {
            Bass.BASS_ChannelStop(channel);
        }

        public void Dispose()
        {
            Bass.BASS_Stop();  // close bass
            Bass.BASS_Free();
        }


        public BASSTimer UpdateTimer
        {
            get { return Timer; }
            set { UpdateTimer = value; }
        }
    }
}
