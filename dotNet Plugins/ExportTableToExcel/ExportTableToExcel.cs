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
 *  TableToExcel.cs: export cad table to excel
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Muggle.AutoCADPlugins.Common.Database;
using WinForm = System.Windows.Forms;

[assembly: CommandClass(typeof(Muggle.AutoCADPlugins.ExportTableToExcel.ExportTableToExcel))]
namespace Muggle.AutoCADPlugins.ExportTableToExcel {
    public class ExportTableToExcel {
        private static readonly string defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string defaultName = "ExportedTable.xls";
        private static readonly string title = "保存文件";
        private static readonly string extension = "xls";
        private static readonly string dialogName = null;
        private static readonly SaveFileDialog.SaveFileDialogFlags flags =
            SaveFileDialog.SaveFileDialogFlags.DoNotTransferRemoteFiles |
            SaveFileDialog.SaveFileDialogFlags.NoUrls |
            SaveFileDialog.SaveFileDialogFlags.NoFtpSites;
        private static SaveFileDialog saveFileDialog;
        private static string folder = defaultFolder, name = defaultName;
        private double Epsilion = 0.000001;
        private bool MergeCells = false;
        private string FileFullName => $"{folder}\\{name}";
        private string StartingMessage => string.Format("\n当前参数：容许误差 = {0}，合并单元格 = {1}，导出文件路径 = {2}",
            Epsilion, MergeCells ? "是" : "否", FileFullName);

        [CommandMethod("ExprotTableToExcel", CommandFlags.UsePickSet)]
        [CommandMethod("ETTE", CommandFlags.UsePickSet)]
        public void ExportCADTableToExcel() {
            if (Type.GetTypeFromProgID("Excel.Application") == null) {
                WinForm.MessageBox.Show(
                    "未在本机上查找到“Excel”应用程序。",
                    "错误",
                    WinForm.MessageBoxButtons.OK,
                    WinForm.MessageBoxIcon.Error);
                return;
            }

            var db = HostApplicationServices.WorkingDatabase;
            var editor = Application.DocumentManager.MdiActiveDocument.Editor;
            editor.WriteMessage(StartingMessage);

            var pmptKeywords = new PromptKeywordOptions(string.Empty) {
                Message = "\n选择操作：",
                AllowNone = true,
            };
            pmptKeywords.Keywords.Add("S", "S", "选择表格(S)", true, true);
            pmptKeywords.Keywords.Add("E", "E", "容许误差(E)", true, true);
            pmptKeywords.Keywords.Add("M", "M", "合并单元格(M)", true, true);
            pmptKeywords.Keywords.Add("F", "F", "设置导出文件(F)", true, true);
            pmptKeywords.Keywords.Default = "S";

            var selectionSetResult = editor.SelectImplied();
            var loop = selectionSetResult.Status != PromptStatus.OK;
            while (loop) {
                var pKeyRes = editor.GetKeywords(pmptKeywords);
                switch (pKeyRes.StringResult) {
                case "S":
                    loop = false;
                    selectionSetResult = SelectTable(editor);
                    break;
                case "E":
                    Epsilion = GetEpsilion(editor);
                    editor.WriteMessage(StartingMessage);
                    break;
                case "M":
                    MergeCells = GetMergeCells(editor);
                    editor.WriteMessage(StartingMessage);
                    break;
                case "F":
                    ChooseFileFullname();
                    editor.WriteMessage(StartingMessage);
                    break;
                default:
                    loop = false;
                    break;
                }
            }
            if (selectionSetResult.Status != PromptStatus.OK) {
                editor.WriteMessage("\n未选择有效内容。");
                return;
            }

            Entities entities = new Entities();
            using (var trans = db.TransactionManager.StartTransaction()) {
                foreach (SelectedObject item in selectionSetResult.Value) {
                    var entity = trans.GetObject(item.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity.GetType() == typeof(BlockReference)) {
                        entities.AddRange(db.GetEntities((BlockReference) entity));
                    } else {
                        entities.Add(entity);
                    }
                }
            }
            /*foreach (var ent in entities) {
                editor.WriteMessage($"\nID: {ent.ObjectId} {ent.GetType().Name} Extents: {ent.GeometricExtents}");
                if (ent is Line line)
                    editor.WriteMessage($"\nStartPoint: {line.StartPoint}, EndPoint: {line.EndPoint}");
            }*/

            if (entities.Count == 0) goto No_Valid_Entities;

            var table = GetExchangeTable(db, entities);
            if (table == null) goto No_Valid_Entities;

            try {
                table.ExprotToExcel($"{FileFullName}", MergeCells);
                WinForm.MessageBox.Show(
                    $"成功导出到：{FileFullName}",
                    "成功",
                    WinForm.MessageBoxButtons.OK,
                    WinForm.MessageBoxIcon.Information);
                return;
            } catch (System.Exception e) {
                WinForm.MessageBox.Show(
                    $"导出文件失败。发生以下错误：\n{e.Message}",
                    "错误",
                    WinForm.MessageBoxButtons.OK,
                    WinForm.MessageBoxIcon.Error);
                return;
            }

        No_Valid_Entities:
            WinForm.MessageBox.Show(
                "未找到有效实体。",
                "错误",
                WinForm.MessageBoxButtons.OK,
                WinForm.MessageBoxIcon.Error);
            return;
        }

        private double GetEpsilion(Editor editor) {
            var pmpt = new PromptDoubleOptions(string.Empty) {
                Message = $"\n输入容许误差（小于此数值的差异，将斜线视为水平或垂直）",
                AllowNone = true,
            };

            var resault = editor.GetDouble(pmpt);
            if (resault.Status == PromptStatus.OK)
                return resault.Value;

            return Epsilion;
        }

        private bool GetMergeCells(Editor editor) {
            var pmpt = new PromptKeywordOptions(string.Empty) {
                Message = $"\n是否合并单元格：",
                AllowNone = true,
            };
            pmpt.Keywords.Add("Y", "Y", "是(Y)", true, true);
            pmpt.Keywords.Add("N", "N", "否(N)", true, true);
            pmpt.Keywords.Default = MergeCells ? "Y" : "N";

            var resault = editor.GetKeywords(pmpt);
            switch (resault.StringResult) {
            case "Y":
                return true;
            case "N":
                return false;
            default:
                return MergeCells;
            }
        }

        private PromptSelectionResult SelectTable(Editor editor) {
            var typedValues = new TypedValue[] {
                    new TypedValue((int) DxfCode.Operator, "<OR"),
                    new TypedValue((int) DxfCode.Start, "LINE"),
                    new TypedValue((int) DxfCode.Start, "LWPOLYLINE"),
                    new TypedValue((int) DxfCode.Start, "TEXT"),
                    new TypedValue((int) DxfCode.Start, "MTEXT"),
                    new TypedValue((int) DxfCode.Start, "INSERT"),
                    new TypedValue((int) DxfCode.Operator, "OR>"),
                };
            var sFilter = new SelectionFilter(typedValues);
            return editor.GetSelection(sFilter);
        }

        private void ChooseFileFullname() {
            if (saveFileDialog is null)
                saveFileDialog = new SaveFileDialog(title, name, extension, dialogName, flags);
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.Filename)) {
                folder = Path.GetDirectoryName(saveFileDialog.Filename);
                name = Path.GetFileName(saveFileDialog.Filename);
            }
        }

        /// <summary>
        /// 根据给定的实体集合，获取 <see cref="ExchangeTable"/> 对象。
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="entities">实体集合</param>
        /// <returns>成功则返回 <see cref="ExchangeTable"/> 对象，失败则返回 null。</returns>
        private ExchangeTable GetExchangeTable(Database db, Entities entities) {
            var plines = (from ent in entities where ent is Polyline select ent).Cast<Polyline>();
            var lines = db.GetLines(plines);
            lines.AddRange((from ent in entities where ent is Line select ent).Cast<Line>());
            if (lines.Count == 0) return null;

            var dbTexts = (from ent in entities where ent is DBText select ent).Cast<DBText>();
            var mTexts = (from ent in entities where ent is MText select ent).Cast<MText>();

            var table = new ExchangeTable();
            foreach (var line in lines) {
                if (line.Length == 0.0) continue;
                if (Math.Abs(line.StartPoint.X - line.EndPoint.X) <= Epsilion) {
                    if (table.BorderVertical.ContainsKey(line.StartPoint.X)) {
                        table.BorderVertical[line.StartPoint.X].Add((line.StartPoint.Y, line.EndPoint.Y));
                    } else {
                        table.BorderVertical.Add(
                            line.StartPoint.X,
                            new List<(double Y1, double Y2)> { (line.StartPoint.Y, line.EndPoint.Y) });
                    }
                } else if (Math.Abs(line.StartPoint.Y - line.EndPoint.Y) <= Epsilion) {
                    if (table.BorderHorizontal.ContainsKey(line.StartPoint.Y)) {
                        table.BorderHorizontal[line.StartPoint.Y].Add((line.StartPoint.X, line.EndPoint.X));
                    } else {
                        table.BorderHorizontal.Add(
                            line.StartPoint.Y,
                            new List<(double X1, double X2)> { (line.StartPoint.X, line.EndPoint.X) });
                    }
                }
            }

            foreach (var dbText in dbTexts) {
                var extents = dbText.GeometricExtents;
                var topleft = extents.TopLeft();
                table.Texts.Add((topleft.X, topleft.Y, dbText.TextString));
            }

            foreach (var mText in mTexts) {
                var extents = mText.GeometricExtents;
                var topleft = extents.TopLeft();
                table.Texts.Add((topleft.X, topleft.Y, mText.Text));
            }

            return table;
        }
    }
}
