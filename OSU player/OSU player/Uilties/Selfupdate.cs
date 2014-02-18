using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Ionic.Zip;

namespace OSUplayer.Uilties
{
    public static class Selfupdate
    {
        private static readonly XmlDocument UpDateXml = new XmlDocument();
        const string Url = "https://raw.github.com/Troogle/OSUplayer/master/";
        private const string Upfile = "http://troogle.pw/OSUplayer.zip";
        private static void Download(string url)
        {
            try
            {
                UpDateXml.Load(new XmlTextReader(url));
                string newver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
                string text = UpDateXml.SelectNodes("/Xml/Text")[0].InnerText.Replace(@"\n", "\n");
                string full = "0";
                var xmlNodeList = UpDateXml.SelectNodes("/Xml/Full");
                if (xmlNodeList != null && xmlNodeList.Count != 0)
                {
                    full = xmlNodeList[0].InnerText;
                }
                if (newver.CompareTo(Core.Version) > 0)
                {
                    var res = RadMessageBox.Show(String.Format("新版本 {0} 发布了~\n更新内容:\n{1}", newver, text), "提醒",
                        MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                    if (res == DialogResult.OK)
                    {
                        if (full == "1")
                        {
                            Process.Start(UpDateXml.SelectNodes("/Xml/Link")[0].InnerText);
                            return;
                        }
                        NotifySystem.Showtip(1000, "提示", "下载更新ing");
                        RadMessageBox.Show("程序将在后台下载更新", "提示", MessageBoxButtons.OK, RadMessageIcon.Info);
                        Update();
                    }
                }

#if (!DEBUG)
                try
                {
                    WebRequest request = WebRequest.Create("http://wenwo.at/counter.php?counter=" + Core.Version);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Timeout = 20000;
                    request.GetResponse();
                }
                catch
                {
                }
#endif
            }
            catch (Exception)
            {
                RadMessageBox.Show("更新配置文件出错!", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        private static void Update()
        {
            var req = (HttpWebRequest)WebRequest.Create(Upfile);
            var res = (HttpWebResponse)req.GetResponse();
            using (var instream = res.GetResponseStream())
            {
                using (var outstream = File.Create("OSUplayer.zip"))
                {
                    byte[] buffer = new byte[1024];
                    int l;
                    do
                    {
                        l = instream.Read(buffer, 0, buffer.Length);
                        if (l > 0)
                            outstream.Write(buffer, 0, l);
                    }
                    while (l > 0);
                }
            }
            string filename = Assembly.GetExecutingAssembly().Location;
            if (File.Exists(filename + ".detele")) File.Delete(filename + ".delete");
            File.Move(filename, filename + ".delete");
            using (var sourceFile = new ZipFile("OSUplayer.zip"))
            {
                sourceFile.ExtractSelectedEntries("OSUplayer.exe", ExtractExistingFileAction.OverwriteSilently);
            }
            File.Move("OSUplayer.exe", filename);
            File.Delete("OSUplayer.zip");
            RadMessageBox.Show("程序将重启完成升级", "提示", MessageBoxButtons.OK, RadMessageIcon.Info);
            var proc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location));
            var p = new Process { StartInfo = { FileName = filename } };
            p.Start();
            foreach (var pr in proc)
            {
                pr.Kill();
            }
        }
        public static void check_update()
        {
            Download(Url + "update.xml");
        }
    }
}