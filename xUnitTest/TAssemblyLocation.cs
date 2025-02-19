using System;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace xUnitTest {
    public class TAssemblyLocation {
        private ITestOutputHelper testOutput;
        public TAssemblyLocation(ITestOutputHelper testOutputHelper) {
            testOutput = testOutputHelper;
        }
        [Fact]
        public void Location() {
            var fullPath = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine($"FullPath = {fullPath}");
            Console.WriteLine($"Root = {Path.GetPathRoot(fullPath)}");
            Console.WriteLine($"Directory = {Path.GetDirectoryName(fullPath)}");
            Console.WriteLine($"FileName = {Path.GetFileName(fullPath)}");
        }
    }
}
