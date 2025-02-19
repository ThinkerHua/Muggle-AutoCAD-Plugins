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
 *  ISheetExtension.cs: extension for NPOI.HSSF.UserModel.ISheet
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Muggle.AutoCADPlugins.Common.NPOIExtensions {
    /// <summary>
    /// <see cref="ISheet"/> 的扩展。
    /// </summary>
    public static class ISheetExtension {
        /// <summary>
        /// 获取当前工作表指定行列处的单元格。
        /// </summary>
        /// <param name="sheet">当前工作表</param>
        /// <param name="rowIndex">指定行序号</param>
        /// <param name="colIndex">指定列序号</param>
        /// <returns>当前工作表指定行列处的单元格。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);

            return row.GetCell(colIndex) ?? row.CreateCell(colIndex);
        }

        /// <summary>
        /// 获取当前工作表中指定单元格所在的合并单元格区域序号。
        /// </summary>
        /// <param name="sheet">当前工作表</param>
        /// <param name="cell">指定单元格</param>
        /// <returns>当前工作表中指定单元格所在的合并单元格区域序号。
        /// 如果指定单元格不在合并单元格区域内，则返回 -1。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int GetMergedRegionIndex(this ISheet sheet, ICell cell) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            if (cell is null) {
                throw new ArgumentNullException(nameof(cell));
            }

            //不要执行这行，其底层实现也是在表格中查找
            //if (!cell.IsMergedCell) return -1;

            for (int i = 0; i < sheet.MergedRegions.Count; i++) {
                if (sheet.MergedRegions[i].IsInRange(cell.RowIndex, cell.ColumnIndex)) return i;
            }

            return -1;
        }

        /// <summary>
        /// 获取当前工作表中指定行列处的单元格所在的合并单元格区域序号。
        /// </summary>
        /// <param name="sheet">当前工作</param>
        /// <param name="rowIndex">指定行序号</param>
        /// <param name="colIndex">指定列序号</param>
        /// <returns>当前工作表中指定行列处的单元格所在的合并单元格区域序号。
        /// 如果指定行列处的单元格未定义，或不在合并单元格区域内，则返回 -1。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int GetMergedRegionIndex(this ISheet sheet, int rowIndex, int colIndex) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            var row = sheet.GetRow(rowIndex);
            if (row == null) return -1;
            var cell = row.GetCell(colIndex);
            if (cell == null) return -1;

            for (int i = 0; i < sheet.MergedRegions.Count; i++) {
                if (sheet.MergedRegions[i].IsInRange(rowIndex, colIndex)) return i;
            }

            return -1;
        }

        /// <summary>
        /// 合并单元格区域但保留原有文本（所有文本拼接）。
        /// </summary>
        /// <remarks>执行此方法前，最好先执行 <see cref="SelectRange(ISheet, int, int, int, int)"/> 方法，
        /// 确保要合并的区域不与其他合并单元格区域冲突。
        /// </remarks>
        /// <param name="sheet">当前工作表</param>
        /// <param name="region">要合并的单元格区域</param>
        /// <returns>合并单元格区域的序号。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int AddMergedRegionButKeepValue(this ISheet sheet, CellRangeAddress region) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            if (region is null) {
                throw new ArgumentNullException(nameof(region));
            }

            var content = string.Empty;
            var newRow = false;
            for (int row = region.MinRow; row <= region.MaxRow; row++) {
                for (int col = region.MinColumn; col <= region.MaxColumn; col++) {
                    var mergedIndex = sheet.GetMergedRegionIndex(row, col);
                    if (mergedIndex > -1) {
                        sheet.RemoveMergedRegion(mergedIndex);
                    }

                    var cell = sheet.GetCell(row, col);

                    if (!string.IsNullOrEmpty(cell.StringCellValue)) {
                        if (newRow) {
                            newRow = false;
                            content += Environment.NewLine;
                        }

                        content += cell.StringCellValue;
                        //不应使用此方法，此方法会连带格式（如框线等）一并清除
                        //sheet.GetRow(row).RemoveCell(cell);
                        cell.SetCellValue(default(string));
                    }
                }

                newRow = true;
            }

            var firstCell = sheet.GetCell(region.MinRow, region.MinColumn);
            firstCell.SetCellValue(content);

            var index = sheet.AddMergedRegion(region);
            return index;
        }

        /// <summary>
        /// 合并单元格区域但保留原有文本（所有文本拼接）。
        /// </summary>
        /// <remarks>执行此方法前，最好先执行 <see cref="SelectRange(ISheet, int, int, int, int)"/> 方法，
        /// 确保要合并的区域不与其他合并单元格区域冲突。
        /// </remarks>
        /// <param name="sheet">当前工作表</param>
        /// <param name="firstRow">指定区域起始行序号</param>
        /// <param name="lastRow">指定区域终止行序号</param>
        /// <param name="firstCol">指定区域起始列序号</param>
        /// <param name="lastCol">指定区域终止列序号</param>
        /// <returns>合并单元格区域的序号。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int AddMergedRegionButKeepValue(this ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            return AddMergedRegionButKeepValue(sheet, new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
        }

        /// <summary>
        /// 选择当前工作表中指定的区域。
        /// </summary>
        /// <remarks>此方法获取的是受合并单元格区域影响的最大区域。例如：<br/>
        /// A4:B5、D5:E6、B7:C8、E8:F9 各自均为合并单元格区域，此方法选择 C6:D7，
        /// 返回的区域将是 A4:F9。
        /// </remarks>
        /// <param name="sheet">当前工作表</param>
        /// <param name="firstRow">指定区域起始行序号</param>
        /// <param name="lastRow">指定区域终止行序号</param>
        /// <param name="firstCol">指定区域起始列序号</param>
        /// <param name="lastCol">指定区域终止列序号</param>
        /// <returns>受合并单元格区域影响的最大区域。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">指定的行列序号不能小于 0。</exception>
        /// <exception cref="ArgumentException">指定的行列序号应升充排列。</exception>
        public static CellRangeAddress SelectRange(this ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            if (firstRow < 0) {
                throw new ArgumentOutOfRangeException(nameof(firstRow), firstRow, "不能小于 0。");
            }

            if (lastRow < 0) {
                throw new ArgumentOutOfRangeException(nameof(lastRow), lastRow, "不能小于 0。");
            }

            if (firstCol < 0) {
                throw new ArgumentOutOfRangeException(nameof(firstCol), firstCol, "不能小于 0。");
            }

            if (lastCol < 0) {
                throw new ArgumentOutOfRangeException(nameof(lastCol), lastCol, "不能小于 0。");
            }

            if (firstRow > lastRow) {
                throw new ArgumentException($"{nameof(firstRow)}不能大于{nameof(lastRow)}。", nameof(firstRow));
            }

            if (firstCol > lastCol) {
                throw new ArgumentException($"{nameof(firstCol)}不能大于{nameof(lastCol)}。", nameof(firstCol));
            }

            var checkedFirstRow = -1;
            var checkedLastRow = -1;
            var checkedFirstCol = -1;
            var checkedLastCol = -1;

            var needToSelectedFirstRow = firstRow;
            var needToSelectedLastRow = lastRow;
            var needToSelectedFirstCol = firstCol;
            var needToSelectedLastCol = lastCol;

            static bool InRange(int rowIndex, int colIndex, int firstRow, int lastRow, int firstCol, int lastCol) {
                return rowIndex >= firstRow && rowIndex <= lastRow && colIndex >= firstCol && colIndex <= lastCol;
            }

            do {
                for (int r = firstRow; r <= lastRow; r++) {
                    for (int c = firstCol; c <= lastCol; c++) {
                        if (InRange(r, c, checkedFirstRow, checkedLastRow, checkedFirstCol, checkedLastCol)) {
                            c = checkedLastCol;
                            continue;
                        }

                        var cell = sheet.GetCell(r, c);
                        if (!cell.IsMergedCell) continue;

                        var region = HSSFSheet.GetMergedRegion((HSSFSheet) sheet, r, (short) c);
                        needToSelectedFirstRow = Math.Min(needToSelectedFirstRow, region.FirstRow);
                        needToSelectedLastRow = Math.Max(needToSelectedLastRow, region.LastRow);
                        needToSelectedFirstCol = Math.Min(needToSelectedFirstCol, region.FirstColumn);
                        needToSelectedLastCol = Math.Max(needToSelectedLastCol, region.LastColumn);
                    }
                }


                checkedFirstRow = firstRow;
                checkedLastRow = lastRow;
                checkedFirstCol = firstCol;
                checkedLastCol = lastCol;
                firstRow = needToSelectedFirstRow;
                lastRow = needToSelectedLastRow;
                firstCol = needToSelectedFirstCol;
                lastCol = needToSelectedLastCol;
            } while (needToSelectedFirstRow != checkedFirstRow || needToSelectedLastRow != checkedLastRow || needToSelectedFirstCol != checkedFirstCol || needToSelectedLastCol != checkedLastCol);

            return new CellRangeAddress(needToSelectedFirstRow, needToSelectedLastRow, needToSelectedFirstCol, needToSelectedLastCol);
        }
    }
}
