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
 *  ExcelOperation.cs: some operations about Excel.
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Autodesk.AutoCAD.DatabaseServices;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Muggle.AutoCADPlugins.Common.ExcelOperation {
    /// <summary>
    /// Excel相关的操作。
    /// </summary>
    public static class ExcelOperation {
        /// <summary>
        /// 单元格（区域）R1C1样式地址匹配模式。<br/>
        /// 分别用 "minRow", "minCol", "maxRow", "maxCol" 匹配组接收起止单元格的行号、列号。
        /// </summary>
        public static readonly string RangeAddressPattern_RC = @"^R(?<minRow>[1-9]\d*)C(?<minCol>[1-9]\d*)(:R(?<maxRow>[1-9]\d*)C(?<maxCol>[1-9]\d*))?$";

        /// <summary>
        /// 获取当前工作表最大行号和列号。
        /// </summary>
        /// <param name="sheet">当前工作表</param>
        /// <returns>最大行号和列号。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static (int MaxRow, int MaxCol) GetMaxRCNum(this Worksheet sheet) {
#if NET48_OR_GREATER
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }
#elif NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(nameof(sheet));
#endif

            var address = sheet.Cells.Address[true, true, XlReferenceStyle.xlR1C1];
            var match = Regex.Match(address, RangeAddressPattern_RC);
            //不必判断 match.Sucess
            _ = int.TryParse(match.Groups["maxRow"].Value, out int maxRow);
            _ = int.TryParse(match.Groups["maxCol"].Value, out int maxCol);

            return (maxRow, maxCol);
        }

        /// <summary>
        /// 获取当前单元格区域的最小行号、最小列号、最大行号、最大列号。
        /// </summary>
        /// <param name="range">当前单元格区域</param>
        /// <returns>当前单元格区域的最小行号、最小列号、最大行号、最大列号。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static (int minRow, int minCol, int maxRow, int maxCol) GetMostRCNum(this Excel.Range range) {
#if NET48_OR_GREATER
            if (range is null) {
                throw new ArgumentNullException(nameof(range));
            }
#elif NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(nameof(range));
#endif

            int minRow = int.MaxValue;
            int maxRow = 1;
            int minCol = int.MaxValue;
            int maxCol = 1;

            var areas = range.Areas;
            foreach (Excel.Range subRange in areas) {
                var address = subRange.Address[true, true, XlReferenceStyle.xlR1C1];
                var match = Regex.Match(address, RangeAddressPattern_RC);
                //不必判断 match.Sucess
                _ = int.TryParse(match.Groups["minRow"].Value, out int sR);
                _ = int.TryParse(match.Groups["minCol"].Value, out int sC);
                _ = int.TryParse(match.Groups["maxRow"].Value, out int eR);
                _ = int.TryParse(match.Groups["maxCol"].Value, out int eC);
                if (eR == 0) eR = sR;
                if (eC == 0) eC = sC;

                minRow = sR < minRow ? sR : minRow;
                minCol = sC < minCol ? sC : minCol;
                maxRow = eR > maxRow ? eR : maxRow;
                maxCol = eC > maxCol ? eC : maxCol;
            }

            return (minRow, minCol, maxRow, maxCol);
        }

        /// <summary>
        /// 将单元格区域转换成边界框集合。
        /// </summary>
        /// <param name="range">单元格区域</param>
        /// <returns>边界框集合。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<Extents2d> RangeToExtents(Excel.Range range) {
#if NET48_OR_GREATER
            if (range is null) {
                throw new ArgumentNullException(nameof(range));
            }
#elif NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(nameof(range));
#endif

            var result = new List<Extents2d>();
            var areas = range.Areas;
            foreach (Excel.Range area in areas) {
                var (minX, minY, maxX, maxY) = ExcelOperation.GetMostRCNum(area);
                result.Add(new Extents2d(minX, minY, maxX, maxY));
            }

            return result;
        }

        /// <summary>
        /// 将边界框集合转换成单元格区域。
        /// </summary>
        /// <param name="sheet">要转换到的工作表</param>
        /// <param name="extents">边界框集合</param>
        /// <returns>单元格区域。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /*public static Range ExtentsToRange(Worksheet sheet, IEnumerable<Extents2d> extents) {
            if (sheet is null) {
                throw new ArgumentNullException(nameof(sheet));
            }

            if (extents is null) {
                throw new ArgumentNullException(nameof(extents));
            }

            var xlApp = sheet.Application;
            var (MaxRowNum, MaxColNum) = sheet.GetMaxRCNum();

            foreach (var e in extents) {
                if (!e.IsValid(false)) continue;

                var minC = (int) Math.Round(e.MinPoint.X);
                var minR = (int) Math.Round(e.MinPoint.Y);
                var maxC = (int) Math.Round(e.MaxPoint.X);
                var maxR = (int) Math.Round(e.MaxPoint.Y);
                minC = minC < 1 ? 1 : minC;
                minR = minR < 1 ? 1 : minR;
                maxC = maxC > MaxColNum ? MaxColNum : maxC;
                maxR = maxR > MaxRowNum ? MaxRowNum : maxR;

                var newExtent = new Extents2d(minC, minR, maxC, maxR);
                var range = sheet.Range[((Range) sheet.Cells[minR, minC]).Address, ((Range) sheet.Cells[maxR, maxC]).Address];
            }

            throw new NotImplementedException();
        }*/

        /// <summary>
        /// 合并单元格但保留所有内容。
        /// </summary>
        /// <param name="range">要合并的单元格区域。
        /// 应当为单个连续区域，即 <see cref="Range.Areas"/> 返回的集合只包含一个对象。</param>
        /// <param name="numberFormatSetToText">将单元格数字格式设置为文本，
        /// 设置为 true 以防止数值被自动转换成意外的格式。默认值 true。</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"><paramref name="range"/> 应当是单个连续区域。</exception>
        public static void MergeKeepContent(Excel.Range range, bool numberFormatSetToText = true) {
#if NET48_OR_GREATER
            if (range is null) {
                throw new ArgumentNullException(nameof(range));
            }
#elif NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(nameof(range));
#endif

            if (range.Areas.Count > 1) {
                throw new ArgumentException($"“{nameof(range)}”应当是单个连续区域。", nameof(range));
            }

            var content = string.Empty;
            foreach (Excel.Range cell in range) {
                if (cell.Value == null) continue;

                content += cell.Value;
                cell.ClearContents();
            }

            range.Merge();
            if (numberFormatSetToText) range.NumberFormat = "@";
            range.Value = content;
        }
    }
}
