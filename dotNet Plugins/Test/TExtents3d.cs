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
 *  TExtents3d.cs: test for Extents3d
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muggle.AutoCADPlugins.Test
{
    public class TExtents3d
    {
        [CommandMethod("Extents3dDC")]
        public void DefaultContructor() {
            var extents1 = new Extents3d();
            var extents2 = new Extents3d(new Point3d(100, 150, 0), new Point3d(500, 600, 0));
            var extents3 = extents1;
            var extents4 = extents2;
            extents3.AddExtents(extents2);
            extents4.AddExtents(extents1);

            var editor = Application.DocumentManager.MdiActiveDocument.Editor;
            editor.WriteMessage(
                $"\n默认构造器构造的实例1：{extents1}" +
                $"\n带参数构造器构造的实例2：{extents2}" +
                $"\n实例1附加实例2：{extents3}" +
                $"\n实例2附加实例1：{extents4}");

            /*  输出结果：
             *  默认构造器构造的实例1：((1E+20,1E+20,1E+20),(-1E+20,-1E+20,-1E+20))
             *  带参数构造器构造的实例2：((100,150,0),(500,600,0))
             *  实例1附加实例2：((100,150,0),(500,600,0))
             *  实例2附加实例1：((100,150,0),(500,600,0))
             *  
             *  默认构造器构造的实例与其他实例之间互相附加是安全的
            */
        }
    }
}
