using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Telerik.WinControls;
using System.Reflection;

namespace OSU_player
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                ThemeResolutionService.LoadPackageResource("OSU_player.Res.Default.tssp");
                ThemeResolutionService.LoadPackageResource("OSU_player.Res.Metro.tssp");
                Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = "Default";
                Application.Run(new Main());
            }
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, "QxQ");
                MessageBox.Show(str, "发生了一些令人悲伤的事情><", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            MessageBox.Show(str, "发生了一些令人悲伤的事情><", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            MessageBox.Show(str, "发生了一些令人悲伤的事情><", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        /// <summary>
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}