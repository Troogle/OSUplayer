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
public partial class Form1 : System.Windows.Forms.Form
	{
		
		//Form 重写 Dispose，以清理组件列表。
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

        //Windows 窗体设计器所必需的
		
		//注意: 以下过程是 Windows 窗体设计器所必需的
		//可以使用 Windows 窗体设计器修改它。
		//不要使用代码编辑器修改它。
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.运行OSUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手动指定OSU目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.重新导入osuToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.重新导入scoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新导入collectionsToolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.打开曲目文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开铺面文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开SB文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.重复歌曲扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.音效ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视频开关ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.播放模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.随机播放ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.顺序播放ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单曲循环ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QQ状态同步ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.音效音乐控制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SB开关ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PlayButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.TrackBar1 = new System.Windows.Forms.TrackBar();
            this.NextButton = new System.Windows.Forms.Button();
            this.LabelQQ = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.TrackBar2 = new System.Windows.Forms.TrackBar();
            this.Label2 = new System.Windows.Forms.Label();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListBox1 = new System.Windows.Forms.ListBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.TrackBar3 = new System.Windows.Forms.TrackBar();
            this.Label4 = new System.Windows.Forms.Label();
            this.TrackBar4 = new System.Windows.Forms.TrackBar();
            this.Button2 = new System.Windows.Forms.Button();
            this.ListDetail = new System.Windows.Forms.ListView();
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.AVsyncer = new System.Windows.Forms.Timer(this.components);
            this.MenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar4)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Location = new System.Drawing.Point(342, 67);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(480, 360);
            this.Panel1.TabIndex = 1;
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3,
            this.关于ToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(882, 28);
            this.MenuStrip1.TabIndex = 2;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.运行OSUToolStripMenuItem,
            this.手动指定OSU目录ToolStripMenuItem,
            this.ToolStripSeparator1,
            this.重新导入osuToolStripMenuItem7,
            this.重新导入scoresToolStripMenuItem,
            this.重新导入collectionsToolStripMenuItem9,
            this.ToolStripSeparator2,
            this.打开曲目文件夹ToolStripMenuItem,
            this.打开铺面文件ToolStripMenuItem,
            this.打开SB文件ToolStripMenuItem,
            this.ToolStripSeparator3,
            this.退出ToolStripMenuItem});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem1.Text = "文件";
            // 
            // 运行OSUToolStripMenuItem
            // 
            this.运行OSUToolStripMenuItem.Name = "运行OSUToolStripMenuItem";
            this.运行OSUToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.运行OSUToolStripMenuItem.Text = "运行OSU!";
            // 
            // 手动指定OSU目录ToolStripMenuItem
            // 
            this.手动指定OSU目录ToolStripMenuItem.Name = "手动指定OSU目录ToolStripMenuItem";
            this.手动指定OSU目录ToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.手动指定OSU目录ToolStripMenuItem.Text = "手动指定OSU目录";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(238, 6);
            // 
            // 重新导入osuToolStripMenuItem7
            // 
            this.重新导入osuToolStripMenuItem7.Name = "重新导入osuToolStripMenuItem7";
            this.重新导入osuToolStripMenuItem7.Size = new System.Drawing.Size(241, 24);
            this.重新导入osuToolStripMenuItem7.Text = "重新导入osu.db";
            // 
            // 重新导入scoresToolStripMenuItem
            // 
            this.重新导入scoresToolStripMenuItem.Name = "重新导入scoresToolStripMenuItem";
            this.重新导入scoresToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.重新导入scoresToolStripMenuItem.Text = "重新导入scores.db";
            // 
            // 重新导入collectionsToolStripMenuItem9
            // 
            this.重新导入collectionsToolStripMenuItem9.Name = "重新导入collectionsToolStripMenuItem9";
            this.重新导入collectionsToolStripMenuItem9.Size = new System.Drawing.Size(241, 24);
            this.重新导入collectionsToolStripMenuItem9.Text = "重新导入collections.db";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(238, 6);
            // 
            // 打开曲目文件夹ToolStripMenuItem
            // 
            this.打开曲目文件夹ToolStripMenuItem.Name = "打开曲目文件夹ToolStripMenuItem";
            this.打开曲目文件夹ToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.打开曲目文件夹ToolStripMenuItem.Text = "打开曲目文件夹";
            // 
            // 打开铺面文件ToolStripMenuItem
            // 
            this.打开铺面文件ToolStripMenuItem.Name = "打开铺面文件ToolStripMenuItem";
            this.打开铺面文件ToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.打开铺面文件ToolStripMenuItem.Text = "打开铺面文件";
            // 
            // 打开SB文件ToolStripMenuItem
            // 
            this.打开SB文件ToolStripMenuItem.Name = "打开SB文件ToolStripMenuItem";
            this.打开SB文件ToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.打开SB文件ToolStripMenuItem.Text = "打开SB文件";
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(238, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重复歌曲扫描ToolStripMenuItem});
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem2.Text = "工具";
            // 
            // 重复歌曲扫描ToolStripMenuItem
            // 
            this.重复歌曲扫描ToolStripMenuItem.Name = "重复歌曲扫描ToolStripMenuItem";
            this.重复歌曲扫描ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.重复歌曲扫描ToolStripMenuItem.Text = "重复歌曲扫描";
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.音效ToolStripMenuItem,
            this.视频开关ToolStripMenuItem,
            this.播放模式ToolStripMenuItem,
            this.QQ状态同步ToolStripMenuItem,
            this.音效音乐控制ToolStripMenuItem,
            this.SB开关ToolStripMenuItem});
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem3.Text = "选项";
            // 
            // 音效ToolStripMenuItem
            // 
            this.音效ToolStripMenuItem.Checked = true;
            this.音效ToolStripMenuItem.CheckOnClick = true;
            this.音效ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.音效ToolStripMenuItem.Name = "音效ToolStripMenuItem";
            this.音效ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.音效ToolStripMenuItem.Text = "音效开关";
            // 
            // 视频开关ToolStripMenuItem
            // 
            this.视频开关ToolStripMenuItem.Checked = true;
            this.视频开关ToolStripMenuItem.CheckOnClick = true;
            this.视频开关ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.视频开关ToolStripMenuItem.Name = "视频开关ToolStripMenuItem";
            this.视频开关ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.视频开关ToolStripMenuItem.Text = "视频开关";
            // 
            // 播放模式ToolStripMenuItem
            // 
            this.播放模式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.随机播放ToolStripMenuItem1,
            this.顺序播放ToolStripMenuItem,
            this.单曲循环ToolStripMenuItem});
            this.播放模式ToolStripMenuItem.Name = "播放模式ToolStripMenuItem";
            this.播放模式ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.播放模式ToolStripMenuItem.Text = "播放模式";
            // 
            // 随机播放ToolStripMenuItem1
            // 
            this.随机播放ToolStripMenuItem1.Checked = true;
            this.随机播放ToolStripMenuItem1.CheckOnClick = true;
            this.随机播放ToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.随机播放ToolStripMenuItem1.Name = "随机播放ToolStripMenuItem1";
            this.随机播放ToolStripMenuItem1.Size = new System.Drawing.Size(138, 24);
            this.随机播放ToolStripMenuItem1.Text = "随机播放";
            // 
            // 顺序播放ToolStripMenuItem
            // 
            this.顺序播放ToolStripMenuItem.CheckOnClick = true;
            this.顺序播放ToolStripMenuItem.Name = "顺序播放ToolStripMenuItem";
            this.顺序播放ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.顺序播放ToolStripMenuItem.Text = "顺序播放";
            // 
            // 单曲循环ToolStripMenuItem
            // 
            this.单曲循环ToolStripMenuItem.CheckOnClick = true;
            this.单曲循环ToolStripMenuItem.Name = "单曲循环ToolStripMenuItem";
            this.单曲循环ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.单曲循环ToolStripMenuItem.Text = "单曲循环";
            // 
            // QQ状态同步ToolStripMenuItem
            // 
            this.QQ状态同步ToolStripMenuItem.Checked = true;
            this.QQ状态同步ToolStripMenuItem.CheckOnClick = true;
            this.QQ状态同步ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.QQ状态同步ToolStripMenuItem.Name = "QQ状态同步ToolStripMenuItem";
            this.QQ状态同步ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.QQ状态同步ToolStripMenuItem.Text = "QQ状态同步";
            // 
            // 音效音乐控制ToolStripMenuItem
            // 
            this.音效音乐控制ToolStripMenuItem.Name = "音效音乐控制ToolStripMenuItem";
            this.音效音乐控制ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.音效音乐控制ToolStripMenuItem.Text = "音效音乐控制";
            // 
            // SB开关ToolStripMenuItem
            // 
            this.SB开关ToolStripMenuItem.Name = "SB开关ToolStripMenuItem";
            this.SB开关ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.SB开关ToolStripMenuItem.Text = "SB开关";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(342, 435);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(57, 23);
            this.PlayButton.TabIndex = 3;
            this.PlayButton.Text = "播放";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Location = new System.Drawing.Point(405, 435);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(43, 23);
            this.PreviousButton.TabIndex = 5;
            this.PreviousButton.Text = "←";
            this.PreviousButton.UseVisualStyleBackColor = true;
            // 
            // TrackBar1
            // 
            this.TrackBar1.AutoSize = false;
            this.TrackBar1.Enabled = false;
            this.TrackBar1.Location = new System.Drawing.Point(503, 433);
            this.TrackBar1.Maximum = 1000;
            this.TrackBar1.Name = "TrackBar1";
            this.TrackBar1.Size = new System.Drawing.Size(363, 28);
            this.TrackBar1.TabIndex = 0;
            this.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackBar1_MouseDown);
            this.TrackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar1_MouseUp);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(454, 435);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(43, 23);
            this.NextButton.TabIndex = 6;
            this.NextButton.Text = "→";
            this.NextButton.UseVisualStyleBackColor = true;
            // 
            // LabelQQ
            // 
            this.LabelQQ.AutoSize = true;
            this.LabelQQ.Location = new System.Drawing.Point(149, 38);
            this.LabelQQ.Name = "LabelQQ";
            this.LabelQQ.Size = new System.Drawing.Size(91, 15);
            this.LabelQQ.TabIndex = 8;
            this.LabelQQ.Text = "当前同步QQ:";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(342, 34);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 9;
            this.Button1.Text = "选择QQ";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TrackBar2
            // 
            this.TrackBar2.AutoSize = false;
            this.TrackBar2.Location = new System.Drawing.Point(832, 67);
            this.TrackBar2.Maximum = 100;
            this.TrackBar2.Name = "TrackBar2";
            this.TrackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrackBar2.Size = new System.Drawing.Size(34, 342);
            this.TrackBar2.TabIndex = 10;
            this.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar2.Value = 100;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(829, 412);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(37, 15);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "音量";
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1});
            this.ListView1.FullRowSelect = true;
            this.ListView1.GridLines = true;
            this.ListView1.Location = new System.Drawing.Point(16, 67);
            this.ListView1.MultiSelect = false;
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(316, 360);
            this.ListView1.TabIndex = 0;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            this.ListView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            this.ListView1.DoubleClick += new System.EventHandler(this.ListView1_DoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "";
            this.ColumnHeader1.Width = 1024;
            // 
            // ListBox1
            // 
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.ItemHeight = 15;
            this.ListBox1.Location = new System.Drawing.Point(12, 472);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(320, 199);
            this.ListBox1.TabIndex = 0;
            this.ListBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(12, 433);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(239, 25);
            this.TextBox1.TabIndex = 13;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(257, 433);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 25);
            this.SearchButton.TabIndex = 14;
            this.SearchButton.Text = "搜索";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(625, 38);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(37, 15);
            this.Label3.TabIndex = 16;
            this.Label3.Text = "音效";
            // 
            // TrackBar3
            // 
            this.TrackBar3.AutoSize = false;
            this.TrackBar3.Location = new System.Drawing.Point(668, 34);
            this.TrackBar3.Maximum = 100;
            this.TrackBar3.Name = "TrackBar3";
            this.TrackBar3.Size = new System.Drawing.Size(154, 27);
            this.TrackBar3.TabIndex = 15;
            this.TrackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar3.Value = 60;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(435, 38);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(37, 15);
            this.Label4.TabIndex = 18;
            this.Label4.Text = "音乐";
            // 
            // TrackBar4
            // 
            this.TrackBar4.AutoSize = false;
            this.TrackBar4.Location = new System.Drawing.Point(478, 34);
            this.TrackBar4.Maximum = 100;
            this.TrackBar4.Name = "TrackBar4";
            this.TrackBar4.Size = new System.Drawing.Size(138, 27);
            this.TrackBar4.TabIndex = 17;
            this.TrackBar4.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar4.Value = 80;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(16, 34);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(127, 23);
            this.Button2.TabIndex = 9;
            this.Button2.Text = "选择Collection";
            this.Button2.UseVisualStyleBackColor = true;
            // 
            // ListDetail
            // 
            this.ListDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader2,
            this.ColumnHeader3});
            this.ListDetail.FullRowSelect = true;
            this.ListDetail.GridLines = true;
            this.ListDetail.Location = new System.Drawing.Point(342, 472);
            this.ListDetail.MultiSelect = false;
            this.ListDetail.Name = "ListDetail";
            this.ListDetail.Size = new System.Drawing.Size(524, 199);
            this.ListDetail.TabIndex = 19;
            this.ListDetail.UseCompatibleStateImageBehavior = false;
            this.ListDetail.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Key";
            this.ColumnHeader2.Width = 256;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Value";
            this.ColumnHeader3.Width = 256;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(213, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 25);
            this.button3.TabIndex = 20;
            this.button3.Text = "初始化";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AVsyncer
            // 
            this.AVsyncer.Tick += new System.EventHandler(this.AVsync);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 681);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ListDetail);
            this.Controls.Add(this.TrackBar3);
            this.Controls.Add(this.ListView1);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.ListBox1);
            this.Controls.Add(this.TrackBar2);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.TrackBar4);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.LabelQQ);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.TrackBar1);
            this.Controls.Add(this.PreviousButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AskForExit);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.MenuStrip MenuStrip1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem 运行OSUToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 手动指定OSU目录ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem 重新导入osuToolStripMenuItem7;
		internal System.Windows.Forms.ToolStripMenuItem 重新导入scoresToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 重新导入collectionsToolStripMenuItem9;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripMenuItem 打开曲目文件夹ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 打开铺面文件ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 打开SB文件ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
		internal System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 重复歌曲扫描ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 音效ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
		internal System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 视频开关ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 播放模式ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 随机播放ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem 顺序播放ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 单曲循环ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem QQ状态同步ToolStripMenuItem;
		internal System.Windows.Forms.Button PlayButton;
		internal System.Windows.Forms.Button PreviousButton;
		internal System.Windows.Forms.TrackBar TrackBar1;
		internal System.Windows.Forms.Button NextButton;
		internal System.Windows.Forms.Label LabelQQ;
		internal System.Windows.Forms.Button Button1;
		internal System.Windows.Forms.TrackBar TrackBar2;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.ListBox ListBox1;
		internal System.Windows.Forms.ListView ListView1;
		internal System.Windows.Forms.ToolStripMenuItem 音效音乐控制ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SB开关ToolStripMenuItem;
		internal System.Windows.Forms.TextBox TextBox1;
		internal System.Windows.Forms.Button SearchButton;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TrackBar TrackBar3;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.TrackBar TrackBar4;
		internal System.Windows.Forms.Panel Panel1;
		internal System.Windows.Forms.Button Button2;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ListView ListDetail;
		internal System.Windows.Forms.ColumnHeader ColumnHeader2;
		internal System.Windows.Forms.ColumnHeader ColumnHeader3;
        private Button button3;
        private Timer AVsyncer;
        private System.ComponentModel.IContainer components;
		
	}
	
}
