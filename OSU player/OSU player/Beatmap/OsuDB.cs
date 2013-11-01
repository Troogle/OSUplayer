using System;
using System.Collections.Generic;
using System.Text;

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
                byte[] array = ReadBytes(len);
                str = Encoding.UTF8.GetString(array, 0, array.Length);
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
                        Score Tscore = new Score() ;
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
        }
        public static void ReadCollect(string file)
        {
        }
    }
}
