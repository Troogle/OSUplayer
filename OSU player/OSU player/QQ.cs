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


namespace OSU_player
{
	public class QQ
	{
		public class QQInfo
		{
			public int uin;
			public string nick;
		}
		public WebBrowser web = new WebBrowser();
		public QQ()
		{
			string url = "http://xui.ptlogin2.qq.com/cgi-bin/qlogin";
			web.Url = new Uri(url);
		}
		public void Send2QQ(int id, string Str)
		{
            Process.Start("QSetinfo",id.ToString() + " " + Str);
		}
		public List<QQInfo> GetQQList()
		{
			List<QQInfo> @ref = new List<QQInfo>();
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
							string str = (string) (uinList.Children[i].InnerText.Trim());
							string[] temp = str.Split(new char[] {' '});
							string nick = temp[0];
							string uin = temp[1];
							uin = (string) (uin.Replace("(", "").Replace(")", ""));
							QQInfo NInfo = new QQInfo();
							NInfo.nick = nick;
							NInfo.uin = Convert.ToInt32(uin);
							@ref.Add(NInfo);
						}
					}
				}
			}
			catch (Exception)
			{
				MessageBox.Show("获取当前在线QQ出错！");
			}
			return @ref;
			
		}
	}
	
}
