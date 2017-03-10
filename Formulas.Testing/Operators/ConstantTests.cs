using NUnit.Framework;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class ConstantTests
    {
        [Test]
        [TestCase(0, 0)]
        [TestCase(2, 2)]
        [TestCase(-2, -2)]
        public void TestConstant(double a, double expected)
        {
            Constant constant = new Constant(a);
            double result = constant.Value;
            Assert.AreEqual(expected, result);
        }
    }
}
