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
 *  DatabaseExtension.cs: extension for Autodesk.AutoCAD.DatabaseServices.Database
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;


namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.DatabaseServices.Database"/> 的扩展。
    /// </summary>
    public static class DatabaseExtension {
        /// <summary>
        /// 获取给定数据库的模型空间中所有的实体。
        /// </summary>
        /// <param name="db">给定数据库</param>
        /// <returns>给定数据库的模型空间中所有的实体。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Entities GetModelSpaceEntities(this Autodesk.AutoCAD.DatabaseServices.Database db) {
            if (db is null) {
                throw new ArgumentNullException(nameof(db));
            }

            var entities = new Entities();
            using (var trans = db.TransactionManager.StartTransaction()) {
                var blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                var modelSpace = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                foreach (var objectId in modelSpace) {
                    entities.Add(trans.GetObject(objectId, OpenMode.ForRead) as Entity);
                }
            }

            return entities;
        }
    }
}
