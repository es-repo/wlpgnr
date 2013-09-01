using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.FormulaTreeGeneration;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Testing.FormulaTreeGeneration
{
    [TestFixture]
    public class FormulaTreeGeneratorTests
    {
        private readonly FormulaTreeGenerator _formulaTreeGenerator;

        public FormulaTreeGeneratorTests()
        {
            Random random = new Random();
            _formulaTreeGenerator = new FormulaTreeGenerator(random, new FormulaTreeNodeFactory(random, null), new OperatorsAndOperandsConstantAcceptanceRules());
        }

        [RowTest]
        [Row(0, 0, 0, 0, ExpectedException=typeof(ArgumentException))]
        [Row(4, 4, 3, 2)]
        [Row(5, 4, 3, 2, ExpectedException = typeof(ArgumentException))]
        [Row(3, 5, 3, 2)]
        [Row(3, 9, 3, 2)]
        public void TestCreateRandomFormulaTree(int dimensionsCount, int variablesCount, int constantsCount, int unaryOperatorsCountForFormulaDiluting)
        {
            FormulaTree formulaTree = _formulaTreeGenerator.CreateRandomFormulaTree(dimensionsCount, variablesCount, constantsCount, 
                unaryOperatorsCountForFormulaDiluting, OperatorsLibrary.All);

            IEnumerable<TreeNode<Operator>> traversedNodes = Tree<Operator>.Traverse(formulaTree.Root, TraversalOrder.BredthFirstPreOrder).Select(ni => ni.Node);
            IEnumerable<Variable> variables = traversedNodes.Where(n => n.Value is Variable).Select(n => (Variable)n.Value);
            IEnumerable<Constant> constants = traversedNodes.Where(n => n.Value is Constant).Select(n => (Constant)n.Value);
            IEnumerable<Operator> unaryOperatorNodes = traversedNodes.Where(n => n.Value.Arity == 1).Select(n => n.Value);

            Assert.AreEqual(dimensionsCount, variables.Select(v => v.Name).Distinct().Count());
            Assert.AreEqual(variablesCount, variables.Count());
            Assert.AreEqual(constantsCount, constants.Count());
            Assert.AreEqual(unaryOperatorsCountForFormulaDiluting, unaryOperatorNodes.Count());
        }

        [RowTest]
        [Row(0, new[] { 0, 0, 0 }, ExpectedException = typeof(ArgumentException))]
        [Row(1, new[] { 1, 1, 0 }, ExpectedException = typeof(ArgumentException))]
        [Row(3, new[] { 3, 3, 3 }, ExpectedException = typeof(ArgumentException))]
        [Row(1, new []{0, 0, 0})]
        [Row(2, new[] { 3, 1, 0 })]
        [Row(3, new[] { 3, 0, 1 })]
        [Row(9, new[] { 1, 6, 1 })]
        public void TestCreateFormulaTree(int zeroArityOperatorsCount, int[] nonZeroOperatorsCounts)
        {
            Random random = new Random();

            IEnumerable<Operator> zeroArityOperators = EnumerableExtensions.Repeat(
                i => new Variable("x" + i.ToString(CultureInfo.InvariantCulture)), zeroArityOperatorsCount);

            IEnumerable<Operator> nonZeroArityOperators = nonZeroOperatorsCounts.Select((i, a) =>
                EnumerableExtensions.Repeat(_ => OperatorsLibrary.All.Where(op => op.Arity == a + 1).TakeRandom(random), nonZeroOperatorsCounts[a])).
                    SelectMany(_ => _);

            FormulaTree formulaTree = _formulaTreeGenerator.CreateFormulaTree(zeroArityOperators, nonZeroArityOperators.Randomize(random));

            IEnumerable<TreeNodeInfo<Operator>> traversedNodes = Tree<Operator>.Traverse(formulaTree.Root, TraversalOrder.BredthFirstPreOrder);
            for (int a = 0; a < 4; a++)
            {
                int aa = a;

                IEnumerable<TreeNode<Operator>> nArityNodes = traversedNodes.Select(ni => ni.Node).Where(n => n.Value.Arity == aa);
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
                    OperatorsLibrary.IfG0,
                    OperatorsLibrary.Sum,
                    OperatorsLibrary.Minus  
                };

            FormulaTree formulaTree = _formulaTreeGenerator.CreateFormulaTree(zeroArityOperators, nonZeroArityOperators);
            TreeNode<Operator> formulaTreeRootExpected = new TreeNode<Operator>(
                OperatorsLibrary.Minus,
                    new TreeNode<Operator>(OperatorsLibrary.Sum,
                        new TreeNode<Operator>(OperatorsLibrary.Mul,
                            new TreeNode<Operator>(new Variable("x0")),
                            new TreeNode<Operator>(new Variable("x1"))),
                        new TreeNode<Operator>(OperatorsLibrary.IfG0,
                            new TreeNode<Operator>(new Variable("x2")),
                            new TreeNode<Operator>(new Variable("x3")),
                            new TreeNode<Operator>(new Variable("x4")))));

            Assert.IsTrue(FormulaTree.Equal(new FormulaTree(formulaTreeRootExpected), formulaTree));
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
            int[] arities = _formulaTreeGenerator.GetNonZeroOperatorsAritySequence(zeroArityOperatorsCount,
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
