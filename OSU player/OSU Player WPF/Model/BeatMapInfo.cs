using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSU_Player_WPF.Model
{
    /// <summary>
    /// osu! 歌曲包 Model
    /// </summary>
    public class BeatMapInfo
    {
        public BeatMapInfo(string pTitle = "")
        {
            this.Title = pTitle;
        }

        /// <summary>
        /// 主标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 歌曲难度列表
        /// </summary>
        public List<string> HardVertion { get; set; }

    }
}
