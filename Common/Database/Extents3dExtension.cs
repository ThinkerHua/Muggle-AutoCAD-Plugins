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
 *  Extents3dExtension.cs: extension for Autodesk.AutoCAD.DatabaseServices.Extents3d
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.DatabaseServices"/>.<see cref="Extents3d"/> 的扩展。
    /// </summary>
    public static class Extents3dExtension {
        /// <summary>
        /// 判断给定边界框是否有效。
        /// </summary>
        /// <param name="extents3D">给定边界框</param>
        /// <param name="flattenedIsValid">扁平化（体积为 0）的边界框算作有效。默认值 true。</param>
        /// <returns>有效返回 true，无效返回 false。</returns>
        public static bool IsValid(this Extents3d extents3D, bool flattenedIsValid = true) {
            var min = extents3D.MinPoint;
            var max = extents3D.MaxPoint;
            if (flattenedIsValid)
                return min.X <= max.X && min.Y <= max.Y && min.Z <= max.Z;
            else
                return !(min.X >= max.X || min.Y >= max.Y || min.Z >= max.Z);
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
