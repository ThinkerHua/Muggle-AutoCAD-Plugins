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
using Autodesk.AutoCAD.DatabaseServices;


namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.DatabaseServices.Database"/> 的扩展。
    /// </summary>
    public static class DatabaseExtension {
        /// <summary>
        /// 获取当前数据库的模型空间中所有的实体。
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <returns>当前数据库的模型空间中所有的实体。
        /// 若无有效实体，则集合 Count 属性等于 0。</returns>
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

        /// <summary>
        /// 获取当前数据库的模型空间中所有实体的ID。
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <returns>当前数据库的模型空间中所有实体的ID。
        /// 若无有效实体，则集合 Count 属性等于 0。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ObjectIdCollection GetModelSpaceEntityIdCollection(this Autodesk.AutoCAD.DatabaseServices.Database db) {
            if (db is null) {
                throw new ArgumentNullException(nameof(db));
            }

            var idCollection = new ObjectIdCollection();
            using (var trans = db.TransactionManager.StartTransaction()) {
                var blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                var modelSpace = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                foreach (var objectId in modelSpace) {
                    idCollection.Add(objectId);
                }
            }

            return idCollection;
        }

        /// <summary>
        /// 获取当前数据库中指定块参照内包含的所有实际显示的实体。
        /// </summary>
        /// <remarks>获取的集合中，<see cref="AttributeReference"/> 类型元素未经转换，如有需要，
        /// 应转换为 <see cref="DBText"/>。</remarks>
        /// <param name="db">当前数据库</param>
        /// <param name="blockRef">指定块参照</param>
        /// <returns>指定块参照内包含的所有实际显示的实体。
        /// 若无有效对象，则集合 Count 属性等于 0。</returns>
        public static Entities GetEntities(this Autodesk.AutoCAD.DatabaseServices.Database db, BlockReference blockRef) {
            if (db is null) {
                throw new ArgumentNullException(nameof(db));
            }

            if (blockRef is null) {
                throw new ArgumentNullException(nameof(blockRef));
            }

            var entities = new Entities();
            using (var trans = db.TransactionManager.StartTransaction()) {
                //var blockTableRecord = trans.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                //foreach (var objectId in blockTableRecord) {
                //    var ent = trans.GetObject(objectId, OpenMode.ForRead) as Entity;
                //    if (ent.GetType() == typeof(BlockReference)) {
                //        entities.AddRange(GetEntities(db, (BlockReference) ent));
                //    } else if (ent.GetType() != typeof(AttributeDefinition)) {
                //        entities.Add(ent);//不要属性定义
                //    }
                //}
                var exploded = new DBObjectCollection();
                blockRef.Explode(exploded);
                foreach (Entity ent in exploded) {
                    if (ent.GetType() == typeof(BlockReference)) {
                        entities.AddRange(db.GetEntities((BlockReference) ent));
                    } else if (ent.GetType() != typeof(AttributeDefinition)) {
                        entities.Add((Entity) ent);//不要属性定义
                    }
                }

                foreach (ObjectId attrId in blockRef.AttributeCollection) {
                    var attrRef = trans.GetObject(attrId, OpenMode.ForRead) as AttributeReference;
                    if (attrRef.Invisible) continue;//跳过不可见属性
                    if (attrRef.IsMTextAttribute) {
                        entities.Add(attrRef.MTextAttribute);//多行文字
                    } else {
                        entities.Add(attrRef);//单行文字
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// 获取当前数据库中给定 <see cref="Polyline"/> 对象集合中各对象分解后的
        /// <see cref="Line"/> 对象集合。
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="polylines">给定 <see cref="Polyline"/> 对象集合</param>
        /// <returns>分解后的 <see cref="Line"/> 对象集合。
        /// 若无有效对象，则集合 Count 属性等于 0。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static List<Line> GetLines(this Autodesk.AutoCAD.DatabaseServices.Database db, IEnumerable<Polyline> polylines) {
            if (db is null) {
                throw new ArgumentNullException(nameof(db));
            }

            if (polylines is null) {
                throw new ArgumentNullException(nameof(polylines));
            }

            var lines = new List<Line>();
            using (var trans = db.TransactionManager.StartTransaction()) {
                var exploded = new DBObjectCollection();
                foreach (var polyline in polylines) {
                    polyline.Explode(exploded);
                }
                foreach (var item in exploded) {
                    if (item is Line line) lines.Add(line);
                }
            }

            return lines;
        }
    }
}
