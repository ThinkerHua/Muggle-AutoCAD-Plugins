using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;

namespace Muggle.AutoCADPlugins.UnitTest {
    [TestClass]
    public class TAssemblyLocation {
        [TestMethod]
        public void Location() {
            var fullPath = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine($"FullPath = {fullPath}");
            Console.WriteLine($"Root = {Path.GetPathRoot(fullPath)}");
            Console.WriteLine($"Directory = {Path.GetDirectoryName(fullPath)}");
            Console.WriteLine($"FileName = {Path.GetFileName(fullPath)}");
        }
    }
}
