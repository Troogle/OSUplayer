using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using Un4seen.Bass;

namespace OSU_player
{

    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Videofiles uni_Video;
        public Audiofiles uni_Audio = new Audiofiles();
        public QQ uni_QQ = new QQ();
        // Thread DelayVideo = new Thread((delegate() { Thread.Sleep(10); }));
        int Nextmode = 3;
        List<ListViewItem> FullList = new List<ListViewItem>();
        BeatmapSet CurrentSet;
        Beatmap CurrentBeatmap;
        BeatmapSet TmpSet;
        Beatmap TmpBeatmap;
        private struct Fxlist
        {
            public int time;
            public List<string> play;
            public float volume;
            public Fxlist(int time, List<string> play, float volume)
            {
                this.time = time;
                this.play = play;
                this.volume = volume;
            }
        }
        List<Fxlist> fxlist = new List<Fxlist>();
        const int maxfxplayer = 128;
        Audiofiles[] fxplayer = new Audiofiles[maxfxplayer];
        float Allvolume = 1.0f;
        float Musicvolume = 0.8f;
        float Fxvolume = 0.6f;
        int fxpos = 0;
        bool playvideo = true;
        bool playfx = true;
        bool playsb = true;
        bool needsave = false;
        bool cannext = true;

        #region 各种方法
        private void AskForExit(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            DialogResult close;
            close = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo);
            if (close == DialogResult.Yes)
            {
                uni_QQ.Send2QQ(Core.uin, "");
                uni_Audio.Dispose();
                for (int j = 0; j < maxfxplayer; j++)
                {
                    fxplayer[j].Dispose();
                }
                Bass.BASS_Stop();
                Bass.BASS_Free();
                if (needsave) { Core.SaveList(); }
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
            }

        }
        private void printdetail()
        {
            ListDetail.Items.Clear();
            ListViewItem tmpl = new ListViewItem("歌曲名称");
            tmpl.SubItems.Add(TmpBeatmap.Title);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("歌手");
            tmpl.SubItems.Add(TmpBeatmap.Artist);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("作者");
            tmpl.SubItems.Add(TmpBeatmap.Creator);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("来源");
            tmpl.SubItems.Add(TmpBeatmap.Source);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("模式");
            tmpl.SubItems.Add(Enum.GetName(typeof(modes), TmpBeatmap.Mode));
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("SetID");
            tmpl.SubItems.Add(TmpBeatmap.beatmapsetId.ToString());
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("ID");
            tmpl.SubItems.Add(TmpBeatmap.beatmapId.ToString());
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("音频文件名称");
            tmpl.SubItems.Add(TmpBeatmap.Audio);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("背景文件名称");
            tmpl.SubItems.Add(TmpBeatmap.Background);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("视频文件名称");
            tmpl.SubItems.Add(TmpBeatmap.video);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("OSU文件版本");
            tmpl.SubItems.Add(TmpBeatmap.FileVersion);
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("HP");
            tmpl.SubItems.Add(TmpBeatmap.HPDrainRate.ToString());
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("CS");
            tmpl.SubItems.Add(TmpBeatmap.CircleSize.ToString());
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("OD");
            tmpl.SubItems.Add(TmpBeatmap.OverallDifficulty.ToString());
            ListDetail.Items.Add(tmpl);
            tmpl = new ListViewItem("AR");
            tmpl.SubItems.Add(TmpBeatmap.ApproachRate.ToString());
            ListDetail.Items.Add(tmpl);
        }
        private void setbg()
        {
            printdetail();
            pictureBox1.Image = Image.FromFile(CurrentBeatmap.Background);
            pictureBox1.Visible = true;
        }
        private void Stop()
        {
            cannext = false;
            uni_Audio.Stop();
            uni_Video.Stop();
            pictureBox1.Visible = true;
            TrackBar1.Enabled = false;
            TrackBar1.Value = 0;
            StopButton.Enabled = false;
            PlayButton.Text = "播放";
            cannext = true;
        }
        private void Play()
        {
            uni_Audio.Open(CurrentBeatmap.Audio);
            if (playfx) { initfx(); fxpos = 0; }
            uni_Audio.UpdateTimer.Tick += new EventHandler(AVsync);
            if (CurrentBeatmap.haveVideo && playvideo)
            {
                pictureBox1.Visible = false;
                panel2.Visible = true;
                if (CurrentBeatmap.videoOffset > 0)
                {
                    uni_Audio.Play(0, Allvolume * Musicvolume);
                    uni_Video.Play(Path.Combine(CurrentBeatmap.Location, CurrentBeatmap.video), CurrentBeatmap.videoOffset);
                }
                else
                {
                    uni_Video.Play(Path.Combine(CurrentBeatmap.Location, CurrentBeatmap.video), 0);
                    uni_Audio.Play(-CurrentBeatmap.videoOffset, Allvolume * Musicvolume);

                }
            }
            else { uni_Audio.Play(0, Allvolume * Musicvolume); }
            TrackBar1.Enabled = true;
            uni_QQ.Send2QQ(Core.uin, CurrentBeatmap.Name);
            PlayButton.Text = "暂停";
            StopButton.Enabled = true;
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
        private void initfx()
        {
            fxlist.Clear();
            Beatmap tmp = CurrentBeatmap;
            int currentT = 0;
            int current = 0;
            CSample nowdefault = tmp.Timingpoints[currentT].sample;
            CSample olddefault = new CSample(0, 0);
            double bpm = tmp.Timingpoints[currentT].bpm;
            double Tbpm = bpm;
            float volume = tmp.Timingpoints[currentT].volume;
            for (int i = 0; i < uni_Audio.durnation * 1000; i++)
            {
                if (currentT + 1 < tmp.Timingpoints.Count)
                {
                    if (tmp.Timingpoints[currentT + 1].offset <= i)
                    {
                        currentT++;
                        nowdefault = tmp.Timingpoints[currentT].sample;
                        volume = tmp.Timingpoints[currentT].volume;
                        if (tmp.Timingpoints[currentT].type == 1)
                        {
                            bpm = tmp.Timingpoints[currentT].bpm;
                            Tbpm = tmp.Timingpoints[currentT].bpm;
                        }
                        else
                        {
                            bpm = Tbpm * tmp.Timingpoints[currentT].bpm;
                        }

                    }
                }
                if (current == tmp.HitObjects.Count) { break; }
                if (tmp.HitObjects[current].starttime > i) { continue; }
                HitObject tmpH = tmp.HitObjects[current];
                CSample tmpSample = nowdefault;
                float volumeH = volume;
                switch (tmpH.type)
                {
                    case ObjectFlag.Normal:
                    case ObjectFlag.NormalNewCombo:
                        if (!tmpH.sample.Equals(olddefault)) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.starttime, CurrentSet.getsamplename
                                (tmpSample, tmpH.allhitsound), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(
                           new Fxlist(tmpH.starttime, CurrentSet.getsamplename
                               (tmpH.A_sample, tmpH.allhitsound), volumeH));
                        }
                        break;
                    case ObjectFlag.Slider:
                    case ObjectFlag.SliderNewCombo:
                        //TODO:每个节点的sampleset
                        if (!tmpH.sample.Equals(olddefault)) { tmpSample = tmpH.sample; }
                        double deltatime = (600 * tmpH.length / (bpm * tmp.SliderMultiplier));
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume / 100; }
                        for (int j = 0; j <= tmpH.repeatcount; j++)
                        {
                            fxlist.Add(
                                new Fxlist((int)(tmpH.starttime + deltatime * j), CurrentSet.getsamplename
                            (tmpSample, tmpH.Hitsounds[j]), volumeH));
                        }
                        break;
                    case ObjectFlag.Spinner:
                    case ObjectFlag.SpinnerNewCombo:
                        if (!tmpH.sample.Equals(olddefault)) { tmpSample = tmpH.sample; }
                        if (tmpH.S_Volume != 0) { volumeH = tmpH.S_Volume; }
                        fxlist.Add(
                            new Fxlist(tmpH.EndTime, CurrentSet.getsamplename(tmpSample, tmpH.allhitsound), volumeH));
                        if (tmpH.A_sample.sample != 0)
                        {
                            fxlist.Add(new Fxlist(tmpH.EndTime, CurrentSet.getsamplename
                                (tmpH.A_sample, tmpH.allhitsound), volumeH));
                        }
                        break;
                    default:
                        break;
                }
                current++;
            }
            fxlist.Sort(Fxlistcompare);
            int index = 0;
            while (index < fxlist.Count)
            {
                if (fxlist[index].play.Count == 0) { fxlist.RemoveAt(index); }
                else { index++; };
            }
            fxlist.Add(new Fxlist(Int32.MaxValue, new List<string> { }, 0));
            /*极度怀疑有无必要,取消了算了
            int index = 0;
            while (index < fxlist.Count-1)
            {
                while (fxlist[index].time == fxlist[index + 1].time)
                {
                    fxlist[index].play.AddRange(fxlist[index + 1].play);
                    fxlist.RemoveAt(index + 1);
                }
                index++;
            }*/
        }
        private void PlayFx(int pos)
        {
            if (!playfx) { return; }
            int k = 0;
            for (int j = 0; j < fxlist[pos].play.Count; j++)
            {
                k = 0;
                while (fxplayer[k].isplaying) { k = (k + 1) % maxfxplayer; }
                fxplayer[k].Open(fxlist[pos].play[j]);
                fxplayer[k].Play(0, Allvolume * Fxvolume * fxlist[pos].volume);
            }
        }
        private void Pause()
        {
            cannext = false;
            uni_Audio.Pause();
            uni_Video.Pause();
            uni_QQ.Send2QQ(Core.uin, "");
            PlayButton.Text = "播放";
        }
        private void Resume()
        {
            uni_Audio.Pause();
            uni_Video.Pause();
            uni_QQ.Send2QQ(Core.uin, CurrentBeatmap.Name);
            PlayButton.Text = "暂停";
            cannext = true;
        }
        private void PlayNext()
        {

            int next;
            int now;
            if (PlayList.SelectedItems.Count == 0) { now = 0; }
            else { now = PlayList.SelectedItems[0].Index; };
            switch (Nextmode)
            {
                case 1: next = (now + 1) % PlayList.Items.Count;
                    break;
                case 2: next = now;
                    break;
                case 3: next = new Random().Next() % PlayList.Items.Count;
                    break;
                default: next = 0;
                    break;
            }
            PlayList.Select();
            if (PlayList.SelectedItems.Count != 0)
            {
                PlayList.SelectedItems[0].Selected = false;
            }
            PlayList.Items[next].Selected = true;
            PlayList.EnsureVisible(next);
            TmpBeatmap = TmpSet.Diffs[0];
            CurrentSet = TmpSet;
            CurrentBeatmap = TmpBeatmap;
            setbg();
            Play();
        }
        private void scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                BeatmapSet tmp = new BeatmapSet(path);
                //tmp.GetDetail();
                Core.allsets.Add(tmp);
                this.backgroundWorker1.ReportProgress(0, tmp.ToString());
            }
            else
            {
                string[] tmpfolder = Directory.GetDirectories(path);
                foreach (string subfolder in tmpfolder)
                {
                    scanforset(subfolder);
                }
            }
        }
        private void initset()
        {
            if (File.Exists("list.db") && Core.LoadList())
            {
                for (int i = 0; i < Core.allsets.Count; i++)
                {
                    BeatmapSet bms = Core.allsets[i];
                    ListViewItem tmpl = new ListViewItem(bms.ToString());
                    tmpl.SubItems.Add(i.ToString());
                    PlayList.Items.Add(tmpl);
                    FullList.Add(tmpl);
                }
            }
            else
            {
                MessageBox.Show("将开始初始化");
                button3.Enabled = false;
                try
                {
                    if (Directory.Exists(Path.Combine(Core.osupath, "Songs")))
                    {
                        this.backgroundWorker1.RunWorkerAsync(Path.Combine(Core.osupath, "Songs"));
                    }
                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    throw (new FormatException("Failed to read song path", ex));
                }
            }
            for (int i = 0; i < maxfxplayer; i++)
            {
                fxplayer[i] = new Audiofiles();
            }
        }
        private void remove(int index)
        {
            PlayList.Enabled = false;
            PlayList.Items.Clear();
            DiffList.Items.Clear();
            DiffList.Enabled = false;
            Core.allsets.RemoveAt(index);
            FullList.RemoveAt(index);
            for (int i = index; i < FullList.Count; i++)
            {
                FullList[i].SubItems[1].Text = (Convert.ToInt32(FullList[i].SubItems[1].Text) + 1).ToString();
            }
            foreach (ListViewItem item in FullList)
            {
                PlayList.Items.Add(item);
            }
            PlayList.Enabled = true;
            DiffList.Enabled = true;
            needsave = true;
            PlayNext();
        }
        #endregion
        private void LoadPreference()
        {
            Core.uin = Properties.Settings.Default.QQuin;
            LabelQQ.Text = "当前同步QQ：" + Core.uin.ToString();
            if (Core.uin == 0)
            {
                if (
                    MessageBox.Show(this, "木有设置过QQ号，需要现在设置么？", "提示", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    using (Form2 dialog = new Form2())
                    {
                        dialog.ShowDialog();
                    }
                    LabelQQ.Text = "当前同步QQ：" + Core.uin.ToString();
                    Properties.Settings.Default.QQuin = Core.uin;
                    Core.syncQQ = true;
                    Properties.Settings.Default.SyncQQ = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    QQ状态同步.Checked = false;
                    Core.syncQQ = false;
                    Properties.Settings.Default.SyncQQ = false;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                Core.syncQQ = Properties.Settings.Default.SyncQQ;
                QQ状态同步.Checked = Core.syncQQ;
            }
            Allvolume = Properties.Settings.Default.Allvolume;
            Fxvolume = Properties.Settings.Default.Fxvolume;
            Musicvolume = Properties.Settings.Default.Musicvolume;
            playfx = Properties.Settings.Default.PlayFx;
            音效.Checked = playfx;
            playsb = Properties.Settings.Default.PlaySB;
            SB开关.Checked = playsb;
            playvideo = Properties.Settings.Default.PlayVideo;
            视频开关.Checked = playvideo;
            Nextmode = Properties.Settings.Default.NextMode;
            switch (Nextmode)
            {
                case 1: 顺序播放.Checked = true;
                    break;
                case 2: 单曲循环.Checked = true;
                    break;
                case 3: 随机播放.Checked = true;
                    break;
                default:
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BassNet.Registration("sqh1994@163.com", "2X280331512622");
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            new Thread(new ThreadStart(Selfupdate.check_update)).Start();
            Core.Getpath();
            LoadPreference();
            initset();
            uni_Video = new Videofiles(this.panel2);
        }
        private void AVsync(object sender, EventArgs e)
        {
            if (!uni_Audio.isplaying && cannext)
            {
                PlayNext();
                return;
            }
            if (fxpos < fxlist.Count)
            {
                while (fxlist[fxpos].time <= (int)Math.Floor(uni_Audio.position * 1000))
                {
                    new Thread(new ThreadStart(delegate() { PlayFx(fxpos); })).Start();
                    if (fxpos + 1 < fxlist.Count) { fxpos++; } else { break; }
                }
            }
            TrackBar1.Value = (int)Math.Round((uni_Audio.position / uni_Audio.durnation) * TrackBar1.Maximum);
            label1.Text = String.Format("{0}:{1:D2} / {2}:{3:D2}", (int)uni_Audio.position / 60,
                (int)uni_Audio.position % 60, (int)uni_Audio.durnation / 60,
                (int)uni_Audio.durnation % 60);

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            scanforset(e.Argument.ToString());
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Length > 0)
            {
                ListViewItem tmpl = new ListViewItem(e.UserState.ToString());
                PlayList.Items.Add(tmpl);
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (ListViewItem item in PlayList.Items)
            {
                item.SubItems.Add(item.Index.ToString());
                FullList.Add(item);
            }
            MessageBox.Show(string.Format("初始化完毕，发现曲目{0}个", Core.allsets.Count));
            PlayList.Items[0].Selected = true;
            Core.SaveList();
            button3.Enabled = true;
        }


        #region 菜单栏
        private void button3_Click(object sender, EventArgs e)
        {
            File.Delete("list.db");
            Core.allsets.Clear();
            FullList.Clear();
            PlayList.Items.Clear();
            DiffList.Items.Clear();
            initset();
        }
        #region 文件
        private void 运行OSU_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Core.osupath, "osu!.exe"));
        }
        private void 手动指定OSU目录_Click(object sender, EventArgs e)
        {
            if (Core.Setpath())
            {
                Core.allsets.Clear();
                PlayList.Items.Clear();
                initset();
            }
        }
        private void 重新导入osu_Click(object sender, EventArgs e)
        {

        }
        private void 重新导入collections_Click(object sender, EventArgs e)
        {

        }
        private void 打开曲目文件夹_Click(object sender, EventArgs e)
        {
            Process.Start(TmpSet.location);
        }
        private void 打开铺面文件_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", TmpBeatmap.Path);

        }
        private void 打开SB文件_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", TmpSet.OsbPath);
        }
        private void 退出_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region 工具
        private void 重复歌曲扫描_Click(object sender, EventArgs e)
        {
            using (DelDulp dialog = new DelDulp())
            {
                dialog.Show();
            }
        }
        #endregion
        #region 选项
        private void 随机播放_Click(object sender, EventArgs e)
        {
            Nextmode = 3;
            Properties.Settings.Default.NextMode = 3;
            Properties.Settings.Default.Save();
        }
        private void 顺序播放_Click(object sender, EventArgs e)
        {
            Nextmode = 1;
            Properties.Settings.Default.NextMode = 1;
            Properties.Settings.Default.Save();
        }
        private void 单曲循环_Click(object sender, EventArgs e)
        {
            Nextmode = 2;
            Properties.Settings.Default.NextMode = 2;
            Properties.Settings.Default.Save();
        }
        private void 音效_Click(object sender, EventArgs e)
        {
            playfx = 音效.Checked;
            Properties.Settings.Default.PlayFx = playfx;
            Properties.Settings.Default.Save();
        }
        private void 视频开关_Click(object sender, EventArgs e)
        {
            playvideo = 视频开关.Checked;
            Properties.Settings.Default.PlayVideo = playvideo;
            Properties.Settings.Default.Save();
        }
        private void QQ状态同步_Click(object sender, EventArgs e)
        {
            if (Core.syncQQ && Core.uin != 0) { uni_QQ.Send2QQ(Core.uin, ""); }
            Core.syncQQ = QQ状态同步.Checked;
            Properties.Settings.Default.SyncQQ = Core.syncQQ;
            Properties.Settings.Default.Save();
        }
        private void SB开关_Click(object sender, EventArgs e)
        {
            playsb = SB开关.Checked;
            Properties.Settings.Default.PlaySB = playsb;
            Properties.Settings.Default.Save();
        }
        #endregion
        private void 关于_Click(object sender, EventArgs e)
        {
            using (About dialog = new About())
            {
                dialog.ShowDialog();
            }

        }

        #endregion
        #region 第一排
        private void Button2_Click(object sender, EventArgs e)
        {

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            using (Form2 dialog = new Form2())
            {
                dialog.ShowDialog();
            }
            LabelQQ.Text = "当前同步QQ：" + Core.uin.ToString();
            Properties.Settings.Default.QQuin = Core.uin;
            Properties.Settings.Default.Save();
        }
        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            Fxvolume = (float)TrackBar3.Value / (float)TrackBar3.Maximum;
            Properties.Settings.Default.Fxvolume = Fxvolume;
            Properties.Settings.Default.Save();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            Musicvolume = (float)trackBar4.Value / (float)trackBar4.Maximum;
            if (uni_Audio != null) { uni_Audio.Volume = Allvolume * Musicvolume; }
            Properties.Settings.Default.Musicvolume = Musicvolume;
            Properties.Settings.Default.Save();
        }
        #endregion
        #region 第二排
        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayList.SelectedItems.Count == 0) { return; }
            DiffList.Items.Clear();
            TmpSet = Core.allsets[Convert.ToInt32(PlayList.SelectedItems[0].SubItems[1].Text)];
            if (!TmpSet.check())
            {
                MessageBox.Show("没事删什么曲子啊><", ">_<");
                remove(Convert.ToInt32(PlayList.SelectedItems[0].SubItems[1].Text));
                return;
            }
            if (!TmpSet.detailed)
            {
                TmpSet.GetDetail();
            }
            foreach (var s in TmpSet.diffstr)
            {
                DiffList.Items.Add(s);
            }
            if (!File.Exists(TmpSet.OsbPath))
            { 打开SB文件.Enabled = false; }
            else
            { 打开SB文件.Enabled = true; }
            DiffList.SelectedIndex = 0;
            //因为改变selectedindex会触发listbox的进程，所以以下省略了
        }
        private void DiffList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayList.SelectedIndices.Count == 0)
            {
                return;
            }
            TmpBeatmap = TmpSet.Diffs[DiffList.SelectedIndex];
            if (!StopButton.Enabled)
            {
                CurrentSet = TmpSet;
                CurrentBeatmap = TmpBeatmap;
                uni_Video.Stop();
                setbg();
            }
            else if (uni_Audio.isplaying)
            {
                printdetail();
            }
            else
            {
                Stop();
                setbg();
            }
        }
        private void PlayList_DoubleClick(object sender, EventArgs e)
        {
            TmpSet = Core.allsets[Convert.ToInt32(PlayList.SelectedItems[0].SubItems[1].Text)];
            TmpBeatmap = TmpSet.Diffs[0];
            CurrentSet = TmpSet;
            CurrentBeatmap = TmpBeatmap;
            Stop();
            setbg();
            Play();
        }
        private void DiffList_DoubleClick(object sender, EventArgs e)
        {
            TmpSet = Core.allsets[Convert.ToInt32(PlayList.SelectedItems[0].SubItems[1].Text)];
            TmpBeatmap = TmpSet.Diffs[DiffList.SelectedIndex];
            CurrentSet = TmpSet;
            CurrentBeatmap = TmpBeatmap;
            Stop();
            setbg();
            Play();
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            Stop();
            PlayNext();
        }
        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            uni_Audio.Seek(TrackBar1.Value * uni_Audio.durnation / TrackBar1.Maximum);
            uni_Video.seek(TrackBar1.Value * uni_Audio.durnation / TrackBar1.Maximum + CurrentBeatmap.videoOffset / 1000);
            fxpos = 0;
            while (fxlist[fxpos].time <= (int)Math.Floor(uni_Audio.position * 1000))
            {
                if (fxpos + 1 < fxlist.Count) { fxpos++; } else { break; }
            }
        }
        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            Allvolume = (float)TrackBar2.Value / (float)TrackBar2.Maximum;
            if (uni_Audio != null) { uni_Audio.Volume = Allvolume * Musicvolume; }
            Properties.Settings.Default.Allvolume = Allvolume;
            Properties.Settings.Default.Save();
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            Stop();
            PlayList.Enabled = false;
            PlayList.Items.Clear();
            DiffList.Items.Clear();
            DiffList.Enabled = false;
            if (TextBox1.Text == "")
            {
                foreach (ListViewItem item in FullList)
                {
                    PlayList.Items.Add(item);
                }
            }
            else
            {
                foreach (ListViewItem item in FullList)
                {
                    if (item.Text.ToLower().Contains(TextBox1.Text.ToLower()))
                    {
                        PlayList.Items.Add(item);
                    }
                }
            }
            if (PlayList.Items.Count != 0) { PlayList.Items[0].Selected = true; }
            PlayList.Enabled = true;
            DiffList.Enabled = true;
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (StopButton.Enabled == false)
            {
                if (CurrentSet == null)
                {
                    PlayList.Items[0].Selected = true;
                }
                Play();
            }
            else
            {

                if (PlayButton.Text == "播放")
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

        }
        #endregion
    }
}