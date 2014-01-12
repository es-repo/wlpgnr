using System;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class VariableTests
    {
        [Test]
        [Row(0.0, 0.0)]
        [Row(2.0, 2.0)]
        [Row(-2.0, -2.0)]
        public void TestVariable(double a, double expected)
        {
            Variable variable = new Variable("x") { Value = a };
            double result = variable.Value;
            Assert.AreEqual(expected, result);
        }

        [Test]
        [Row(null, ExpectedException = typeof(ArgumentException))]
        [Row("", ExpectedException = typeof(ArgumentException))]
        [Row("123", ExpectedException = typeof(ArgumentException))]
        [Row("_123")]
        [Row("_")]
        [Row("x")]
        [Row("x1")]
        [Row("x2y")]
        [Row("xyz")]
        [Row("x y", ExpectedException = typeof(ArgumentException))]
        public void TestName(string name)
        {
            Variable v = new Variable(name);
            Assert.AreEqual(name, v.Name);
        }
    }
}
