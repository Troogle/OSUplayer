using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
namespace OSU_player
{
    public class Selfupdate
    {
        public Selfupdate()
        {
        }
        static XmlDocument UpDateXml = new XmlDocument();
        static string url = "";
        static string ver = "";
        static string info = "";
        static string temp = Environment.GetEnvironmentVariable("Temp").ToString() + "\\";
        static public void download(string url)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                if (System.IO.File.Exists(temp + "update.xml")) { System.IO.File.Delete(temp + "update.xml"); }
                myWebClient.DownloadFile(url, temp + "update.xml");
                DialogResult res;
                UpDateXml.Load(temp + "update.xml");
                string newver = "";
                string text = "";
                newver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
                text = UpDateXml.SelectNodes("/Xml/Text")[0].InnerText;
                if (newver.CompareTo(ver) > 0)
                {
                    res = RadMessageBox.Show(String.Format("新版本{0}发布了~\n版本更新提示{1}", newver, text), "提醒", MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        Process.Start(UpDateXml.SelectNodes("/Xml/Link")[0].InnerText);
                    }
                }
            }
            catch (Exception)
            {
                RadMessageBox.Show("更新配置文件出错!", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        public static void check_update()
        {
            try
            {
                UpDateXml.LoadXml(Properties.Resources.update);
                url = UpDateXml.SelectNodes("/Xml/Url")[0].InnerText;
                ver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
            }
            catch (Exception)
            {
                RadMessageBox.Show("本地配置文件出错!", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
                return;
            }
            download(url + "update.xml");
        }
    }
}
