using System;
using System.Collections.Generic;
using System.IO;
namespace OSUplayer.OsuFiles
{
    public struct Score
    {
        public string player;
        public int score;
        public modes mode;
        public string mod;
        public int hit300;
        public int hit320;
        public int hit200;
        public int hit100;
        public int hit50;
        public int totalhit;
        public int miss;
        public int maxCombo;
        public DateTime time;
        public double acc;
    }

    public class BinaryReader : System.IO.BinaryReader
    {
        public BinaryReader(System.IO.Stream stream) : base(stream) { }
        public override string ReadString()
        {
            if (ReadByte() == 0x0b) { return base.ReadString(); }
            return "";
        }
    }
    class OsuDB
    {
        private static int Scorecompare(Score a, Score b)
        {
            if (a.score > b.score)
            {
                return 1;
            }
            else if (a.score == b.score) { return 0; }
            else return -1;
        }
        public static void ReadScore(string file)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                BinaryReader reader = new BinaryReader(fs);
                int version = reader.ReadInt32();
                int count = reader.ReadInt32();
                int stash = 0;
                string stashs = "";
                for (int i = 0; i < count; i++)
                {
                    string songmd5 = reader.ReadString();
                    int scorecount = reader.ReadInt32();
                    List<Score> Nscore = new List<Score>();
                    for (int j = 0; j < scorecount; j++)
                    {
                        Score Tscore = new Score();
                        Tscore.mode = (modes)reader.ReadByte();
                        stash = reader.ReadInt32();
                        stashs = reader.ReadString();
                        Tscore.player = reader.ReadString();
                        stashs = reader.ReadString();
                        Tscore.hit300 = reader.ReadInt16();
                        Tscore.hit100 = reader.ReadInt16();
                        Tscore.hit50 = reader.ReadInt16();
                        Tscore.hit320 = reader.ReadInt16();
                        Tscore.hit200 = reader.ReadInt16();
                        Tscore.miss = reader.ReadInt16();
                        Tscore.score = reader.ReadInt32();
                        Tscore.maxCombo = reader.ReadInt16();
                        reader.ReadBoolean();//isperfect
                        Tscore.mod = modconverter(reader.ReadUInt32() + reader.ReadByte() << 32);
                        Tscore.time = new DateTime(reader.ReadInt64());
                        stash = reader.ReadInt32();
                        stash = reader.ReadInt32();
                        Tscore.acc = getacc(Tscore);
                        Tscore.totalhit = Tscore.hit300 + Tscore.hit100 + Tscore.hit50 + Tscore.miss;
                        Nscore.Add(Tscore);
                    }
                    Nscore.Sort(Scorecompare);
                    Core.Scores.Add(songmd5, Nscore);
                }
            }
        }
        public static void ReadDb(string file)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                BinaryReader reader = new BinaryReader(fs);
                int dbversion = reader.ReadInt32();
                int stash = reader.ReadInt32(); //folders
                stash = reader.ReadInt32();
                stash = reader.ReadInt32();
                stash = reader.ReadByte();
                string stashs = reader.ReadString(); //player
                bool stashb = false;
                int mapcount = reader.ReadInt32();
                Beatmap tmpbm = new Beatmap();
                BeatmapSet tmpset = new BeatmapSet();
                for (int i = 0; i < mapcount; i++)
                {
                    tmpbm.ArtistRomanized = reader.ReadString();
                    stashs = reader.ReadString();
                    if (stashs != "") { tmpbm.Artist = stashs; }
                    tmpbm.TitleRomanized = reader.ReadString();
                    stashs = reader.ReadString();
                    if (stashs != "") { tmpbm.Title = stashs; }
                    tmpbm.Creator = reader.ReadString();
                    tmpbm.Version = reader.ReadString();
                    tmpbm.Audio = reader.ReadString();
                    tmpbm.hash = reader.ReadString();
                    tmpbm.Name = reader.ReadString();
                    stash = reader.ReadByte(); //4=ranked 5=app 2=Unranked
                    stash = reader.ReadUInt16(); //circles
                    stash = reader.ReadUInt16(); //sliders
                    stash = reader.ReadUInt16(); //spinners
                    Int64 stashB = reader.ReadInt64(); //最后编辑
                    stash = reader.ReadByte(); //AR
                    stash = reader.ReadByte(); //CS
                    stash = reader.ReadByte(); //HP
                    stash = reader.ReadByte(); //OD
                    double stashD = reader.ReadDouble(); //SV
                    stash = reader.ReadInt32(); //playtime
                    stash = reader.ReadInt32(); //totaltime
                    stash = reader.ReadInt32(); //preview
                    stash = reader.ReadInt32(); //timting points 数
                    for (int j = 0; j < stash; j++)
                    {
                        stashD = reader.ReadDouble(); //bpm
                        stashD = reader.ReadDouble(); //offset
                        stashb = reader.ReadBoolean();//红线
                    }
                    tmpbm.beatmapId = reader.ReadInt32();
                    tmpbm.beatmapsetId = reader.ReadInt32();
                    stash = reader.ReadInt32(); //threadid 
                    stash = reader.ReadByte();//Ranking osu
                    stash = reader.ReadByte();//Ranking taiko
                    stash = reader.ReadByte();//Ranking ctb
                    stash = reader.ReadByte();//Ranking mania
                    tmpbm.offset = reader.ReadInt16();
                    stashD = reader.ReadSingle();  //stack
                    tmpbm.mode = reader.ReadByte();
                    tmpbm.Source = reader.ReadString();
                    tmpbm.tags = reader.ReadString();
                    stash = reader.ReadInt16();//online-offset
                    if (tmpbm.offset == 0 && stash != 0) { tmpbm.offset = stash; }
                    stashs = reader.ReadString(); //title-font
                    stashb = reader.ReadBoolean(); //unplayed
                    stashB = reader.ReadInt64(); //last_play
                    stashb = reader.ReadBoolean(); //osz2
                    tmpbm.Location = Path.Combine(Core.osupath, reader.ReadString());
                    stashB = reader.ReadInt64(); //最后同步
                    stashb = reader.ReadBoolean(); //忽略音效
                    stashb = reader.ReadBoolean(); //忽略皮肤
                    stashb = reader.ReadBoolean(); //禁用sb
                    stash = reader.ReadByte(); //背景淡化
                    stashB = reader.ReadInt64();
                    if (tmpset.count == 0) { tmpset.add(tmpbm); }
                    else
                    {
                        if (tmpset.Contains(tmpbm)) { tmpset.add(tmpbm); }
                        else
                        {
                            Core.allsets.Add(tmpset);
                            tmpset = new BeatmapSet();
                            tmpset.add(tmpbm);
                        }
                    }
                    tmpbm = new Beatmap();
                }
            }
        }
        public static void ReadCollect(string file)
        {
            Core.Collections.Clear();
            using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                BinaryReader reader = new BinaryReader(fs);
                int version = reader.ReadInt32();
                int count = reader.ReadInt32();
                string md5 = "";
                for (int i = 0; i < count; i++)
                {
                    string title = reader.ReadString();
                    int itemcount = reader.ReadInt32();
                    List<int> Nset = new List<int>();
                    for (int j = 0; j < itemcount; j++)
                    {
                        md5 = reader.ReadString();
                        for (int k = 0; k < Core.allsets.Count; k++)
                        {
                            if (Core.allsets[k].md5.Contains(md5) && (!Nset.Contains(k))) { Nset.Add(k); }
                        }
                    }
                    Core.Collections.Add(title, Nset);
                }
            }
        }
        public static double getacc(Score S)
        {
            switch (S.mode)
            {
                case modes.Osu:
                    return (S.hit300 * 6 + S.hit100 * 2 + S.hit50)
                        / (double)((S.hit300 + S.hit100 + S.hit50 + S.miss) * 6);
                case modes.Taiko:
                    return (S.hit300 * 2 + S.hit100)
                        / (double)((S.hit300 + S.hit100 + S.miss) * 2);
                case modes.CTB:
                    return (S.hit300 + S.hit100 + S.hit50)
                        / (double)(S.hit300 + S.hit100 + S.hit50 + S.hit200 + S.miss);
                case modes.Mania:
                    return (S.hit300 * 6 + S.hit320 * 6 + S.hit200 * 4 + S.hit100 * 2 + S.hit50)
                        / (double)((S.hit300 + S.hit320 + S.hit200 + S.hit100 + S.hit50 + S.miss) * 6);
                default:
                    return 0;
            }
        }
        public static string modconverter(long mod)
        {
            string cmod = "";
            if (mod == 0) { cmod = "None"; }
            else
            {
                for (int i = 0; i < (int)mods.Random; i++)
                {
                    if ((mod & 1) == 1)
                    {
                        cmod += " " + Enum.GetName(typeof(mods), i);
                    }
                    mod = mod >> 1;
                }
            }
            return cmod;
        }
    }
}
