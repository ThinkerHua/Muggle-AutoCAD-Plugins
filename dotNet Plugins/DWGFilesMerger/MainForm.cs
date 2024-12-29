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
 *  MainForm.cs: user interface of merger for DWG files
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.IO;
using System.Windows.Forms;

namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            nud_RowSpacing.Maximum = int.MaxValue;
            nud_ColumnSpacing.Maximum = int.MaxValue;
        }

        private void CombineMethodChanged(object sender, EventArgs e) {
            if (rbtn_OriginalPosition.Checked) {
                rbtn_ByRows.Enabled = false;
                rbtn_ByColumns.Enabled = false;
                nud_Num.Enabled = false;
                nud_RowSpacing.Enabled = false;
                nud_ColumnSpacing.Enabled = false;
                rbtn_NoTag.Enabled = false;
                rbtn_SequenceNumber.Enabled = false;
                rbtn_FileName.Enabled = false;
            } else if (rbtn_Arranged.Checked) {
                rbtn_ByRows.Enabled = true;
                rbtn_ByColumns.Enabled = true;
                nud_Num.Enabled = true;
                nud_RowSpacing.Enabled = true;
                nud_ColumnSpacing.Enabled = true;
                rbtn_NoTag.Enabled = true;
                rbtn_SequenceNumber.Enabled = true;
                rbtn_FileName.Enabled = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = "Merged.dwg";
            tbox_SourceFloder.Text = folder;
            tbox_TargetFile.Text = $"{folder}\\{fileName}";
        }

        private void SelectFolder(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog {
                SelectedPath = tbox_SourceFloder.Text,
            };
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                tbox_SourceFloder.Text = dialog.SelectedPath;
        }

        private void EditFileName(object sender, EventArgs e) {
            var dialog = new SaveFileDialog {
                Filter = "DWG Files(*.dwg)|*.dwg",
                FilterIndex = 0,
                OverwritePrompt = true,
                InitialDirectory = Path.GetDirectoryName(tbox_TargetFile.Text),
                FileName = Path.GetFileName(tbox_TargetFile.Text),
            };
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
                tbox_TargetFile.Text = dialog.FileName;
        }

        private void Execute(object sender, EventArgs e) {
            var btn = sender as Button;
            btn.Text = "请稍候...";
            btn.Enabled = false;

            var searchOption =
                rbtn_TopDirectory.Checked ? SearchOption.TopDirectoryOnly
                : SearchOption.AllDirectories;
            var mergerMethod =
                rbtn_OriginalPosition.Checked ? DWGFilesMerger.MergerMethodEnum.OriginalPosition
                : DWGFilesMerger.MergerMethodEnum.Arranged;
            var arrangementStyle =
                rbtn_ByRows.Checked ? DWGFilesMerger.ArrangementStyleEnum.ByRows
                : DWGFilesMerger.ArrangementStyleEnum.ByColumns;
            var tagType =
                rbtn_NoTag.Checked ? DWGFilesMerger.TagTypeEnum.None
                : rbtn_SequenceNumber.Checked ? DWGFilesMerger.TagTypeEnum.SequenceNumber
                : DWGFilesMerger.TagTypeEnum.FileName;
            var success = false;
            try {
                DWGFilesMerger.Merge(
                    tbox_SourceFloder.Text,
                    tbox_TargetFile.Text,
                    searchOption,
                    mergerMethod,
                    arrangementStyle,
                    (int) nud_Num.Value,
                    (int) nud_RowSpacing.Value,
                    (int) nud_ColumnSpacing.Value,
                    tagType);
                success = true;
            } catch (Exception exc) {
                MessageBox.Show($"合并失败，发生如下错误：\n{exc}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (success) {
                MessageBox.Show("合并成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btn.Text = "执行(&E)";
            btn.Enabled = true;
        }

        private void RowSpacingChanged(object sender, EventArgs e) {
            if (nud_RowSpacing.Value == 0) rbtn_NoTag.Checked = true;
        }
    }
}
