using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeTests
    {
        private TreeNode<Operator> _formulaRoot;
        private Variable _xVariable;
        private Variable _yVariable;

        [SetUp]
        public void SetUp()
        {
            _xVariable = new Variable("x");
            _yVariable = new Variable("y");

            // 2*x + 7*(-y + x)
            _formulaRoot =
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(2)),
                        new TreeNode<Operator>(_xVariable)),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(7)),
                        new TreeNode<Operator>(OperatorsLibrary.Sum,
                            new TreeNode<Operator>(OperatorsLibrary.Minus,
                                new TreeNode<Operator>(_yVariable)),
                            new TreeNode<Operator>(_xVariable))));
        }
        
        [Test]
        public void TestSelectVariables()
        {
            string[] variableNames = FormulaTree.SelectVariables(_formulaRoot).Select(v => v.Name).ToArray();
            string[] expectedVariableNames = {_xVariable.Name, _yVariable.Name};
            CollectionAssert.AreEqual(expectedVariableNames, variableNames);
        }
        
        [TestCase(5.0, 3.0, 24)]
        [TestCase(0.0, 0.0, 0.0)]
        public void TestEvaluate(double xVariableValue, double yVariableValue, double expectedResult)
        {
            FormulaTree formulaTree = new FormulaTree(_formulaRoot);
            formulaTree.Variables[0].Value = xVariableValue;
            formulaTree.Variables[1].Value = yVariableValue;
            double result = new FormulaTree(_formulaRoot).Evaluate();
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("sum x x", 1, 3, -1, 3, -1, -1, 3, 3, new[] { 2.0f, 2.0f, 2.0f, 4.0f, 4.0f, 4.0f, 6.0f, 6.0f, 6.0f })]
        [TestCase("sum x y", 1, 3, 1, 3, -1, -1, 3, 3, new[] { 2.0f, 3.0f, 4.0f, 3.0f, 4.0f, 5.0f, 4.0f, 5.0f, 6.0f })]
        [TestCase("sum sum x y z", 1, 3, 1, 3, -3, 3, 3, 3, new [] { -1.0f, 0.0f, 1.0f, 1.0f, 2.0f, 3.0f, 3.0f, 4.0f, 5.0f })]
        public void TestEvaluateRangesIn2DProjection(string formula, double rangeXStart, double rangeXEnd,
            double rangeYStart, double rangeYEnd,
            double rangeZStart, double rangeZEnd,
            int xCount, int yCount, float[] expectedResults)
        {
            FormulaTree formulaTree = FormulaTreeSerializer.Deserialize(formula);
            List<Range> ranges = new List<Range>();
            if (!rangeXStart.Equals(-1))
                ranges.Add(new Range(rangeXStart, rangeXEnd));

            if (!rangeYStart.Equals(-1))
                ranges.Add(new Range(rangeYStart, rangeYEnd));

            if (!rangeZStart.Equals(-1))
                ranges.Add(new Range(rangeZStart, rangeZEnd));

            float[] results = new float[xCount * yCount];
            formulaTree.EvaluateRangesIn2DProjection(ranges.ToArray(), xCount, 0, yCount, results);
            CollectionAssert.AreEqual(expectedResults, results);
        }
    }
}
