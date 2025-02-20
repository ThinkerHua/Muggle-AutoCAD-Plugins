﻿/*==============================================================================
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
 *  DWGFilesMerger.cs: merger for DWG files
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using Autodesk.AutoCAD.Runtime;
using Muggle.AutoCADPlugins.DWGFilesMerger.View;

[assembly: ExtensionApplication(typeof(Muggle.AutoCADPlugins.DWGFilesMerger.DWGFilesMerger))]
[assembly: CommandClass(typeof(Muggle.AutoCADPlugins.DWGFilesMerger.DWGFilesMerger))]
namespace Muggle.AutoCADPlugins.DWGFilesMerger {
    public class DWGFilesMerger : IExtensionApplication {
        private MainWindow form;

        public void Initialize() {

        }

        [CommandMethod("MergeDWGFiles")]
        public void MergeDWGFiles() {
            form ??= new MainWindow();
            form.ShowDialog();
        }

        public void Terminate() {

        }
    }
}
