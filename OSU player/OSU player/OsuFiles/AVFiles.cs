using System;
using Un4seen.Bass;

namespace OSUplayer.OsuFiles
{
    public interface IAudiofile
    {
        double Durnation { get; }
        double Position { get; }
        bool Isplaying { get; }
        float Volume { get; set; }
        void Play(float volume);
        void Pause();
        void Stop();
        void Seek(double time);
        void Open(string path);
    }

    /// <summary>
    ///     Class for Audio Playback
    /// </summary>
    public class BassAudio : IDisposable, IAudiofile
    {
        private const int Interval = 1;
        private int _channel;
        private bool _isPaused;
        private bool _isopened;
        private BASSTimer _timer = new BASSTimer();

        public static void init()
        {
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
        }
        public BASSTimer UpdateTimer
        {
            get { return _timer; }
            //set { UpdateTimer = value; }
        }

        public double Durnation
        {
            get { return Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetLength(_channel)); }
        }

        public double Position
        {
            get { return Bass.BASS_ChannelBytes2Seconds(_channel, Bass.BASS_ChannelGetPosition(_channel)); }
        }

        public bool Isplaying
        {
            get { return Bass.BASS_ChannelIsActive(_channel) == BASSActive.BASS_ACTIVE_PLAYING; }
        }

        public void Play(float volume)
        {
            _timer.Stop();
            if (!_isopened) return;
            if (_channel != 0 && Bass.BASS_ChannelPlay(_channel, true))
            {
                _timer.Start();
                _isPaused = false;
            }
            else
            {
                throw new FormatException(Bass.BASS_ErrorGetCode().ToString());
            }
            Volume = volume;
        }

        public void Pause()
        {
            if (_isPaused)
            {
                _timer.Start();
                Bass.BASS_ChannelPlay(_channel, false);
            }
            else
            {
                _timer.Stop();
                Bass.BASS_ChannelPause(_channel);
            }
            _isPaused = !_isPaused;
        }

        public void Stop()
        {
            _timer.Stop();
            _isPaused = true;
            Bass.BASS_ChannelStop(_channel);
        }

        public void Seek(double time)
        {
            Bass.BASS_ChannelSetPosition(_channel, Bass.BASS_ChannelSeconds2Bytes(_channel, time));
        }

        public float Volume
        {
            get
            {
                float vol = 1.0f;
                Bass.BASS_ChannelGetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, ref vol);
                return vol;
            }
            set { Bass.BASS_ChannelSetAttribute(_channel, BASSAttribute.BASS_ATTRIB_VOL, value); }
        }

        public void Open(string path)
        {
            _timer = new BASSTimer(Interval);
            Bass.BASS_StreamFree(_channel);
            _channel = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
            _isopened = true;
            if (_channel == 0)
            {
                //throw (new FormatException(Bass.BASS_ErrorGetCode().ToString()));
                _isopened = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            _isopened = false;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Bass.BASS_Stop();
                Bass.BASS_Free();
            }
            _timer.Dispose();
        }
    }
}