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
 *  Form_DWGFilesMerger.cs: user interface of merger for DWG files
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.IO;
using System.Windows.Forms;

namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    public partial class Form_DWGFilesMerger : Form {
        public Form_DWGFilesMerger() {
            InitializeComponent();
        }

        private void CombineMethodChanged(object sender, EventArgs e) {
            if (rbtn_OriginalPosition.Checked) {
                rbtn_ByRows.Enabled = false;
                rbtn_ByColumns.Enabled = false;
                nud_Num.Enabled = false;
                nud_RowSpacing.Enabled = false;
                nud_ColumnSpacing.Enabled = false;
            } else if (rbtn_Arranged.Checked) {
                rbtn_ByRows.Enabled = true;
                rbtn_ByColumns.Enabled = true;
                nud_Num.Enabled = true;
                nud_RowSpacing.Enabled = true;
                nud_ColumnSpacing.Enabled = true;
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
            try {
                DWGFilesMerger.Merge(
                    tbox_SourceFloder.Text,
                    tbox_TargetFile.Text,
                    searchOption,
                    mergerMethod,
                    arrangementStyle,
                    (int) nud_Num.Value,
                    (int) nud_RowSpacing.Value,
                    (int) nud_ColumnSpacing.Value);
            } catch (Exception exc) {
                MessageBox.Show(exc.ToString());
            }

            btn.Text = "执行(&E)";
            btn.Enabled = true;
        }
    }
}
