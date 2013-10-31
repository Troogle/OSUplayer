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
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSU_player
{
    /// <summary>
    /// OSU文件内属性
    /// </summary>
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
    /// <summary>
    /// HitObject的类别
    /// </summary>
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
    /// <summary>
    /// 打击音效，可以叠加
    /// </summary>
    public enum HitSound
    {
        Normal = 0,
        Whistle = 2,
        Finish = 4,
        Clap = 8
    }
    /// <summary>
    /// 打击音效组的前缀
    /// </summary>
    public enum TSample
    {
        None = 0,
        Normal = 1,
        Soft = 2,
        Drum = 3
    }
    /// <summary>
    /// 打击音效组
    /// </summary>
    public struct CSample
    {
        public int sample;
        public int sampleset;
        public CSample(int sample, int sampleset)
        {
            this.sampleset = sampleset;
            this.sample = sample;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is CSample))
                return false;
            CSample mys = (CSample)obj;
            return mys.sample == this.sample && mys.sampleset == this.sampleset;
        }
    }
    /// <summary>
    /// 获取的QQ信息
    /// </summary>
    public struct QQInfo
    {
        public int uin;
        public string nick;
        public QQInfo(int uin, string nick)
        {
            this.uin = uin;
            this.nick = nick;
        }

    }
    public class Core
    {
        public Core()
        {
        }
        /// <summary>
        /// OSU的路径
        /// </summary>
        public static string osupath;
        /// <summary>
        /// 所有的set
        /// </summary>
        public static List<BeatmapSet> allsets = new List<BeatmapSet>();
        public static string defaultBG = Path.Combine(Application.StartupPath, "default\\") + "defaultBG.png";
        public static string defaultAudio = Path.Combine(Application.StartupPath, "default\\") + "blank.wav";
        /// <summary>
        /// 选定的QQ号
        /// </summary>
        public static int uin;
        /// <summary>
        /// 获取OSU路径
        /// </summary>
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
                while (!Setpath()) { };
            }
        }
        public static bool syncQQ = true;
        /// <summary>
        /// 设置路径
        /// </summary>
        /// <returns>是否选择正常路径</returns>
        public static bool Setpath()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            try
            {
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    if (File.Exists(dialog.SelectedPath + "\\osu!.exe"))
                    {
                        osupath = dialog.SelectedPath;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("为什么你总是想卖萌OAO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("为什么你总是想卖萌OxO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        /// <summary>
        /// 载入播放列表
        /// </summary>
        /// <returns>是否正常载入</returns>
        public static bool LoadList()
        {
            try
            {
                using (FileStream fs = new FileStream("list.db", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    allsets = (List<BeatmapSet>)formatter.Deserialize(fs);
                }
                return true;
            }
            catch
            {
                File.Delete("list.db");
                return false;
            }
        }
        public static void SaveList()
        {
                using (FileStream fs = new FileStream("list.db", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, allsets);
                }
        }
    }
}