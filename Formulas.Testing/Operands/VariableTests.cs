using System;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operands;

namespace Formulas.Testing.Operands
{
    [TestFixture]
    public class VariableTests
    {
        [Test]
        public void TestValue()
        {
            Variable variable = new Variable("x");
            const double value = 5.5;
            variable.SetValue(value);
            Assert.AreEqual(value, variable.Value); 
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void TestNonAssignedValueAccess()
        {
            Variable variable = new Variable("x");
            Assert.AreEqual(0, variable.Value);
        }
    }
}
