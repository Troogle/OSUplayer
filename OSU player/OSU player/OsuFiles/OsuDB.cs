using OSUplayer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OSUplayer.OsuFiles
{
    public class ScoreRecord
    {
        public int Version;
        public int Modint;
        public string DiffHash;
        public string DiffName;
        public string ReplayHash;
        public bool IsPerfect;
        public int Hit100;
        public int Hit200;
        public int Hit300;
        public int Hit320;
        public int Hit50;
        public int MaxCombo;
        public int Miss;
        public Modes Mode;
        public string Player;
        public int Score;
        public DateTime Time;
        public int Totalhit;
        public double Acc
        {
            get
            {
                switch (Mode)
                {
                    case Modes.Osu:
                        return (Hit300 * 6 + Hit100 * 2 + Hit50)
                               / (double)((Hit300 + Hit100 + Hit50 + Miss) * 6);
                    case Modes.Taiko:
                        return (Hit300 * 2 + Hit100)
                               / (double)((Hit300 + Hit100 + Miss) * 2);
                    case Modes.CTB:
                        return (Hit300 + Hit100 + Hit50)
                               / (double)(Hit300 + Hit100 + Hit50 + Hit200 + Miss);
                    case Modes.Mania:
                        return (Hit300 * 6 + Hit320 * 6 + Hit200 * 4 + Hit100 * 2 + Hit50)
                               / (double)((Hit300 + Hit320 + Hit200 + Hit100 + Hit50 + Miss) * 6);
                    default:
                        return 0;
                }
            }
        }
        public string Mod
        {
            get
            {
                if (Modint == 0)
                {
                    return "None";
                }
                var cmod = "";
                for (int i = 0; i < (int)Mods.LastMod; i++)
                {
                    if ((Modint & 1) == 1)
                    {
                        cmod += " " + Enum.GetName(typeof(Mods), i);
                    }
                    Modint = Modint >> 1;
                }
                return cmod;
            }
        }
        private Image RankExtracted(bool sContition, bool aContition, bool bContition, bool cContition)
        {
            if (Acc == 1)
            {
                if (Mod.Contains("HD") || Mod.Contains("FL"))
                {
                    return Resources.XH;
                }
                return Resources.X;
            }
            if (sContition)
            {
                if (Mod.Contains("HD") || Mod.Contains("FL"))
                {
                    return Resources.SH;
                }
                return Resources.S;
            }
            if (aContition)
            {
                return Resources.A;
            }
            if (bContition)
            {
                return Resources.B;
            }
            if (cContition)
            {
                return Resources.C;
            }
            return Resources.D;
        }
        public Image Rank
        {
            get
            {
                switch (Mode)
                {
                    case Modes.Osu:
                    case Modes.Taiko:
                        return RankExtracted(
                            ((Hit300) / (double)Totalhit > 0.9 && Hit50 / (double)Totalhit < 0.01 && Miss == 0),
                            (((Hit300) / (double)Totalhit > 0.9) || ((Hit300) / (double)Totalhit > 0.8 && Miss == 0)),
                            (((Hit300) / (double)Totalhit > 0.8) || ((Hit300) / (double)Totalhit > 0.7 && Miss == 0)),
                            ((Hit300) / (double)Totalhit > 0.6));
                    case Modes.CTB:
                        return RankExtracted(Acc >= 0.9801, Acc >= 0.9401, Acc >= 0.9001, Acc >= 0.8501);
                    case Modes.Mania:
                        return RankExtracted(Acc > 0.95, Acc > 0.90, Acc > 0.80, Acc > 0.70);
                    default:
                        return Resources.D;
                }
            }
        }
        public String Details
        {
            get
            {
                return String.Format(
                    "Player:{0},Date:{1},Score: {2},Diff:{3},Mods:{4},Mode:{5},300:{6},100:{7},50:{8},Miss:{9},Maxcombo:{10}",
                    Player, Time, Score, DiffName,
                    Mod, Mode,
                    Hit300, Hit100, Hit50, Miss, MaxCombo);
            }
        }
    }

    public class BinaryReader : System.IO.BinaryReader
    {
        public BinaryReader(Stream stream)
            : base(stream)
        { }

        public override string ReadString()
        {
            return ReadByte() == 0x0b ? base.ReadString() : "";
        }

        public DateTime ReadDateTime()//method_2
        {
            return new DateTime(ReadInt64());
        }

        public byte[] ReadBytes()//method_0
        {
            var count = ReadInt32();
            if (count > 0)
            {
                return ReadBytes(count);
            }
            return count < 0 ? null : new byte[0];
        }

        private char[] ReadChars()//method_1
        {
            var count = ReadInt32();
            if (count > 0)
            {
                return ReadChars(count);
            }
            return count < 0 ? null : new char[0];
        }
        public object ReadVar()
        {
            switch (ReadByte())
            {
                case 1:
                    return ReadBoolean();
                case 2:
                    return ReadByte();
                case 3:
                    return ReadUInt16();
                case 4:
                    return ReadUInt32();
                case 5:
                    return ReadUInt64();
                case 6:
                    return ReadSByte();
                case 7:
                    return ReadInt16();
                case 8:
                    return ReadInt32();
                case 9:
                    return ReadInt64();
                case 10:
                    return ReadChar();
                case 11:
                    return base.ReadString();
                case 12:
                    return ReadSingle();
                case 13:
                    return ReadDouble();
                case 14:
                    return ReadDecimal();
                case 15:
                    return ReadDateTime();
                case 0x10:
                    return ReadBytes();
                case 0x11:
                    return ReadChars();
                case 0x12:
                    return Deserializer.Deserialize(BaseStream);
            }
            return null;
        }
    }

    internal static class OsuDB
    {
        private static int Scorecompare(ScoreRecord a, ScoreRecord b)
        {
            if (a.Score > b.Score)
            {
                return 1;
            }
            if (a.Score == b.Score)
            {
                return 0;
            }
            return -1;
        }
        public static void ReadScore(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var reader = new BinaryReader(fs);
                reader.ReadInt32(); //version
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string songmd5 = reader.ReadString();
                    int scorecount = reader.ReadInt32();
                    var nscore = new List<ScoreRecord>();
                    for (int j = 0; j < scorecount; j++)
                    {
                        var tscore = new ScoreRecord
                        {
                            Mode = (Modes)reader.ReadByte(),
                            Version = reader.ReadInt32(),
                            DiffHash = reader.ReadString(),
                            Player = reader.ReadString(),
                            ReplayHash = reader.ReadString(),
                            Hit300 = reader.ReadUInt16(),
                            Hit100 = reader.ReadUInt16(),
                            Hit50 = reader.ReadUInt16(),
                            Hit320 = reader.ReadUInt16(),
                            Hit200 = reader.ReadUInt16(),
                            Miss = reader.ReadUInt16(),
                            Score = reader.ReadInt32(),
                            MaxCombo = reader.ReadUInt16(),
                            IsPerfect = reader.ReadBoolean(),
                            Modint = reader.ReadInt32()
                        };
                        reader.ReadString();//??? 均为空
                        // Debug.Assert(tmp == "");
                        tscore.Time = reader.ReadDateTime();
                        reader.ReadBytes();//??? 均为null
                        //  Debug.Assert(tmp1 == null);
                        if (tscore.Version >= 20140721)
                        {
                            reader.ReadInt64();//Online ID
                        }
                        else
                        {
                            reader.ReadInt32();
                        }
                        tscore.Totalhit = tscore.Hit300 + tscore.Hit100 + tscore.Hit50 + tscore.Miss;
                        nscore.Add(tscore);
                    }
                    nscore.Sort(Scorecompare);
                    Core.Scores.Add(songmd5, nscore);
                }
            }
        }
        public static void ReadDb(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var reader = new BinaryReader(fs);
                var version = reader.ReadInt32();  //version
                reader.ReadInt32(); //folders
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadByte();
                reader.ReadString(); //player
                int mapcount = reader.ReadInt32();
                var tmpbm = new Beatmap();
                var tmpset = new BeatmapSet();
                for (int i = 0; i < mapcount; i++)
                {
                    tmpbm.ArtistRomanized = reader.ReadString();
                    string stashs = reader.ReadString();
                    if (stashs != "")
                    {
                        tmpbm.Artist = stashs;
                    }
                    tmpbm.TitleRomanized = reader.ReadString();
                    stashs = reader.ReadString();
                    if (stashs != "")
                    {
                        tmpbm.Title = stashs;
                    }
                    tmpbm.Creator = reader.ReadString();
                    tmpbm.Version = reader.ReadString();
                    tmpbm.Audio = reader.ReadString();
                    tmpbm.Hash = reader.ReadString();
                    tmpbm.Name = reader.ReadString();
                    reader.ReadByte(); //4=ranked 5=app 2=Unranked
                    tmpbm.Totalhitcount = reader.ReadUInt16(); //circles
                    tmpbm.Totalhitcount += reader.ReadUInt16(); //sliders
                    tmpbm.Totalhitcount += reader.ReadUInt16(); //spinners
                    reader.ReadInt64(); //最后编辑
                    if (version >= 20140612)
                    {
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                    }
                    else
                    {
                        reader.ReadByte(); //AR
                        reader.ReadByte(); //CS
                        reader.ReadByte(); //HP
                        reader.ReadByte(); //OD
                    }
                    reader.ReadDouble(); //SV
                    if (version >= 20140609)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            var num = reader.ReadInt32();
                            if (num >= 0)
                            {
                                for (int xxoo = 0; xxoo < num; xxoo++)
                                {
                                    reader.ReadVar();
                                    reader.ReadVar();
                                }
                            }
                        }
                    }
                    reader.ReadInt32(); //playtime
                    reader.ReadInt32(); //totaltime
                    reader.ReadInt32(); //preview
                    int stash = reader.ReadInt32();
                    for (int j = 0; j < stash; j++)
                    {
                        reader.ReadDouble(); //bpm
                        reader.ReadDouble(); //offset
                        reader.ReadBoolean(); //红线
                    }
                    tmpbm.BeatmapID = reader.ReadInt32();
                    tmpbm.BeatmapsetID = reader.ReadInt32();
                    reader.ReadInt32(); //threadid 
                    reader.ReadByte(); //Ranking osu
                    reader.ReadByte(); //Ranking taiko
                    reader.ReadByte(); //Ranking ctb
                    reader.ReadByte(); //Ranking mania
                    tmpbm.Offset = reader.ReadInt16();
                    reader.ReadSingle(); //stack
                    tmpbm.Mode = (Modes)reader.ReadByte();
                    tmpbm.Source = reader.ReadString();
                    tmpbm.Tags = reader.ReadString();
                    stash = reader.ReadInt16(); //online-offset
                    if (tmpbm.Offset == 0 && stash != 0)
                    {
                        tmpbm.Offset = stash;
                    }
                    reader.ReadString(); //title-font
                    reader.ReadBoolean(); //unplayed
                    reader.ReadInt64(); //last_play
                    reader.ReadBoolean(); //osz2
                    tmpbm.Location = reader.ReadString();
                    reader.ReadInt64(); //最后同步
                    reader.ReadBoolean(); //忽略音效
                    reader.ReadBoolean(); //忽略皮肤
                    reader.ReadBoolean(); //禁用sb
                    reader.ReadBoolean(); //禁用视频
                    reader.ReadBoolean();//VisualSettingsOverride
                    if (version < 20140608)
                    {
                        reader.ReadInt16();//背景淡化
                    }
                    reader.ReadInt32();//LastEditTime
                    reader.ReadByte();//ManiaSpeed
                    if (tmpset.count == 0)
                    {
                        tmpset.Add(tmpbm);
                    }
                    else
                    {
                        if (tmpset.Contains(tmpbm))
                        {
                            tmpset.Add(tmpbm);
                        }
                        else
                        {
                            if (!Core.Allsets.ContainsKey(tmpset.GetHash())) Core.Allsets.Add(tmpset.GetHash(), tmpset);
                            else
                            {
                                Core.Allsets[tmpset.GetHash()]= tmpset;
                            }                      
                            tmpset = new BeatmapSet();
                            tmpset.Add(tmpbm);
                        }
                    }
                    tmpbm = new Beatmap();
                }
            }
        }
        public static void ReadCollect(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var reader = new BinaryReader(fs);
                reader.ReadInt32(); //version
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string title = reader.ReadString();
                    int itemcount = reader.ReadInt32();
                    var nset = new List<string>();
                    for (int j = 0; j < itemcount; j++)
                    {
                        string md5 = reader.ReadString();
                        foreach (var beatmapSet in Core.Allsets)
                        {
                            if (beatmapSet.Value.Diffs.ContainsKey(md5) && (!nset.Contains(beatmapSet.Key)))
                            {
                                nset.Add(beatmapSet.Key);
                            }
                        }
                    }
                    Core.Collections.Add(title, nset);
                }
            }
        }
    }
}