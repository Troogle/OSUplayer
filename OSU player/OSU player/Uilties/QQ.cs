using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using OSUplayer.Properties;

namespace OSUplayer.Uilties
{
    /// <summary>
    ///     获取的QQ信息
    /// </summary>
    public class QQInfo
    {
        public string Nick { get; private set; }
        public string Uin { get; private set; }
        static string FindBetween(string source, string begin, string after)
        {
            var firstIndex = source.IndexOf(begin, StringComparison.Ordinal) + begin.Length;
            return source.Substring(
                firstIndex, source.IndexOf(after, firstIndex, StringComparison.Ordinal) - firstIndex
            );
        }
        public QQInfo(string uin)
        {
            Uin = uin;
            var request = WebRequest.Create("http://r.qzone.qq.com/cgi-bin/user/cgi_personal_card?uin=" + uin);
            var data = request.GetResponse().GetResponseStream();
            if (data == null) return;
            using (var sr = new StreamReader(data))
            {
                var html = sr.ReadToEnd();
                Nick = FindBetween(html, @"""nickname"":""", @"""");
            }
        }
    }

    public class QQ
    {
        /// <summary>
        ///     获取目前登陆QQ列表
        /// </summary>
        public List<QQInfo> GetQQList = new List<QQInfo>();
        public QQ()
        {
            foreach (var qq in Directory.GetFiles(@"\\.\Pipe\", "QQ_*_pipe", SearchOption.AllDirectories))
            {
                GetQQList.Add(new QQInfo(qq.Substring(12, qq.Length - 17)));
            }
        }
        /// <summary>
        ///     推送消息给指定ID
        /// </summary>
        /// <param name="id">QQ号</param>
        /// <param name="str">推送的内容</param>
        public static void Send2QQ(string str)
        {
            try
            {
                if (!Settings.Default.SyncQQ)
                {
                    return;
                }
                var objAdminType = Type.GetTypeFromProgID("QQCPHelper.CPAdder");
                var args = new object[4];
                args[0] = Settings.Default.QQuin;
                args[1] = 65542;
                args[2] = str;
                args[3] = "";
                var objAdmin = Activator.CreateInstance(objAdminType);
                objAdminType.InvokeMember("PutRSInfo", BindingFlags.InvokeMethod, null, objAdmin, args);
            }
            catch
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("QQ_Push_Fail_Text"));
            }
        }
    }
}