using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OSUplayer.Graphic;

namespace OSUplayer.OsuFiles.StoryBoard
{/*
    public struct SBEvent
    {
        public EventType Type;
        public int Easing;
        //0 - none【没有缓冲】
        //1 - start fast and slow down【开始快结束慢】
        //2 - start slow and speed up【开始慢结束快】
        public int StartTime;
        public int EndTime;
        public float StartXf; //F,S,R(只用x),V 'F stands for float-option
        public float StartYf;
        public float EndYf;
        public float EndXf;
        public int StartX; //M,MX,MY（只用x/y)
        public int StartY;
        public int EndX;
        public int EndY;
        //P只用startx H - 水平翻转(0) V - 垂直翻转(1) A - additive-blend colour (2)
        public int R1; //C
        public int G1;
        public int B1;
        public int R2;
        public int G2;
        public int B2;
        public int Volume; //Play
    }
    public struct TriggerEvent
    {
        public int TriggerStart;
        public int TriggerEnd;
        public SBEvent[] Events;
        public int Count;
    }*/
    public class SBelement
    {
        public readonly List<TActionNode> F;
        public readonly List<TActionNode> X;
        public readonly List<TActionNode> Y;
        public readonly List<TActionNode> SX;
        public readonly List<TActionNode> SY;
        public readonly List<TActionNode> R;
        public readonly List<TActionNode> C;
        public readonly List<TActionNode> P;
        public ElementType Type;
        public ElementLayer Layers;
        public ElementOrigin Origin; //sample时无
        public int X0; //sample时无时无
        public int Y0;
        public int Z;//z轴，第一个元素为0，随后递增
        //Animation only
        public int FrameCount;
        public int Framedelay;
        public int Lasttime = Int32.MinValue;
        public int Starttime = Int32.MaxValue;
        public ElementLoopType Looptype; //默认LoopForever【一直循环】

        public SBelement()
        {
            F = new List<TActionNode>();
            X = new List<TActionNode>();
            Y = new List<TActionNode>();
            SX = new List<TActionNode>();
            SY = new List<TActionNode>();
            R = new List<TActionNode>();
            C = new List<TActionNode>();
            P = new List<TActionNode>();
        }

        //事件触发循环可以被游戏时间事件触发. 虽然叫做循环, 事件触发循环循环时只执行一次
        //触发器循环和普通循环一样是从零计数. 如果两个重叠, 第一个将会被停止且被一个从头开始的循环替代.
        //如果他们和任何存在的故事版事件重叠,他们将不会循环直到那些故事版事件不在生效
    }
    public class SBFile
    {
        public readonly List<SBelement> Element;
        public SBFile()
        {
            Element = new List<SBelement>();
        }
    }
    public class StoryBoard
    {
        public readonly Dictionary<string, SBFile> Elements = new Dictionary<string, SBFile>();
        //TODO:单独抽取trigger并作索引
        private readonly Dictionary<string, string> _variables = new Dictionary<string, string>();
        //public Dictionary<Triggertype, TriggerEvent> trigger = new Dictionary<Triggertype, TriggerEvent>();
        //目录由beatmapfiles.location-->beatmap.location
        private readonly string _location;
        private static string Picknext(ref string str, bool change = true)
        {
            string ret;
            if (!str.Contains(","))
            {
                ret = str;
                if (change) { str = ""; }
            }
            else
            {
                ret = str.Substring(0, str.IndexOf(",", StringComparison.Ordinal));
                if (change) { str = str.Substring(str.IndexOf(",", StringComparison.Ordinal) + 1); }
            }
            return ret;
        }
        private static int Color(int r, int g, int b)
        {
            return (b << 0x10) | (g << 8) | r;
        }
        private string Dealevents(ref SBelement element, StreamReader reader)
        {
            var row = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                // ReSharper disable PossibleNullReferenceException
                if (row.Trim() == "") { row = reader.ReadLine(); continue; }
                if (row.StartsWith("//") || row.Length == 0) { row = reader.ReadLine(); continue; }
                if (row.StartsWith("[")) { return (row); }
                if (!row.StartsWith(" ")) { return (row); }
                //do variables change first
                foreach (var replace in _variables)
                {
                    if (row.Contains(replace.Key))
                    {
                        row = row.Replace(replace.Key, replace.Value);
                    }
                }

                if (row.StartsWith(" L"))
                {
                    //④对于L的处理：直接复制_L,time difference,loopcount
                    //row = reader.ReadLine();
                    Picknext(ref row);
                    var delta = Convert.ToInt32(Picknext(ref row));
                    var loopcount = Convert.ToInt32(Picknext(ref row));
                    row = reader.ReadLine();
                    //delta: 循环开始的时间和此系列SB事件第一次生效的最初时间之间的时间差, 单位是毫秒
                    while (!reader.EndOfStream && row.StartsWith("  "))
                    {
                        for (var i = 1; i <= loopcount; i++)
                        {
                            Dealevent((row.Substring(1)), ref element, i * delta);
                        }
                        row = reader.ReadLine();
                    }
                    return (row);
                }
                if (row.StartsWith(" T"))
                {
                    row = reader.ReadLine();
                    while (!reader.EndOfStream && row.StartsWith("  "))
                    {
                        //For i As Integer = 0 To tmpe.startT
                        //    dealevent(raw(currentrow).Substring(1), element, i * tmpe.easing, currentrow)
                        //    currentrow -= 1
                        //Next
                        row = reader.ReadLine();
                    }
                    return (row);
                }
                Dealevent(row, ref element, 0);
                row = reader.ReadLine();
            }
            return ("");
        }
        private void Dealevent(string str, ref SBelement element, int delta)
        {
            var tmp = str.Split(new[] { ',' });
            var type = (EventType)Enum.Parse(typeof(EventType), tmp[0].Trim());
            var easing = Convert.ToInt32(tmp[1]);
            var startT = Convert.ToInt32(tmp[2]) + delta;
            int endT;
            //②_M,0,1000,1000,320,240,320,240-->_M,0,1000,,320,240,320,240(开始结束时间相同）
            if (tmp[3] == "") { endT = startT; } else { endT = Convert.ToInt32(tmp[3]) + delta; }
            if (type == EventType.P)
            {
                switch (tmp[4])
                {
                    case "H":
                        element.P.Add(new TActionNode(startT, 1, 3));
                        element.P.Add(new TActionNode(endT, 1, 4));
                        break;
                    case "V":
                        element.P.Add(new TActionNode(startT, 2, 3));
                        element.P.Add(new TActionNode(endT, 2, 4));
                        break;
                    case "A":
                        element.P.Add(new TActionNode(startT, 4, 3));
                        element.P.Add(new TActionNode(endT, 4, 4));
                        break;
                }
                return;
            }
            int point = 4;
            int time = startT;
            delta = endT - startT;
            if (element.Starttime > time) { element.Starttime = time; }
            if (element.Lasttime < endT) { element.Lasttime = endT; }
            while (point < tmp.Length)
            {
                switch (type)
                {
                    case EventType.F:
                        //③_M,0,1000,,320,240,320,240-->_M,0,1000,,320,240 (开始结束值相同）
                        if (point != tmp.Length - 1)
                        {
                            element.F.Add(new TActionNode(time, Convert.ToSingle(tmp[point]) * 255, easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.F.Add(new TActionNode(time, Convert.ToSingle(tmp[point]) * 255, 4));
                            if (time < endT) { element.F.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]) * 255, 4)); }
                            return;
                        }
                        break;
                    case EventType.MX:
                        if (point != tmp.Length - 1)
                        {
                            element.X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { element.X.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.MY:
                        if (point != tmp.Length - 1)
                        {
                            element.Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { element.Y.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.M:
                        if (point != tmp.Length - 2)
                        {
                            element.X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            element.Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { element.X.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            point++;
                            element.Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { element.Y.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.S:
                        if (point != tmp.Length - 1)
                        {
                            element.SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            element.SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            element.SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT)
                            {
                                element.SX.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4));
                                element.SY.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4));
                            }
                            return;
                        }
                        break;
                    case EventType.V:
                        if (point != tmp.Length - 2)
                        {
                            element.SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            element.SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { element.SX.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            point++;
                            element.SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { element.SY.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.R:
                        if (point != tmp.Length - 1)
                        {
                            element.R.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            element.R.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { element.R.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.C:
                        if (point != tmp.Length - 3)
                        {
                            element.C.Add(new TActionNode(
                                time,
                                Color(Convert.ToInt32(tmp[point]),
                                      Convert.ToInt32(tmp[point + 1]),
                                      Convert.ToInt32(tmp[point + 2])),
                                easing));
                            point += 3;
                            time += delta;
                        }
                        else
                        {
                            element.C.Add(new TActionNode(
                                time,
                                Color(Convert.ToInt32(tmp[point]),
                                      Convert.ToInt32(tmp[point + 1]),
                                      Convert.ToInt32(tmp[point + 2])),
                                4));
                            if (time < endT)
                            {
                                element.C.Add(new TActionNode(
                                    endT,
                                    Color(Convert.ToInt32(tmp[point]),
                                    Convert.ToInt32(tmp[point + 1]),
                                    Convert.ToInt32(tmp[point + 2])),
                                    4));
                            }
                            return;
                        }
                        break;
                    //throw (new FormatException("Failed to read .osb file"));
                }
                //_event,easing,starttime,endtime,val1,val2,val3,...,valN
                if (element.Lasttime < time) { element.Lasttime = time; }
            }
        }
        private void Dealfile(StreamReader reader, ref int element)
        {
            var position = "";
            var row = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (row == null) throw new Exception();
                if (row.Trim() == "") { row = reader.ReadLine(); continue; }
                if (row.StartsWith("//") || row.Length == 0) { row = reader.ReadLine(); continue; }
                if (row.StartsWith("["))
                {
                    position = row.Substring(1, row.Length - 2);
                    row = reader.ReadLine(); continue;
                }
                switch (position)
                {
                    case "Variables":
                        {
                            var name = row.Split(new[] { '=' }, 2)[0];
                            var replace = row.Split(new[] { '=' }, 2)[1];
                            name = name.Substring(1, name.Length - 1);
                            _variables.Add(name, replace);
                            row = reader.ReadLine();
                            break;
                        }
                    case "Events":
                        //do variables change first
                        foreach (var replace in _variables)
                        {
                            if (row.Contains(replace.Key))
                            {
                                row = row.Replace(replace.Key, replace.Value);
                            }
                        }
                        if (row.StartsWith("Sample") || row.StartsWith("5,"))
                        {
                            //Sample,time,layer,"filepath",volume
                            //var tmp = row.Split(new[] { ',' });
                            //var path = tmp[3].Substring(1, tmp[3].Length - 2);
                            //element++;
                            /*var tmpe = new SBelement
                            {
                                Type = ElementType.Sample,
                                Layers = (ElementLayer)(Enum.Parse(typeof(ElementLayer), tmp[2])),
                            };
                            var tmpev = new SBEvent
                            {
                                startT = Convert.ToInt32(tmp[1]),
                                Type = EventType.Play,
                                volume = tmp.Length < 5 ? 100 : Convert.ToInt32(tmp[4])
                            };
                            if (Elements.ContainsKey(path))
                            {
                                //tmpe = Elements[path];
                            }
                            //else
                            //{ 
                                //Elements.Add(path, tmpe);
                           //}*/
                            row = reader.ReadLine();
                            //  elements[element].events.Add(tmpev);
                        }
                        else if (row.StartsWith("Animation") || row.StartsWith("6,"))
                        {
                            //Animation,"layer","origin","filepath",x,y,frameCount,frameDelay,looptype
                            var tmp = row.Split(new[] { ',' });
                            var path = tmp[3].Substring(1, tmp[3].Length - 2);
                            var tmpe = new SBelement
                            {
                                Type = ElementType.Animation,
                                Layers = (ElementLayer)(Enum.Parse(typeof(ElementLayer), tmp[1])),
                                Origin = (ElementOrigin)(Enum.Parse(typeof(ElementOrigin), tmp[2])),
                                X0 = Convert.ToInt32(tmp[4]),
                                Y0 = Convert.ToInt32(tmp[5]),
                                Z = element,
                                FrameCount = Convert.ToInt32(tmp[6]),
                                Framedelay = (int)(Convert.ToDouble(tmp[7])),
                                Looptype = (ElementLoopType)(Enum.Parse(typeof(ElementLoopType), tmp[8]))
                            };
                            if (tmpe.Layers == ElementLayer.Fail) { row = reader.ReadLine(); continue; }
                            if (!Elements.ContainsKey(path)) Elements.Add(path, new SBFile());
                            row = Dealevents(ref tmpe, reader);
                            if (tmpe.F.Count != 0 && tmpe.F[0].Value == 0)
                            { }
                            else
                            {
                                tmpe.F.Insert(0, new TActionNode(tmpe.Starttime - 1, 255f, 4));
                            }
                            tmpe.F.Add(new TActionNode(tmpe.Lasttime + 1, 0f, 4));
                            Elements[path].Element.Add(tmpe);
                            element++;
                        }
                        else if (row.StartsWith("Sprite") || row.StartsWith("4,"))
                        {
                            //Sprite,"layer","origin","filepath",x,y
                            var tmp = row.Split(new[] { ',' });
                            var path = tmp[3].Substring(1, tmp[3].Length - 2);
                            var tmpe = new SBelement
                            {
                                Type = ElementType.Sprite,
                                Layers = (ElementLayer)(Enum.Parse(typeof(ElementLayer), tmp[1])),
                                Origin = (ElementOrigin)(Enum.Parse(typeof(ElementOrigin), tmp[2])),
                                X0 = Convert.ToInt32(tmp[4]),
                                Y0 = Convert.ToInt32(tmp[5]),
                                Z = element
                            };
                            if (tmpe.Layers == ElementLayer.Fail) { row = reader.ReadLine(); continue; }
                            if (File.Exists(Path.Combine(_location, path)))
                            {
                                if (!Elements.ContainsKey(path)) Elements.Add(path, new SBFile());
                                row = Dealevents(ref tmpe, reader);
                                if (tmpe.F.Count != 0 && tmpe.F[0].Value == 0)
                                { }
                                else
                                {
                                    tmpe.F.Insert(0, new TActionNode(tmpe.Starttime - 1, 255f, 4));
                                }
                                tmpe.F.Add(new TActionNode(tmpe.Lasttime + 1, 0f, 4));
                                Elements[path].Element.Add(tmpe);
                                element++;
                            }
                            else { row = reader.ReadLine(); }
                        }
                        else if (row.StartsWith("0,"))
                        {
                            var tmp = row.Split(new[] { ',' });
                            var path = tmp[2].Substring(1, tmp[2].Length - 2);
                            var tmpe = new SBelement
                            {
                                Type = ElementType.Sprite,
                                Layers = ElementLayer.Background,
                                Origin = ElementOrigin.TopLeft,
                            };
                            if (File.Exists(Path.Combine(_location, path)))
                            {
                                if (!Elements.ContainsKey(path)) Elements.Add(path, new SBFile());
                                var tmpbm = new Bitmap(Path.Combine(_location, path));
                                var bgScale = Math.Min(640f / tmpbm.Width, 480f / tmpbm.Height);
                                row = Dealevents(ref tmpe, reader);
                                tmpe.F.Insert(0, new TActionNode(0, 255f, 4));
                                tmpe.SX.Insert(0, new TActionNode(0, bgScale, 4));
                                tmpe.SY.Insert(0, new TActionNode(0, bgScale, 4));
                                Elements[path].Element.Add(tmpe);
                                element++;
                            }
                            else { row = reader.ReadLine(); }
                        }
                        else { row = reader.ReadLine(); }
                        break;
                    default:
                        row = reader.ReadLine();
                        break;
                }
            }
        }
        public StoryBoard(string osu, string osb, string location)
        {
            _location = location;
            try
            {
                var currentelement = 0;
                StreamReader reader;
                using (reader = new StreamReader(osu)) { Dealfile(reader, ref currentelement); }
                if (osb == null) return;
                using (reader = new StreamReader(osb)) { Dealfile(reader, ref currentelement); }
            }
            catch (Exception e)
            {
                throw (new FormatException("Failed to read .osb file", e));
            }

        }
    }
}
