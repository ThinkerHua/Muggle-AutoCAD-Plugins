using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Muggle.AutoCADPlugins.Common.Database;
using Muggle.AutoCADPlugins.Common.Geometry;
using System;
using System.IO;

[assembly: CommandClass(typeof(Muggle.AutoCADPlugins.DWGFilesMerger.DWGFilesMerger))]
namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    public class DWGFilesMerger {

        [CommandMethod("MergeDWGFiles")]
        public static void MergeDWGFiles() {
            var form = new Form_DWGFilesMerger();
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
        internal static void Merge(
            string folder, string targetFile,
            SearchOption searchOption = SearchOption.TopDirectoryOnly,
            MergerMethodEnum mergerMethod = MergerMethodEnum.OriginalPosition,
            ArrangementStyleEnum arrangementStyle = ArrangementStyleEnum.ByRows,
            int numPerGroup = 10,
            int rowSpacing = 500,
            int columnSpacing = 500) {

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

            double preLength = 0, preWidth = 0;//length为X轴方向，width为Y轴方向
            Extents3d boundingBox;
            Point2d sourcePosition, targetPosition = new Point2d();
            Matrix3d matrix;
            using (var targetDB = new Database(true, false))
            using (var targetTrans = targetDB.TransactionManager.StartTransaction()) {
                for (int i = 0; i < files.Length; i++) {

                    using (var readDB = new Database(false, false)) {
                        readDB.ReadDwgFile(files[i], FileOpenMode.OpenForReadAndReadShare, true, null); switch (mergerMethod) {
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
                            matrix = Matrix3d.Displacement(new Vector3d(0, 0, 0));
                            break;
                        }
                        targetDB.Insert(matrix, readDB, false);
                    }
                }

                targetTrans.Commit();
                targetDB.SaveAs(targetFile, DwgVersion.Current);
            }
        }
    }
}
