using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;

namespace OSUplayer.Uilties
{
    public static class Selfupdate
    {
        private static readonly XmlDocument UpDateXml = new XmlDocument();
        const string Url = "https://raw.github.com/Troogle/OSUplayer/master/";
        private static void Download(string url)
        {
            try
            {
                UpDateXml.Load(new XmlTextReader(url));
                string newver = "";
                string text = "";
                newver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
                text = UpDateXml.SelectNodes("/Xml/Text")[0].InnerText.Replace(@"\n", "\n");
                if (newver.CompareTo(Core.Version) > 0)
                {
                    var res = RadMessageBox.Show(String.Format("新版本 {0} 发布了~\n更新内容:\n{1}", newver, text), "提醒",
                        MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                    if (res == DialogResult.OK)
                    {
                        Process.Start(UpDateXml.SelectNodes("/Xml/Link")[0].InnerText);
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

        public static void check_update()
        {
            Download(Url + "update.xml");
        }
    }
}