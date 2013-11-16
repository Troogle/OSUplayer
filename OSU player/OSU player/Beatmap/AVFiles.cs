using System;
using System.Threading;
using Un4seen.Bass;
using System.Windows.Forms;
namespace OSU_player
{
    /// <summary>
    /// Class for Audio Playback
    /// </summary>
    public class Audiofiles : IDisposable
    {
        private int channel = 0;
        private string Path = "";
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
        public void Play(float volume)
        {
            Timer.Stop();
            if (channel != 0 && Bass.BASS_ChannelPlay(channel, true))
            {
                Timer.Start();
                isPaused = false;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
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
            isPaused = true;
            Bass.BASS_ChannelStop(channel);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Bass.BASS_Stop();
                Bass.BASS_Free();
            }
            Timer.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
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
            Bass.BASS_StreamFree(channel);
            BASSFlag flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN;
            channel = Bass.BASS_StreamCreateFile(Path, 0, 0, flag);
            if (channel == 0)
            {
                throw (new FormatException(Bass.BASS_ErrorGetCode().ToString()));
            }
        }
        public BASSTimer UpdateTimer
        {
            get { return Timer; }
            set { UpdateTimer = value; }
        }
    }
}
