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
public partial class Main : System.Windows.Forms.Form
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
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.运行OSU = new System.Windows.Forms.ToolStripMenuItem();
            this.手动指定OSU目录 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.重新导入osu = new System.Windows.Forms.ToolStripMenuItem();
            this.重新导入scores = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.打开曲目文件夹 = new System.Windows.Forms.ToolStripMenuItem();
            this.打开铺面文件 = new System.Windows.Forms.ToolStripMenuItem();
            this.打开SB文件 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.退出 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.重复歌曲扫描 = new System.Windows.Forms.ToolStripMenuItem();
            this.香蕉分析器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.音效 = new System.Windows.Forms.ToolStripMenuItem();
            this.视频开关 = new System.Windows.Forms.ToolStripMenuItem();
            this.播放模式 = new System.Windows.Forms.ToolStripMenuItem();
            this.QQ状态同步 = new System.Windows.Forms.ToolStripMenuItem();
            this.SB开关 = new System.Windows.Forms.ToolStripMenuItem();
            this.关于 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ListDetail = new System.Windows.Forms.ListView();
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ScoreBox = new System.Windows.Forms.ListBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TrackMusic = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TrackFx = new System.Windows.Forms.TrackBar();
            this.PlayList = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SetNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label3 = new System.Windows.Forms.Label();
            this.DiffList = new System.Windows.Forms.ListBox();
            this.TrackVolume = new System.Windows.Forms.TrackBar();
            this.Label4 = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.LabelQQ = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.TrackSeek = new System.Windows.Forms.TrackBar();
            this.StopButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.顺序播放 = new OSU_player.ToolStripRadioButtonMenuItem();
            this.单曲循环 = new OSU_player.ToolStripRadioButtonMenuItem();
            this.随机播放 = new OSU_player.ToolStripRadioButtonMenuItem();
            this.MenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackMusic)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackFx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackSeek)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3,
            this.关于});
            this.MenuStrip1.Location = new System.Drawing.Point(4, 4);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(884, 28);
            this.MenuStrip1.TabIndex = 2;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.运行OSU,
            this.手动指定OSU目录,
            this.ToolStripSeparator1,
            this.重新导入osu,
            this.重新导入scores,
            this.ToolStripSeparator2,
            this.打开曲目文件夹,
            this.打开铺面文件,
            this.打开SB文件,
            this.ToolStripSeparator3,
            this.退出});
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem1.Text = "文件";
            // 
            // 运行OSU
            // 
            this.运行OSU.Name = "运行OSU";
            this.运行OSU.Size = new System.Drawing.Size(209, 24);
            this.运行OSU.Text = "运行OSU!";
            this.运行OSU.Click += new System.EventHandler(this.运行OSU_Click);
            // 
            // 手动指定OSU目录
            // 
            this.手动指定OSU目录.Name = "手动指定OSU目录";
            this.手动指定OSU目录.Size = new System.Drawing.Size(209, 24);
            this.手动指定OSU目录.Text = "手动指定OSU目录";
            this.手动指定OSU目录.Click += new System.EventHandler(this.手动指定OSU目录_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // 重新导入osu
            // 
            this.重新导入osu.Name = "重新导入osu";
            this.重新导入osu.Size = new System.Drawing.Size(209, 24);
            this.重新导入osu.Text = "重新初始化";
            this.重新导入osu.Click += new System.EventHandler(this.重新导入osu_Click);
            // 
            // 重新导入scores
            // 
            this.重新导入scores.Name = "重新导入scores";
            this.重新导入scores.Size = new System.Drawing.Size(209, 24);
            this.重新导入scores.Text = "重新导入scores.db";
            this.重新导入scores.Click += new System.EventHandler(this.重新导入scores_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            // 
            // 打开曲目文件夹
            // 
            this.打开曲目文件夹.Name = "打开曲目文件夹";
            this.打开曲目文件夹.Size = new System.Drawing.Size(209, 24);
            this.打开曲目文件夹.Text = "打开曲目文件夹";
            this.打开曲目文件夹.Click += new System.EventHandler(this.打开曲目文件夹_Click);
            // 
            // 打开铺面文件
            // 
            this.打开铺面文件.Name = "打开铺面文件";
            this.打开铺面文件.Size = new System.Drawing.Size(209, 24);
            this.打开铺面文件.Text = "打开铺面文件";
            this.打开铺面文件.Click += new System.EventHandler(this.打开铺面文件_Click);
            // 
            // 打开SB文件
            // 
            this.打开SB文件.Name = "打开SB文件";
            this.打开SB文件.Size = new System.Drawing.Size(209, 24);
            this.打开SB文件.Text = "打开SB文件";
            this.打开SB文件.Click += new System.EventHandler(this.打开SB文件_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            // 
            // 退出
            // 
            this.退出.Name = "退出";
            this.退出.Size = new System.Drawing.Size(209, 24);
            this.退出.Text = "退出";
            this.退出.Click += new System.EventHandler(this.退出_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重复歌曲扫描,
            this.香蕉分析器ToolStripMenuItem});
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem2.Text = "工具";
            // 
            // 重复歌曲扫描
            // 
            this.重复歌曲扫描.Name = "重复歌曲扫描";
            this.重复歌曲扫描.Size = new System.Drawing.Size(168, 24);
            this.重复歌曲扫描.Text = "重复歌曲扫描";
            this.重复歌曲扫描.Click += new System.EventHandler(this.重复歌曲扫描_Click);
            // 
            // 香蕉分析器ToolStripMenuItem
            // 
            this.香蕉分析器ToolStripMenuItem.Enabled = false;
            this.香蕉分析器ToolStripMenuItem.Name = "香蕉分析器ToolStripMenuItem";
            this.香蕉分析器ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.香蕉分析器ToolStripMenuItem.Text = "香蕉分析器";
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.音效,
            this.视频开关,
            this.播放模式,
            this.QQ状态同步,
            this.SB开关});
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(51, 24);
            this.ToolStripMenuItem3.Text = "选项";
            // 
            // 音效
            // 
            this.音效.Checked = true;
            this.音效.CheckOnClick = true;
            this.音效.CheckState = System.Windows.Forms.CheckState.Checked;
            this.音效.Name = "音效";
            this.音效.Size = new System.Drawing.Size(162, 24);
            this.音效.Text = "音效开关";
            this.音效.Click += new System.EventHandler(this.音效_Click);
            // 
            // 视频开关
            // 
            this.视频开关.Checked = true;
            this.视频开关.CheckOnClick = true;
            this.视频开关.CheckState = System.Windows.Forms.CheckState.Checked;
            this.视频开关.Name = "视频开关";
            this.视频开关.Size = new System.Drawing.Size(162, 24);
            this.视频开关.Text = "视频开关";
            this.视频开关.Click += new System.EventHandler(this.视频开关_Click);
            // 
            // 播放模式
            // 
            this.播放模式.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.顺序播放,
            this.单曲循环,
            this.随机播放});
            this.播放模式.Name = "播放模式";
            this.播放模式.Size = new System.Drawing.Size(162, 24);
            this.播放模式.Text = "播放模式";
            // 
            // QQ状态同步
            // 
            this.QQ状态同步.Checked = true;
            this.QQ状态同步.CheckOnClick = true;
            this.QQ状态同步.CheckState = System.Windows.Forms.CheckState.Checked;
            this.QQ状态同步.Name = "QQ状态同步";
            this.QQ状态同步.Size = new System.Drawing.Size(162, 24);
            this.QQ状态同步.Text = "QQ状态同步";
            this.QQ状态同步.Click += new System.EventHandler(this.QQ状态同步_Click);
            // 
            // SB开关
            // 
            this.SB开关.Enabled = false;
            this.SB开关.Name = "SB开关";
            this.SB开关.Size = new System.Drawing.Size(162, 24);
            this.SB开关.Text = "SB开关";
            this.SB开关.Click += new System.EventHandler(this.SB开关_Click);
            // 
            // 关于
            // 
            this.关于.Name = "关于";
            this.关于.Size = new System.Drawing.Size(51, 24);
            this.关于.Text = "关于";
            this.关于.Click += new System.EventHandler(this.关于_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.TextBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TrackMusic);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.TrackFx);
            this.panel1.Controls.Add(this.PlayList);
            this.panel1.Controls.Add(this.Label3);
            this.panel1.Controls.Add(this.DiffList);
            this.panel1.Controls.Add(this.TrackVolume);
            this.panel1.Controls.Add(this.Label4);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.Label2);
            this.panel1.Controls.Add(this.Button2);
            this.panel1.Controls.Add(this.Button1);
            this.panel1.Controls.Add(this.LabelQQ);
            this.panel1.Controls.Add(this.NextButton);
            this.panel1.Controls.Add(this.TrackSeek);
            this.panel1.Controls.Add(this.StopButton);
            this.panel1.Controls.Add(this.PlayButton);
            this.panel1.Location = new System.Drawing.Point(4, 34);
            this.panel1.MinimumSize = new System.Drawing.Size(882, 651);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 663);
            this.panel1.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(340, 442);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(524, 214);
            this.tabControl1.TabIndex = 43;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ListDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(516, 185);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ListDetail
            // 
            this.ListDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader2,
            this.ColumnHeader3});
            this.ListDetail.FullRowSelect = true;
            this.ListDetail.GridLines = true;
            this.ListDetail.Location = new System.Drawing.Point(6, 6);
            this.ListDetail.MultiSelect = false;
            this.ListDetail.Name = "ListDetail";
            this.ListDetail.Size = new System.Drawing.Size(504, 173);
            this.ListDetail.TabIndex = 40;
            this.ListDetail.UseCompatibleStateImageBehavior = false;
            this.ListDetail.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Key";
            this.ColumnHeader2.Width = 120;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Value";
            this.ColumnHeader3.Width = 300;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ScoreBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(516, 185);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "成绩信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ScoreBox
            // 
            this.ScoreBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScoreBox.FormattingEnabled = true;
            this.ScoreBox.ItemHeight = 15;
            this.ScoreBox.Location = new System.Drawing.Point(6, 6);
            this.ScoreBox.Name = "ScoreBox";
            this.ScoreBox.Size = new System.Drawing.Size(504, 169);
            this.ScoreBox.TabIndex = 0;
            // 
            // TextBox1
            // 
            this.TextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox1.Location = new System.Drawing.Point(14, 406);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(239, 25);
            this.TextBox1.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(434, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 42;
            this.label1.Text = "00:00/00:00";
            // 
            // TrackMusic
            // 
            this.TrackMusic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackMusic.AutoSize = false;
            this.TrackMusic.Location = new System.Drawing.Point(599, 7);
            this.TrackMusic.Maximum = 100;
            this.TrackMusic.Name = "TrackMusic";
            this.TrackMusic.Size = new System.Drawing.Size(88, 27);
            this.TrackMusic.TabIndex = 41;
            this.TrackMusic.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackMusic.Value = 80;
            this.TrackMusic.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(344, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(480, 360);
            this.panel2.TabIndex = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 360);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TrackFx
            // 
            this.TrackFx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackFx.AutoSize = false;
            this.TrackFx.Location = new System.Drawing.Point(736, 7);
            this.TrackFx.Maximum = 100;
            this.TrackFx.Name = "TrackFx";
            this.TrackFx.Size = new System.Drawing.Size(88, 27);
            this.TrackFx.TabIndex = 37;
            this.TrackFx.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackFx.Value = 60;
            this.TrackFx.Scroll += new System.EventHandler(this.TrackBar3_Scroll);
            // 
            // PlayList
            // 
            this.PlayList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.SetNum});
            this.PlayList.FullRowSelect = true;
            this.PlayList.GridLines = true;
            this.PlayList.Location = new System.Drawing.Point(14, 40);
            this.PlayList.MultiSelect = false;
            this.PlayList.Name = "PlayList";
            this.PlayList.Size = new System.Drawing.Size(320, 360);
            this.PlayList.TabIndex = 24;
            this.PlayList.UseCompatibleStateImageBehavior = false;
            this.PlayList.View = System.Windows.Forms.View.Details;
            this.PlayList.SelectedIndexChanged += new System.EventHandler(this.PlayList_SelectedIndexChanged);
            this.PlayList.DoubleClick += new System.EventHandler(this.PlayList_DoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "";
            this.ColumnHeader1.Width = 500;
            // 
            // SetNum
            // 
            this.SetNum.Text = "SetNum";
            this.SetNum.Width = 0;
            // 
            // Label3
            // 
            this.Label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(693, 11);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(37, 15);
            this.Label3.TabIndex = 38;
            this.Label3.Text = "音效";
            // 
            // DiffList
            // 
            this.DiffList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiffList.FormattingEnabled = true;
            this.DiffList.ItemHeight = 15;
            this.DiffList.Location = new System.Drawing.Point(14, 442);
            this.DiffList.Name = "DiffList";
            this.DiffList.Size = new System.Drawing.Size(320, 214);
            this.DiffList.TabIndex = 25;
            this.DiffList.SelectedIndexChanged += new System.EventHandler(this.DiffList_SelectedIndexChanged);
            this.DiffList.DoubleClick += new System.EventHandler(this.DiffList_DoubleClick);
            // 
            // TrackVolume
            // 
            this.TrackVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackVolume.AutoSize = false;
            this.TrackVolume.Location = new System.Drawing.Point(834, 40);
            this.TrackVolume.Maximum = 100;
            this.TrackVolume.Name = "TrackVolume";
            this.TrackVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrackVolume.Size = new System.Drawing.Size(34, 342);
            this.TrackVolume.TabIndex = 33;
            this.TrackVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackVolume.Value = 100;
            this.TrackVolume.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(556, 11);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(37, 15);
            this.Label4.TabIndex = 39;
            this.Label4.Text = "音乐";
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(259, 406);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 25);
            this.SearchButton.TabIndex = 36;
            this.SearchButton.Text = "搜索";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(831, 385);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(37, 15);
            this.Label2.TabIndex = 34;
            this.Label2.Text = "音量";
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(14, 7);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(127, 23);
            this.Button2.TabIndex = 32;
            this.Button2.Text = "选择Collection";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(344, 7);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 31;
            this.Button1.Text = "选择QQ";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // LabelQQ
            // 
            this.LabelQQ.AutoSize = true;
            this.LabelQQ.Location = new System.Drawing.Point(151, 11);
            this.LabelQQ.Name = "LabelQQ";
            this.LabelQQ.Size = new System.Drawing.Size(91, 15);
            this.LabelQQ.TabIndex = 30;
            this.LabelQQ.Text = "当前同步QQ:";
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NextButton.Location = new System.Drawing.Point(470, 408);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(43, 23);
            this.NextButton.TabIndex = 29;
            this.NextButton.Text = "→";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // TrackSeek
            // 
            this.TrackSeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackSeek.AutoSize = false;
            this.TrackSeek.Enabled = false;
            this.TrackSeek.Location = new System.Drawing.Point(519, 406);
            this.TrackSeek.Maximum = 1000;
            this.TrackSeek.Name = "TrackSeek";
            this.TrackSeek.Size = new System.Drawing.Size(349, 28);
            this.TrackSeek.TabIndex = 23;
            this.TrackSeek.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackSeek.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // StopButton
            // 
            this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(407, 408);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(57, 23);
            this.StopButton.TabIndex = 28;
            this.StopButton.Text = "停止";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayButton.Location = new System.Drawing.Point(344, 408);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(57, 23);
            this.PlayButton.TabIndex = 27;
            this.PlayButton.Text = "播放";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // 顺序播放
            // 
            this.顺序播放.CheckOnClick = true;
            this.顺序播放.Name = "顺序播放";
            this.顺序播放.Size = new System.Drawing.Size(138, 24);
            this.顺序播放.Text = "顺序播放";
            this.顺序播放.Click += new System.EventHandler(this.顺序播放_Click);
            // 
            // 单曲循环
            // 
            this.单曲循环.CheckOnClick = true;
            this.单曲循环.Name = "单曲循环";
            this.单曲循环.Size = new System.Drawing.Size(138, 24);
            this.单曲循环.Text = "单曲循环";
            this.单曲循环.Click += new System.EventHandler(this.单曲循环_Click);
            // 
            // 随机播放
            // 
            this.随机播放.Checked = true;
            this.随机播放.CheckOnClick = true;
            this.随机播放.CheckState = System.Windows.Forms.CheckState.Checked;
            this.随机播放.Name = "随机播放";
            this.随机播放.Size = new System.Drawing.Size(138, 24);
            this.随机播放.Text = "随机播放";
            this.随机播放.Click += new System.EventHandler(this.随机播放_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 705);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.MinimumSize = new System.Drawing.Size(910, 752);
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Osu Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AskForExit);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TrackMusic)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackFx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackSeek)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.MenuStrip MenuStrip1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem 运行OSU;
		internal System.Windows.Forms.ToolStripMenuItem 手动指定OSU目录;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem 重新导入osu;
        internal System.Windows.Forms.ToolStripMenuItem 重新导入scores;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripMenuItem 打开曲目文件夹;
		internal System.Windows.Forms.ToolStripMenuItem 打开铺面文件;
		internal System.Windows.Forms.ToolStripMenuItem 打开SB文件;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
		internal System.Windows.Forms.ToolStripMenuItem 关于;
		internal System.Windows.Forms.ToolStripMenuItem 重复歌曲扫描;
		internal System.Windows.Forms.ToolStripMenuItem 音效;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
		internal System.Windows.Forms.ToolStripMenuItem 退出;
		internal System.Windows.Forms.ToolStripMenuItem 视频开关;
        internal System.Windows.Forms.ToolStripMenuItem 播放模式;
        internal System.Windows.Forms.ToolStripMenuItem QQ状态同步;
        internal System.Windows.Forms.ToolStripMenuItem SB开关;
        private System.ComponentModel.IContainer components;
        internal ToolStripRadioButtonMenuItem 随机播放;
        internal ToolStripRadioButtonMenuItem 顺序播放;
        internal ToolStripRadioButtonMenuItem 单曲循环;
        private Panel panel1;
        internal TextBox TextBox1;
        private Label label1;
        internal TrackBar TrackMusic;
        internal Panel panel2;
        internal ListView ListDetail;
        internal ColumnHeader ColumnHeader2;
        internal ColumnHeader ColumnHeader3;
        internal TrackBar TrackFx;
        internal ListView PlayList;
        internal ColumnHeader ColumnHeader1;
        internal Label Label3;
        internal ListBox DiffList;
        internal TrackBar TrackVolume;
        internal Label Label4;
        internal Button SearchButton;
        internal Label Label2;
        internal Button Button2;
        internal Button Button1;
        internal Label LabelQQ;
        internal Button NextButton;
        internal TrackBar TrackSeek;
        internal Button StopButton;
        internal Button PlayButton;
        private ColumnHeader SetNum;
        private ToolStripMenuItem 香蕉分析器ToolStripMenuItem;
        private PictureBox pictureBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListBox ScoreBox;
		
	}
	
}
