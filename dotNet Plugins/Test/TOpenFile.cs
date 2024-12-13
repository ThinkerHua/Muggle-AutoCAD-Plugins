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
 *  TOpenFile.cs: tester for OpenFileDialog
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWinForm = System.Windows.Forms;

namespace Muggle.AutoCADPlugins.Test {
    public class TOpenFile {
        [CommandMethod("ReadFile")]
        public void ReadFile() {
            var dialog = new OpenFileDialog(
                "打开文件",
                string.Empty,
                "dwg",
                string.Empty,
                OpenFileDialog.OpenFileDialogFlags.NoUrls | OpenFileDialog.OpenFileDialogFlags.NoFtpSites | OpenFileDialog.OpenFileDialogFlags.NoShellExtensions | OpenFileDialog.OpenFileDialogFlags.DoNotTransferRemoteFiles);
            var result = dialog.ShowDialog();
            if (result != SysWinForm.DialogResult.OK) return;

            using (var db = new Database(false, false)) {
                db.ReadDwgFile(dialog.Filename, FileOpenMode.OpenForReadAndReadShare, true, null);
                using (var trans = db.TransactionManager.StartTransaction()) {
                    var blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    var modelSpace = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                    var str = string.Empty;
                    foreach (var objectId in modelSpace) {
                        str += objectId + ", ";
                    }
                    var editor = Application.DocumentManager.MdiActiveDocument.Editor;
                    editor.WriteMessage(str);
                }

            }

        }
    }
}
