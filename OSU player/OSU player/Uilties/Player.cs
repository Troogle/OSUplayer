using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OSUplayer.Graphic;
using OSUplayer.OsuFiles;
using System.Diagnostics;
using Device = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using Color = Microsoft.Xna.Framework.Graphics.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace OSUplayer.Graphic
{
    class Player : IDisposable
    {
        public Audiofiles uniAudio;
        private struct Fxlist
        {
            public int time;
            public List<int> play;
            public float volume;
            public Fxlist(int time, List<int> play, float volume)
            {
                this.time = time;
                this.play = play;
                this.volume = volume;
            }
        }
        List<Fxlist> fxlist = new List<Fxlist>();
        const int maxfxplayer = 128;
        Audiofiles[] fxplayer = new Audiofiles[maxfxplayer];
        string[] fxname = new string[maxfxplayer];
        bool cannext = true;
        public bool willnext = false;
        bool videoexist = false;
        bool SBexist = false;
        int fxpos = 0;
        float Allvolume { get { return Core.Allvolume; } }
        float Musicvolume { get { return Core.Musicvolume; } }
        float Fxvolume { get { return Core.Fxvolume; } }
        bool Playvideo { get { return Core.playvideo; } }
        bool Playfx { get { return Core.playfx; } }
        bool Playsb { get { return Core.playsb; } }
        Beatmap Map { get { return Core.CurrentBeatmap; } }
        BeatmapSet Set { get { return Core.CurrentSet; } }
        /// <summary>
        /// 渲染区域大小
        /// </summary>
        Rectangle sizeRect;
        Rectangle showRect;
        /// <summary>
        /// 渲染区域的handle
        /// </summary>
        IntPtr handle;
        GraphicsDevice device = null;
        VideoDecoder decoder;
        Texture2D VideoTexture;
        Texture2D BGTexture;
        Texture2D Black;
        PresentationParameters presentParams = new PresentationParameters();
        Vector2 VideoPosition;
        float VideoScale;
        Vector2 BGPosition;
        float BGScale;
        List<TGraphic> SBelements = new List<TGraphic>();
        SpriteBatch AlphaSprite;
        SpriteBatch AdditiveSprite;
        bool deviceislost = false;
        public Player(IntPtr Shandle, Size Ssize)
        {
            uniAudio = new Audiofiles();
            handle = Shandle;
            sizeRect = new Rectangle(0, 0, 640, 480);
            showRect = new Rectangle(0, 0, Ssize.Width, Ssize.Height);
            /* presentParams.DeviceWindowHandle=handle;
             presentParams.IsFullScreen=false;
             device = new GraphicsDevice();
             device.Reset(presentParams, GraphicsAdapter.DefaultAdapter);*/
            presentParams.IsFullScreen = false;
            presentParams.SwapEffect = SwapEffect.Copy;
            presentParams.BackBufferHeight = sizeRect.Height;
            presentParams.BackBufferWidth = sizeRect.Width;
            device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
                DeviceType.Hardware, handle, CreateOptions.MixedVertexProcessing, presentParams);
            AlphaSprite = new SpriteBatch(device);
            AdditiveSprite = new SpriteBatch(device);
            for (int i = 0; i < maxfxplayer; i++)
            {
                fxplayer[i] = new Audiofiles();
            }
            using (MemoryStream s = new MemoryStream())
            {
                OSUplayer.Properties.Resources.defaultBG.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                BGTexture = Texture2D.FromFile(device, s);
            }
        }
        public void resize(Size size)
        {
            showRect = new Rectangle(0, 0, size.Width, size.Height);
        }
        bool CanRender()
        {
            if (deviceislost == false) { return true; }
            switch (device.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    return false;
                case GraphicsDeviceStatus.Normal:
                    deviceislost = false;
                    return true;
                case GraphicsDeviceStatus.NotReset:
                    device.Reset(device.PresentationParameters);
                    deviceislost = false;
                    return true;
                default:
                    return false;
            }
        }
        public void initvideo()
        {
            decoder = new VideoDecoder(10);
            decoder.Open(Path.Combine(Map.Location, Map.Video));
            Bitmap black = new Bitmap(Properties.Resources.BlackBase, decoder.width, decoder.height);
            using (MemoryStream s = new MemoryStream())
            {
                black.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                Black = Texture2D.FromFile(device, s);
            }
            VideoTexture = new Texture2D(device, decoder.width, decoder.height, 1, 0, SurfaceFormat.Bgr32);
            VideoScale = (float)sizeRect.Width / decoder.width < (float)sizeRect.Height / decoder.height ? (float)sizeRect.Width / decoder.width : (float)sizeRect.Height / decoder.height;
            VideoPosition = new Vector2((sizeRect.Width - decoder.width * VideoScale) / 2, (sizeRect.Height - decoder.height * VideoScale) / 2);
            videoexist = true;
        }
        public void initBG()
        {
            if (Map.Background != "" && !File.Exists(Map.Background))
            {
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "没事删什么BG TAT", System.Windows.Forms.ToolTipIcon.Info);
                Map.Background = "";
            }
            if (Map.Background == "")
            {
                using (MemoryStream s = new MemoryStream())
                {
                    OSUplayer.Properties.Resources.defaultBG.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                    s.Seek(0, SeekOrigin.Begin);
                    BGTexture = Texture2D.FromFile(device, s);
                }
            }
            else
            {
                using (FileStream s = new FileStream(Map.Background, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    BGTexture = Texture2D.FromFile(device, s);
                }
            }
            BGScale = (float)sizeRect.Width / BGTexture.Width < (float)sizeRect.Height / BGTexture.Height ? (float)sizeRect.Width / BGTexture.Width : (float)sizeRect.Height / BGTexture.Height;
            BGPosition = new Vector2((sizeRect.Width - BGTexture.Width * BGScale) / 2, (sizeRect.Height - BGTexture.Height * BGScale) / 2);
        }
        public void RenderSB()
        {
            foreach (var element in SBelements)
            {
                element.Update((int)(position * 1000));
                if ((element.parameter & 4) == 4)
                { element.Draw(AdditiveSprite); }
                else
                { element.Draw(AlphaSprite); }
            }
        }
        public void initSB()
        {
            Map.Getsb();
            for (int i = 0; i < Map.SB.Elements.Count; i++)
            {
                if (Map.SB.Elements[i].Type != OsuFiles.StoryBoard.ElementType.Sample)
                {
                    TGraphic element = new TGraphic(device, Map.SB.Elements[i], Map.Location, i);
                    element.SetAlphaAction(new TSpriteAction(Map.SB.Elements[i].F));
                    element.SetScaleXAction(new TSpriteAction(Map.SB.Elements[i].SX));
                    element.SetScaleYAction(new TSpriteAction(Map.SB.Elements[i].SY));
                    element.SetRotateAction(new TSpriteAction(Map.SB.Elements[i].R));
                    element.SetColorAction(new TSpriteAction(Map.SB.Elements[i].C));
                    element.SetXAction(new TSpriteAction(Map.SB.Elements[i].X));
                    element.SetYAction(new TSpriteAction(Map.SB.Elements[i].Y));
                    element.SetParameterAction(new TSpriteAction(Map.SB.Elements[i].P, false, false));
                    SBelements.Add(element);
                }
            }
            SBexist = true;
        }
        public void RenderVideo(SpriteBatch sprite)
        {
            if (position - (double)Map.VideoOffset / 1000 < 0) { return; }
            VideoTexture.SetData<byte>(decoder.GetFrame(Convert.ToInt32(position * 1000 - Map.VideoOffset)));
            sprite.Draw(Black, sizeRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
            sprite.Draw(VideoTexture, VideoPosition, null, Color.White, 0f, Vector2.Zero, VideoScale, SpriteEffects.None, 0.9f);
        }
        public void RenderBG(SpriteBatch sprite)
        {
            sprite.Draw(BGTexture, BGPosition, null, Color.White, 0f, Vector2.Zero, BGScale, SpriteEffects.None, 1f);
        }
        public void Render()
        {
            if (device == null || device.IsDisposed || !CanRender()) { return; }
            device.Clear(Color.Black);
            AlphaSprite.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            RenderBG(AlphaSprite);
            if (videoexist) { RenderVideo(AlphaSprite); }
            AdditiveSprite.Begin(SpriteBlendMode.Additive, SpriteSortMode.BackToFront, SaveStateMode.None);
            if (SBexist) { RenderSB(); }
            AdditiveSprite.End();
            AlphaSprite.End();
            try
            {
                device.Present(sizeRect, showRect, handle);
            }
            catch { deviceislost = true; }

        }
        public void Dispose()
        {
            device.Dispose();
            uniAudio.Dispose();
            for (int j = 0; j < maxfxplayer; j++)
            {
                fxplayer[j].Dispose();
            }
        }
        private int Fxlistcompare(Fxlist a, Fxlist b)
        {
            if (a.time > b.time)
            {
                return 1;
            }
            else if (a.time == b.time) { return 0; }
            else return -1;
        }
        private List<int> setplayer(ref int player, List<string> filenames)
        {
            List<int> ret = new List<int>();
            bool f = true;
            foreach (string filename in filenames)
            {
                f = true;
                for (int i = 0; i < player; i++)
                {
                    if (fxname[i] == filename) { ret.Add(i); f = false; break; }
                }
                if (f)
                {
                    fxname[player] = filename;
                    fxplayer[player].Open(filename);
                    ret.Add(player);
                    player++;
                }
            }
            return (ret);
        }
        private void initfx()
        {
            fxlist.Clear();
            for (int i = 0; i < maxfxplayer; i++)
            {
                fxname[i] = "";
            }
            int currentT = 0;
            int current = 0;
            CSample nowdefault = Map.Timingpoints[currentT].sample;
            CSample olddefault = new CSample(0, 0);
            double bpm = Map.Timingpoints[currentT].bpm;
            double Tbpm = bpm;
            float volume = Map.Timingpoints[currentT].volume;
            int player = 0;
            for (int i = 0; i < uniAudio.Durnation * 1000; i++)
            {
                if (currentT + 1 < Map.Timingpoints.Count)
                {
                    if (Map.Timingpoints[currentT + 1].offset <= i)
                    {
                        currentT++;
                        nowdefault = Map.Timingpoints[currentT].sample;
                        volume = Map.Timingpoints[currentT].volume;
                        if (Map.Timingpoints[currentT].type == 1)
                        {
                            bpm = Map.Timingpoints[currentT].bpm;
                            Tbpm = Map.Timingpoints[currentT].bpm;
                        }
                        else
                        {
                            bpm = Tbpm * Map.Timingpoints[currentT].bpm;
                        }
                    }
                }
                if (current == Map.HitObjects.Count) { break; }
                if (Map.HitObjects[current].starttime > i) { continue; }
                HitObject tmpH = Map.HitObjects[current];
                CSample tmpSample = nowdefault;
                float volumeH = volume;
                switch (tmpH.type)
                {
                    case ObjectFlag.Normal:
                    case ObjectFlag.NormalNewCombo:
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.starttime, setplayer(ref player, Set.getsamplename
                                (tmpSample, tmpH.allhitsound)), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(
                           new Fxlist(tmpH.starttime, setplayer(ref player, Set.getsamplename
                               (tmpH.A_sample, tmpH.allhitsound)), volumeH));
                        }
                        break;
                    case ObjectFlag.Slider:
                    case ObjectFlag.SliderNewCombo:
                        //TODO:每个节点的sampleset
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        double deltatime = (600.0 * tmpH.length / bpm / Map.SliderMultiplier);
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume / 100; }
                        for (int j = 0; j <= tmpH.repeatcount; j++)
                        {
                            fxlist.Add(
                                new Fxlist((int)(tmpH.starttime + deltatime * j),
                                    setplayer(ref player, Set.getsamplename
                            (tmpSample, tmpH.Hitsounds[j])), volumeH));
                        }
                        break;
                    case ObjectFlag.Spinner:
                    case ObjectFlag.SpinnerNewCombo:
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.EndTime, setplayer(ref player, Set.getsamplename(tmpSample, tmpH.allhitsound)), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(new Fxlist(tmpH.EndTime, setplayer(ref player, Set.getsamplename
                                (tmpH.A_sample, tmpH.allhitsound)), volumeH));
                        }
                        break;
                    default:
                        break;
                }
                current++;
            }
            fxlist.Sort(Fxlistcompare);
            fxlist.Add(new Fxlist(Int32.MaxValue, new List<int>(), 0));
        }
        private void PlayFx(int pos)
        {
            if (!Playfx) { return; }
            for (int j = 0; j < fxlist[pos].play.Count; j++)
            {
                fxplayer[fxlist[pos].play[j]].Play(Allvolume * Fxvolume * fxlist[pos].volume);
            }
        }
        public void SetVolume(int set, float volume)
        {
            switch (set)
            {
                case 1:
                    if (uniAudio != null) { uniAudio.Volume = Allvolume * Musicvolume; }
                    break;
                case 2:
                    if (uniAudio != null) { uniAudio.Volume = Allvolume * Musicvolume; }
                    break;
                case 3:
                default:
                    break;
            }
        }
        public void Stop()
        {
            cannext = false;
            if (videoexist) { videoexist = false; VideoTexture.Dispose(); decoder.Dispose(); }
            if (SBexist) { SBexist = false; SBelements.Clear(); }
            uniAudio.Stop();
        }
        public void Play()
        {
            willnext = false; cannext = true; videoexist = false;
            uniAudio.Open(Map.Audio);
            if (Playfx) { initfx(); fxpos = 0; }
            uniAudio.UpdateTimer.Tick += new EventHandler(AVsync);
            if (Map.haveVideo && Playvideo && File.Exists(Path.Combine(Map.Location, Map.Video))) { initvideo(); }
            if (Map.haveSB && Playsb) { initSB(); }
            uniAudio.Play(Allvolume * Musicvolume);
        }
        public void Pause()
        {
            cannext = false;
            uniAudio.Pause();
        }
        public void Resume()
        {
            uniAudio.Pause();
            cannext = true;
        }
        public double durnation
        { get { return uniAudio.Durnation; } }
        public double position
        { get { return uniAudio.Position; } }
        public bool isplaying
        { get { return uniAudio.Isplaying; } }
        public void seek(double time)
        {
            cannext = false;
            uniAudio.Seek(time);
            fxpos = 0;
            if (Playfx)
            {
                while (fxlist[fxpos].time <= uniAudio.Position * 1000 && fxpos < fxlist.Count)
                {
                    fxpos++;
                }
            }
            cannext = true;
        }
        private void AVsync(object sender, EventArgs e)
        {
            if (!uniAudio.Isplaying && cannext)
            {
                willnext = true;
                return;
            }
            if (fxpos < fxlist.Count)
            {
                while (fxlist[fxpos].time <= uniAudio.Position * 1000)
                {
                    while ((fxlist[fxpos + 1].time <= uniAudio.Position * 1000)) { fxpos++; }
                    PlayFx(fxpos);
                    fxpos++;
                }
            }
        }
    }
}
