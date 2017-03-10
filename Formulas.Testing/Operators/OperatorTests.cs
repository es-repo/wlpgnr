using NUnit.Framework;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class OperatorTests
    {
        [Test]
        [TestCase(new double[] {1, 2}, 3)]
        public void TestBinaryOperator(double[] operands, double expectedResult)
        {
            Operator sum = new Sum();
            double result = sum.Evaluate(() => operands[0], () => operands[1])();
            Assert.AreEqual(expectedResult, result);
        }
    }
}
