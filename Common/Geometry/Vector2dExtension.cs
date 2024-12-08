using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;

namespace Muggle.AutoCADPlugins.Common.Geometry {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.Geometry"/>.<see cref="Vector2d"/> 的扩展。
    /// </summary>
    public static class Vector2dExtension {
        /// <summary>
        /// 将给定二维向量提升为三维向量。
        /// </summary>
        /// <param name="vector2D">给定二维向量</param>
        /// <param name="z">Z坐标值，默认 0。</param>
        /// <returns>提升到三维的向量，X、Y坐标值与原二维向量一致，Z坐标为给定值。</returns>
        public static Vector3d To3d(this Vector2d vector2D, double z = 0) {
            return new Vector3d(vector2D.X, vector2D.Y, z);
        }
    }
}
