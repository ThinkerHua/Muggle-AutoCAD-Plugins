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
 *  Entities.cs: class of the collection of entity, with some useful methods 
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;

namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// 实体集合类型。
    /// </summary>
    public class Entities : List<Entity> {
        #region Methods
        /// <summary>
        /// 获取实体集合的边界框。
        /// </summary>
        /// <returns>实体集合的边界框。空集合返回 <see cref="Extents3d"/> 类型默认构造函数构造的实例。</returns>
        public Extents3d GetBoundingBox() {
            if (Count == 0) return new Extents3d();

            var extents = new Extents3d();
            Extents3d subExtents;
            foreach (var entity in this) {
                try {
                    subExtents = entity.GeometricExtents;
                } catch {
                    continue;
                }

                if (!subExtents.IsValid()) continue;

                extents.AddExtents(subExtents);
            }

            return extents;
        }
        #endregion
    }
}
