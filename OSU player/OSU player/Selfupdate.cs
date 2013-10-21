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

using System.Net;
using System.IO;

namespace OSU_player
{
    public class Selfupdate
    {
        public Selfupdate()
        {
            temp = Environment.GetEnvironmentVariable("Temp").ToString() + "\\";
        }
        static string XmlFilePath = Application.StartupPath + "\\" + "update.xml";
        static System.Xml.XmlDocument UpDateXml = new System.Xml.XmlDocument();
        static string url = "";
        static string ver = "";
        static string temp;
        static public void download(string url)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(temp + "update.xml", System.IO.FileMode.Create);
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                myrp.Close();
                Myrq.Abort();
                DialogResult res;
                UpDateXml.Load(temp + "update.xml");
                string newver = "";
                newver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
                if (newver.CompareTo(ver) > 0)
                {
                    res = MessageBox.Show("新版本" + newver + "发布了~", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        Process.Start((string)(UpDateXml.SelectNodes("/Xml/Link")[0].InnerText));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("更新配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void check_update()
        {
            try
            {
                UpDateXml.Load(XmlFilePath);
                url = UpDateXml.SelectNodes("/Xml/Url")[0].InnerText;
                ver = UpDateXml.SelectNodes("/Xml/Version")[0].InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("本地配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            download(url + "update.xml");
        }
    }

}
