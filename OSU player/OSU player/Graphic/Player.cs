using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OSUplayer.OsuFiles;
using OSUplayer.OsuFiles.StoryBoard;
using OSUplayer.Properties;
using OSUplayer.Uilties;
using Color = Microsoft.Xna.Framework.Graphics.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace OSUplayer.Graphic
{
    struct Fxlist
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
    class Player : IDisposable
    {
        private BassAudio uniAudio;
        List<Fxlist> fxlist = new List<Fxlist>();
        const int MaxFxplayer = 128;
        BassAudio[] fxplayer;
        string[] fxname = new string[MaxFxplayer];
        bool _cannext = true;
        public bool Willnext = false;
        bool _videoExist = false;
        bool _sbExist = false;
        int _fxpos = 0;
        static float Allvolume { get { return Settings.Default.Allvolume; } }
        static float Musicvolume { get { return Settings.Default.Musicvolume; } }
        static float Fxvolume { get { return Settings.Default.Fxvolume; } }
        static bool Playvideo { get { return Settings.Default.PlayVideo; } }
        static bool Playfx { get { return Settings.Default.PlayFx; } }
        static bool Playsb { get { return Settings.Default.PlaySB; } }
        Beatmap Map { get { return Core.CurrentBeatmap; } }
        BeatmapSet Set { get { return Core.CurrentSet; } }
        /// <summary>
        /// 渲染区域大小
        /// </summary>
        Rectangle _showRect;

        GraphicsDevice device = null;
        VideoDecoder _decoder;
        Texture2D _videoTexture;
        Texture2D _bgTexture;
        Texture2D _black;
        Vector2 _screenCenter;
        Vector2 _videoCenter;
        float _videoScale;
        Vector2 _bgCenter;
        float _bgScale;
        float _sbScale;
        Matrix SBtramsform;
        List<TGraphic> SBelements = new List<TGraphic>();
        SpriteBatch AlphaSprite;
        SpriteBatch AdditiveSprite;
        bool _deviceislost = false;
        public Player(IntPtr shandle, Size ssize)
        {
            uniAudio = new BassAudio();
            _showRect = new Rectangle(0, 0, ssize.Width, ssize.Height);
            _screenCenter = new Vector2((float)ssize.Width / 2, (float)ssize.Height / 2);
            _sbScale = Math.Min(_showRect.Width / 640f, _showRect.Height / 480f);
            SBtramsform = Matrix.CreateTranslation(-320, -240, 0) * Matrix.CreateScale(_sbScale, _sbScale, 1) * Matrix.CreateTranslation(new Vector3(_screenCenter, 0));
            /* presentParams.DeviceWindowHandle=handle;
             presentParams.IsFullScreen=false;
             device = new GraphicsDevice();
             device.Reset(presentParams, GraphicsAdapter.DefaultAdapter);*/
            var presentParams = new PresentationParameters
            {
                IsFullScreen = false,
                SwapEffect = SwapEffect.Discard,
                BackBufferHeight = Math.Max(_showRect.Height, 1),
                BackBufferWidth = Math.Max(_showRect.Width, 1)
            };
            device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, shandle, CreateOptions.MixedVertexProcessing, presentParams);
            AlphaSprite = new SpriteBatch(device);
            AdditiveSprite = new SpriteBatch(device);
            fxplayer = new BassAudio[MaxFxplayer];
            for (int i = 0; i < MaxFxplayer; i++)
            {
                fxplayer[i] = new BassAudio();
            }
            using (var s = new MemoryStream())
            {
                Resources.defaultBG.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                _bgTexture = Texture2D.FromFile(device, s);
            }
        }
        public void Resize(Size size)
        {
            if (device == null || size.Width == 0 || size.Height == 0)
                return;
            _showRect = new Rectangle(0, 0, size.Width, size.Height);
            _screenCenter = new Vector2((float)size.Width / 2, (float)size.Height / 2);
            device.PresentationParameters.BackBufferWidth = _showRect.Width;
            device.PresentationParameters.BackBufferHeight = _showRect.Height;
            device.Reset();
            _bgScale = Math.Min((float)_showRect.Width / _bgTexture.Width, (float)_showRect.Height / _bgTexture.Height);
            if (_videoExist) { _videoScale = Math.Min((float)_showRect.Width / _decoder.width, (float)_showRect.Height / _decoder.height); }
            _sbScale = Math.Min(_showRect.Width / 640f, _showRect.Height / 480f);
            SBtramsform = Matrix.CreateTranslation(-320, -240, 0) * Matrix.CreateScale(_sbScale, _sbScale, 1) * Matrix.CreateTranslation(new Vector3(_screenCenter, 0));
        }
        bool CanRender()
        {
            if (_deviceislost == false) { return true; }
            switch (device.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    return false;
                case GraphicsDeviceStatus.Normal:
                    _deviceislost = false;
                    return true;
                case GraphicsDeviceStatus.NotReset:
                    //AlphaSprite.Dispose();
                   // AdditiveSprite.Dispose();
                    //_bgTexture.Dispose();
                    //if (_black!=null) _black.Dispose();
                    //if (_videoTexture != null) _videoTexture.Dispose();
                    //Thread.Sleep(10000);
                    //TODO:Fix Invalid Method Call
                    device.Reset();
                    //AlphaSprite = new SpriteBatch(device);
                    //AdditiveSprite = new SpriteBatch(device);
                    _deviceislost = false;
                    return true;
                default:
                    return false;
            }
        }

        private void Initvideo()
        {
            _decoder = new VideoDecoder(10);
            _decoder.Open(Map.Video);
            var black = new Bitmap(Resources.BlackBase, _decoder.width, _decoder.height);
            using (var s = new MemoryStream())
            {
                black.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin);
                _black = Texture2D.FromFile(device, s);
            }
            _videoTexture = new Texture2D(device, _decoder.width, _decoder.height, 1, 0, SurfaceFormat.Bgr32);
            _videoScale = Math.Min((float)_showRect.Width / _decoder.width, (float)_showRect.Height / _decoder.height);
            _videoCenter = new Vector2((float)_decoder.width / 2, (float)_decoder.height / 2);
            _videoExist = true;
        }
        public void InitBG()
        {
            if (Map.Background != "" && !File.Exists(Map.Background))
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("BG_Loss_Tip_Text"));
                Map.Background = "";
            }
            if (Map.Background == "")
            {
                using (var s = new MemoryStream())
                {
                    Resources.defaultBG.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                    s.Seek(0, SeekOrigin.Begin);
                    _bgTexture = Texture2D.FromFile(device, s);
                }
            }
            else
            {
                using (var s = new FileStream(Map.Background, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    _bgTexture = Texture2D.FromFile(device, s);
                }
            }
            _bgScale = Math.Min((float)_showRect.Width / _bgTexture.Width, (float)_showRect.Height / _bgTexture.Height);
            _bgCenter = new Vector2((float)_bgTexture.Width / 2, (float)_bgTexture.Height / 2);
        }

        private void RenderSB()
        {
            foreach (var element in SBelements)
            {
                element.Update((int)(Position * 1000));
                element.Draw((element.parameter & 4) == 4 ? AdditiveSprite : AlphaSprite);
            }
        }

        private void InitSB()
        {
            Map.Getsb();
            if (Map.SB.Elements.Count == 1) return;
            foreach (var sbFile in Map.SB.Elements)
            {
                //if (Map.SB.Elements[i].Type == OsuFiles.StoryBoard.ElementType.Sample) continue;
                if (sbFile.Value.Element.Count == 0) continue;
                Texture2D[] tmpTexture = null;
                foreach (var sbelement in sbFile.Value.Element)
                {

                    switch (sbelement.Type)
                    {
                        case ElementType.Sprite:
                            if (tmpTexture == null || tmpTexture.Length != 1)
                            {
                                if (File.Exists(Path.Combine(Map.Location, sbFile.Key)))
                                {
                                    using (
                                        var s = new FileStream(Path.Combine(Map.Location, sbFile.Key), FileMode.Open,
                                            FileAccess.Read, FileShare.ReadWrite))
                                    {
                                        tmpTexture = new[] { Texture2D.FromFile(device, s) };
                                    }
                                }
                                else
                                {
                                    tmpTexture = new[] { new Texture2D(device, 1, 1, 0, 0, SurfaceFormat.Bgr32) };
                                }
                            }
                            break;
                        case ElementType.Animation:
                            var prefix = Path.Combine(Map.Location, sbFile.Key);
                            var ext = prefix.Substring(prefix.LastIndexOf(".") + 1);
                            prefix = prefix.Substring(0, prefix.LastIndexOf("."));
                            tmpTexture = new Texture2D[sbelement.FrameCount];
                            for (var i = 0; i < sbelement.FrameCount; i++)
                            {
                                if (File.Exists(prefix + i + "." + ext))
                                {
                                    using (
                                        var s = new FileStream(prefix + i + "." + ext, FileMode.Open, FileAccess.Read,
                                            FileShare.ReadWrite))
                                    {
                                        tmpTexture[i] = Texture2D.FromFile(device, s);
                                    }
                                }
                                else
                                {
                                    tmpTexture[i] = new Texture2D(device, 1, 1, 0, 0, SurfaceFormat.Bgr32);
                                }
                            }
                            break;
                    }
                    var element = new TGraphic(device, sbelement, tmpTexture);
                    element.SetAlphaAction(new TSpriteAction(sbelement.F));
                    element.SetScaleXAction(new TSpriteAction(sbelement.SX));
                    element.SetScaleYAction(new TSpriteAction(sbelement.SY));
                    element.SetRotateAction(new TSpriteAction(sbelement.R));
                    element.SetColorAction(new TSpriteAction(sbelement.C));
                    element.SetXAction(new TSpriteAction(sbelement.X));
                    element.SetYAction(new TSpriteAction(sbelement.Y));
                    element.SetParameterAction(new TSpriteAction(sbelement.P, false, false));
                    SBelements.Add(element);
                }
            }
            _sbExist = true;
        }

        private void RenderVideo(SpriteBatch sprite)
        {
            if (Position - (double)Map.VideoOffset / 1000 < 0) { return; }
            _videoTexture.SetData(_decoder.GetFrame(Convert.ToInt32(Position * 1000 - Map.VideoOffset)));
            sprite.Draw(_black, _showRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            sprite.Draw(_videoTexture, _screenCenter, null, Color.White, 0f, _videoCenter, _videoScale, SpriteEffects.None, 1f);
        }

        private void RenderBG(SpriteBatch sprite)
        {
            sprite.Draw(_bgTexture, _screenCenter, null, Color.White, 0f, _bgCenter, _bgScale, SpriteEffects.None, 1f);
        }
        public void Render()
        {
            if (device == null || device.IsDisposed || !CanRender()) { return; }
            device.Clear(Color.Black);
            AlphaSprite.Begin();
            if (!_sbExist) { RenderBG(AlphaSprite); }
            if (_videoExist) { RenderVideo(AlphaSprite); }
            AlphaSprite.End();
            if (_sbExist)
            {
                AlphaSprite.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None, SBtramsform);
                AdditiveSprite.Begin(SpriteBlendMode.Additive, SpriteSortMode.BackToFront, SaveStateMode.None, SBtramsform);
                RenderSB();
                AdditiveSprite.End();
                AlphaSprite.End();
            }
            try
            {
                device.Present();
            }
            catch { _deviceislost = true; }

        }
        public void Dispose()
        {
            device.Dispose();
            uniAudio.Dispose();
            for (int j = 0; j < MaxFxplayer; j++)
            {
                fxplayer[j].Dispose();
            }
        }
        private List<int> Setplayer(ref int player, List<string> filenames)
        {
            var ret = new List<int>();
            bool f = true;
            foreach (var filename in filenames)
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
        private void Initfx()
        {
            fxlist.Clear();
            for (int i = 0; i < MaxFxplayer; i++)
            {
                fxname[i] = "";
            }
            int currentT = 0;
            int current = 0;
            var nowdefault = Map.Timingpoints[currentT].sample;
            var olddefault = new CSample(0, 0);
            double bpm = Map.Timingpoints[currentT].bpm;
            double tbpm = bpm;
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
                            tbpm = Map.Timingpoints[currentT].bpm;
                        }
                        else
                        {
                            bpm = tbpm * Map.Timingpoints[currentT].bpm;
                        }
                    }
                }
                if (current == Map.HitObjects.Count) { break; }
                if (Map.HitObjects[current].starttime > i) { continue; }
                var tmpH = Map.HitObjects[current];
                var tmpSample = nowdefault;
                float volumeH = volume;
                switch (tmpH.type)
                {
                    case ObjectFlag.Normal:
                    case ObjectFlag.NormalNewCombo:
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.starttime, Setplayer(ref player, Set.Getsamplename
                                (tmpSample, tmpH.allhitsound)), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(
                           new Fxlist(tmpH.starttime, Setplayer(ref player, Set.Getsamplename
                               (tmpH.A_sample, tmpH.allhitsound)), volumeH));
                        }
                        break;
                    case ObjectFlag.Slider:
                    case ObjectFlag.SliderNewCombo:
                        //TODO:每个节点的sampleset
                        //TODO:滑条tick
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        double deltatime = (600.0 * tmpH.length / bpm / Map.SliderMultiplier);
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume / 100; }
                        for (int j = 0; j <= tmpH.repeatcount; j++)
                        {
                            fxlist.Add(
                                new Fxlist((int)(tmpH.starttime + deltatime * j),
                                    Setplayer(ref player, Set.Getsamplename
                            (tmpSample, tmpH.Hitsounds[j])), volumeH));
                        }
                        break;
                    case ObjectFlag.Spinner:
                    case ObjectFlag.SpinnerNewCombo:
                        if (tmpH.sample != olddefault) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.EndTime, Setplayer(ref player, Set.Getsamplename(tmpSample, tmpH.allhitsound)), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(new Fxlist(tmpH.EndTime, Setplayer(ref player, Set.Getsamplename
                                (tmpH.A_sample, tmpH.allhitsound)), volumeH));
                        }
                        break;
                }
                current++;
            }
            fxlist.Sort((a, b) => a.time.CompareTo(b.time));
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
            _cannext = false;
            if (_videoExist) { _videoExist = false; _videoTexture.Dispose(); _decoder.Dispose(); }
            if (_sbExist) { _sbExist = false; SBelements.Clear(); }
            uniAudio.Stop();
        }
        public void Play()
        {
            Willnext = false; _cannext = true; _videoExist = false;
            BassAudio.Init();
            uniAudio.Open(Map.Audio);
            if (Playfx) { Initfx(); _fxpos = 0; }
            uniAudio.UpdateTimer.Tick += AVsync;
            _videoExist = false; _sbExist = false;
            if (Map.HaveVideo && Playvideo && File.Exists(Map.Video)) { Initvideo(); }
            if (Map.HaveSB && Playsb) { InitSB(); }
            uniAudio.Play(Allvolume * Musicvolume);
        }
        public void Pause()
        {
            _cannext = false;
            uniAudio.Pause();
        }
        public void Resume()
        {
            uniAudio.Pause();
            _cannext = true;
        }
        public double Durnation
        { get { return uniAudio.Durnation; } }
        public double Position
        { get { return uniAudio.Position; } }
        public bool Isplaying
        { get { return uniAudio.Isplaying; } }
        public void Seek(double time)
        {
            _cannext = false;
            uniAudio.Seek(time);
            _fxpos = 0;
            if (Playfx)
            {
                while (fxlist[_fxpos].time <= uniAudio.Position * 1000 && _fxpos < fxlist.Count)
                {
                    _fxpos++;
                }
            }
            _cannext = true;
        }
        private void AVsync(object sender, EventArgs e)
        {
            if (!uniAudio.Isplaying && _cannext)
            {
                Willnext = true;
                return;
            }
            if (_fxpos < fxlist.Count)
            {
                while (fxlist[_fxpos].time <= uniAudio.Position * 1000)
                {
                    while ((fxlist[_fxpos + 1].time <= uniAudio.Position * 1000)) { _fxpos++; }
                    PlayFx(_fxpos);
                    _fxpos++;
                }
            }
        }
    }
}
