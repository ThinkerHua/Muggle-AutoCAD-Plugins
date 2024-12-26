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
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Muggle.AutoCADPlugins.Common.ExcelOperation;
using Excel = Microsoft.Office.Interop.Excel;

namespace Muggle.AutoCADPlugins.ExportTableToExcel {
    /// <summary>
    /// 在AutoCAD与Excel之间进行转换的中间体。
    /// </summary>
    public class ExchangeTable {
        /// <summary>
        /// 表格水平边框。
        /// Key 为边框 Y 坐标；
        /// Value 为边框线集合，元素的 X1, X2 字段分别表示此段边框线的起始、终止 X 坐标。
        /// </summary>
        public Dictionary<double, List<(double X1, double X2)>> BorderHorizontal { get; set; } = new Dictionary<double, List<(double X1, double X2)>>();

        /// <summary>
        /// 表格垂直边框。
        /// Key 为边框 X 坐标；
        /// Value 为边框线集合，元素的 Y1, Y2 字段分别表示此段边框线的起始、终止 Y 坐标。
        /// </summary>
        public Dictionary<double, List<(double Y1, double Y2)>> BorderVertical { get; set; } = new Dictionary<double, List<(double Y1, double Y2)>>();

        /// <summary>
        /// 表格文本。
        /// 元素的 X, Y 字段表示此文本边界框左上角的坐标值，
        /// Text 字段表示此文本内容。
        /// </summary>
        public List<(double X, double Y, string Text)> Texts { get; set; } = new List<(double X, double Y, string Text)>();

        /// <summary>
        /// 将当前表格导出到指定文件。
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <param name="mergeCells">是否合并单元格</param>
        /// <exception cref="ArgumentException">
        /// <see cref="BorderHorizontal"/> 或 <see cref="BorderVertical"/> 为空集合。
        /// </exception>
        /// <exception cref="Exception">
        /// 未在本机上查找到“Excel”应用程序，或打开“Excel”应用程序失败。
        /// </exception>
        public void ExprotToExcel(string filePath, bool mergeCells) {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentException($"“{nameof(filePath)}”不能为 null 或空。", nameof(filePath));
            }

            if (BorderHorizontal.Count == 0) {
                throw new ArgumentException($"“{nameof(BorderHorizontal)}”不能为空。", nameof(BorderHorizontal));
            }

            if (BorderVertical.Count == 0) {
                throw new ArgumentException($"“{nameof(BorderVertical)}”不能为空。", nameof(BorderVertical));
            }

            var xlType = Type.GetTypeFromProgID("Excel.Application") ?? throw new Exception("未在本机上查找到“Excel”应用程序。");

            Excel.Application xlApp = null;
            Excel.Workbook xlBook = null;
            Excel.Worksheet xlSheet = null;
            bool newFile = !File.Exists(filePath);
            bool newApp = false;
            bool newBook = false;

            #region 获取xlApp
            try {
                //已打开Excel
                xlApp = (Excel.Application) Marshal.GetActiveObject("Excel.Application");

                if (!newFile) {
                    foreach (Workbook book in xlApp.Workbooks) {
                        //已打开此文件
                        if (book.FullName == filePath) {
                            xlBook = book;
                            break;
                        }
                    }
                }

                //没有打开此文件，不使用已打开的程序，需要新开一个程序
                if (xlBook == null) xlApp = null;
            } catch (COMException) {

            }

            //未打开Excel，新开一个
            if (xlApp == null) {
                xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
                xlApp.Visible = false;
                xlApp.SheetsInNewWorkbook = 1;

                newApp = true;
            }

            //无论是否是新开的程序，都关闭警告和刷新屏幕
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            #endregion

            #region 获取xlBook
            if (xlBook == null) {
                if (!newFile) {
                    xlBook = xlApp.Workbooks.Open(filePath);
                } else {
                    xlBook = xlApp.Workbooks.Add();
                }

                newBook = true;
            }
            #endregion

            #region 获取xlSheet
            //找到空表格作为工作表
            foreach (Excel.Worksheet sheet in xlBook.Worksheets) {
                var usedRange = sheet.UsedRange;
                if (usedRange.Count == 1 && usedRange.Formula == string.Empty) {
                    xlSheet = sheet;
                    break;
                }
            }

            //没有空表格，新建一个
            if (xlSheet == null) {
                xlSheet = xlBook.Sheets.Add(After: xlBook.Sheets[xlBook.Sheets.Count]);
            }

            xlSheet.Cells.NumberFormat = "@";//单元格格式设置为文本，防止某些数据变成日期
            #endregion

            try {
                //行降序排列，列升序排列
                var orderedHOR = BorderHorizontal.OrderByDescending(item => item.Key).ToArray();
                var orderedVER = BorderVertical.OrderBy(item => item.Key).ToArray();
                var orderedTXT = Texts.OrderBy(item => item.X).ThenByDescending(item => item.Y).ToArray();

                Range range, cell;
                //输出文字
                foreach (var (X, Y, Text) in orderedTXT) {
                    var row = Array.FindIndex(orderedHOR, item => item.Key < Y);
                    if (row <= 0) continue;//超出表格范围
                    var col = Array.FindIndex(orderedVER, item => item.Key > X);
                    if (col <= 0) continue;//超出表格范围

                    cell = xlSheet.Cells[row, col] as Range;
                    cell.Formula += Text;
                }

                //设置横边框
                for (int i = 0; i < orderedHOR.Length; i++) {
                    var row = i + 1;
                    foreach (var (X1, X2) in orderedHOR[i].Value) {
                        var minX = X1 < X2 ? X1 : X2;
                        var maxX = X1 < X2 ? X2 : X1;
                        var col1 = Array.FindIndex(orderedVER, item => item.Key > minX);
                        if (col1 <= 0) continue;
                        var col2 = Array.FindIndex(orderedVER, item => item.Key >= maxX);
                        if (col2 <= 0) col2 = orderedVER.Length - 1;

                        var cell1 = xlSheet.Cells[row, col1] as Range;
                        var cell2 = xlSheet.Cells[row, col2] as Range;
                        range = xlSheet.Range[cell1.Address, cell2.Address];

                        var borderEdgeTop = range.Borders[XlBordersIndex.xlEdgeTop];
                        borderEdgeTop.LineStyle = XlLineStyle.xlContinuous;
                        borderEdgeTop.Weight = XlBorderWeight.xlThin;
                    }
                }

                //设置竖边框
                for (int i = 0; i < orderedVER.Length; i++) {
                    var col = i + 1;
                    foreach (var (Y1, Y2) in orderedVER[i].Value) {
                        var minY = Y1 < Y2 ? Y1 : Y2;
                        var maxY = Y1 < Y2 ? Y2 : Y1;
                        var row1 = Array.FindIndex(orderedHOR, item => item.Key < maxY);
                        if (row1 <= 0) continue;
                        var row2 = Array.FindIndex(orderedHOR, item => item.Key <= minY);
                        if (row2 <= 0) row2 = orderedHOR.Length - 1;

                        var cell1 = xlSheet.Cells[row1, col] as Range;
                        var cell2 = xlSheet.Cells[row2, col] as Range;
                        range = xlSheet.Range[cell1.Address, cell2.Address];

                        var borderEdgeLeft = range.Borders[XlBordersIndex.xlEdgeLeft];
                        borderEdgeLeft.LineStyle = XlLineStyle.xlContinuous;
                        borderEdgeLeft.Weight = XlBorderWeight.xlThin;
                    }
                }

                //合并单元格
                //=====这段代码取得的表格范围不正确=====
                //range = xlSheet.UsedRange;
                //var (minRow, minCol, maxRow, maxCol) = range.GetMostRCNum();
                //======================================
                if (!mergeCells) goto SkipMergeCells;

                var minRow = 1;
                var minCol = 1;
                var maxRow = orderedHOR.Length - 1;
                var maxCol = orderedVER.Length - 1;
                for (var row = minRow; row <= maxRow; row++) {
                    for (var col = minCol + 1; col <= maxCol; col++) {
                        cell = xlSheet.Cells[row, col];
                        if ((XlLineStyle) cell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle != XlLineStyle.xlLineStyleNone)
                            continue;

                        int col2;
                        for (col2 = col + 1; col2 <= maxCol; ++col2) {
                            cell = xlSheet.Cells[row, col2];
                            if ((XlLineStyle) cell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle != XlLineStyle.xlLineStyleNone)
                                break;
                        }

                        range = xlSheet.Range[((Range) xlSheet.Cells[row, col - 1]).Address, ((Range) xlSheet.Cells[row, col2 - 1]).Address];
                        range.Select();
                        range = xlApp.Selection;
                        ExcelOperation.MergeKeepContent(range);

                        col = col2;
                    }
                }

                for (var col = minCol; col <= maxCol; col++) {
                    for (var row = minRow + 1; row <= maxRow; row++) {
                        cell = xlSheet.Cells[row, col];

                        if ((XlLineStyle) cell.Borders[XlBordersIndex.xlEdgeTop].LineStyle != XlLineStyle.xlLineStyleNone)
                            continue;

                        int row2;
                        for (row2 = row + 1; row2 <= maxRow; row2++) {
                            cell = xlSheet.Cells[row2, col];
                            if ((XlLineStyle) cell.Borders[XlBordersIndex.xlEdgeTop].LineStyle != XlLineStyle.xlLineStyleNone)
                                break;
                        }

                        range = xlSheet.Range[((Range) xlSheet.Cells[row - 1, col]).Address, ((Range) xlSheet.Cells[row2 - 1, col]).Address];
                        range.Select();
                        range = xlApp.Selection;
                        ExcelOperation.MergeKeepContent(range);

                        row = row2;
                    }
                }

            SkipMergeCells:;

            } catch (Exception) {

                throw;
            } finally {
                //是新工作薄则保存关闭，否则不处理
                if (newBook) {
                    xlBook.SaveAs(filePath, FileFormat: XlFileFormat.xlExcel8);
                    xlBook.Close();
                }

                xlApp.DisplayAlerts = true;
                xlApp.ScreenUpdating = true;
                //是新开程序则退出，否则不处理
                if (newApp) {
                    xlApp.Quit();
                }
            }
        }

    }
}
