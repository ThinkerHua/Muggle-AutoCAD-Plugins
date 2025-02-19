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
 *  Point2dExtension.cs: extension for Autodesk.AutoCAD.Geometry.Point2d
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using Autodesk.AutoCAD.Geometry;

namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Point2d"/> 的扩展。
    /// </summary>
    public static class Point2dExtension {
        /// <summary>
        /// 将 <see cref="Point2d"/> 转换为 <see cref="ValueTuple{T1, T2}"/>。
        /// </summary>
        /// <param name="point">当前二维点</param>
        /// <returns>值元组的X、Y字段分别为二维点的X、Y属性。</returns>
        public static (double X, double Y) ToValueTuple(this Point2d point) {
            return (point.X, point.Y);
        }
    }
}
