using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class ConstantTests
    {
        [RowTest]
        [Row(0, 0)]
        [Row(2, 2)]
        [Row(-2, -2)]
        public void TestConstant(double a, double expected)
        {
            Constant constant = new Constant(a);
            double result = constant.Value;
            Assert.AreEqual(expected, result);
        }
    }
}
