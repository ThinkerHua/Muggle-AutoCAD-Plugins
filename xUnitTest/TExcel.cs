using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Xunit;
using Xunit.Abstractions;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelOperation = Muggle.AutoCADPlugins.Common.ExcelOperation.ExcelOperation;

namespace xUnitTest {
    public class TExcel {
        private ITestOutputHelper testOutput;
        public TExcel(ITestOutputHelper testOutputHelper) {
            testOutput = testOutputHelper;
        }

        [Fact]
        public void TUsedRange() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Excel.Worksheet;
            var range = xlSheet.UsedRange;

            Console.WriteLine($"UsedRange = {range.Address}, Count = {range.Count}, Content is null = {range.Value is null}");

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果：
             *  UsedRange = $A$1, Count = 1, Content is null = True
             */
        }
        [Fact]
        public void TSheetsIndex() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();

            try {
                var xlSheet = xlBook.Sheets[1] as Worksheet;
                Console.WriteLine("索引从1开始。");
                Console.WriteLine($"Sheet's name = {xlSheet.Name}");
            } catch {
                Console.WriteLine("索引从0开始。");
            }

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果：
             *  索引从1开始。
             *  Sheet's name = Sheet1
             */
        }
        [Fact]
        public void TMergeArea() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            var range = xlSheet.Cells[2, 2] as Excel.Range;
            var mergeArea = range.MergeArea;

            Console.WriteLine($"Range = {range.Address}");
            Console.WriteLine($"MergeArea = {mergeArea.Address}");
            Console.WriteLine($"(Range == MergeArea) is {range == mergeArea}");
            Console.WriteLine($"(Range.Address == MergeArea.Address) is {range.Address == mergeArea.Address}");
            Console.WriteLine($"Range.Equals(MergeArea) is {range.Equals(mergeArea)}");

            range = xlSheet.Range[((Excel.Range) xlSheet.Cells[1, 1]).Address, ((Excel.Range) xlSheet.Cells[2, 2]).Address];
            range.Merge(false);
            mergeArea = ((Excel.Range) xlSheet.Cells[2, 2]).MergeArea;

            Console.WriteLine();
            Console.WriteLine($"Range = {range.Address}");
            Console.WriteLine($"MergeArea = {mergeArea.Address}");
            Console.WriteLine($"(Range == MergeArea) is {range == mergeArea}");
            Console.WriteLine($"(Range.Address == MergeArea.Address) is {range.Address == mergeArea.Address}");
            Console.WriteLine($"Range.Equals(MergeArea) is {range.Equals(mergeArea)}");

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果：
             *  Range = $B$2
             *  MergeArea = $B$2
             *  (Range == MergeArea) is False
             *  (Range.Address == MergeArea.Address) is True
             *  Range.Equals(MergeArea) is False
             *  
             *  Range = $A$1:$B$2
             *  MergeArea = $A$1:$B$2
             *  (Range == MergeArea) is False
             *  (Range.Address == MergeArea.Address) is True
             *  Range.Equals(MergeArea) is False
             */
        }
        [Fact]
        public void TOffset() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            var range = xlSheet.Cells[1, 1] as Excel.Range;
            try {
                var offset = range.Offset[-1, -1];
                Console.WriteLine($"Offseted cell is {offset.Address}");
            } catch (COMException) {
                Console.WriteLine($"超出索引范围！");
            }

            xlBook.Close();
            xlApp.Quit();
        }
        [Fact]
        public void TMergeMethod() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            xlSheet.Cells[1, 1] = 11; xlSheet.Cells[1, 2] = 12; xlSheet.Cells[1, 3] = 13; xlSheet.Cells[1, 4] = 14;
            xlSheet.Cells[2, 1] = 21; xlSheet.Cells[2, 2] = 22; xlSheet.Cells[2, 3] = 23; xlSheet.Cells[2, 4] = 24;
            xlSheet.Cells[3, 1] = 31; xlSheet.Cells[3, 2] = 32; xlSheet.Cells[3, 3] = 33; xlSheet.Cells[3, 4] = 34;
            xlSheet.Cells[4, 1] = 41; xlSheet.Cells[4, 2] = 42; xlSheet.Cells[4, 3] = 43; xlSheet.Cells[4, 4] = 44;
            xlSheet.Range[((Excel.Range) xlSheet.Cells[1, 1]).Address, ((Excel.Range) xlSheet.Cells[2, 2]).Address].Merge(false);
            xlSheet.Range[((Excel.Range) xlSheet.Cells[3, 3]).Address, ((Excel.Range) xlSheet.Cells[4, 4]).Address].Merge(false);
            xlSheet.Range[((Excel.Range) xlSheet.Cells[2, 2]).Address, ((Excel.Range) xlSheet.Cells[3, 3]).Address].Merge(false);

            xlApp.DisplayAlerts = true;
            xlApp.ScreenUpdating = true;
            xlApp.Visible = true;

            /*
             *  显示内容：
             *  A1:D4被合并，合并单元格值为11，取消合并单元格后，除A1单元格外，其余单元格内容为空
             */
        }
        [Fact]
        public void TMergeKeepContent() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            xlSheet.Cells[1, 1] = "=11"; xlSheet.Cells[1, 2] = "=12"; xlSheet.Cells[1, 3] = "=13"; xlSheet.Cells[1, 4] = "=14";
            xlSheet.Cells[2, 1] = "=21"; xlSheet.Cells[2, 2] = "=22"; xlSheet.Cells[2, 3] = "=23"; xlSheet.Cells[2, 4] = "=24";
            xlSheet.Cells[3, 1] = "=31"; xlSheet.Cells[3, 2] = "=32"; xlSheet.Cells[3, 3] = "=33"; xlSheet.Cells[3, 4] = "=34";
            xlSheet.Cells[4, 1] = "=41"; xlSheet.Cells[4, 2] = "=42"; xlSheet.Cells[4, 3] = "=43"; xlSheet.Cells[4, 4] = "=44";
            ExcelOperation.MergeKeepContent(xlSheet.Range[((Excel.Range) xlSheet.Cells[1, 1]).Address, ((Excel.Range) xlSheet.Cells[4, 4]).Address]);

            xlApp.DisplayAlerts = true;
            xlApp.ScreenUpdating = true;
            xlApp.Visible = true;

            /*
             *  显示内容：
             *  A1:D4被合并，合并单元格值为“11121314212223243132333441424344”；
             *  取消合并单元格后，除A1单元格外，其余单元格内容为空。
             */
        }
        [Fact]
        public void THowDoesForeachInRangeWork() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            ((Excel.Range) xlSheet.Cells[1, 1]).Value = 1; ((Excel.Range) xlSheet.Cells[1, 2]).Value = 4; ((Excel.Range) xlSheet.Cells[1, 3]).Value = 7;
            ((Excel.Range) xlSheet.Cells[2, 1]).Value = 2; ((Excel.Range) xlSheet.Cells[2, 2]).Value = 5; ((Excel.Range) xlSheet.Cells[2, 3]).Value = 8;
            ((Excel.Range) xlSheet.Cells[3, 1]).Value = 3; ((Excel.Range) xlSheet.Cells[3, 2]).Value = 6; ((Excel.Range) xlSheet.Cells[3, 3]).Value = 9;

            var str = string.Empty;
            foreach (Excel.Range cell in xlSheet.UsedRange) {
                str += cell.Value + ", ";
            }
            Console.WriteLine(str);

            /*
             *  输出结果：
             *  1, 4, 7, 2, 5, 8, 3, 6, 9, 
             */

            var range1 = xlSheet.Range[((Excel.Range) xlSheet.Cells[4, 1]).Address, ((Excel.Range) xlSheet.Cells[6, 3]).Address];
            range1.Merge();
            range1.Value = "range1";
            var range2 = xlSheet.Range[((Excel.Range) xlSheet.Cells[8, 5]).Address, ((Excel.Range) xlSheet.Cells[10, 7]).Address];
            range2.Merge();
            range2.Value = "range2";
            ((Excel.Range) xlSheet.Cells[4, 4]).Value = "4, 4";
            ((Excel.Range) xlSheet.Cells[4, 5]).Value = "4, 5";
            ((Excel.Range) xlSheet.Cells[4, 6]).Value = "4, 6";
            ((Excel.Range) xlSheet.Cells[4, 7]).Value = "4, 7";

            ((Excel.Range) xlSheet.Cells[5, 4]).Value = "5, 4";
            ((Excel.Range) xlSheet.Cells[5, 5]).Value = "5, 5";
            ((Excel.Range) xlSheet.Cells[5, 6]).Value = "5, 6";
            ((Excel.Range) xlSheet.Cells[5, 7]).Value = "5, 7";

            ((Excel.Range) xlSheet.Cells[6, 4]).Value = "6, 4";
            ((Excel.Range) xlSheet.Cells[6, 5]).Value = "6, 5";
            ((Excel.Range) xlSheet.Cells[6, 6]).Value = "6, 6";
            ((Excel.Range) xlSheet.Cells[6, 7]).Value = "6, 7";

            ((Excel.Range) xlSheet.Cells[7, 1]).Value = "7, 1";
            ((Excel.Range) xlSheet.Cells[7, 2]).Value = "7, 2";
            ((Excel.Range) xlSheet.Cells[7, 3]).Value = "7, 3";
            ((Excel.Range) xlSheet.Cells[7, 4]).Value = "7, 4";
            ((Excel.Range) xlSheet.Cells[7, 5]).Value = "7, 5";
            ((Excel.Range) xlSheet.Cells[7, 6]).Value = "7, 6";
            ((Excel.Range) xlSheet.Cells[7, 7]).Value = "7, 7";

            ((Excel.Range) xlSheet.Cells[8, 1]).Value = "8, 1";
            ((Excel.Range) xlSheet.Cells[8, 2]).Value = "8, 2";
            ((Excel.Range) xlSheet.Cells[8, 3]).Value = "8, 3";
            ((Excel.Range) xlSheet.Cells[8, 4]).Value = "8, 4";

            ((Excel.Range) xlSheet.Cells[9, 1]).Value = "9, 1";
            ((Excel.Range) xlSheet.Cells[9, 2]).Value = "9, 2";
            ((Excel.Range) xlSheet.Cells[9, 3]).Value = "9, 3";
            ((Excel.Range) xlSheet.Cells[9, 4]).Value = "9, 4";

            ((Excel.Range) xlSheet.Cells[10, 1]).Value = "10, 1";
            ((Excel.Range) xlSheet.Cells[10, 2]).Value = "10, 2";
            ((Excel.Range) xlSheet.Cells[10, 3]).Value = "10, 3";
            ((Excel.Range) xlSheet.Cells[10, 4]).Value = "10, 4";

            str = string.Empty;
            var range = xlSheet.Range[((Excel.Range) xlSheet.Cells[4, 1]).Address, ((Excel.Range) xlSheet.Cells[10, 7]).Address];
            foreach (Excel.Range cell in range) {
                str += '<' + (string) cell.Value + '>' + ", ";
            }
            Console.WriteLine(str);

            /*
             *  输出结果：
             *  <range1>, , , <4, 4>, <4, 5>, <4, 6>, <4, 7>,
             *  , , , <5, 4>, <5, 5>, <5, 6>, <5, 7>,
             *  , , , <6, 4>, <6, 5>, <6, 6>, <6, 7>,
             *  <7, 1>, <7, 2>, <7, 3>, <7, 4>, <7, 5>, <7, 6>, <7, 7>,
             *  <8, 1>, <8, 2>, <8, 3>, <8, 4>, <range2>, , ,
             *  <9, 1>, <9, 2>, <9, 3>, <9, 4>, , , ,
             *  <10, 1>, <10, 2>, <10, 3>, <10, 4>, , , , 
             */

            xlBook.Close();
            xlApp.Quit();
        }
        [Fact]
        public void TEmptyCellValue() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            Console.WriteLine(((Excel.Range) xlSheet.Cells[1, 1]).Value is null);
            Console.WriteLine((string) ((Excel.Range) xlSheet.Cells[1, 1]).Value == string.Empty);
            Console.WriteLine(((Excel.Range) xlSheet.Cells[1, 1]).Formula is null);
            Console.WriteLine((string) ((Excel.Range) xlSheet.Cells[1, 1]).Formula == string.Empty);

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果
             *  True
             *  False
             *  False
             *  True
             */
        }
        [Fact]
        public void TGetRCNumRange() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            Excel.Range range = xlSheet.Range[((Excel.Range) xlSheet.Cells[1, 3]).Address, ((Excel.Range) xlSheet.Cells[3, 4]).Address];
            range.Merge(false);
            range = xlSheet.Range[((Excel.Range) ((Excel.Range) xlSheet.Cells[4, 1])).Address, ((Excel.Range) xlSheet.Cells[6, 2]).Address];
            range.Merge(false);

            range = xlSheet.Range[((Excel.Range) xlSheet.Cells[3, 2]).Address, ((Excel.Range) xlSheet.Cells[4, 3]).Address];

            Excel.Range newRange = null;
            foreach (Excel.Range cell in range) {
                if (newRange == null) newRange = cell;
                var mergeArea = cell.MergeArea;
                if (mergeArea.Address != cell.Address)
                    newRange = xlApp.Union(newRange, mergeArea);
                else
                    newRange = xlApp.Union(newRange, cell);
            }
            range = newRange;
            Console.WriteLine($"Range.Address = {range.Address}");

            var (minRow, minCol, maxRow, maxCol) = ExcelOperation.GetMostRCNum(range);


            Console.WriteLine($"{minRow}, {minCol}, {maxRow}, {maxCol}");

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果：
             *  Range.Address = $B$3,$C$1:$D$3,$A$4:$B$6,$C$4
             *  1, 1, 6, 4
             */
        }
        [Fact]
        public void TRangeSelect() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            Excel.Range range = xlSheet.Range[((Excel.Range) ((Excel.Range) xlSheet.Cells[1, 3])).Address, ((Excel.Range) xlSheet.Cells[3, 4]).Address];
            range.Merge(false);
            range = xlSheet.Range[((Excel.Range) ((Excel.Range) xlSheet.Cells[4, 1])).Address, ((Excel.Range) xlSheet.Cells[6, 2]).Address];
            range.Merge(false);

            range = xlSheet.Range[((Excel.Range) xlSheet.Cells[3, 2]).Address, ((Excel.Range) xlSheet.Cells[4, 3]).Address];
            range.Select();
            range = (Excel.Range) xlApp.Selection;

            Console.WriteLine(range.Address);

            xlBook.Close();
            xlApp.Quit();

            /*
             *  输出结果：
             *  $A$1:$D$6
             */
        }
        [Fact]
        public void TAreasItemIndexStartNumber() {
            var xlType = Type.GetTypeFromProgID("Excel.Application");
            var xlApp = Activator.CreateInstance(xlType) as Excel.Application ?? throw new Exception("打开“Excel”应用程序失败。");
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
            xlApp.ScreenUpdating = false;
            xlApp.SheetsInNewWorkbook = 1;
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = xlBook.ActiveSheet as Worksheet;

            var range = xlSheet.Range[((Excel.Range) xlSheet.Cells[1, 1]).Address, ((Excel.Range) xlSheet.Cells[4, 4]).Address];
            var areas = range.Areas;
            try {
                var item = areas.Item[0];
                Console.WriteLine("Index start with 0.");
            } catch {
                Console.WriteLine("Index start with 1.");
            }

            xlBook.Close();
            xlApp.Quit();
            /*
             *  输出结果：
             *  Index start with 1.
             */
        }
    }
}
