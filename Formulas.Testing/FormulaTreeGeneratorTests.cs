using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeGeneratorTests
    {
        [RowTest]
        [Row(0, 0, 0, ExpectedException=typeof(ArgumentException))]
        [Row(4, 3, 2)]
        public void TestCreateRandomFormulaTree(int variablesCount, int constantsCount, int unaryOperatorsCountForFormulaDiluting)
        {
            FormulaTreeNode formulaTree = FormulaTreeGenerator.CreateRandomFormulaTree(variablesCount, constantsCount, unaryOperatorsCountForFormulaDiluting,
                OperatorsLibrary.All);

            IEnumerable<FormulaTreeNode> traversedNodes = Tree<Operator>.TraverseBredthFirstPreOrder(formulaTree).Select(ni => (FormulaTreeNode)ni.Node);
            IEnumerable<Variable> variables = traversedNodes.Where(n => n.Operator is Variable).Select(n => (Variable) n.Operator);
            IEnumerable<Constant> constants = traversedNodes.Where(n => n.Operator is Constant).Select(n => (Constant)n.Operator);
            IEnumerable<Operator> unaryOperatorNodes = traversedNodes.Where(n => n.Operator.Arity == 1).Select(n => n.Operator);

            Assert.AreEqual(variablesCount, variables.Count());
            Assert.AreEqual(constantsCount, constants.Count());
            Assert.AreEqual(unaryOperatorsCountForFormulaDiluting, unaryOperatorNodes.Count());
        }

        [RowTest]
        [Row(0, new[] { 0, 0, 0 }, ExpectedException = typeof(ArgumentException))]
        [Row(1, new[] { 1, 1, 0 }, ExpectedException = typeof(ArgumentException))]
        [Row(3, new[] { 3, 3, 3 }, ExpectedException = typeof(ArgumentException))]
        [Row(1, new []{0, 0, 0})]
        [Row(2, new []{3, 1, 0 })]
        [Row(3, new []{ 3, 0, 1 })]
        [Row(9, new[] { 1, 6, 1 })]
        public void TestCreateFormulaTree(int zeroArityOperatorsCount, int[] nonZeroOperatorsCountS)
        {
            Random random = new Random();

            IEnumerable<Operator> zeroArityOperators = EnumerableExtensions.Repeat(
                i => new Variable("x" + i.ToString(CultureInfo.InvariantCulture)), zeroArityOperatorsCount).Cast<Operator>();

            IEnumerable<Operator> nonZeroArityOperators = nonZeroOperatorsCountS.Select((i, a) =>
                EnumerableExtensions.Repeat(_ => OperatorsLibrary.All.Where(op => op.Arity == a + 1).TakeRandom(random), nonZeroOperatorsCountS[a])).
                    SelectMany(_ => _);
            
            FormulaTreeNode formulaTree = FormulaTreeGenerator.CreateFormulaTree(zeroArityOperators, nonZeroArityOperators.Randomize(random));

            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedNodes = Tree<Operator>.TraverseBredthFirstPreOrder(formulaTree);
            for (int a = 0; a < 4; a++)
            {
                IEnumerable<FormulaTreeNode> nArityNodes = traversedNodes.Select(ni => (FormulaTreeNode)ni.Node).Where(n => n.Operator.Arity == a);
                Assert.IsTrue(nArityNodes.All(n => n.Children.Count == a));
            }
        }

        [Test]
        public void TestCreateFormulaTreeSpecific()
        {
            IEnumerable<Operator> zeroArityOperators = EnumerableExtensions.Repeat(i => (Operator)new Variable("x" + i.ToString(CultureInfo.InvariantCulture)), 5);
            IEnumerable<Operator> nonZeroArityOperators = new List<Operator>
                {
                    OperatorsLibrary.Mul,
                    OperatorsLibrary.Conditional,
                    OperatorsLibrary.Sum,
                    OperatorsLibrary.Minus  
                };

            FormulaTreeNode formulaTree = FormulaTreeGenerator.CreateFormulaTree(zeroArityOperators, nonZeroArityOperators);
            FormulaTreeNode formulaTreeExpected = new FormulaTreeNode(
                OperatorsLibrary.Minus,
                    new FormulaTreeNode(OperatorsLibrary.Sum,
                        new FormulaTreeNode(OperatorsLibrary.Mul,
                            new FormulaTreeNode(new Variable("x0")),
                            new FormulaTreeNode(new Variable("x1"))),
                        new FormulaTreeNode(OperatorsLibrary.Conditional,
                            new FormulaTreeNode(new Variable("x2")),
                            new FormulaTreeNode(new Variable("x3")),
                            new FormulaTreeNode(new Variable("x4")))));
            
            Assert.IsTrue(Utilities.AreFormulaTreesEqual(formulaTreeExpected, formulaTree));
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
