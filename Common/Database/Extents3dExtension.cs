using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.DatabaseServices"/>.<see cref="Extents3d"/> 的扩展。
    /// </summary>
    public static class Extents3dExtension {
        /// <summary>
        /// 判断给定边界框是否有效，
        /// 即其字段 <see cref="Extents3d.MinPoint"/> 的每个字段值（X、Y、Z）
        /// 是否均不大于字段 <see cref="Extents3d.MaxPoint"/> 的对应字段值。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>有效返回 true，无效返回 false。</returns>
        public static bool IsValid(this Extents3d extents3D) {
            var min = extents3D.MinPoint;
            var max = extents3D.MaxPoint;
            return min.X <= max.X && min.Y <= max.Y && min.Z <= max.Z;
        }
        /// <summary>
        /// 获取给定边界框的左上角二维点。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>给定边界框的左上角二维点。</returns>
        public static Point2d TopLeft(this Extents3d extents3D) {
            return new Point2d(extents3D.MinPoint.X, extents3D.MaxPoint.Y);
        }
        /// <summary>
        /// 获取给定边界框的右下角二维点。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>给定边界框的右下角二维点。</returns>
        public static Point2d BottomRight(this Extents3d extents3D) {
            return new Point2d(extents3D.MaxPoint.X, extents3D.MinPoint.Y);
        }
        /// <summary>
        /// 获取给定边界框的长度（X轴方向）。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>给定边界框的长度（X轴方向）。</returns>
        public static double Length(this Extents3d extents3D) {
            return extents3D.MaxPoint.X - extents3D.MinPoint.X;
        }
        /// <summary>
        /// 获取给定边界框的宽度（Y轴方向）。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>给定边界框的宽度（Y轴方向）。</returns>
        public static double Width(this Extents3d extents3D) {
            return extents3D.MaxPoint.Y - extents3D.MinPoint.Y;
        }
        /// <summary>
        /// 获取给定边界框的高度（Z轴方向）。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <returns>给定边界框的高度（Z轴方向）。</returns>
        public static double Height(this Extents3d extents3D) {
            return extents3D.MaxPoint.Z - extents3D.MinPoint.Z;
        }
    }
}
