using System;
using System.IO;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Muggle.AutoCADPlugins.DWGFilesMerger.Model;

namespace Muggle.AutoCADPlugins.DWGFilesMerger.ViewModels {

    public partial class MainWindowViewModel : ObservableObject {

        public MainWindowViewModel() {
            SourceFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DestinationFileFullName = $"{SourceFolder}\\{defaultFileName}";
            NumPerGroup = 10;
            RowSpacing = 100;
            ColSpacing = 100;
        }

        private static readonly string defaultFileName = "Merged.dwg";

        [ObservableProperty]
        private string _sourceFolder;

        [ObservableProperty]
        private string _destinationFileFullName;

        [ObservableProperty]
        private SearchOption _searchDepth;

        [ObservableProperty]
        private WayOfMergerEnum _wayOfMerger;

        [ObservableProperty]
        private ArrangementStyleEnum _arrangementStyle;

        [ObservableProperty]
        private int _numPerGroup;

        [ObservableProperty]
        private int _rowSpacing;

        [ObservableProperty]
        private int _colSpacing;

        [ObservableProperty]
        private TagTypeEnum _tagType;

        partial void OnRowSpacingChanged(int oldValue, int newValue) {
            if (newValue <= 0) TagType = TagTypeEnum.None;
        }

        partial void OnTagTypeChanged(TagTypeEnum oldValue, TagTypeEnum newValue) {
            if (RowSpacing <= 0 && newValue != TagTypeEnum.None) TagType = TagTypeEnum.None;
        }

        [RelayCommand]
        public void SelectFolder() {
            var dialog = new FolderBrowserDialog {
                SelectedPath = SourceFolder,
            };
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                SourceFolder = dialog.SelectedPath;
        }

        [RelayCommand]
        public void ModifyDestinationFile() {
            var dialog = new SaveFileDialog {
                Filter = "DWG Files(*.dwg)|*.dwg",
                FilterIndex = 0,
                OverwritePrompt = true,
                InitialDirectory = Path.GetDirectoryName(DestinationFileFullName),
                FileName = Path.GetFileName(DestinationFileFullName),
            };
            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
                DestinationFileFullName = dialog.FileName;
        }

        [RelayCommand]
        public void Merge() {
            try {
                Merger.Merge(
                    SourceFolder,
                    DestinationFileFullName,
                    SearchDepth,
                    WayOfMerger,
                    ArrangementStyle,
                    NumPerGroup,
                    RowSpacing,
                    ColSpacing,
                    TagType);

                MessageBox.Show("合并成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception e) {
                MessageBox.Show($"合并失败，发生如下错误：\n{e.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
