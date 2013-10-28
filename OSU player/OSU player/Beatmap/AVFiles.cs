using Microsoft.DirectX.AudioVideoPlayback;
using System;
using System.Threading;
using Un4seen.Bass;

namespace OSU_player
{
    /// <summary>
    /// Class for Audio Playback
    /// </summary>
    public class Audiofiles : IDisposable
    {
        private int channel = 0;
        private string Path = "";
        private SYNCPROC _sync = null;
        private int Interval = 1;
        private BASSTimer Timer = new BASSTimer();
        private bool isPaused = false;
        public Audiofiles()
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
        public bool isplaying
        {
            get { return Bass.BASS_ChannelIsActive(channel) == BASSActive.BASS_ACTIVE_PLAYING; }
        }
        public void Play(int offset,float volume = 1.0f)
        {
            Timer.Stop();
            Thread.Sleep(offset);
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
        public void Stop()
        {
            Timer.Stop();
            Bass.BASS_ChannelStop(channel);
        }
        public void Dispose()
        {
            Bass.BASS_StreamFree(channel);
            channel = 0;
        }
        public void Seek(double time)
        {
            Bass.BASS_ChannelSetPosition(channel, Bass.BASS_ChannelSeconds2Bytes(channel, time));
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
        public void Open(string path)
        {
            Path = path;
            Timer = new BASSTimer(Interval);
            _sync = new SYNCPROC(ReachedEnd);
            Bass.BASS_StreamFree(channel);
            BASSFlag flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN;
            channel = Bass.BASS_StreamCreateFile(Path, 0, 0, flag);
            if (channel == 0)
            {
                throw (new FormatException(Bass.BASS_ErrorGetCode().ToString()));
            }
        }
        private void ReachedEnd(int handle, int channel, int data, IntPtr user)
        {
            Stop();
        }
        public BASSTimer UpdateTimer
        {
            get { return Timer; }
            set { UpdateTimer = value; }
        }
    }
    /// <summary>
    /// Class for Audio Playback
    /// </summary>
    public class Videofiles
    {
        private Video videofile = new Video(Core.defaultBG);
        public double durnation { get { return videofile.Duration; } }
        public double position { get { return videofile.CurrentPosition; } }
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
        public void Play(System.Windows.Forms.Panel panel,int offset)
        {
            Thread.Sleep(offset);
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

}
