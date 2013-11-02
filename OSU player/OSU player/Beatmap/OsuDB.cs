using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OSU_player
{
    public class BinaryReader : System.IO.BinaryReader
    {
        public BinaryReader(System.IO.Stream stream) : base(stream) { }
        public override string ReadString()
        {
            string str = "";
            if (ReadByte() == 0x0b)
            {
                int len = Read7BitEncodedInt();
                byte[] bytes = ReadBytes(len);
                str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            return str;
        }
    }
    class OsuDB
    {

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
                        stash = reader.ReadByte();
                        stash = reader.ReadInt32();
                        stashs = reader.ReadString();
                        Tscore.player = reader.ReadString();
                        stashs = reader.ReadString();
                        Tscore.hit300 = reader.ReadInt16();
                        Tscore.hit100 = reader.ReadInt16();
                        Tscore.hit50 = reader.ReadInt16();
                        stash = reader.ReadInt16();
                        stash = reader.ReadInt16();
                        Tscore.miss = reader.ReadInt16();
                        Tscore.score = reader.ReadInt32();
                        Tscore.maxCombo = reader.ReadInt16();
                        Tscore.mod = Core.modconverter(reader.ReadInt32());
                        stash = reader.ReadInt16();
                        Tscore.time = new DateTime(reader.ReadInt64());
                        stash = reader.ReadInt32();
                        stash = reader.ReadInt32();
                        Nscore.Add(Tscore);
                    }
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
                    if (stashs != "") { tmpbm.Artist=stashs; }
                    tmpbm.TitleRomanized= reader.ReadString();
                    stashs = reader.ReadString();
                    if (stashs != "") { tmpbm.Title = stashs; }
                    tmpbm.Creator = reader.ReadString();
                    tmpbm.Version = reader.ReadString();
                    tmpbm.Audio = reader.ReadString();
                    tmpbm.hash = reader.ReadString();
                    tmpbm.Name= reader.ReadString();
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
                    tmpbm.offset = reader.ReadUInt16();
                    stashD = reader.ReadSingle();  //stack
                    stash = reader.ReadByte(); //mode
                    tmpbm.Source = reader.ReadString();
                    tmpbm.tags = reader.ReadString();
                    stash = reader.ReadInt16();
                    if (tmpbm.offset == 0 && stash != 0) { tmpbm.offset = stash; }
                    stashs = reader.ReadString(); //title-font
                    stashb = reader.ReadBoolean(); //unplayed
                    stashB = reader.ReadInt64(); //最后玩
                    stashb = reader.ReadBoolean(); //osz2
                    tmpbm.Location = Path.Combine(Core.osupath,reader.ReadString());
                    stashB = reader.ReadInt64(); //最后同步
                    stashb = reader.ReadBoolean(); //忽略音效
                    stashb = reader.ReadBoolean(); //忽略皮肤
                    stashb = reader.ReadBoolean(); //禁用sb
                    stash = reader.ReadByte(); //背景淡化
                    stashB = reader.ReadInt64();
                    if (tmpset.count == 0) { tmpset.add(tmpbm);}
                    if (tmpset.Contains(tmpbm)) { tmpset.add(tmpbm); }
                    else
                    {
                        Core.allsets.Add(tmpset);
                        tmpset = new BeatmapSet();
                        tmpset.add(tmpbm);
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
                        for (int k=0;k<Core.allsets.Count;k++)
                        {
                            if (Core.allsets[k].md5.Contains(md5) && (!Nset.Contains(k))) { Nset.Add(k); }
                        }
                    }
                    Core.Collections.Add(title, Nset);
                }
            }
        }
    }
}
