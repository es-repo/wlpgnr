using System;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaGeneratorTests
    {
        [Test]
        public void TestCreateRandomFormula()
        {
            //FormulaGenerator
        }

        [RowTest]
        [Row(0, 0, 0.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, -1, 0.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, 1, 1.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, 0, 0.99, 0, 0, 0, true)]
        [Row(1, 1, 0.99, 1, 0, 0, true)]
        [Row(1, 3, 0.99, 3, 0, 0, true)]
        [Row(2, 3, 0.99, 3, 1, 0, true)]
        [Row(3, 3, 0, 3, 2, 0, true)]
        [Row(5, 3, 0, 3, 4, 0, true)]
        [Row(100, 200, 0.5, 200, 33, 33, false)]
        public void TestGetNonZeroOperatorsAritySequence(int zeroArityOperatorsCount, int unaryArityOperatorsCount, double ternaryVsBinaryOperatorOccurenceProbability,
            int expectedUnaryOperatorsCount, int expectedBinaryOperatorsCount, int expectedTernaryOperatorsCount, bool precise)
        {
            Random random = new Random();
            int[] arities = FormulaGenerator.GetNonZeroOperatorsAritySequence(random, zeroArityOperatorsCount,
                unaryArityOperatorsCount, ternaryVsBinaryOperatorOccurenceProbability).ToArray();

            double unaryOperatorsCount = arities.Count(a => a == 1);
            double binaryOperatorsCount = arities.Count(a => a == 2); 
            double ternaryOperatorsCount = arities.Count(a => a == 3);

            Assert.AreEqual(expectedUnaryOperatorsCount, unaryOperatorsCount);
            if (precise)
            {
                Assert.AreEqual(expectedBinaryOperatorsCount, binaryOperatorsCount);
                Assert.AreEqual(expectedTernaryOperatorsCount, ternaryOperatorsCount);
            }
            else
            {
                const int inaccuracy = 10;
                Assert.Between(expectedBinaryOperatorsCount, binaryOperatorsCount - inaccuracy, binaryOperatorsCount + inaccuracy);
                Assert.Between(expectedTernaryOperatorsCount, ternaryOperatorsCount - inaccuracy, ternaryOperatorsCount + inaccuracy);
            }
        }
    }
}
