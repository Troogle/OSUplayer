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
        private SYNCPROC _sync = null;
        private int Interval = 1;
        private BASSTimer Timer = new BASSTimer();
        private bool isPaused = false;
        private bool autorelease;
        public Audiofiles(bool Autorelease = false)
        {autorelease=Autorelease;
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
        public void Play(int offset, float volume = 1.0f )
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
            BASSFlag flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN|BASSFlag.BASS_STREAM_AUTOFREE;
            channel = Bass.BASS_StreamCreateFile(Path, 0, 0, flag);
            Bass.BASS_ChannelSetSync(0, BASSSync.BASS_SYNC_END, 0, _sync, IntPtr.Zero);
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
    /// Class for Video Playback
    /// </summary>
    public class Videofiles : IDisposable
    {
        private VlcPlayer vlc_player_;
        private bool is_playinig_;
        string pluginPath = Application.StartupPath + "\\vlc\\plugins\\";
        public double durnation { get { return vlc_player_.Duration(); } }
        public double position { get { return vlc_player_.GetPlayTime(); } }
        public Videofiles(Panel panel)
        {
            vlc_player_ = new VlcPlayer(pluginPath);
            IntPtr render_wnd = panel.Handle;
            vlc_player_.SetRenderWindow((int)render_wnd);
            is_playinig_ = false;
        }
        public void Play(string FileName,int offset)
        {
           // Thread.Sleep(offset);
            vlc_player_.PlayFile(FileName);
            is_playinig_ = true;

        }
        public void Pause()
        {
            vlc_player_.Pause();
        }
        public void seek(double time)
        {
            vlc_player_.SetPlayTime(time);
        }
        public void Stop()
        {
            vlc_player_.Stop();
            is_playinig_ = false;
        }
        public void Dispose()
        {

        }
    }
}
