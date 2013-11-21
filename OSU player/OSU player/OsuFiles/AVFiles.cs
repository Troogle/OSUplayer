using System;
using Un4seen.Bass;
namespace OSUplayer.OsuFiles
{
    /// <summary>
    /// Class for Audio Playback
    /// </summary>
    public class Audiofiles : IDisposable
    {
        private int channel = 0;
        private string path = "";
        private int interval = 1;
        private BASSTimer timer = new BASSTimer();
        private bool isopened = false;
        private bool isPaused = false;
        public Audiofiles()
        {
        }
        public double Durnation
        {
            get
            {
                return Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));
            }
        }
        public double Position
        {
            get
            {
                return Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetPosition(channel));
            }
        }
        public bool Isplaying
        {
            get { return Bass.BASS_ChannelIsActive(channel) == BASSActive.BASS_ACTIVE_PLAYING; }
        }
        public void Play(float volume)
        {
            timer.Stop();
            if (isopened)
            {
                if (channel != 0 && Bass.BASS_ChannelPlay(channel, true))
                {
                    timer.Start();
                    isPaused = false;
                }
                else
                {
                    throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
                }
                Volume = volume;
            }
        }
        public void Pause()
        {
            if (isPaused)
            {
                timer.Start();
                Bass.BASS_ChannelPlay(channel, false);
            }
            else
            {
                timer.Stop();
                Bass.BASS_ChannelPause(channel);
            }
            isPaused = !isPaused;
        }
        public void Stop()
        {
            timer.Stop();
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
            timer.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            isopened = false;
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
            path = path;
            timer = new BASSTimer(interval);
            Bass.BASS_StreamFree(channel);
            BASSFlag flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN;
            channel = Bass.BASS_StreamCreateFile(path, 0, 0, flag);
            isopened = true;
            if (channel == 0)
            {
                //throw (new FormatException(Bass.BASS_ErrorGetCode().ToString()));
                isopened = false;
            }
        }
        public BASSTimer UpdateTimer
        {
            get { return timer; }
            set { UpdateTimer = value; }
        }
    }
}
