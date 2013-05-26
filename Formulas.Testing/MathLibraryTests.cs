using System;
using System.Linq;
using MbUnit.Framework;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class MathLibraryTests
    {
        [RowTest]
        [Row(1, new []{0, Math.PI, Math.PI + 0.01}, new [] {0.0, 0.0, 0.0})]
        [Row(4, new[] { 0, Math.PI / 2, Math.PI / 2 + 0.01, Math.PI * 2 - 0.01, -Math.PI * 8 - 0.01 }, new[] { 0.0, 1.0, 1.0, -1.0, -1.0 })]
        public void TestFastSin(int precalculatedSinusesCount, double[] values, double[] expectedSinuses)
        {
            MathLibrary.Init(precalculatedSinusesCount);
            double[] sinuses = values.Select(v => MathLibrary.FastSin(v)).ToArray();
            CollectionAssert.AreEqual(expectedSinuses, sinuses);
        }
    }
}
