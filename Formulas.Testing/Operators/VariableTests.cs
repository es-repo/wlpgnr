using NUnit.Framework;
using System;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Testing.Operators
{
    [TestFixture]
    public class VariableTests
    {
        [TestCase(0.0, 0.0)]
        [TestCase(2.0, 2.0)]
        [TestCase(-2.0, -2.0)]
        public void TestVariable(double a, double expected)
        {
            Variable variable = new Variable("x") { Value = a };
            double result = variable.Value;
            Assert.AreEqual(expected, result);
        }

        [TestCase("_123")]
        [TestCase("_")]
        [TestCase("x")]
        [TestCase("x1")]
        [TestCase("x2y")]
        [TestCase("xyz")]
        public void TestName(string name)
        {
            Variable v = new Variable(name);
            Assert.AreEqual(name, v.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("123")]
        [TestCase("x y")]
        public void TestInvalidName(string name)
        {
            Assert.That(() => new Variable(name), Throws.TypeOf<ArgumentException>());
        }
    }
}
