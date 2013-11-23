using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using Telerik.WinControls;
namespace OSUplayer.Uilties
{
    public class Selfupdate
    {
        public Selfupdate()
        {
        }
        static XmlDocument UpDateXml = new XmlDocument();
        static string url = "";
        static string ver = "";
        static public void download(string url)
        {
            try
            {
                UpDateXml.Load(new XmlTextReader(url));
                DialogResult res;
                string newver = "";
                string text = "";
                newver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
                text = UpDateXml.SelectNodes("/Xml/Text")[0].InnerText.Replace(@"\n", "\n");
                if (newver.CompareTo(ver) > 0)
                {
                    res = RadMessageBox.Show(String.Format("新版本 {0} 发布了~\n更新内容:\n{1}", newver, text), "提醒", MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        Process.Start(UpDateXml.SelectNodes("/Xml/Link")[0].InnerText);
                    }
                }
             /*   try
                {
                    WebRequest request;
                    request = WebRequest.Create("http://wenwo.at/counter.php?counter=" + Core.Version);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Timeout = 20000;
                    request.GetResponse();
                }
                catch { }*/
            }
            catch (Exception)
            {
                RadMessageBox.Show("更新配置文件出错!", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        public static void check_update()
        {
            url = "https://raw.github.com/Troogle/OSUplayer/master/";
            ver = Core.Version;
            download(url + "update.xml");
        }
    }
}
