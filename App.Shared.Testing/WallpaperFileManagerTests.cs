using System;
using System.IO;
using MbUnit.Framework;
using WallpaperGenerator.App.Core;
using WallpaperGenerator.App.Core.Testing;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.App.Shared.Testing
{
    [TestFixture]
    public class WallpaperFileManagerTests
    {
        [Test]
        public void TestSave()
        {
            FormulaRenderArguments formulaRenderArguments = FormulaRenderArguments.FromString(
                    "2.1,3.6;1.91,6.87;-0.62,2.26;-4.66,2.25\r\n0,0,0,0;-0.59,-1.43,0.47,0.2;0.21,-0.98,0.88,0.28\r\nSum Atan Pow x2 Cbrt Sin x1 Atan Pow Sin Sum x2 x0 x3");
            WorkflowRenderResult formulaRenderResult = new WorkflowRenderResult(formulaRenderArguments, null, new FormulaBitmapMock(new Size()), TimeSpan.Zero);

            WallpaperFileManager wallpaperFileManager = new WallpaperFileManager(Path.GetTempPath());
            Tuple<string, string> filesPath = wallpaperFileManager.Save(formulaRenderResult, false);
            
            Assert.IsTrue(File.Exists(filesPath.Item1));
            Assert.IsNull(filesPath.Item2);

            filesPath = wallpaperFileManager.Save(formulaRenderResult, true);

            Assert.IsTrue(File.Exists(filesPath.Item1));
            Assert.IsTrue(File.Exists(filesPath.Item2));
        }
    }
}
