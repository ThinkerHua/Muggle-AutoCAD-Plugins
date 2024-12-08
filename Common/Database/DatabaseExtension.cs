using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;


namespace Muggle.AutoCADPlugins.Common.Database {
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
