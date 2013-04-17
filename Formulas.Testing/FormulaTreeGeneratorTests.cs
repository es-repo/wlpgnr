using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeGeneratorTests
    {
        [Test]
        public void TestCreateRandomFormulaTree()
        {
            //FormulaTreeGenerator
        }

        [RowTest]
        [Row(0, 0, 0, 0, ExpectedException=typeof(ArgumentException))]
        [Row(1, 1, 1, 0, ExpectedException = typeof(ArgumentException))]
        [Row(3, 3, 3, 3, ExpectedException = typeof(ArgumentException))]
        //[Row(1, 0, ExpectedException = typeof(ArgumentException))]
        //[Row(1, 0, ExpectedException = typeof(ArgumentException))]
        public void TestCreateFormulaTree(int zeroArityOperatorsCount, int unaryOperatorsCount, int binaryOperatorsCount, int ternaryOperatorsCount)
        {
            Random random = new Random();

            IEnumerable<Operator> zeroArityOperators = EnumerableExtensions.Repeat(
                i => new Variable("x" + i.ToString(CultureInfo.InvariantCulture)), zeroArityOperatorsCount).Cast<Operator>();

            IEnumerable<Operator> unaryOperators = EnumerableExtensions.Repeat(
                i => OperatorsLibrary.All.Where(op => op.Arity == 1).TakeRandom(random), unaryOperatorsCount);

            IEnumerable<Operator> binaryOperators = EnumerableExtensions.Repeat(
                i => OperatorsLibrary.All.Where(op => op.Arity == 2).TakeRandom(random), unaryOperatorsCount);

            IEnumerable<Operator> ternaryOperators = EnumerableExtensions.Repeat(
                i => OperatorsLibrary.All.Where(op => op.Arity == 2).TakeRandom(random), unaryOperatorsCount);

            IEnumerable<Operator> nonZeroArityOperators = unaryOperators.Concat(binaryOperators).Concat(ternaryOperators);
            IEnumerable<Operator> nonZeroArityOperatorsShuffled = EnumerableExtensions.Repeat(i => nonZeroArityOperators.TakeRandom(random), nonZeroArityOperators.Count());

            FormulaTreeGenerator.CreateFormulaTree(zeroArityOperators, nonZeroArityOperatorsShuffled);
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
            int[] arities = FormulaTreeGenerator.GetNonZeroOperatorsAritySequence(random, zeroArityOperatorsCount,
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
