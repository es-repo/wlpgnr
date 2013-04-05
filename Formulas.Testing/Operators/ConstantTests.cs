using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

namespace Formulas.Testing.Operators
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
            double result = constant.Evaluate();
            Assert.AreEqual(expected, result);
        }
    }
}
