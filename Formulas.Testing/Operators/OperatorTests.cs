using System;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class OperatorTests
    {
        [RowTest]
        [Row(new double[] {1, 2}, 3)]
        [Row(new double[] { 1 }, 0, ExpectedException = typeof(ArgumentException))]
        [Row(new double[] { 1, 2, 3 }, 0, ExpectedException = typeof(ArgumentException))]
        public void TestBinaryOperator(double[] operands, double expectedResult)
        {
            Operator sum = new Sum();
            double result = sum.Evaluate(operands);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
