using OSU_Player_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSU_Player_WPF.Service
{
    /// <summary>
    /// 程序初始化逻辑
    /// </summary>
    public class GetInitialize
    {
        /// <summary>
        /// 初始化主程序信息
        /// </summary>
        /// <returns></returns>
        public MainAppInfo InitializeMainAppinfo()
        {
            MainAppInfo mai1 = new MainAppInfo();
            mai1.MainTitle = "osu! Player";
            mai1.Creator = ".Troogle";

            return mai1;
        }
    }
}
