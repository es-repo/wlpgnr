using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operands;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace Formulas.Testing.Operators.Arithmetic
{
    [TestFixture]
    public class MinusTests
    {
        [RowTest]
        [Row(0, 0)]
        [Row(2, -2)]
        [Row(-2, 2)]
        public void TestMinus(double a, double expected)
        {
            Operator minus = new Minus();
            Operand operandA = new Constant(a);
            double result = minus.Calculate(operandA);
            Assert.AreEqual(expected, result);
        }
    }
}
