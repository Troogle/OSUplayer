// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using Microsoft.DirectX.AudioVideoPlayback;
using Un4seen.Bass;

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
        private Audio audiofile;
        public double durnation
        {
            get
            {
                return audiofile.Duration;
            }
        }
        public double position
        {
            get
            {
                return audiofile.CurrentPosition;
            }
        }
        public void init(string path)
        {
            try
            {
                audiofile.Dispose();
            }
            catch (Exception)
            {
            }
            audiofile = new Audio(path, true);
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
        public void seek(double time)
        {
            audiofile.SeekCurrentPosition(time * 10000000, SeekPositionFlags.AbsolutePositioning);
        }
    }
    public class Videofiles
    {
        private Video videofile = new Video(Core.defaultBG);
        public double durnation
        {
            get
            {
                return videofile.Duration;
            }
        }
        public double position
        {
            get
            {
                return videofile.CurrentPosition;
            }
        }
        public bool isplaying { get { return videofile.Playing; } }
        public void init(string path)
        {
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
        public void Dispose()
        {
            try
            {
                videofile.Stop();
                videofile.Owner = null;
                videofile.Dispose();
            }
            catch (Exception)
            {

            }
        }
    }
}
