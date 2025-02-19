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
 *  ExchangeTable.cs: exchange table between AutoCAD and Excel
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Muggle.AutoCADPlugins.Common.NPOIExtensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using WinForm = System.Windows.Forms;

namespace Muggle.AutoCADPlugins.ExportTableToExcel {
    /// <summary>
    /// 文本信息。
    /// </summary>
    public class TextInfo {
        /// <summary>
        /// 文本二维边界框。
        /// </summary>
        public (double minX, double minY, double maxX, double maxY) Extents { get; set; }
        /// <summary>
        /// 文本高度。
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 文本内容。
        /// </summary>
        public string Text { get; set; }
    }
    /// <summary>
    /// 在AutoCAD与Excel之间进行转换的中间体。
    /// </summary>
    public class ExchangeTable {
        private const double EPSILON = 0.000001;
        /// <summary>
        /// 表格水平边框。
        /// Key 为边框 Y 坐标；
        /// Value 为边框线集合，元素的 X1, X2 字段分别表示此段边框线的起始、终止 X 坐标。
        /// </summary>
        public Dictionary<double, List<(double X1, double X2)>> BorderHorizontal { get; } = new Dictionary<double, List<(double X1, double X2)>>();

        /// <summary>
        /// 表格垂直边框。
        /// Key 为边框 X 坐标；
        /// Value 为边框线集合，元素的 Y1, Y2 字段分别表示此段边框线的起始、终止 Y 坐标。
        /// </summary>
        public Dictionary<double, List<(double Y1, double Y2)>> BorderVertical { get; } = new Dictionary<double, List<(double Y1, double Y2)>>();

        /// <summary>
        /// 表格文本。
        /// </summary>
        public List<TextInfo> TextInfos { get; } = new List<TextInfo>();

        /// <summary>
        /// 将当前表格导出到指定文件。
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <param name="mergeCells">是否合并单元格</param>
        /// <exception cref="ArgumentException">
        /// <see cref="BorderHorizontal"/> 或 <see cref="BorderVertical"/> 为空集合。
        /// </exception>
        public void ExprotToXls(string filePath, bool mergeCells = false) {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentException($"“{nameof(filePath)}”不能为 null 或空。", nameof(filePath));
            }

            if (BorderHorizontal.Count == 0) {
                throw new ArgumentException($"“{nameof(BorderHorizontal)}”不能为空。", nameof(BorderHorizontal));
            }

            if (BorderVertical.Count == 0) {
                throw new ArgumentException($"“{nameof(BorderVertical)}”不能为空。", nameof(BorderVertical));
            }

            try {
                //线条端点对齐（某些表格线条端点不对齐）
                var xValues = BorderVertical.Keys.OrderBy(x => x).ToArray();
                var yValues = BorderHorizontal.Keys.OrderByDescending(y => y).ToArray();
                foreach (var item in BorderHorizontal) {
                    for (int i = 0; i < item.Value.Count; i++) {
                        var (X1, X2) = item.Value[i];
                        var x1 = xValues.OrderBy(x => Math.Abs(x - X1)).First();
                        var x2 = xValues.OrderBy(x => Math.Abs(x - X2)).First();
                        item.Value[i] = (x1, x2);
                    }
                }
                foreach (var item in BorderVertical) {
                    for (int i = 0; i < item.Value.Count; i++) {
                        var (Y1, Y2) = item.Value[i];
                        var y1 = yValues.OrderBy(y => Math.Abs(y - Y1)).First();
                        var y2 = yValues.OrderBy(y => Math.Abs(y - Y2)).First();
                        item.Value[i] = (y1, y2);
                    }
                }

                //碎线条归整
                static void GroupPieces(Dictionary<double, List<(double, double)>> keyValuePairs) {
                    foreach (var item in keyValuePairs) {
                        var newValue = UnionOfIntervals(item.Value);
                        item.Value.Clear();
                        item.Value.AddRange(newValue);
                    }
                }
                GroupPieces(BorderHorizontal);
                GroupPieces(BorderVertical);

                //行降序排列，列升序排列
                var orderedHBValues = BorderHorizontal.OrderByDescending(item => item.Key).ToArray();
                var orderedVBValues = BorderVertical.OrderBy(item => item.Key).ToArray();

                //文字按行列序号分组并排列
                static bool LessOrEqual(double a, double b) => Math.Abs(a - b) <= EPSILON || a < b;
                var groupOrderedTXT = TextInfos.GroupBy(
                    item => Array.FindLastIndex(xValues, value => LessOrEqual(value, item.Extents.minX)),
                    (colIndex, colGroup) => new {
                        colIndex,
                        ColGroup = colGroup.GroupBy(
                            item => Array.FindLastIndex(yValues, value => LessOrEqual(item.Extents.maxY, value)),
                            (rowIndex, rowGroup) => new {
                                rowIndex,
                                RowGroup = rowGroup.OrderBy(item => item.Extents.minX).ThenByDescending(item => item.Extents.maxY)
                            })
                    });

                //读取或新建工作簿
                HSSFWorkbook xlBook = null;
                if (File.Exists(filePath)) {
                    do {
                        try {
                            using var fileStream = new FileStream(filePath, FileMode.Open);
                            xlBook = new HSSFWorkbook(fileStream);
                        } catch (IOException) {
                            var result = WinForm.MessageBox.Show(
                                "文件被占用，请关闭占用程序重试。",
                                "错误",
                                WinForm.MessageBoxButtons.RetryCancel,
                                WinForm.MessageBoxIcon.Error);

                            if (result != WinForm.DialogResult.Retry) {
                                throw new Exception("已取消操作。");
                            }
                        }
                    } while (xlBook == null);
                } else {
                    xlBook = new HSSFWorkbook();
                }

                //新建工作表
                var sheetNames = new List<string>();
                foreach (var sheet in xlBook) {
                    sheetNames.Add(sheet.SheetName);
                }
                var sheetIndex = 0;
                do {
                    sheetIndex++;
                } while (sheetNames.Exists(name => name == $"Sheet{sheetIndex}"));
                var xlSheet = xlBook.CreateSheet($"Sheet{sheetIndex}");

                //设置页面属性
                xlSheet.PrintSetup.Landscape = false;
                xlSheet.PrintSetup.PaperSize = (short) PaperSize.A4_Small;
                xlSheet.FitToPage = false;
                xlSheet.SetMargin(MarginType.TopMargin, 1.5 / 2.55);
                xlSheet.SetMargin(MarginType.BottomMargin, 1.5 / 2.55);
                xlSheet.SetMargin(MarginType.LeftMargin, 2 / 2.55);
                xlSheet.SetMargin(MarginType.RightMargin, 1.5 / 2.55);
                xlSheet.SetMargin(MarginType.HeaderMargin, 1 / 2.55);
                xlSheet.SetMargin(MarginType.FooterMargin, 1 / 2.55);

                //输出文字
                foreach (var groupX in groupOrderedTXT) {
                    var col = groupX.colIndex;
                    foreach (var groupY in groupX.ColGroup) {
                        var row = groupY.rowIndex;
                        var txt = string.Join("", groupY.RowGroup.Select(item => item.Text));
                        var cell = xlSheet.GetCell(row, col);
                        cell.SetCellValue(txt);
                    }
                }

                //设置横边框
                for (int rIndex = 0; rIndex < yValues.Length; rIndex++) {
                    foreach (var (X1, X2) in orderedHBValues[rIndex].Value) {
                        var col1 = Array.FindIndex(xValues, value => value == X1);
                        var col2 = Array.FindIndex(xValues, value => value == X2);
                        if (col2 == xValues.Length - 1) col2--;

                        for (int col = col1; col <= col2; col++) {
                            var cell = xlSheet.GetCell(rIndex, col);
                            CellUtil.SetCellStyleProperty(cell, CellUtil.BORDER_TOP, BorderStyle.Thin);
                        }
                    }
                }

                //设置竖边框
                for (int cIndex = 0; cIndex < xValues.Length; cIndex++) {
                    foreach (var (Y1, Y2) in orderedVBValues[cIndex].Value) {
                        var row1 = Array.FindIndex(yValues, value => value == Y2);
                        var row2 = Array.FindIndex(yValues, value => value == Y1);
                        if (row2 == yValues.Length - 1) row2--;

                        for (int row = row1; row <= row2; row++) {
                            var cell = xlSheet.GetCell(row, cIndex);
                            CellUtil.SetCellStyleProperty(cell, CellUtil.BORDER_LEFT, BorderStyle.Thin);
                        }
                    }
                }

                //表格中使用数量最多的字高
                var fontHeight = TextInfos.GroupBy(item => item.Height).OrderByDescending(item => item.Count()).First().First().Height;
                //设置列宽
                for (int c = 0; c < xValues.Length - 1; c++) {
                    var width = xValues[c + 1] - xValues[c];
                    xlSheet.SetColumnWidth(c, width / fontHeight * 2 * 256);
                }
                //设置行高
                for (int r = 0; r < yValues.Length - 1; r++) {
                    var height = yValues[r] - yValues[r + 1];
                    var row = xlSheet.GetRow(r) ?? xlSheet.CreateRow(r);
                    row.HeightInPoints = (float) (height / fontHeight) * 10;
                }

                //合并单元格
                if (!mergeCells) goto SkipMergeCells;
                //左右合并
                for (int c = 1; c < xValues.Length - 1; c++) {
                    for (int r = 0; r < yValues.Length - 1; r++) {
                        if (!orderedVBValues[c].Value.Exists(value => value.Y1 < yValues[r] && value.Y2 >= yValues[r])) {
                            var range = xlSheet.SelectRange(r, r, c - 1, c);
                            xlSheet.AddMergedRegionButKeepValue(range);
                        }
                    }
                }
                //上下合并
                for (int r = 1; r < yValues.Length - 1; r++) {
                    for (int c = 0; c < xValues.Length - 1; c++) {
                        if (!orderedHBValues[r].Value.Exists(value => value.X1 <= xValues[c] && value.X2 > xValues[c])) {
                            var range = xlSheet.SelectRange(r - 1, r, c, c);
                            xlSheet.AddMergedRegionButKeepValue(range);
                        }
                    }
                }


            SkipMergeCells:;

                //保存
                var saved = false;
                do {
                    try {
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        xlBook.Write(fileStream);
                        saved = true;
                    } catch (IOException) {
                        var result = WinForm.MessageBox.Show(
                            "文件被占用，请关闭占用程序重试。",
                            "错误",
                            WinForm.MessageBoxButtons.RetryCancel,
                            WinForm.MessageBoxIcon.Error);

                        if (result != WinForm.DialogResult.Retry) {
                            throw new Exception("已取消操作。");
                        }
                    }
                } while (saved == false);

            } catch {
                throw;
            }
        }

        internal static List<(double s, double e)> UnionOfIntervals(
            List<(double s, double e)> intervals,
            double epsilon = 0) {

            //先将所有区间转换为左端点小于右端点的形式
            for (int i = 0; i < intervals.Count; i++) {
                if (intervals[i].e < intervals[i].s) intervals[i] = (intervals[i].e, intervals[i].s);
            }

            //先按左端点排序，再按右端点排序
            var ordered = intervals.OrderBy(item => item.s).ThenBy(item => item.e).ToArray();

            var result = new List<(double s, double e)>();
            var start = ordered[0].s;
            var end = ordered[0].e;
            for (int i = 1; i < intervals.Count; i++) {
                if (ordered[i].s - end > epsilon) {
                    result.Add((start, end));
                    start = ordered[i].s;
                    end = ordered[i].e;
                } else {
                    end = ordered[i].e > end ? ordered[i].e : end;
                }
            }
            result.Add((start, end));

            return result;
        }
    }
}
