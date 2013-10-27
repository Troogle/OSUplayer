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
            this.QQ状态同步ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.音效音乐控制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SB开关ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.AVsyncer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListDetail = new System.Windows.Forms.ListView();
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TrackBar3 = new System.Windows.Forms.TrackBar();
            this.PlayList = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SetNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label3 = new System.Windows.Forms.Label();
            this.DiffList = new System.Windows.Forms.ListBox();
            this.TrackBar2 = new System.Windows.Forms.TrackBar();
            this.Label4 = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.LabelQQ = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.TrackBar1 = new System.Windows.Forms.TrackBar();
            this.StopButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.顺序播放ToolStripMenuItem = new OSU_player.ToolStripRadioButtonMenuItem();
            this.单曲循环ToolStripMenuItem = new OSU_player.ToolStripRadioButtonMenuItem();
            this.随机播放ToolStripMenuItem1 = new OSU_player.ToolStripRadioButtonMenuItem();
            this.MenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3,
            this.关于ToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(4, 4);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(884, 28);
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
            this.顺序播放ToolStripMenuItem,
            this.单曲循环ToolStripMenuItem,
            this.随机播放ToolStripMenuItem1});
            this.播放模式ToolStripMenuItem.Name = "播放模式ToolStripMenuItem";
            this.播放模式ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.播放模式ToolStripMenuItem.Text = "播放模式";
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
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.TextBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBar4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.ListDetail);
            this.panel1.Controls.Add(this.TrackBar3);
            this.panel1.Controls.Add(this.PlayList);
            this.panel1.Controls.Add(this.Label3);
            this.panel1.Controls.Add(this.DiffList);
            this.panel1.Controls.Add(this.TrackBar2);
            this.panel1.Controls.Add(this.Label4);
            this.panel1.Controls.Add(this.SearchButton);
            this.panel1.Controls.Add(this.Label2);
            this.panel1.Controls.Add(this.Button2);
            this.panel1.Controls.Add(this.Button1);
            this.panel1.Controls.Add(this.LabelQQ);
            this.panel1.Controls.Add(this.NextButton);
            this.panel1.Controls.Add(this.TrackBar1);
            this.panel1.Controls.Add(this.StopButton);
            this.panel1.Controls.Add(this.PlayButton);
            this.panel1.Location = new System.Drawing.Point(4, 34);
            this.panel1.MinimumSize = new System.Drawing.Size(882, 651);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 651);
            this.panel1.TabIndex = 21;
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
            // trackBar4
            // 
            this.trackBar4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar4.AutoSize = false;
            this.trackBar4.Location = new System.Drawing.Point(599, 7);
            this.trackBar4.Maximum = 100;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(88, 27);
            this.trackBar4.TabIndex = 41;
            this.trackBar4.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar4.Value = 60;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(344, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(480, 360);
            this.panel2.TabIndex = 26;
            // 
            // ListDetail
            // 
            this.ListDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader2,
            this.ColumnHeader3});
            this.ListDetail.FullRowSelect = true;
            this.ListDetail.GridLines = true;
            this.ListDetail.Location = new System.Drawing.Point(344, 445);
            this.ListDetail.MultiSelect = false;
            this.ListDetail.Name = "ListDetail";
            this.ListDetail.Size = new System.Drawing.Size(524, 199);
            this.ListDetail.TabIndex = 40;
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
            // TrackBar3
            // 
            this.TrackBar3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackBar3.AutoSize = false;
            this.TrackBar3.Location = new System.Drawing.Point(736, 7);
            this.TrackBar3.Maximum = 100;
            this.TrackBar3.Name = "TrackBar3";
            this.TrackBar3.Size = new System.Drawing.Size(88, 27);
            this.TrackBar3.TabIndex = 37;
            this.TrackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar3.Value = 60;
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
            this.ColumnHeader1.Width = 315;
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
            this.DiffList.Location = new System.Drawing.Point(14, 445);
            this.DiffList.Name = "DiffList";
            this.DiffList.Size = new System.Drawing.Size(320, 199);
            this.DiffList.TabIndex = 25;
            this.DiffList.SelectedIndexChanged += new System.EventHandler(this.DiffList_SelectedIndexChanged);
            this.DiffList.DoubleClick += new System.EventHandler(this.DiffList_DoubleClick);
            // 
            // TrackBar2
            // 
            this.TrackBar2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackBar2.AutoSize = false;
            this.TrackBar2.Location = new System.Drawing.Point(834, 40);
            this.TrackBar2.Maximum = 100;
            this.TrackBar2.Name = "TrackBar2";
            this.TrackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TrackBar2.Size = new System.Drawing.Size(34, 342);
            this.TrackBar2.TabIndex = 33;
            this.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar2.Value = 100;
            this.TrackBar2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar2_MouseUp);
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
            // TrackBar1
            // 
            this.TrackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackBar1.AutoSize = false;
            this.TrackBar1.Enabled = false;
            this.TrackBar1.Location = new System.Drawing.Point(519, 406);
            this.TrackBar1.Maximum = 1000;
            this.TrackBar1.Name = "TrackBar1";
            this.TrackBar1.Size = new System.Drawing.Size(349, 28);
            this.TrackBar1.TabIndex = 23;
            this.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackBar1_MouseDown);
            this.TrackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar1_MouseUp);
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
            // 顺序播放ToolStripMenuItem
            // 
            this.顺序播放ToolStripMenuItem.CheckOnClick = true;
            this.顺序播放ToolStripMenuItem.Name = "顺序播放ToolStripMenuItem";
            this.顺序播放ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.顺序播放ToolStripMenuItem.Text = "顺序播放";
            this.顺序播放ToolStripMenuItem.Click += new System.EventHandler(this.顺序播放ToolStripMenuItem_Click);
            // 
            // 单曲循环ToolStripMenuItem
            // 
            this.单曲循环ToolStripMenuItem.CheckOnClick = true;
            this.单曲循环ToolStripMenuItem.Name = "单曲循环ToolStripMenuItem";
            this.单曲循环ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.单曲循环ToolStripMenuItem.Text = "单曲循环";
            this.单曲循环ToolStripMenuItem.Click += new System.EventHandler(this.单曲循环ToolStripMenuItem_Click);
            // 
            // 随机播放ToolStripMenuItem1
            // 
            this.随机播放ToolStripMenuItem1.Checked = true;
            this.随机播放ToolStripMenuItem1.CheckOnClick = true;
            this.随机播放ToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.随机播放ToolStripMenuItem1.Name = "随机播放ToolStripMenuItem1";
            this.随机播放ToolStripMenuItem1.Size = new System.Drawing.Size(138, 24);
            this.随机播放ToolStripMenuItem1.Text = "随机播放";
            this.随机播放ToolStripMenuItem1.Click += new System.EventHandler(this.随机播放ToolStripMenuItem1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 693);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.MinimumSize = new System.Drawing.Size(910, 740);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Osu Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AskForExit);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).EndInit();
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
        internal System.Windows.Forms.ToolStripMenuItem QQ状态同步ToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem 音效音乐控制ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SB开关ToolStripMenuItem;
        private Button button3;
        private Timer AVsyncer;
        private System.ComponentModel.IContainer components;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        internal ToolStripRadioButtonMenuItem 随机播放ToolStripMenuItem1;
        internal ToolStripRadioButtonMenuItem 顺序播放ToolStripMenuItem;
        internal ToolStripRadioButtonMenuItem 单曲循环ToolStripMenuItem;
        private Panel panel1;
        internal TextBox TextBox1;
        private Label label1;
        internal TrackBar trackBar4;
        internal Panel panel2;
        internal ListView ListDetail;
        internal ColumnHeader ColumnHeader2;
        internal ColumnHeader ColumnHeader3;
        internal TrackBar TrackBar3;
        internal ListView PlayList;
        internal ColumnHeader ColumnHeader1;
        internal Label Label3;
        internal ListBox DiffList;
        internal TrackBar TrackBar2;
        internal Label Label4;
        internal Button SearchButton;
        internal Label Label2;
        internal Button Button2;
        internal Button Button1;
        internal Label LabelQQ;
        internal Button NextButton;
        internal TrackBar TrackBar1;
        internal Button StopButton;
        internal Button PlayButton;
        private ColumnHeader SetNum;
		
	}
	
}
