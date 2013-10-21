// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using System.IO;

namespace OSU_player
{
    public class Core
    {
        public Core()
        {
            defaultBG = Path.Combine(Application.StartupPath, "default\\") + "defaultBG.png";

        }
        public static string osupath;
        public static List<BeatmapSet> allsets = new List<BeatmapSet>();
        public static string defaultBG;
        public static string getsong()
        {
            return "";
        }
        public static void Getpath()
        {
            string str = "";
            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("osu!\\shell\\open\\command");
                str = rk.GetValue("").ToString();
                str = str.Substring(1, str.IndexOf("\"", 1) - 9);
                osupath = str;
            }
            catch (Exception)
            {
                MessageBox.Show("读取游戏目录出错! 请手动指定", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                osupath = "";
            }
        }
        public static void Superscanforset()
        {
            if (System.IO.Directory.Exists(System.IO.Path.Combine(osupath, "Songs")))
                scanforset(System.IO.Path.Combine(osupath, "Songs\\1108"));
        }
        public static void scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                BeatmapSet tmp = new BeatmapSet(path);
                //tmp.GetDetail()
                allsets.Add(tmp);
                ListViewItem tmpl = new ListViewItem(tmp.name);
                Form1.Default.ListView1.Items.Add(tmpl);
            }
            else
            {
                string[] subfolder = Directory.GetDirectories(path);
                for (int i = 0; i <= subfolder.Length - 1; i++)
                {
                    scanforset(System.IO.Path.Combine(path, subfolder[i]));
                }
            }
        }
    }
    public enum OSUfile
    {
        FileVersion,
        AudioFilename,
        AudioHash,
        AudioLeadIn,
        PreviewTime,
        Countdown,
        SampleSet,
        StackLeniency,
        Mode,
        LetterboxInBreaks,
        StoryFireInFront,
        EpilepsyWarning,
        CountdownOffset,
        WidescreenStoryboard,
        EditorBookmarks,
        EditorDistanceSpacing,
        UseSkinSprites,
        OverlayPosition,
        SkinPreference,
        SpecialStyle,
        CustomSamples,
        Bookmarks,
        DistanceSpacing,
        BeatDivisor,
        GridSize,
        CurrentTime,
        Title,
        TitleUnicode,
        Artist,
        ArtistUnicode,
        Creator,
        Version,
        Source,
        Tags,
        BeatmapID,
        BeatmapSetID,
        HPDrainRate,
        CircleSize,
        OverallDifficulty,
        ApproachRate,
        SliderMultiplier,
        SliderTickRate,
        OSUfilecount
    }
    public enum ObjectFlag
    {
        Normal = 1,
        Slider = 2,
        NewCombo = 4,
        NormalNewCombo = 5,
        SliderNewCombo = 6,
        Spinner = 8,
        SpinnerNewCombo = 12,
        ColourHax = 112,
        Hold = 128,
        ManiaLong = 128
    }
    public enum HitSound
    {
        Normal = 0,
        Whistle = 2,
        Finish = 4,
        Clap = 8
    }
    public enum TSample
    {
        Normal,
        Soft,
        Drum
    }
    public struct CSample
    {
        public TSample sample;
        public int sampleset;
        public CSample(int sample, int sampleset)
        {
            this.sampleset = sampleset;
            this.sample = (TSample)sample;
        }
    }
}
