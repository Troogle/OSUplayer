using OSUplayer.Uilties;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace OSUplayer
{
    static class Program
    {
        private static Mutex _mutex;
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
            Application.ThreadException += Application_ThreadException;
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            try
            {
                bool ret;
                _mutex = new Mutex(true, Application.ProductName, out ret);
                if (!ret)
                {
                    MessageBox.Show(@"The Program is already running!", @"Tips", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.ExitThread();
                    return;
                }
                var filename = Assembly.GetExecutingAssembly().Location;
                if (File.Exists(filename + ".detele")) File.Delete(filename + ".delete");
                if (!File.Exists("bass_fx.dll"))
                    if (File.Exists(Properties.Settings.Default.OSUpath + "\\bass_fx.dll"))
                        File.Copy(Properties.Settings.Default.OSUpath + "\\bass_fx.dll", "bass_fx.dll");
                    else
                        MessageBox.Show(@"如果不能播放，请重新下载完整包！\nPlease re-download the full pack if it can't play!", @"Tips", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Un4seen.Bass.BassNet.Registration(PrivateConfig.BassEmail, PrivateConfig.BassReg);
                Un4seen.Bass.AddOn.Fx.BassFx.LoadMe();
                LanguageManager.InitLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
                /*var mainwindow = new Main();
                 mainwindow.Show();
                 while (mainwindow.Created)
                 {
                     Application.DoEvents();
                     Core.Render();
                     Thread.Sleep(5);
                 }*/
                Application.Run(new Main());
            }
            #region 异常处理
            catch (Exception ex)
            {
                GetExceptionMsg(ex);
                NotifySystem.Showtip(1000, "发生了一些令人悲伤的事情><", "程序将试图继续运行", ToolTipIcon.Error);
            }
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            GetExceptionMsg(e.Exception);
            NotifySystem.Showtip(1000, "发生了一些令人悲伤的事情><", "程序将试图继续运行", ToolTipIcon.Error);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            GetExceptionMsg(e.ExceptionObject as Exception);
            NotifySystem.Showtip(1000, "发生了一些令人悲伤的事情><", "程序将试图继续运行", ToolTipIcon.Error);
        }

        /// <summary>
        /// 记录自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        static void GetExceptionMsg(Exception ex)
        {
            /*
                        try
                        {
                            WebRequest request;
                            if (ex != null)
                            {
                                int start = ex.StackTrace.IndexOf("OSUplayer");
                                request = WebRequest.Create("http://wenwo.at/counter.php?error=" + ex.GetType().Name + ex.Message + " " + ex.StackTrace.Substring(start, 300 > ex.StackTrace.Length - start - 1 ? ex.StackTrace.Length - start - 1 : 300));
                                request.Credentials = CredentialCache.DefaultCredentials;
                                request.Timeout = 2000;
                                request.GetResponse();
                            }
                        }
                        catch
                        {
                            return;
                        }
            */
            var sb = new StringBuilder();
            if (ex == null) return;
            sb.AppendLine("【异常类型】：" + ex.GetType().Name);
            sb.AppendLine("【异常信息】：" + ex.Message);
            sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            using (var writer = File.AppendText("Errlog.txt"))
            {
                writer.WriteLine(sb.ToString());
            }
        }
            #endregion
    }
}