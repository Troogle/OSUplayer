using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
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
            Application.SetCompatibleTextRenderingDefault(false);
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            try
            {
                ThemeResolutionService.LoadPackageResource("OSU_player.Res.Light.tssp");
                ThemeResolutionService.ApplicationThemeName = "Light";
                Un4seen.Bass.BassNet.Registration("sqh1994@163.com", "2X280331512622");
                Un4seen.Bass.Bass.BASS_Init(-1, 44100, Un4seen.Bass.BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                Main Mainwindow = new Main();
                Mainwindow.Show();
                while (Mainwindow.Created)
                {
                    Application.DoEvents();
                    Core.Render();
                    Thread.Sleep(5);
                }

                //   Application.Run(new Main());
            }
            #region 异常处理
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, "QxQ");
                NotifyIcon notifyIcon1=new NotifyIcon();
                notifyIcon1.ShowBalloonTip(1000, "发生了一些令人悲伤的事情><", "错误已上报，程序将试图继续运行", System.Windows.Forms.ToolTipIcon.Error);
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            Core.notifyIcon1.ShowBalloonTip(1000, "发生了一些令人悲伤的事情><", "错误已上报，程序将试图继续运行", System.Windows.Forms.ToolTipIcon.Error);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            Core.notifyIcon1.ShowBalloonTip(1000, "发生了一些令人悲伤的事情><", "错误已上报，程序将试图继续运行", System.Windows.Forms.ToolTipIcon.Error);
        }
        /// <summary>
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            try
            {
                WebRequest request;
                if (ex != null)
                {
                    int start=ex.StackTrace.IndexOf("OSU_player");
                    request = WebRequest.Create("http://wenwo.at/counter.php?error=" +ex.GetType().Name+ex.Message + " " + ex.StackTrace.Substring(start, 300 > ex.StackTrace.Length - start - 1 ? ex.StackTrace.Length - start - 1 : 300));
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Timeout = 2000;
                    WebResponse response;
                    response = request.GetResponse();
                }
            }
            catch {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
                using (StreamWriter writer = File.CreateText("Errlog.txt"))
                {
                   // writer.WriteLine(ex.ToString());
                    writer.WriteLine(sb.ToString());
                }
            }
            return sb.ToString();

        }
            #endregion
    }
}