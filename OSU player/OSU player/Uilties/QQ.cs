using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using OSUplayer.Properties;

namespace OSUplayer.Uilties
{
    /// <summary>
    ///     获取的QQ信息
    /// </summary>
    public struct QQInfo
    {
        public string nick;
        public string uin;

        public QQInfo(string uin, string nick)
        {
            this.uin = uin;
            this.nick = nick;
        }
    }

    public class QQ
    {
        private readonly WebBrowser web = new WebBrowser();

        /// <summary>
        ///     获取目前登陆QQ列表
        /// </summary>
        public List<QQInfo> GetQQList = new List<QQInfo>();
        //提前加载浏览器，给浏览器加载的时间
        public QQ()
        {
            const string url = "http://xui.ptlogin2.qq.com/cgi-bin/qlogin";
            web.Url = new Uri(url);
            web.DocumentCompleted += web_DocumentCompleted;
        }

        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument doc = web.Document;
            HtmlElement uinList = doc.GetElementById("list_uin");
            if (uinList != null)
            {
                for (int i = 0; i <= uinList.Children.Count - 1; i++)
                {
                    string str = uinList.Children[i].InnerText.Trim();
                    var NInfo =
                        new QQInfo(
                            str.Substring(str.LastIndexOf("(") + 1, str.LastIndexOf(")") - str.LastIndexOf("(") - 1),
                            str.Substring(0, str.LastIndexOf("(") - 1));
                    GetQQList.Add(NInfo);
                }
            }
        }

        /// <summary>
        ///     推送消息给指定ID
        /// </summary>
        /// <param name="id">QQ号</param>
        /// <param name="str">推送的内容</param>
        public static void Send2QQ(string id, string str)
        {
            try
            {
                if (!Settings.Default.SyncQQ)
                {
                    return;
                }
                Type objAdminType = Type.GetTypeFromProgID("QQCPHelper.CPAdder");
                var args = new object[4];
                args[0] = id;
                args[1] = 65542;
                args[2] = str;
                args[3] = "";
                object objAdmin = Activator.CreateInstance(objAdminType);
                objAdminType.InvokeMember("PutRSInfo", BindingFlags.InvokeMethod, null, objAdmin, args);
            }
            catch
            {
                NotifySystem.Showtip(1000, "OSUplayer", "QQ推送失败！", ToolTipIcon.Info);
            }
        }
    }
}