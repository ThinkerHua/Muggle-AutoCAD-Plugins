/*==============================================================================
 *  Muggle AutoCAD-Plugins - tools and plugins for AutoCAD
 *
 *  Copyright © 2024 Huang YongXing. 
 *
 *  This library is free software, licensed under the terms of the GNU 
 *  General Public License as published by the Free Software Foundation, 
 *  either version 3 of the License, or (at your option) any later version. 
 *  You should have received a copy of the GNU General Public License 
 *  along with this program. If not, see <http://www.gnu.org/licenses/>. 
 *==============================================================================
 *  Form_DWGFilesMerger.Designer.cs: form designer for merger for DWG files
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System.Windows.Forms;

namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.gbox_Method = new System.Windows.Forms.GroupBox();
            this.rbtn_Arranged = new System.Windows.Forms.RadioButton();
            this.rbtn_OriginalPosition = new System.Windows.Forms.RadioButton();
            this.gbox_ArrangementStyle = new System.Windows.Forms.GroupBox();
            this.nud_ColumnSpacing = new System.Windows.Forms.NumericUpDown();
            this.nud_Num = new System.Windows.Forms.NumericUpDown();
            this.nud_RowSpacing = new System.Windows.Forms.NumericUpDown();
            this.lb_ColumnSpacing = new System.Windows.Forms.Label();
            this.lb_Num = new System.Windows.Forms.Label();
            this.lb_RowSpacing = new System.Windows.Forms.Label();
            this.pbox_ByColumns = new System.Windows.Forms.PictureBox();
            this.pbox_ByRows = new System.Windows.Forms.PictureBox();
            this.rbtn_ByColumns = new System.Windows.Forms.RadioButton();
            this.rbtn_ByRows = new System.Windows.Forms.RadioButton();
            this.btn_SaveAs = new System.Windows.Forms.Button();
            this.tbox_TargetFile = new System.Windows.Forms.TextBox();
            this.lb_TargetFile = new System.Windows.Forms.Label();
            this.btn_SelectSourceFloder = new System.Windows.Forms.Button();
            this.tbox_SourceFloder = new System.Windows.Forms.TextBox();
            this.lb_SourceFolder = new System.Windows.Forms.Label();
            this.btn_Execute = new System.Windows.Forms.Button();
            this.gbox_SearchDepth = new System.Windows.Forms.GroupBox();
            this.rbtn_TopDirectory = new System.Windows.Forms.RadioButton();
            this.rbtn_AllDirectory = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gBox_Tag = new System.Windows.Forms.GroupBox();
            this.rbtn_FileName = new System.Windows.Forms.RadioButton();
            this.rbtn_NoTag = new System.Windows.Forms.RadioButton();
            this.rbtn_SequenceNumber = new System.Windows.Forms.RadioButton();
            this.gbox_Method.SuspendLayout();
            this.gbox_ArrangementStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ColumnSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Num)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_RowSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ByColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ByRows)).BeginInit();
            this.gbox_SearchDepth.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gBox_Tag.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbox_Method
            // 
            this.gbox_Method.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.gbox_Method, 2);
            this.gbox_Method.Controls.Add(this.rbtn_Arranged);
            this.gbox_Method.Controls.Add(this.rbtn_OriginalPosition);
            this.gbox_Method.Location = new System.Drawing.Point(4, 148);
            this.gbox_Method.Margin = new System.Windows.Forms.Padding(4);
            this.gbox_Method.Name = "gbox_Method";
            this.gbox_Method.Padding = new System.Windows.Forms.Padding(4);
            this.gbox_Method.Size = new System.Drawing.Size(469, 62);
            this.gbox_Method.TabIndex = 10;
            this.gbox_Method.TabStop = false;
            this.gbox_Method.Text = "合并方式";
            // 
            // rbtn_Arranged
            // 
            this.rbtn_Arranged.AutoSize = true;
            this.rbtn_Arranged.Checked = true;
            this.rbtn_Arranged.Location = new System.Drawing.Point(8, 25);
            this.rbtn_Arranged.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_Arranged.Name = "rbtn_Arranged";
            this.rbtn_Arranged.Size = new System.Drawing.Size(112, 19);
            this.rbtn_Arranged.TabIndex = 11;
            this.rbtn_Arranged.TabStop = true;
            this.rbtn_Arranged.Text = "排列合并(&A)";
            this.rbtn_Arranged.UseVisualStyleBackColor = true;
            this.rbtn_Arranged.CheckedChanged += new System.EventHandler(this.CombineMethodChanged);
            // 
            // rbtn_OriginalPosition
            // 
            this.rbtn_OriginalPosition.AutoSize = true;
            this.rbtn_OriginalPosition.Location = new System.Drawing.Point(260, 25);
            this.rbtn_OriginalPosition.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_OriginalPosition.Name = "rbtn_OriginalPosition";
            this.rbtn_OriginalPosition.Size = new System.Drawing.Size(112, 19);
            this.rbtn_OriginalPosition.TabIndex = 12;
            this.rbtn_OriginalPosition.Text = "原位合并(&O)";
            this.rbtn_OriginalPosition.UseVisualStyleBackColor = true;
            this.rbtn_OriginalPosition.CheckedChanged += new System.EventHandler(this.CombineMethodChanged);
            // 
            // gbox_ArrangementStyle
            // 
            this.gbox_ArrangementStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.gbox_ArrangementStyle, 2);
            this.gbox_ArrangementStyle.Controls.Add(this.nud_ColumnSpacing);
            this.gbox_ArrangementStyle.Controls.Add(this.nud_Num);
            this.gbox_ArrangementStyle.Controls.Add(this.nud_RowSpacing);
            this.gbox_ArrangementStyle.Controls.Add(this.lb_ColumnSpacing);
            this.gbox_ArrangementStyle.Controls.Add(this.lb_Num);
            this.gbox_ArrangementStyle.Controls.Add(this.lb_RowSpacing);
            this.gbox_ArrangementStyle.Controls.Add(this.pbox_ByColumns);
            this.gbox_ArrangementStyle.Controls.Add(this.pbox_ByRows);
            this.gbox_ArrangementStyle.Controls.Add(this.rbtn_ByColumns);
            this.gbox_ArrangementStyle.Controls.Add(this.rbtn_ByRows);
            this.gbox_ArrangementStyle.Location = new System.Drawing.Point(4, 218);
            this.gbox_ArrangementStyle.Margin = new System.Windows.Forms.Padding(4);
            this.gbox_ArrangementStyle.Name = "gbox_ArrangementStyle";
            this.gbox_ArrangementStyle.Padding = new System.Windows.Forms.Padding(4);
            this.gbox_ArrangementStyle.Size = new System.Drawing.Size(469, 144);
            this.gbox_ArrangementStyle.TabIndex = 13;
            this.gbox_ArrangementStyle.TabStop = false;
            this.gbox_ArrangementStyle.Text = "排列形式";
            // 
            // nud_ColumnSpacing
            // 
            this.nud_ColumnSpacing.Location = new System.Drawing.Point(355, 101);
            this.nud_ColumnSpacing.Margin = new System.Windows.Forms.Padding(4);
            this.nud_ColumnSpacing.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_ColumnSpacing.Name = "nud_ColumnSpacing";
            this.nud_ColumnSpacing.Size = new System.Drawing.Size(80, 25);
            this.nud_ColumnSpacing.TabIndex = 21;
            this.nud_ColumnSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_ColumnSpacing.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nud_Num
            // 
            this.nud_Num.Location = new System.Drawing.Point(355, 34);
            this.nud_Num.Margin = new System.Windows.Forms.Padding(4);
            this.nud_Num.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Num.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Num.Name = "nud_Num";
            this.nud_Num.Size = new System.Drawing.Size(80, 25);
            this.nud_Num.TabIndex = 17;
            this.nud_Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Num.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nud_RowSpacing
            // 
            this.nud_RowSpacing.Location = new System.Drawing.Point(355, 68);
            this.nud_RowSpacing.Margin = new System.Windows.Forms.Padding(4);
            this.nud_RowSpacing.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_RowSpacing.Name = "nud_RowSpacing";
            this.nud_RowSpacing.Size = new System.Drawing.Size(80, 25);
            this.nud_RowSpacing.TabIndex = 19;
            this.nud_RowSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_RowSpacing.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_RowSpacing.ValueChanged += new System.EventHandler(this.RowSpacingChanged);
            // 
            // lb_ColumnSpacing
            // 
            this.lb_ColumnSpacing.AutoSize = true;
            this.lb_ColumnSpacing.Location = new System.Drawing.Point(268, 104);
            this.lb_ColumnSpacing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_ColumnSpacing.Name = "lb_ColumnSpacing";
            this.lb_ColumnSpacing.Size = new System.Drawing.Size(76, 15);
            this.lb_ColumnSpacing.TabIndex = 20;
            this.lb_ColumnSpacing.Text = "列间距(&X)";
            // 
            // lb_Num
            // 
            this.lb_Num.AutoSize = true;
            this.lb_Num.Location = new System.Drawing.Point(252, 36);
            this.lb_Num.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_Num.Name = "lb_Num";
            this.lb_Num.Size = new System.Drawing.Size(91, 15);
            this.lb_Num.TabIndex = 16;
            this.lb_Num.Text = "每组数量(&U)";
            // 
            // lb_RowSpacing
            // 
            this.lb_RowSpacing.AutoSize = true;
            this.lb_RowSpacing.Location = new System.Drawing.Point(268, 70);
            this.lb_RowSpacing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_RowSpacing.Name = "lb_RowSpacing";
            this.lb_RowSpacing.Size = new System.Drawing.Size(76, 15);
            this.lb_RowSpacing.TabIndex = 18;
            this.lb_RowSpacing.Text = "行间距(&Y)";
            // 
            // pbox_ByColumns
            // 
            this.pbox_ByColumns.BackgroundImage = global::Muggle.AutoCADPlugins.DWGFilesMerger.Properties.Resources.ArrangementStyle_ByColumn;
            this.pbox_ByColumns.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbox_ByColumns.Location = new System.Drawing.Point(103, 52);
            this.pbox_ByColumns.Margin = new System.Windows.Forms.Padding(4);
            this.pbox_ByColumns.Name = "pbox_ByColumns";
            this.pbox_ByColumns.Size = new System.Drawing.Size(80, 75);
            this.pbox_ByColumns.TabIndex = 3;
            this.pbox_ByColumns.TabStop = false;
            // 
            // pbox_ByRows
            // 
            this.pbox_ByRows.BackgroundImage = global::Muggle.AutoCADPlugins.DWGFilesMerger.Properties.Resources.ArrangementStyle_ByRow;
            this.pbox_ByRows.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbox_ByRows.Location = new System.Drawing.Point(8, 52);
            this.pbox_ByRows.Margin = new System.Windows.Forms.Padding(4);
            this.pbox_ByRows.Name = "pbox_ByRows";
            this.pbox_ByRows.Size = new System.Drawing.Size(80, 75);
            this.pbox_ByRows.TabIndex = 2;
            this.pbox_ByRows.TabStop = false;
            // 
            // rbtn_ByColumns
            // 
            this.rbtn_ByColumns.AutoSize = true;
            this.rbtn_ByColumns.Location = new System.Drawing.Point(103, 25);
            this.rbtn_ByColumns.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_ByColumns.Name = "rbtn_ByColumns";
            this.rbtn_ByColumns.Size = new System.Drawing.Size(82, 19);
            this.rbtn_ByColumns.TabIndex = 15;
            this.rbtn_ByColumns.Text = "按列(&C)";
            this.rbtn_ByColumns.UseVisualStyleBackColor = true;
            // 
            // rbtn_ByRows
            // 
            this.rbtn_ByRows.AutoSize = true;
            this.rbtn_ByRows.Checked = true;
            this.rbtn_ByRows.Location = new System.Drawing.Point(8, 25);
            this.rbtn_ByRows.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_ByRows.Name = "rbtn_ByRows";
            this.rbtn_ByRows.Size = new System.Drawing.Size(82, 19);
            this.rbtn_ByRows.TabIndex = 14;
            this.rbtn_ByRows.TabStop = true;
            this.rbtn_ByRows.Text = "按行(&R)";
            this.rbtn_ByRows.UseVisualStyleBackColor = true;
            // 
            // btn_SaveAs
            // 
            this.btn_SaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SaveAs.Location = new System.Drawing.Point(481, 41);
            this.btn_SaveAs.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SaveAs.Name = "btn_SaveAs";
            this.btn_SaveAs.Size = new System.Drawing.Size(100, 29);
            this.btn_SaveAs.TabIndex = 9;
            this.btn_SaveAs.Text = "选择(&N)";
            this.btn_SaveAs.UseVisualStyleBackColor = true;
            this.btn_SaveAs.Click += new System.EventHandler(this.EditFileName);
            // 
            // tbox_TargetFile
            // 
            this.tbox_TargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbox_TargetFile.Location = new System.Drawing.Point(103, 43);
            this.tbox_TargetFile.Margin = new System.Windows.Forms.Padding(4);
            this.tbox_TargetFile.Name = "tbox_TargetFile";
            this.tbox_TargetFile.Size = new System.Drawing.Size(370, 25);
            this.tbox_TargetFile.TabIndex = 8;
            // 
            // lb_TargetFile
            // 
            this.lb_TargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_TargetFile.AutoSize = true;
            this.lb_TargetFile.Location = new System.Drawing.Point(4, 48);
            this.lb_TargetFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_TargetFile.Name = "lb_TargetFile";
            this.lb_TargetFile.Size = new System.Drawing.Size(91, 15);
            this.lb_TargetFile.TabIndex = 7;
            this.lb_TargetFile.Text = "目标文件(&T)";
            this.lb_TargetFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_SelectSourceFloder
            // 
            this.btn_SelectSourceFloder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SelectSourceFloder.Location = new System.Drawing.Point(481, 4);
            this.btn_SelectSourceFloder.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectSourceFloder.Name = "btn_SelectSourceFloder";
            this.btn_SelectSourceFloder.Size = new System.Drawing.Size(100, 29);
            this.btn_SelectSourceFloder.TabIndex = 3;
            this.btn_SelectSourceFloder.Text = "选择(&F)";
            this.btn_SelectSourceFloder.UseVisualStyleBackColor = true;
            this.btn_SelectSourceFloder.Click += new System.EventHandler(this.SelectFolder);
            // 
            // tbox_SourceFloder
            // 
            this.tbox_SourceFloder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbox_SourceFloder.Location = new System.Drawing.Point(103, 6);
            this.tbox_SourceFloder.Margin = new System.Windows.Forms.Padding(4);
            this.tbox_SourceFloder.Name = "tbox_SourceFloder";
            this.tbox_SourceFloder.Size = new System.Drawing.Size(370, 25);
            this.tbox_SourceFloder.TabIndex = 2;
            // 
            // lb_SourceFolder
            // 
            this.lb_SourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_SourceFolder.AutoSize = true;
            this.lb_SourceFolder.Location = new System.Drawing.Point(4, 11);
            this.lb_SourceFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_SourceFolder.Name = "lb_SourceFolder";
            this.lb_SourceFolder.Size = new System.Drawing.Size(91, 15);
            this.lb_SourceFolder.TabIndex = 1;
            this.lb_SourceFolder.Text = "源目录(&S)";
            this.lb_SourceFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_Execute
            // 
            this.btn_Execute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Execute.Location = new System.Drawing.Point(481, 372);
            this.btn_Execute.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(100, 50);
            this.btn_Execute.TabIndex = 0;
            this.btn_Execute.Text = "执行(&E)";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.Execute);
            // 
            // gbox_SearchDepth
            // 
            this.gbox_SearchDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.gbox_SearchDepth, 2);
            this.gbox_SearchDepth.Controls.Add(this.rbtn_TopDirectory);
            this.gbox_SearchDepth.Controls.Add(this.rbtn_AllDirectory);
            this.gbox_SearchDepth.Location = new System.Drawing.Point(4, 78);
            this.gbox_SearchDepth.Margin = new System.Windows.Forms.Padding(4);
            this.gbox_SearchDepth.Name = "gbox_SearchDepth";
            this.gbox_SearchDepth.Padding = new System.Windows.Forms.Padding(4);
            this.gbox_SearchDepth.Size = new System.Drawing.Size(469, 62);
            this.gbox_SearchDepth.TabIndex = 4;
            this.gbox_SearchDepth.TabStop = false;
            this.gbox_SearchDepth.Text = "搜索深度";
            // 
            // rbtn_TopDirectory
            // 
            this.rbtn_TopDirectory.AutoSize = true;
            this.rbtn_TopDirectory.Checked = true;
            this.rbtn_TopDirectory.Location = new System.Drawing.Point(8, 25);
            this.rbtn_TopDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_TopDirectory.Name = "rbtn_TopDirectory";
            this.rbtn_TopDirectory.Size = new System.Drawing.Size(127, 19);
            this.rbtn_TopDirectory.TabIndex = 5;
            this.rbtn_TopDirectory.TabStop = true;
            this.rbtn_TopDirectory.Text = "仅当前目录(&D)";
            this.rbtn_TopDirectory.UseVisualStyleBackColor = true;
            // 
            // rbtn_AllDirectory
            // 
            this.rbtn_AllDirectory.AutoSize = true;
            this.rbtn_AllDirectory.Location = new System.Drawing.Point(260, 25);
            this.rbtn_AllDirectory.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_AllDirectory.Name = "rbtn_AllDirectory";
            this.rbtn_AllDirectory.Size = new System.Drawing.Size(112, 19);
            this.rbtn_AllDirectory.TabIndex = 6;
            this.rbtn_AllDirectory.Text = "含子目录(&I)";
            this.rbtn_AllDirectory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lb_SourceFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbox_ArrangementStyle, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.gbox_SearchDepth, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbox_Method, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbox_SourceFloder, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_SelectSourceFloder, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_TargetFile, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbox_TargetFile, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_SaveAs, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.gBox_Tag, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btn_Execute, 2, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(586, 427);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // gBox_Tag
            // 
            this.gBox_Tag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.gBox_Tag, 2);
            this.gBox_Tag.Controls.Add(this.rbtn_FileName);
            this.gBox_Tag.Controls.Add(this.rbtn_NoTag);
            this.gBox_Tag.Controls.Add(this.rbtn_SequenceNumber);
            this.gBox_Tag.Location = new System.Drawing.Point(4, 370);
            this.gBox_Tag.Margin = new System.Windows.Forms.Padding(4);
            this.gBox_Tag.Name = "gBox_Tag";
            this.gBox_Tag.Padding = new System.Windows.Forms.Padding(4);
            this.gBox_Tag.Size = new System.Drawing.Size(469, 52);
            this.gBox_Tag.TabIndex = 14;
            this.gBox_Tag.TabStop = false;
            this.gBox_Tag.Text = "标签";
            // 
            // rbtn_FileName
            // 
            this.rbtn_FileName.AutoSize = true;
            this.rbtn_FileName.Location = new System.Drawing.Point(260, 25);
            this.rbtn_FileName.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_FileName.Name = "rbtn_FileName";
            this.rbtn_FileName.Size = new System.Drawing.Size(97, 19);
            this.rbtn_FileName.TabIndex = 1;
            this.rbtn_FileName.Text = "文件名(&L)";
            this.rbtn_FileName.UseVisualStyleBackColor = true;
            // 
            // rbtn_NoTag
            // 
            this.rbtn_NoTag.AutoSize = true;
            this.rbtn_NoTag.Checked = true;
            this.rbtn_NoTag.Location = new System.Drawing.Point(8, 25);
            this.rbtn_NoTag.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_NoTag.Name = "rbtn_NoTag";
            this.rbtn_NoTag.Size = new System.Drawing.Size(67, 19);
            this.rbtn_NoTag.TabIndex = 1;
            this.rbtn_NoTag.TabStop = true;
            this.rbtn_NoTag.Text = "无(&G)";
            this.rbtn_NoTag.UseVisualStyleBackColor = true;
            // 
            // rbtn_SequenceNumber
            // 
            this.rbtn_SequenceNumber.AutoSize = true;
            this.rbtn_SequenceNumber.Location = new System.Drawing.Point(127, 25);
            this.rbtn_SequenceNumber.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_SequenceNumber.Name = "rbtn_SequenceNumber";
            this.rbtn_SequenceNumber.Size = new System.Drawing.Size(82, 19);
            this.rbtn_SequenceNumber.TabIndex = 1;
            this.rbtn_SequenceNumber.Text = "序号(&Q)";
            this.rbtn_SequenceNumber.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 427);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DWG Files Merger";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbox_Method.ResumeLayout(false);
            this.gbox_Method.PerformLayout();
            this.gbox_ArrangementStyle.ResumeLayout(false);
            this.gbox_ArrangementStyle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ColumnSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Num)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_RowSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ByColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ByRows)).EndInit();
            this.gbox_SearchDepth.ResumeLayout(false);
            this.gbox_SearchDepth.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gBox_Tag.ResumeLayout(false);
            this.gBox_Tag.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbox_Method;
        private System.Windows.Forms.Button btn_SaveAs;
        private System.Windows.Forms.TextBox tbox_TargetFile;
        private System.Windows.Forms.Label lb_TargetFile;
        private System.Windows.Forms.Button btn_SelectSourceFloder;
        private System.Windows.Forms.TextBox tbox_SourceFloder;
        private System.Windows.Forms.Label lb_SourceFolder;
        private System.Windows.Forms.RadioButton rbtn_Arranged;
        private System.Windows.Forms.RadioButton rbtn_OriginalPosition;
        private System.Windows.Forms.GroupBox gbox_ArrangementStyle;
        private System.Windows.Forms.PictureBox pbox_ByColumns;
        private System.Windows.Forms.PictureBox pbox_ByRows;
        private System.Windows.Forms.RadioButton rbtn_ByColumns;
        private System.Windows.Forms.RadioButton rbtn_ByRows;
        private System.Windows.Forms.Label lb_ColumnSpacing;
        private System.Windows.Forms.Label lb_RowSpacing;
        private System.Windows.Forms.NumericUpDown nud_ColumnSpacing;
        private System.Windows.Forms.NumericUpDown nud_RowSpacing;
        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.NumericUpDown nud_Num;
        private System.Windows.Forms.Label lb_Num;
        private System.Windows.Forms.GroupBox gbox_SearchDepth;
        private System.Windows.Forms.RadioButton rbtn_TopDirectory;
        private System.Windows.Forms.RadioButton rbtn_AllDirectory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private GroupBox gBox_Tag;
        private RadioButton rbtn_SequenceNumber;
        private RadioButton rbtn_FileName;
        private RadioButton rbtn_NoTag;
    }
}

