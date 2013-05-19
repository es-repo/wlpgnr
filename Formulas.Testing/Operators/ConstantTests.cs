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
            Operator constant = new Constant(a);
            double result = constant.Evaluate(0, 0, 0, 0);
            Assert.AreEqual(expected, result);
        }
    }
}
