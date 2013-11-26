using System;
using System.Collections.Generic;
using System.IO;
using OSUplayer.Graphic;
using System.Drawing;
namespace OSUplayer.OsuFiles.StoryBoard
{
    public enum ElementType
    {
        Background,
        Video,
        Break,
        Colour,
        Sprite,
        Sample,
        Animation
    }
    public enum ElementLayer
    {
        Background,
        Fail,
        Pass,
        Foreground
    }
    public enum ElementOrigin
    {
        TopLeft,
        TopCentre,
        TopRight,
        CentreLeft,
        Centre,
        CentreRight,
        BottomLeft,
        BottomCentre,
        BottomRight
    }
    public enum ElementLoopType
    {
        LoopOnce,
        LoopForever
    }
    public enum EventType
    {
        //F - fade【隐藏(淡入淡出)】
        //M - move【移动】
        //S - scale【缩放】
        //V - vector scale (width and height separately)【矢量缩放(宽高分别变动)】
        //R - rotate【旋转】
        //C - colour【颜色】
        //L - loop【循环】
        //T - Event-triggered loop【事件触发循环】
        //P - Parameters【参数】
        //Play - 播放sample
        F,
        MX,
        MY,
        M,
        S,
        V,
        R,
        C,
        L,
        T,
        P,
        Play
    }
    public enum Triggertype
    {
        HitSoundClap,
        HitSoundFinish,
        HitSoundWhistle,
        Passing,
        Failing
    }
    public struct SBvar
    {
        public string name;
        public string replace;
    }
    public struct SBEvent
    {
        public EventType Type;
        public int easing;
        //0 - none【没有缓冲】
        //1 - start fast and slow down【开始快结束慢】
        //2 - start slow and speed up【开始慢结束快】
        public int startT;
        public int endT;
        public float startxF; //F,S,R(只用x),V 'F stands for float-option
        public float startyF;
        public float endyF;
        public float endxF;
        public int startx; //M,MX,MY（只用x/y)
        public int starty;
        public int endx;
        public int endy;
        //P只用startx H - 水平翻转(0) V - 垂直翻转(1) A - additive-blend colour (2)
        public int r1; //C
        public int g1;
        public int b1;
        public int r2;
        public int g2;
        public int b2;
        public int volume; //Play
    }
    public struct TriggerEvent
    {
        public int triggerstart;
        public int triggerend;
        public SBEvent[] events;
        public int count;
    }
    public class SBelement
    {
        public List<TActionNode> F = new List<TActionNode>();
        public List<TActionNode> X = new List<TActionNode>();
        public List<TActionNode> Y = new List<TActionNode>();
        public List<TActionNode> SX = new List<TActionNode>();
        public List<TActionNode> SY = new List<TActionNode>();
        public List<TActionNode> R = new List<TActionNode>();
        public List<TActionNode> C = new List<TActionNode>();
        public List<TActionNode> P = new List<TActionNode>();
        public ElementType Type;
        public ElementLayer Layers;
        public ElementOrigin Origin; //sample时无
        public string Path;
        public int x; //sample时无时无
        public int y;
        //Animation only
        public int FrameCount;
        public int Framedelay;
        public int Lasttime = Int32.MinValue;
        public int Starttime = Int32.MaxValue;
        public ElementLoopType Looptype; //默认LoopForever【一直循环】
        //事件触发循环可以被游戏时间事件触发. 虽然叫做循环, 事件触发循环循环时只执行一次
        //触发器循环和普通循环一样是从零计数. 如果两个重叠, 第一个将会被停止且被一个从头开始的循环替代.
        //如果他们和任何存在的故事版事件重叠,他们将不会循环直到那些故事版事件不在生效
    }
    public class StoryBoard
    {
        public List<SBelement> Elements = new List<SBelement>();
        //TODO:单独抽取trigger并作索引
        public List<SBvar> Variables = new List<SBvar>();
        //public Dictionary<Triggertype, TriggerEvent> trigger = new Dictionary<Triggertype, TriggerEvent>();
        //目录由beatmapfiles.location-->beatmap.location
        public string location;
        private string Picknext(ref string str, bool change = true)
        {
            string ret = "";
            if (!str.Contains(","))
            {
                ret = str;
                if (change) { str = ""; }
            }
            else
            {
                ret = str.Substring(0, str.IndexOf(","));
                if (change) { str = str.Substring(str.IndexOf(",") + 1); }
            }
            return ret;
        }
        private int Color(int r, int g, int b)
        {
            return (b << 0x10) | (g << 8) | r;
        }
        private string Dealevents(int element, StreamReader reader)
        {
            string row = "";
            row = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (row.Trim() == "") { row = reader.ReadLine(); continue; }
                if (row.StartsWith("//") || row.Length == 0) { row = reader.ReadLine(); continue; }
                if (row.StartsWith("[")) { return (row); }
                if (!row.StartsWith(" ")) { return (row); }
                //do variables change first
                foreach (SBvar tmpvar in Variables)
                {
                    if (row.Contains(tmpvar.name)) { row.Replace(tmpvar.name, tmpvar.replace); }
                }
                if (row.StartsWith(" L"))
                {
                    //④对于L的处理：直接复制_L,time difference,loopcount
                    //row = reader.ReadLine();
                    Picknext(ref row);
                    int delta = Convert.ToInt32(Picknext(ref row));
                    int loopcount = Convert.ToInt32(Picknext(ref row));
                    row = reader.ReadLine();
                    //delta: 循环开始的时间和此系列SB事件第一次生效的最初时间之间的时间差, 单位是毫秒
                    while (!reader.EndOfStream && row.StartsWith("  "))
                    {
                        for (int i = 1; i <= loopcount; i++)
                        {
                            dealevent((row.Substring(1)), element, i * delta);
                        }
                        row = reader.ReadLine();
                    }
                    return (row);
                }
                else if (row.StartsWith(" T"))
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
                else
                {
                    dealevent(row, element, 0);
                    row = reader.ReadLine();
                }
            }
            return ("");
        }
        private void dealevent(string str, int element, int delta)
        {
            string[] tmp = str.Split(new char[] { ',' });
            EventType type = (EventType)Enum.Parse(typeof(EventType), tmp[0].Trim());
            int easing = Convert.ToInt32(tmp[1]);
            int startT = Convert.ToInt32(tmp[2]) + delta;
            int endT;
            //②_M,0,1000,1000,320,240,320,240-->_M,0,1000,,320,240,320,240(开始结束时间相同）
            if (tmp[3] == "") { endT = startT; } else { endT = Convert.ToInt32(tmp[3]) + delta; }
            if (type == EventType.P)
            {
                switch (tmp[4])
                {
                    case "H":
                        Elements[element].P.Add(new TActionNode(startT, 1, 3));
                        Elements[element].P.Add(new TActionNode(endT, 1, 4));
                        break;
                    case "V":
                        Elements[element].P.Add(new TActionNode(startT, 2, 3));
                        Elements[element].P.Add(new TActionNode(endT, 2, 4));
                        break;
                    case "A":
                        Elements[element].P.Add(new TActionNode(startT, 4, 3));
                        Elements[element].P.Add(new TActionNode(endT, 4, 4));
                        break;
                }
                return;
            }
            int point = 4;
            int time = startT;
            delta = endT - startT;
            if (Elements[element].Starttime > time) { Elements[element].Starttime = time; }
            if (Elements[element].Lasttime < endT) { Elements[element].Lasttime = endT; }
            while (point < tmp.Length)
            {
                switch (type)
                {
                    case EventType.F:
                        //③_M,0,1000,,320,240,320,240-->_M,0,1000,,320,240 (开始结束值相同）
                        if (point != tmp.Length - 1)
                        {
                            Elements[element].F.Add(new TActionNode(time, Convert.ToSingle(tmp[point]) * 255, easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].F.Add(new TActionNode(time, Convert.ToSingle(tmp[point]) * 255, 4));
                            if (time < endT) { Elements[element].F.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]) * 255, 4)); }
                            return;
                        }
                        break;
                    case EventType.MX:
                        if (point != tmp.Length - 1)
                        {
                            Elements[element].X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { Elements[element].X.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.MY:
                        if (point != tmp.Length - 1)
                        {
                            Elements[element].Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { Elements[element].Y.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.M:
                        if (point != tmp.Length - 2)
                        {
                            Elements[element].X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            Elements[element].Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].X.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { Elements[element].X.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            point++;
                            Elements[element].Y.Add(new TActionNode(time, Convert.ToInt32(tmp[point]), 4));
                            if (time < endT) { Elements[element].Y.Add(new TActionNode(endT, Convert.ToInt32(tmp[point]), 4)); }
                            point++; return;
                        }
                        break;
                    case EventType.S:
                        if (point != tmp.Length - 1)
                        {
                            Elements[element].SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            Elements[element].SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            Elements[element].SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT)
                            {
                                Elements[element].SX.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4));
                                Elements[element].SY.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4));
                            }
                            point++;
                            return;
                        }
                        break;
                    case EventType.V:
                        if (point != tmp.Length - 2)
                        {
                            Elements[element].SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            Elements[element].SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].SX.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { Elements[element].SX.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            point++;
                            Elements[element].SY.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { Elements[element].SY.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            point++;
                            return;
                        }
                        break;
                    case EventType.R:
                        if (point != tmp.Length - 1)
                        {
                            Elements[element].R.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), easing));
                            point++;
                            time += delta;
                        }
                        else
                        {
                            Elements[element].R.Add(new TActionNode(time, Convert.ToSingle(tmp[point]), 4));
                            if (time < endT) { Elements[element].R.Add(new TActionNode(endT, Convert.ToSingle(tmp[point]), 4)); }
                            return;
                        }
                        break;
                    case EventType.C:
                        if (point != tmp.Length - 3)
                        {
                            Elements[element].C.Add(new TActionNode(
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
                            Elements[element].C.Add(new TActionNode(
                                time,
                                Color(Convert.ToInt32(tmp[point]),
                                      Convert.ToInt32(tmp[point + 1]),
                                      Convert.ToInt32(tmp[point + 2])),
                                4));
                            if (time < endT)
                            {
                                Elements[element].C.Add(new TActionNode(
                                    endT,
                                    Color(Convert.ToInt32(tmp[point]),
                                    Convert.ToInt32(tmp[point + 1]),
                                    Convert.ToInt32(tmp[point + 2])),
                                    4));
                            }
                            return;
                        }
                        break;
                    default:
                        //throw (new FormatException("Failed to read .osb file"));
                        break;
                }
                //_event,easing,starttime,endtime,val1,val2,val3,...,valN
                if (Elements[element].Lasttime < time) { Elements[element].Lasttime = time; }
            }
        }
        private void dealfile(StreamReader reader, ref int element)
        {
            string row;
            string[] tmp = null;
            SBelement tmpe;
            string Position = "";
            row = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (row.Trim() == "") { row = reader.ReadLine(); continue; }
                if (row.StartsWith("//") || row.Length == 0) { row = reader.ReadLine(); continue; }
                if (row.StartsWith("["))
                {
                    Position = row.Substring(1, row.Length - 2);
                    row = reader.ReadLine(); continue;
                }
                switch (Position)
                {
                    case "Variables":
                        {
                            SBvar tmpvar = new SBvar();
                            tmpvar.name = row.Split(new char[] { '=' }, 2)[0];
                            tmpvar.replace = row.Split(new char[] { '=' }, 2)[1];
                            tmpvar.name.Substring(1, tmpvar.name.Length - 1);
                            Variables.Add(tmpvar);
                            row = reader.ReadLine();
                            break;
                        }
                    case "Events":
                        //do variables change first
                        foreach (SBvar tmpvar in Variables)
                        {
                            if (row.Contains(tmpvar.name)) { row.Replace(tmpvar.name, tmpvar.replace); }
                        }
                        if (row.StartsWith("Sample") || row.StartsWith("5,"))
                        {
                            //Sample,time,layer,"filepath",volume
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Sample;
                            tmpe.Layers = (ElementLayer)(System.Enum.Parse(typeof(ElementLayer), tmp[2]));
                            tmpe.Path = tmp[3].Substring(1, tmp[3].Length - 2);
                            Elements.Add(tmpe);
                            element++;
                            SBEvent tmpev = new SBEvent();
                            tmpev.startT = Convert.ToInt32(tmp[1]);
                            tmpev.Type = EventType.Play;
                            if (tmp.Length < 5)
                            {
                                tmpev.volume = 100;
                            }
                            else
                            {
                                tmpev.volume = Convert.ToInt32(tmp[4]);
                            }
                            row = reader.ReadLine();
                            //  elements[element].events.Add(tmpev);
                        }
                        else if (row.StartsWith("Animation") || row.StartsWith("6,"))
                        {
                            //Animation,"layer","origin","filepath",x,y,frameCount,frameDelay,looptype
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Animation;
                            tmpe.Layers = (ElementLayer)(System.Enum.Parse(typeof(ElementLayer), tmp[1]));
                            if (tmpe.Layers == ElementLayer.Fail) { row = reader.ReadLine(); continue; }
                            tmpe.Origin = (ElementOrigin)(System.Enum.Parse(typeof(ElementOrigin), tmp[2]));
                            tmpe.Path = tmp[3].Substring(1, tmp[3].Length - 2);
                            tmpe.x = Convert.ToInt32(tmp[4]);
                            tmpe.y = Convert.ToInt32(tmp[5]);
                            tmpe.FrameCount = Convert.ToInt32(tmp[6]);
                            tmpe.Framedelay = (int)(Convert.ToDouble(tmp[7]));
                            tmpe.Looptype = (ElementLoopType)(System.Enum.Parse(typeof(ElementLoopType), tmp[8]));
                            Elements.Add(tmpe);
                            element++;
                            row = Dealevents(element, reader);
                            if (Elements[element].F.Count != 0 && Elements[element].F[0].Value == 0)
                            { }
                            else
                            {
                                Elements[element].F.Insert(0, new TActionNode(Elements[element].Starttime - 1, 255f, 4));
                            }
                            Elements[element].F.Add(new TActionNode(Elements[element].Lasttime + 1, 0f, 4));
                        }
                        else if (row.StartsWith("Sprite") || row.StartsWith("4,"))
                        {
                            //Sprite,"layer","origin","filepath",x,y
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Sprite;
                            tmpe.Layers = (ElementLayer)(System.Enum.Parse(typeof(ElementLayer), tmp[1]));
                            if (tmpe.Layers == ElementLayer.Fail) { row = reader.ReadLine(); continue; }
                            tmpe.Origin = (ElementOrigin)(System.Enum.Parse(typeof(ElementOrigin), tmp[2]));
                            tmpe.Path = tmp[3].Substring(1, tmp[3].Length - 2);
                            tmpe.x = Convert.ToInt32(tmp[4]);
                            tmpe.y = Convert.ToInt32(tmp[5]);
                            if (File.Exists(Path.Combine(location, tmpe.Path)))
                            {
                                Elements.Add(tmpe);
                                element++;
                                row = Dealevents(element, reader);
                                if (Elements[element].F.Count != 0 && Elements[element].F[0].Value == 0)
                                { }
                                else
                                {
                                    Elements[element].F.Insert(0, new TActionNode(Elements[element].Starttime - 1, 255f, 4));
                                }
                                Elements[element].F.Add(new TActionNode(Elements[element].Lasttime + 1, 0f, 4));
                            }
                            else { row = reader.ReadLine(); }
                        }
                        else if (row.StartsWith("0,"))
                        {
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Sprite;
                            tmpe.Layers = ElementLayer.Background;
                            tmpe.Origin = ElementOrigin.TopLeft;
                            tmpe.Path = tmp[2].Substring(1, tmp[2].Length - 2);
                            if (File.Exists(Path.Combine(location, tmpe.Path)))
                            {
                                Bitmap tmpbm = new Bitmap(Path.Combine(location, tmpe.Path));
                                float BGScale = 640f / tmpbm.Width < 480f / tmpbm.Height ? 640f / tmpbm.Width : 480f / tmpbm.Height;
                                Elements.Add(tmpe);
                                element++;
                                row = Dealevents(element, reader);
                                Elements[element].F.Insert(0, new TActionNode(0, 255f, 4));
                                Elements[element].SX.Insert(0, new TActionNode(0, BGScale, 4));
                                Elements[element].SY.Insert(0, new TActionNode(0, BGScale, 4));
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
            StreamReader reader;
            this.location = location;
            try
            {
                int currentelement = -1;
                using (reader = new StreamReader(osu)) { dealfile(reader, ref currentelement); }
                if (osb != null) { using (reader = new StreamReader(osb)) { dealfile(reader, ref currentelement); } }
            }
            catch (SystemException e)
            {
                throw (new FormatException("Failed to read .osb file", e));
            }

        }
    }
}
