using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OSUplayer.Graphic;
using OSUplayer.OSUFiles;
using System.Diagnostics;
using Device = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using Color = Microsoft.Xna.Framework.Graphics.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace OSUplayer.Graphic
{
    class Player : IDisposable
    {
        public Audiofiles uni_Audio;
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
        bool playvideo { get { return Core.playvideo; } }
        bool playfx { get { return Core.playfx; } }
        bool playsb { get { return Core.playsb; } }
        Beatmap Map { get { return Core.CurrentBeatmap; } }
        BeatmapSet Set { get { return Core.CurrentSet; } }
        /// <summary>
        /// 渲染区域大小
        /// </summary>
        Size size;
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
        Rectangle sizerect;
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
            uni_Audio = new Audiofiles();
            handle = Shandle;
            size = Ssize;
            sizerect = new Rectangle(0, 0, size.Width, size.Height);
            /* presentParams.DeviceWindowHandle=handle;
             presentParams.IsFullScreen=false;
             device = new GraphicsDevice();
             device.Reset(presentParams, GraphicsAdapter.DefaultAdapter);*/
            presentParams.IsFullScreen = false;
            presentParams.SwapEffect = SwapEffect.Discard;
            device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
                DeviceType.Hardware, handle, CreateOptions.MixedVertexProcessing, presentParams);
            AlphaSprite = new SpriteBatch(device);
            AdditiveSprite = new SpriteBatch(device);
            for (int i = 0; i < maxfxplayer; i++)
            {
                fxplayer[i] = new Audiofiles();
            }
            /*        Bitmap black = new Bitmap(Properties.Resources.BlackBase, size);
                    using (MemoryStream s = new MemoryStream())
                    {
                        black.Save(s,System.Drawing.Imaging.ImageFormat.Png);
                        s.Seek(0, SeekOrigin.Begin);
                        Black = Texture2D.FromFile(device, s);
                    }*/
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
            //decoder.Dispose();
            decoder = new VideoDecoder(10);
            decoder.Open(Path.Combine(Map.Location, Map.Video));
            Bitmap black = new Bitmap(Properties.Resources.BlackBase, decoder.width, decoder.height);
            using (MemoryStream s = new MemoryStream())
            {
                black.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                Black = Texture2D.FromFile(device, s);
            }
            //VideoTexture = Texture.FromBitmap(device, new Bitmap(Properties.Resources.BlackBase, decoder.width, decoder.height), 0, Pool.Managed);
            VideoTexture = new Texture2D(device, decoder.width, decoder.height, 1, 0, SurfaceFormat.Bgr32);
            VideoScale = (float)size.Width / decoder.width < (float)size.Height / decoder.height ? (float)size.Width / decoder.width : (float)size.Height / decoder.height;
            //scaleMatrix.Scale(scalef, scalef, 0.0f);
            //rotateMatrix.RotateZ(0f);
            //transformMatrix.Translate(new Vector3((size.Width - decoder.width * scalef) / 2, (size.Height - decoder.height * scalef) / 2, 0));
            VideoPosition = new Vector2((size.Width - decoder.width * VideoScale) / 2, (size.Height - decoder.height * VideoScale) / 2);
            //Videorect = new Rectangle(0, 0, decoder.width, decoder.height);
            videoexist = true;
        }
        public void initBG()
        {
            if (Map.Background != "" && !File.Exists(Map.Background))
            {
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "没事删什么BG TAT", System.Windows.Forms.ToolTipIcon.Info);
                Map.Background = "";
            }
            //Bitmap CurrentBG;
            if (Map.Background == "")
            {
                using (MemoryStream s = new MemoryStream())
                {
                    Core.defaultBG.Save(s, System.Drawing.Imaging.ImageFormat.Png);
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

            // BGTexture = Texture.FromBitmap(device, CurrentBG, 0, Pool.Managed);
            BGScale = (float)size.Width / BGTexture.Width < (float)size.Height / BGTexture.Height ? (float)size.Width / BGTexture.Width : (float)size.Height / BGTexture.Height;
            //bgscaleMatrix.Scale(scalef, scalef, 0.0f);
            //bgrotateMatrix.RotateZ(0f);
            //bgtransformMatrix.Translate(new Vector3((size.Width - CurrentBG.Width * scalef) / 2, (size.Height - CurrentBG.Height * scalef) / 2, 0));
            BGPosition = new Vector2((size.Width - BGTexture.Width * BGScale) / 2, (size.Height - BGTexture.Height * BGScale) / 2);
            //BGrect = new Rectangle(0, 0, CurrentBG.Width, CurrentBG.Height);

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
            for (int i = 0; i < Map.SB.elements.Count; i++)
            {
                if (Map.SB.elements[i].Type != OSUFiles.StoryBoard.ElementType.Sample)
                {
                    TGraphic element = new TGraphic(device, Map.SB.elements[i], Map.Location);
                    element.SetAlphaAction(new TSpriteAction(Map.SB.elements[i].F));
                    element.SetScaleXAction(new TSpriteAction(Map.SB.elements[i].SX));
                    element.SetScaleYAction(new TSpriteAction(Map.SB.elements[i].SY));
                    element.SetRotateAction(new TSpriteAction(Map.SB.elements[i].R));
                    element.SetColorAction(new TSpriteAction(Map.SB.elements[i].C));
                    element.SetXAction(new TSpriteAction(Map.SB.elements[i].X));
                    element.SetYAction(new TSpriteAction(Map.SB.elements[i].Y));
                    element.SetParameterAction(new TSpriteAction(Map.SB.elements[i].P, false, false));
                    SBelements.Add(element);
                }
            }
            SBexist = true;
        }
        public void RenderVideo(SpriteBatch sprite)
        {
            if (position - (double)Map.VideoOffset / 1000 < 0) { return; }
            VideoTexture.SetData<byte>(decoder.GetFrame(Convert.ToInt32(position * 1000 - Map.VideoOffset)));
            //VideoStream = VideoTexture.LockRectangle(0, LockFlags.None);
            //VideoStream.Write(decoder.GetFrame(Convert.ToInt32(position * 1000 - Map.VideoOffset)), 0, decoder.height * decoder.width * 4);
            //VideoTexture.UnlockRectangle(0);
            //sprite.Transform = Matrix.Scaling(1f, 1f, 0);
            sprite.Draw(Black, sizerect, Color.White);
            //sprite.Draw(Black, new Rectangle(new Point(0, 0), size), Vector3.Empty, Vector3.Empty, Color.White);
            //sprite.Transform = rotateMatrix * scaleMatrix * transformMatrix;
            //sprite.Draw(VideoTexture, video, Vector3.Empty, Vector3.Empty, Color.White);
            sprite.Draw(VideoTexture, VideoPosition, null, Color.White, 0f, Vector2.Zero, VideoScale, SpriteEffects.None, 1f);
        }
        public void RenderBG(SpriteBatch sprite)
        {
            //sprite.Transform = bgrotateMatrix * bgscaleMatrix * bgtransformMatrix;
            // sprite.Draw(BGTexture, bg, Vector3.Empty, Vector3.Empty, Color.White);
            sprite.Draw(BGTexture, BGPosition, null, Color.White, 0f, Vector2.Zero, BGScale, SpriteEffects.None, 1f);
        }
        public void Render()
        {
            if (device == null || device.IsDisposed || !CanRender()) { return; }
            device.Clear(Color.Black);
            //device.BeginScene();
            AlphaSprite.Begin(SpriteBlendMode.AlphaBlend);
            RenderBG(AlphaSprite);
            if (videoexist) { RenderVideo(AlphaSprite); }
            AdditiveSprite.Begin(SpriteBlendMode.Additive);
            if (SBexist) { RenderSB(); }
            AdditiveSprite.End();
            AlphaSprite.End();
            //device.EndScene();
            try
            {
                device.Present();
            }
            catch { deviceislost = true; }

        }
        public void Dispose()
        {
            device.Dispose();
            uni_Audio.Dispose();
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
            for (int i = 0; i < uni_Audio.Durnation * 1000; i++)
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
            if (!playfx) { return; }
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
                    if (uni_Audio != null) { uni_Audio.Volume = Allvolume * Musicvolume; }
                    break;
                case 2:
                    if (uni_Audio != null) { uni_Audio.Volume = Allvolume * Musicvolume; }
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
            uni_Audio.Stop();
        }
        public void Play()
        {
            willnext = false; cannext = true; videoexist = false;
            //initBG();
            uni_Audio.Open(Map.Audio);
            if (playfx) { initfx(); fxpos = 0; }
            uni_Audio.UpdateTimer.Tick += new EventHandler(AVsync);
            if (Map.haveVideo && playvideo && File.Exists(Path.Combine(Map.Location, Map.Video))) { initvideo(); }
            if (Map.haveSB && playsb) { initSB(); }
            uni_Audio.Play(Allvolume * Musicvolume);
        }
        public void Pause()
        {
            cannext = false;
            uni_Audio.Pause();
        }
        public void Resume()
        {
            uni_Audio.Pause();
            cannext = true;
        }
        public double durnation
        { get { return uni_Audio.Durnation; } }
        public double position
        { get { return uni_Audio.Position; } }
        public bool isplaying
        { get { return uni_Audio.Isplaying; } }
        public void seek(double time)
        {
            cannext = false;
            uni_Audio.Seek(time);
            fxpos = 0;
            if (playfx)
            {
                while (fxlist[fxpos].time <= uni_Audio.Position * 1000 && fxpos < fxlist.Count)
                {
                    fxpos++;
                }
            }
            cannext = true;
        }
        private void AVsync(object sender, EventArgs e)
        {
            if (!uni_Audio.Isplaying && cannext)
            {
                willnext = true;
                return;
            }
            if (fxpos < fxlist.Count)
            {
                while (fxlist[fxpos].time <= uni_Audio.Position * 1000)
                {
                    while ((fxlist[fxpos + 1].time <= uni_Audio.Position * 1000)) { fxpos++; }
                    PlayFx(fxpos);
                    fxpos++;
                }
            }
        }
    }
}
