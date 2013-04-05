using System;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

namespace Formulas.Testing.Operators
{
    [TestFixture]
    public class VariableTests
    {
        [RowTest]
        [Row(0.0, 0.0)]
        [Row(2.0, 2.0)]
        [Row(-2.0, -2.0)]
        [Row(null, 0, ExpectedException = typeof(InvalidOperationException))]
        public void TestVariable(double? a, double expected)
        {
            Operator constant = new Variable("x") { Value = a };
            double result = constant.Evaluate();
            Assert.AreEqual(expected, result);
        }
    }
}
