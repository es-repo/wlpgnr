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
        [Row(0, 0.0, 0.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, 1.0, 1.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, 1.0, 1.0, 0, 0, 0, true, ExpectedException = typeof(ArgumentException))]
        [Row(1, 0.99, 0.0, 0, 0, 0, true)]
        [Row(2, 0.0, 0.0, 0, 1, 0, true)]
        public void TestGetOperatorsAritySequence(int zeroArityOperatorsCount,
            double givenUnaryOperatorOccurenceProbability, double givenTernaryOperatorOccurenceProbability,
            int expectedUnaryOperatorsCount, int expectedBinaryOperatorsCount, int expectedTernaryOperatorsCount, 
            bool precise)
        {
            Random random = new Random();
            int[] arities = FormulaGenerator.GetOperatorsAritySequence(random, zeroArityOperatorsCount, 
                givenUnaryOperatorOccurenceProbability, givenTernaryOperatorOccurenceProbability).ToArray();

            double unaryOperatorsCount = arities.Count(a => a == 1);
            double binaryOperatorsCount = arities.Count(a => a == 2); 
            double ternaryOperatorsCount = arities.Count(a => a == 3);

            if (precise)
            {
                Assert.AreEqual(expectedUnaryOperatorsCount, unaryOperatorsCount);
                Assert.AreEqual(expectedBinaryOperatorsCount, binaryOperatorsCount);
                Assert.AreEqual(expectedTernaryOperatorsCount, ternaryOperatorsCount);
            }
            else
            {
                const int inaccuracy = 10;
                Assert.Between(expectedUnaryOperatorsCount, unaryOperatorsCount - inaccuracy, unaryOperatorsCount + inaccuracy);
                Assert.Between(expectedBinaryOperatorsCount, binaryOperatorsCount - inaccuracy, binaryOperatorsCount + inaccuracy);
                Assert.Between(expectedTernaryOperatorsCount, ternaryOperatorsCount - inaccuracy, ternaryOperatorsCount + inaccuracy);
            }
        }
    }
}
