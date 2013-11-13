using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
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
            web.DocumentCompleted += web_DocumentCompleted;
        }

        void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument doc = web.Document;
            HtmlElement uinList = doc.GetElementById("list_uin");
            if (uinList != null)
            {
                for (var i = 0; i <= uinList.Children.Count - 1; i++)
                {
                    string str = uinList.Children[i].InnerText.Trim();
                    QQInfo NInfo = new QQInfo(str.Substring(str.LastIndexOf("(") + 1, str.LastIndexOf(")") - str.LastIndexOf("(") - 1), str.Substring(0, str.LastIndexOf("(") - 1));
                    GetQQList.Add(NInfo);
                }
            }
            Refreshed = true;

        }
        /// <summary>
        /// 推送消息给指定ID
        /// </summary>
        /// <param name="id">QQ号</param>
        /// <param name="Str">推送的内容</param>
        public void Send2QQ(string id, string Str)
        {
            if (!Core.syncQQ) { return; }
            object objAdmin = null;
            Type objAdminType = Type.GetTypeFromProgID("QQCPHelper.CPAdder");
            Object[] args = new object[4];
            args[0] = id;
            args[1] = 65542;
            args[2] = Str;
            args[3] = "";
            objAdmin = System.Activator.CreateInstance(objAdminType);
            objAdminType.InvokeMember("PutRSInfo", System.Reflection.BindingFlags.InvokeMethod, null, objAdmin, args);
        }
        /// <summary>
        /// 获取目前登陆QQ列表
        /// </summary>
        public List<QQInfo> GetQQList = new List<QQInfo>();
        public bool Refreshed = false;
    }
}
