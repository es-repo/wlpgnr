using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operands;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace Formulas.Testing.Operators.Arithmetic
{
    [TestFixture]
    public class SumTests
    {
        [RowTest]
        [Row(0, 0, 0)]
        [Row(2, 2, 4)]
        [Row(2, 0, 2)]
        public void TestSum(double a, double b, double expected)
        {
            Operator sum = new Sum(); 
            Operand constantA = new Constant(a);
            Variable variableB = new Variable();
            variableB.SetValue(b);
            double result = sum.Calculate(constantA, variableB);
            Assert.AreEqual(expected, result);
        }
    }
}
