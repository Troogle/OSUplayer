using System;
using System.Collections.Generic;
using System.IO;
namespace OSU_player
{
    public class StoryBoard
    {
        public List<SBelement> elements = new List<SBelement>();
        //TODO:单独抽取trigger并作索引
        public List<SBvar> Variables = new List<SBvar>();
        //public Dictionary<Triggertype, TriggerEvent> trigger = new Dictionary<Triggertype, TriggerEvent>();
        //目录由beatmapfiles.location-->beatmap.location
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
            public double startxF; //F,S,R(只用x),V 'F stands for float-option
            public double startyF;
            public double endyF;
            public double endxF;
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
            public List<SBEvent> events = new List<SBEvent>();
            public ElementType Type;
            public ElementLayer Layers;
            public ElementOrigin Origin; //sample时无
            public string path;
            public int x; //sample时无时无
            public int y;
            //Animation only
            public int frameCount;
            public int framedelay;
            public ElementLoopType Looptype; //默认LoopForever【一直循环】
            //事件触发循环可以被游戏时间事件触发. 虽然叫做循环, 事件触发循环循环时只执行一次
            //触发器循环和普通循环一样是从零计数. 如果两个重叠, 第一个将会被停止且被一个从头开始的循环替代.
            //如果他们和任何存在的故事版事件重叠,他们将不会循环直到那些故事版事件不在生效
        }
        private string picknext(ref string str)
        {
            string ret = "";
            if (!str.Contains(","))
            {
                ret = str;
                str = "";
            }
            else
            {
                ret = str.Substring(0, str.IndexOf(","));
                str = str.Substring(str.IndexOf(",") + 1);
            }
            return ret;
        }
        private void dealevents(int element, StreamReader reader)
        {
            string row = "";
            row = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (row.Trim() == "") { row = reader.ReadLine(); continue; }
                if (row.StartsWith("//") || row.Length == 0) { row = reader.ReadLine(); continue; }
                if (row.StartsWith("[")) { return; }
                if (!row.StartsWith(" ")) { return; }
                //do variables change first
                foreach (SBvar tmpvar in Variables)
                {
                    if (row.Contains(tmpvar.name)) { row.Replace(tmpvar.name, tmpvar.replace); }
                }
                if (row.StartsWith(" L"))
                {
                    //④对于L的处理：直接复制_L,time difference,loopcount
                    row = reader.ReadLine();
                    picknext(ref row);
                    int easing = Convert.ToInt32(picknext(ref row));
                    int loopcount = Convert.ToInt32(picknext(ref row));
                    //tmpe.easing:time difference
                    //time difference : 循环开始的时间和此系列SB事件第一次生效的最初时间之间的时间差, 单位是毫秒
                    //tmpe.startT:loopcount
                    while (!reader.EndOfStream && row.StartsWith("  "))
                    {
                        for (int i = 0; i <= loopcount; i++)
                        {
                            dealevent((row.Substring(1)), element, i * easing);
                        }
                        row = reader.ReadLine();
                    }
                    return;
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
                }
                else
                {
                    dealevent(row, element, 0);
                    row = reader.ReadLine();
                }
            }
        }
        private void dealevent(string str, int element, int delta)
        {
            SBEvent tmpe = new SBEvent();
            string op = "";
            string tmp = "";
            op = picknext(ref str);
            tmpe.easing = Convert.ToInt32(picknext(ref str));
            tmpe.startT = Convert.ToInt32(picknext(ref str)) + delta;
            //②_M,0,1000,1000,320,240,320,240-->_M,0,1000,,320,240,320,240(开始结束时间相同）
            tmp = picknext(ref str);
            if (tmp == "")
            {
                tmpe.endT = tmpe.startT;
            }
            else
            {
                tmpe.endT = Convert.ToInt32(tmp) + delta;
            }
            string first = string.Concat(op, ",", tmpe.easing.ToString(), ",", tmpe.startT.ToString(), ",", tmpe.endT.ToString(), ",");
            switch (op)
            {
                case " F":
                    tmpe.startxF = Convert.ToDouble(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endxF = tmpe.startxF;
                    }
                    else
                    {
                        tmpe.endxF = Convert.ToDouble(tmp);
                    }
                    //③_M,0,1000,,320,240,320,240-->_M,0,1000,,320,240 (开始结束值相同）
                    break;
                case " MX":
                    tmpe.startx = Convert.ToInt32(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endx = tmpe.startx;
                    }
                    else
                    {
                        tmpe.endx = Convert.ToInt32(tmp);
                    }
                    break;
                case " MY":
                    tmpe.starty = Convert.ToInt32(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endy = tmpe.starty;
                    }
                    else
                    {
                        tmpe.endy = Convert.ToInt32(tmp);
                    }
                    break;
                case " M":
                    tmpe.startx = Convert.ToInt32(picknext(ref str));
                    tmpe.starty = Convert.ToInt32(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endx = tmpe.startx;
                    }
                    else
                    {
                        tmpe.endx = Convert.ToInt32(tmp);
                    }
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endy = tmpe.starty;
                    }
                    else
                    {
                        tmpe.endy = Convert.ToInt32(tmp);
                    }
                    break;
                case " S":
                    tmpe.startxF = Convert.ToDouble(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endxF = tmpe.startxF;
                    }
                    else
                    {
                        tmpe.endxF = Convert.ToDouble(tmp);
                    }
                    break;
                case " V":
                    tmpe.startxF = Convert.ToDouble(picknext(ref str));
                    tmpe.startyF = Convert.ToDouble(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endxF = tmpe.startxF;
                    }
                    else
                    {
                        tmpe.endxF = Convert.ToDouble(tmp);
                    }
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endyF = tmpe.startyF;
                    }
                    else
                    {
                        tmpe.endyF = Convert.ToDouble(tmp);
                    }
                    break;
                case " R":
                    tmpe.startxF = Convert.ToDouble(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.endxF = tmpe.startxF;
                    }
                    else
                    {
                        tmpe.endxF = Convert.ToDouble(tmp);
                    }
                    break;
                case " C":
                    tmpe.r1 = Convert.ToInt32(picknext(ref str));
                    tmpe.g1 = Convert.ToInt32(picknext(ref str));
                    tmpe.b1 = Convert.ToInt32(picknext(ref str));
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.r2 = tmpe.r1;
                    }
                    else
                    {
                        tmpe.r2 = Convert.ToInt32(tmp);
                    }
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.g2 = tmpe.g1;
                    }
                    else
                    {
                        tmpe.g2 = Convert.ToInt32(tmp);
                    }
                    tmp = picknext(ref str);
                    if (tmp == "")
                    {
                        tmpe.b2 = tmpe.b1;
                    }
                    else
                    {
                        tmpe.b2 = Convert.ToInt32(tmp);
                    }
                    break;
                case " P":
                    switch (picknext(ref str))
                    {
                        case "H":
                            tmpe.startx = 0;
                            break;
                        case "V":
                            tmpe.startx = 1;
                            break;
                        case "A":
                            tmpe.startx = 2;
                            break;
                    }
                    break;
                default:
                    {
                        //throw (new FormatException("Failed to read .osb file"));
                        break;
                    }
            }
            elements[element].events.Add(tmpe);
            //_event,easing,starttime,endtime,val1,val2,val3,...,valN
            if (str != "")
            {
                dealevent(first + str, element, tmpe.endT - tmpe.startT);
            }
        }
        private void dealfile(StreamReader reader, ref int element)
        {
            string row;
            string[] tmp = null;
            SBelement tmpe = new SBelement();
            string Position = "";
            while (!reader.EndOfStream)
            {
                row = reader.ReadLine();
                if (row.Trim() == "") { continue; }
                if (row.StartsWith("//") || row.Length == 0) { continue; }
                if (row.StartsWith("[")) { Position = row.Substring(1, row.Length - 2); continue; }
                switch (Position)
                {
                    case "Variables":
                        {
                            SBvar tmpvar = new SBvar();
                            tmpvar.name = row.Split(new char[] { '=' }, 2)[0];
                            tmpvar.replace = row.Split(new char[] { '=' }, 2)[1];
                            tmpvar.name.Substring(1, tmpvar.name.Length - 1);
                            Variables.Add(tmpvar);
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
                            tmpe.path = tmp[3];
                            elements.Add(tmpe);
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
                            elements[element].events.Add(tmpev);
                        }
                        else if (row.StartsWith("Animation") || row.StartsWith("6,"))
                        {
                            //Animation,"layer","origin","filepath",x,y,frameCount,frameDelay,looptype
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Animation;
                            tmpe.Layers = (ElementLayer)(System.Enum.Parse(typeof(ElementLayer), tmp[1]));
                            tmpe.Origin = (ElementOrigin)(System.Enum.Parse(typeof(ElementOrigin), tmp[2]));
                            tmpe.path = tmp[3];
                            tmpe.x = Convert.ToInt32(tmp[4]);
                            tmpe.y = Convert.ToInt32(tmp[5]);
                            tmpe.frameCount = Convert.ToInt32(tmp[6]);
                            tmpe.framedelay = (int)(Convert.ToDouble(tmp[7]));
                            tmpe.Looptype = (ElementLoopType)(System.Enum.Parse(typeof(ElementLoopType), tmp[8]));
                            elements.Add(tmpe);
                            element++;
                            dealevents(element, reader);
                        }
                        else if (row.StartsWith("Sprite") || row.StartsWith("4,"))
                        {
                            //Sprite,"layer","origin","filepath",x,y
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Sprite;
                            tmpe.Layers = (ElementLayer)(System.Enum.Parse(typeof(ElementLayer), tmp[1]));
                            tmpe.Origin = (ElementOrigin)(System.Enum.Parse(typeof(ElementOrigin), tmp[2]));
                            tmpe.path = tmp[3];
                            tmpe.x = Convert.ToInt32(tmp[4]);
                            tmpe.y = Convert.ToInt32(tmp[5]);
                            elements.Add(tmpe);
                            element++;
                            dealevents(element, reader);
                        }
                        else if (row.StartsWith("0,"))
                        {
                            tmp = row.Split(new char[] { ',' });
                            tmpe = new SBelement();
                            tmpe.Type = ElementType.Sprite;
                            tmpe.Layers = ElementLayer.Background;
                            tmpe.Origin = ElementOrigin.Centre;
                            tmpe.path = tmp[2];
                            elements.Add(tmpe);
                            element++;
                            dealevents(element, reader);
                        }
                        break;
                }

            }
        }
        public StoryBoard(string osu, string osb)
        {
            StreamReader reader;
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
