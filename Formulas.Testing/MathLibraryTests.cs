using MbUnit.Framework;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class MathLibraryTests
    {
        [RowTest]
        [Row(0, 0, 0)]
        [Row(2, 2, 4)]
        [Row(-2, 2, 0)]
        public void TestSum(double a, double b, double expectedResult)
        {
            double result = MathLibrary.Sum(a, b);  
            Assert.AreEqual(expectedResult, result);
        }
    }
}
