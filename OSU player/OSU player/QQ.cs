using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OSU_player
{
    public class QQ
    {

        public WebBrowser web = new WebBrowser();
        //提前加载浏览器，给浏览器加载的时间
        public QQ()
        {
            string url = "http://xui.ptlogin2.qq.com/cgi-bin/qlogin";
            web.Url = new Uri(url);
        }
        /// <summary>
        /// 推送消息给指定ID
        /// </summary>
        /// <param name="id">QQ号</param>
        /// <param name="Str">推送的内容</param>
        public void Send2QQ(int id, string Str)
        {
            Process.Start("QSetinfo", id.ToString() + " " + Str);
        }
        /// <summary>
        /// 获取目前登陆QQ列表
        /// </summary>
        public List<QQInfo> GetQQList()
        {
            List<QQInfo> ret = new List<QQInfo>();
            try
            {
                if (web.ReadyState == WebBrowserReadyState.Complete)
                {
                    HtmlDocument doc = web.Document;
                    HtmlElement uinList = doc.GetElementById("list_uin");
                    if (uinList != null)
                    {
                        for (var i = 0; i <= uinList.Children.Count - 1; i++)
                        {
                            string str = uinList.Children[i].InnerText.Trim();
                            string[] temp = str.Split(new char[] { ' ' });
                            QQInfo NInfo = new QQInfo(Convert.ToInt32(temp[1].Substring(1, temp[1].Length - 2)), temp[0]);
                            ret.Add(NInfo);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("获取当前在线QQ出错！重试下试试？");
            }
            return ret;

        }
    }

}
