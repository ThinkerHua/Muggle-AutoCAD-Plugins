using System;
using System.IO;
using System.Linq;
using Muggle.AutoCADPlugins.Common.NPOIExtensions;
using Muggle.AutoCADPlugins.ExportTableToExcel;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Util;
using Xunit;
using Xunit.Abstractions;

namespace xUnitTest {
    public class TNPOI {
        private readonly ITestOutputHelper _output;
        public TNPOI(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void TCreatTable() {
            try {
                var fileStream = File.ReadAllText(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "serialized table.txt"));

                var deSerialized = JsonConvert.DeserializeObject<ExchangeTable>(fileStream);

                deSerialized.ExprotToXls(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "exported.xls"),
                    true);
            } catch (Exception ex) {
                _output.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void TMergeRegion() {
            var xlBook = new HSSFWorkbook();
            var xlSheet = xlBook.CreateSheet("Sheet1");
            xlSheet.AddMergedRegion(new CellRangeAddress(2, 3, 2, 3));
            xlSheet.AddMergedRegion(new CellRangeAddress(5, 6, 5, 6));

            //此方法报错：
            //System.InvalidOperationException : Cannot add merged region D4:F7 to sheet
            //because it overlaps with an existing merged region (C3:D4).
            //xlSheet.AddMergedRegion(new CellRangeAddress(3, 6, 3, 5));

            xlSheet.AddMergedRegionUnsafe(new CellRangeAddress(3, 6, 3, 5));

            var ranges = CellRangeUtil.MergeCellRanges(
                new CellRangeAddress[] {
                    new CellRangeAddress(2, 3, 2, 3),
                    new CellRangeAddress(5, 6, 5, 6)
                });

            _output.WriteLine(
                string.Join("\n",
                ranges.Select(item => item.ToString()))
                );

            using (var fileStream = new FileStream(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Testing.xls"),
                FileMode.Create)) {

                xlBook.Write(fileStream);
            }
        }

        [Fact]
        public void TSelectRange() {
            var xlBook = new HSSFWorkbook();
            var xlSheet = xlBook.CreateSheet("Sheet1");

            xlSheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 5));
            _output.WriteLine($"Merged Regions count: {xlSheet.MergedRegions.Count}");
            _output.WriteLine(string.Join("\n", xlSheet.MergedRegions));

            var range = xlSheet.SelectRange(1, 1, 5, 6);
            _output.WriteLine($"\nSelected Range: {range}");

            xlSheet.AddMergedRegionButKeepValue(range);
            _output.WriteLine($"\nMerged Regions count: {xlSheet.MergedRegions.Count}");
            _output.WriteLine(string.Join("\n", xlSheet.MergedRegions));

            using (var fileStream = new FileStream(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Testing.xls"),
                FileMode.Create)) {

                xlBook.Write(fileStream);
            }
        }
    }
}
