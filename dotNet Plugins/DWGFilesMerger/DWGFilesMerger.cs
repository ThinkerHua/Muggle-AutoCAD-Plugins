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
 *  DWGFilesMerger.cs: merger for DWG files
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Muggle.AutoCADPlugins.Common.Database;
using Muggle.AutoCADPlugins.Common.Geometry;

[assembly: CommandClass(typeof(Muggle.AutoCADPlugins.DWGFilesMerger.DWGFilesMerger))]
namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    public class DWGFilesMerger {

        [CommandMethod("MergeDWGFiles")]
        public static void MergeDWGFiles() {
            var form = new MainForm();
            form.ShowDialog();
        }
        public enum MergerMethodEnum {
            OriginalPosition,
            Arranged,
        }
        public enum ArrangementStyleEnum {
            ByRows,
            ByColumns,
        }
        public enum TagTypeEnum {
            None,
            SequenceNumber,
            FileName,
        }
        internal static void Merge(
            string folder, string targetFile,
            SearchOption searchOption = SearchOption.TopDirectoryOnly,
            MergerMethodEnum mergerMethod = MergerMethodEnum.OriginalPosition,
            ArrangementStyleEnum arrangementStyle = ArrangementStyleEnum.ByRows,
            int numPerGroup = 10,
            int rowSpacing = 500,
            int columnSpacing = 500,
            TagTypeEnum tagType = TagTypeEnum.None) {

            if (string.IsNullOrEmpty(folder)) {
                throw new ArgumentException($"“{nameof(folder)}”不能为 null 或空。", nameof(folder));
            }

            if (string.IsNullOrEmpty(targetFile)) {
                throw new ArgumentException($"“{nameof(targetFile)}”不能为 null 或空。", nameof(targetFile));
            }

            if (numPerGroup < 1) throw new ArgumentOutOfRangeException(nameof(numPerGroup), $"“{nameof(numPerGroup)}”不能小于 1 。");

            var files = Directory.GetFiles(folder, "*.dwg", searchOption);
            if (files.Length == 0)
                throw new System.Exception("目录为空，或目录中没有\"*.dwg\"文件。");

            var currentDatabase = HostApplicationServices.WorkingDatabase;

            double preLength = 0, preWidth = 0;//length为X轴方向，width为Y轴方向
            Extents3d boundingBox = new Extents3d();
            Point2d sourcePosition, targetPosition = new Point2d();
            Matrix3d matrix, originMatrix = Matrix3d.Displacement(new Vector3d(0, 0, 0));
            ObjectId txtStyID = new ObjectId();
            string tagStr;
            using (var targetDB = new Database(true, false))
            using (var targetTrans = targetDB.TransactionManager.StartTransaction()) {
                var blkTbl = targetTrans.GetObject(targetDB.BlockTableId, OpenMode.ForRead) as BlockTable;
                var blkTblRec = targetTrans.GetObject(blkTbl[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                //为标签创建文字样式
                if (tagType != TagTypeEnum.None) {
                    var tagTxtSty = new TextStyleTableRecord() {
                        Name = "Tag",
                        FileName = "gbenor.shx",
                        BigFontFileName = "gbcbig.shx",
                    };

                    var txtStyTbl = targetTrans.GetObject(targetDB.TextStyleTableId, OpenMode.ForWrite) as TextStyleTable;
                    txtStyID = txtStyTbl.Add(tagTxtSty);
                    targetTrans.AddNewlyCreatedDBObject(tagTxtSty, true);
                }
                //序号标签补0长度
                var lengthOfFilesNumber = files.Length.ToString().Length;

                for (int i = 0; i < files.Length; i++) {

                    //导入图纸
                    using (var readDB = new Database(false, false)) {
                        readDB.ReadDwgFile(files[i], FileOpenMode.OpenForReadAndReadShare, true, null);
                        switch (mergerMethod) {
                        case MergerMethodEnum.Arranged:
                            boundingBox = readDB.GetModelSpaceEntities().GetBoundingBox();
                            if (!boundingBox.IsValid()) continue;//没有图形

                            sourcePosition = boundingBox.TopLeft();

                            switch (arrangementStyle) {
                            case ArrangementStyleEnum.ByColumns:
                                if (i % numPerGroup == 0) {
                                    targetPosition = new Point2d(targetPosition.X + preLength + columnSpacing, 0);
                                    preLength = 0; preWidth = 0;
                                }//新的一列

                                targetPosition += new Vector2d(0, -preWidth - rowSpacing);

                                preLength = boundingBox.Length() > preLength ? boundingBox.Length() : preLength;
                                preWidth = boundingBox.Width();
                                break;

                            case ArrangementStyleEnum.ByRows:
                            default:
                                if (i % numPerGroup == 0) {
                                    targetPosition = new Point2d(0, targetPosition.Y - preWidth - rowSpacing);
                                    preLength = 0; preWidth = 0;
                                }//新的一行

                                targetPosition += new Vector2d(preLength + columnSpacing, 0);

                                preWidth = boundingBox.Width() > preWidth ? boundingBox.Width() : preWidth;
                                preLength = boundingBox.Length();
                                break;
                            }

                            matrix = Matrix3d.Displacement((targetPosition - sourcePosition).To3d());
                            break;

                        case MergerMethodEnum.OriginalPosition:
                        default:
                            matrix = originMatrix;
                            break;
                        }
                        targetDB.Insert(matrix, readDB, false);
                    }

                    //添加标签
                    if (mergerMethod == MergerMethodEnum.OriginalPosition || rowSpacing <= 0) continue;
                    switch (tagType) {
                    case TagTypeEnum.SequenceNumber:
                        tagStr = i.ToString($"D{lengthOfFilesNumber}");
                        break;
                    case TagTypeEnum.FileName:
                        tagStr = Path.GetFileNameWithoutExtension(files[i]);
                        break;
                    case TagTypeEnum.None:
                    default:
                        continue;
                    }

                    var insPt = new Point3d(targetPosition.X + boundingBox.Length() * 0.5, targetPosition.Y + rowSpacing * 0.1, 0);
                    using (var txt = new DBText() {
                        Height = rowSpacing * 0.6,
                        Position = insPt,
                        HorizontalMode = TextHorizontalMode.TextCenter,
                        AlignmentPoint = insPt,
                        TextString = tagStr,
                        TextStyleId = txtStyID,
                    }) {
                        HostApplicationServices.WorkingDatabase = targetDB;
                        txt.SetDatabaseDefaults(targetDB);
                        txt.AdjustAlignment(targetDB);
                        HostApplicationServices.WorkingDatabase = currentDatabase;
                        blkTblRec.UpgradeOpen();
                        blkTblRec.AppendEntity(txt);
                        blkTblRec.DowngradeOpen();
                        targetTrans.AddNewlyCreatedDBObject(txt, true);
                    }
                }

                targetTrans.Commit();
                targetDB.SaveAs(targetFile, DwgVersion.Current);
            }
        }
    }
}
