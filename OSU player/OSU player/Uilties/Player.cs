using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OSU_player.Graphic;
using OSU_player.OSUFiles;
using System.Diagnostics;
namespace OSU_player.Graphic
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
        Size size { get { return Core.size; } }
        IntPtr handle { get { return Core.handle; } }
        Device device = null;
        VideoDecoder decoder = new VideoDecoder(100);
        Texture VideoTexture;
        Texture BGTexture;
        Texture Black;
        PresentParameters presentParams = new PresentParameters();
        Rectangle video;
        Matrix transformMatrix = new Matrix();
        Matrix rotateMatrix = new Matrix();
        Matrix scaleMatrix = new Matrix();
        Rectangle bg;
        Matrix bgtransformMatrix = new Matrix();
        Matrix bgrotateMatrix = new Matrix();
        Matrix bgscaleMatrix = new Matrix();
        List<TGraphic> SBelements = new List<TGraphic>();
        GraphicsStream VideoStream;
        Sprite NormalSprite;
        Sprite AlphaSprite;
        bool deviceislost = false;
        public Player()
        {
            uni_Audio = new Audiofiles();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, handle, CreateFlags.SoftwareVertexProcessing, presentParams);
            NormalSprite = new Sprite(device);
            AlphaSprite = new Sprite(device);
            for (int i = 0; i < maxfxplayer; i++)
            {
                fxplayer[i] = new Audiofiles();
            }
        }
        bool CanRender()
        {
            if (deviceislost == false)
                return true;
            int Result;
            device.CheckCooperativeLevel(out Result);
            switch ((ResultCode)Result)
            {
                case ResultCode.Success:
                    deviceislost = false;
                    return true;
                case ResultCode.DeviceLost:
                    return false;
                case ResultCode.DeviceNotReset:
                    try
                    {
                        device.Reset(device.PresentationParameters);
                        deviceislost = false;
                        return true;
                    }
                    catch (DirectXException exception)
                    {
                        if ((ResultCode)exception.ErrorCode == ResultCode.DeviceLost)
                            return false;
                        throw exception;
                    }
                default:
                    DirectXException DXException = new DirectXException();
                    DXException.ErrorCode = Result;
                    throw DXException;
            }
        }
        public void initvideo()
        {
            decoder = new VideoDecoder(100);
            decoder.Open(Path.Combine(Map.Location, Map.Video));
            VideoTexture = Texture.FromBitmap(device, new Bitmap(Properties.Resources.BlackBase, decoder.width, decoder.height), 0, Pool.Managed);
            Black = Texture.FromBitmap(device, new Bitmap(Properties.Resources.BlackBase, size.Width, size.Height), 0, Pool.Managed);
            float scalef = (float)size.Width / decoder.width < (float)size.Height / decoder.height ? (float)size.Width / decoder.width : (float)size.Height / decoder.height;
            scaleMatrix.Scale(scalef, scalef, 0.0f);
            rotateMatrix.RotateZ(0f);
            transformMatrix.Translate(new Vector3((size.Width - decoder.width * scalef) / 2, (size.Height - decoder.height * scalef) / 2, 0));
            video = new Rectangle(0, 0, decoder.width, decoder.height);
            videoexist = true;
        }
        public void initBG()
        {
            if (Map.Background != "" && !File.Exists(Map.Background))
            {
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "没事删什么BG TAT", System.Windows.Forms.ToolTipIcon.Info);
                Map.Background = "";
            }
            Bitmap CurrentBG;
            if (Map.Background == "")
            { CurrentBG = new Bitmap(Core.defaultBG); }
            else { CurrentBG = new Bitmap(Map.Background); }
            BGTexture = Texture.FromBitmap(device, CurrentBG, 0, Pool.Managed);
            float scalef = (float)size.Width / CurrentBG.Width < (float)size.Height / CurrentBG.Height ? (float)size.Width / CurrentBG.Width : (float)size.Height / CurrentBG.Height;
            bgscaleMatrix.Scale(scalef, scalef, 0.0f);
            bgrotateMatrix.RotateZ(0f);
            bgtransformMatrix.Translate(new Vector3((size.Width - CurrentBG.Width * scalef) / 2, (size.Height - CurrentBG.Height * scalef) / 2, 0));
            bg = new Rectangle(0, 0, CurrentBG.Width, CurrentBG.Height);

        }
        public void RenderSB()
        {
            foreach (var element in SBelements)
            {
                element.Update((int)(position * 1000));
                if (element.parameter == 3)
                { element.Draw(AlphaSprite); }
                else
                { element.Draw(NormalSprite); }
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
                    element.SetParameterAction(new TSpriteAction(Map.SB.elements[i].P));
                    SBelements.Add(element);
                }
            }
            SBexist = true;
        }
        public void RenderVideo(Sprite sprite)
        {
            if (position - (double)Map.VideoOffset / 1000 < 0) { return; }
            VideoStream = VideoTexture.LockRectangle(0, LockFlags.None);
            VideoStream.Write(decoder.GetFrame(Convert.ToInt32(position * 1000 - Map.VideoOffset)), 0, decoder.height * decoder.width * 4);
            VideoTexture.UnlockRectangle(0);
            sprite.Transform = Matrix.Scaling(1f, 1f,0);
            sprite.Draw(Black, new Rectangle(new Point(0, 0), size), Vector3.Empty, Vector3.Empty, Color.White);
            sprite.Transform = rotateMatrix * scaleMatrix * transformMatrix;
            sprite.Draw(VideoTexture, video, Vector3.Empty, Vector3.Empty, Color.White);

        }
        public void RenderBG(Sprite sprite)
        {
            sprite.Transform = bgrotateMatrix * bgscaleMatrix * bgtransformMatrix;
            sprite.Draw(BGTexture, bg, Vector3.Empty, Vector3.Empty, Color.White);
        }
        public void Render()
        {
            if (device == null || device.Disposed || !CanRender()) { return; }
            device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
            device.BeginScene();
            NormalSprite.Begin(SpriteFlags.None);
            AlphaSprite.Begin(SpriteFlags.AlphaBlend);
            RenderBG(NormalSprite);
            if (videoexist) { RenderVideo(NormalSprite); }
            if (SBexist) { RenderSB(); }
            NormalSprite.End();
            AlphaSprite.End();
            device.EndScene();
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
            for (int i = 0; i < uni_Audio.durnation * 1000; i++)
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
        { get { return uni_Audio.durnation; } }
        public double position
        { get { return uni_Audio.position; } }
        public bool isplaying
        { get { return uni_Audio.isplaying; } }
        public void seek(double time)
        {
            cannext = false;
            uni_Audio.Seek(time);
            fxpos = 0;
            if (playfx)
            {
                while (fxlist[fxpos].time <= uni_Audio.position * 1000 && fxpos < fxlist.Count)
                {
                    fxpos++;
                }
            }
            cannext = true;
        }
        private void AVsync(object sender, EventArgs e)
        {
            if (!uni_Audio.isplaying && cannext)
            {
                willnext = true;
                return;
            }
            if (fxpos < fxlist.Count)
            {
                while (fxlist[fxpos].time <= uni_Audio.position * 1000)
                {
                    while ((fxlist[fxpos + 1].time <= uni_Audio.position * 1000)) { fxpos++; }
                    PlayFx(fxpos);
                    fxpos++;
                }
            }
        }
    }
}
