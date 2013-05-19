using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeTests
    {
        private FormulaTreeNode _formulaRoot;
        private Variable _xVariable;
        private Variable _yVariable;

        [SetUp]
        public void SetUp()
        {
            _xVariable = new Variable("x");
            _yVariable = new Variable("y");

            // 2*x + 7*(-y + x)
            _formulaRoot =
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Constant(2)),
                        new FormulaTreeNode(_xVariable)),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Constant(7)),
                        new FormulaTreeNode(OperatorsLibrary.Sum,
                            new FormulaTreeNode(OperatorsLibrary.Minus,
                                new FormulaTreeNode(_yVariable)),
                            new FormulaTreeNode(_xVariable))));
        }
        
        [Test]
        public void TestSelectVariables()
        {
            string[] variableNames = FormulaTree.SelectVariables(_formulaRoot).Select(v => v.Name).ToArray();
            string[] expectedVariableNames = new []{_xVariable.Name, _yVariable.Name};
            CollectionAssert.AreElementsEqual(expectedVariableNames, variableNames);
        }
        
        [RowTest]
        [Row(5.0, 3.0, 24)]
        [Row(0.0, 0.0, 0.0)]
        public void TestEvaluate(double xVariableValue, double yVariableValue, double expectedResult)
        {
            FormulaTree formulaTree = new FormulaTree(_formulaRoot);
            formulaTree.Variables[0].Value = xVariableValue;
            formulaTree.Variables[1].Value = yVariableValue;
            double result = new FormulaTree(_formulaRoot).Evaluate();
            Assert.AreEqual(expectedResult, result);
        }

        [RowTest]
        [Row(new []{5.0}, new []{3.0}, new []{24.0})]
        [Row(new[] { 1.0, 2.0 }, new[] { 1.0, 2.0 }, new[] { 2.0, -5.0, 11.0, 4.0 })]
        [Row(new[] { 0.0, 0.0, 0.0 }, new[] { 0.0, 0.0, 0.0 }, new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0})]
        public void TestEvaluateSeries(double[] xVariableValues, double[] yVariableValues, double[] expectedResults)
        {
            // 2*x + 7*(-y + x)
            double[] results = new FormulaTree(_formulaRoot).EvaluateSeries(xVariableValues, yVariableValues).ToArray();
            CollectionAssert.AreElementsEqual(expectedResults, results);
        }

        [RowTest]
        [Row(0, 2, 0, 2, new [] {0.0, -7.0, 9.0, 2.0 })]
        [Row(0, 3, 0, 3, new [] {0.0, -7.0, -14.0, 9.0, 2.0, -5.0, 18.0, 11.0, 4.0 })]
        [Row(-1, 3, -1, 3, new[] { -2.0, -9.0, -16.0, 7.0, 0.0, -7.0, 16.0, 9.0, 2.0 })]
        public void TestEvaluateRanges(int rangeXStart, int rangeXCount, int rangeYStart, int rangeYCount, double[] expectedResults)
        {
            // 2*x + 7*(-y + x)
            Range rangeX = new Range(rangeXStart, rangeXCount);
            Range rangeY = new Range(rangeYStart, rangeYCount);
            double[] results = new FormulaTree(_formulaRoot).EvaluateRanges(rangeX, rangeY).ToArray();
            CollectionAssert.AreElementsEqual(expectedResults, results);
        }

        [RowTest]
        [Row("sum(x x)", new[] { 1.0, 2.0, 3.0 }, null, null, new[] { 2.0, 4.0, 6.0 })]
        [Row("sum(x y)", new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0, 3.0 }, null, new[] { 2.0, 3.0, 4.0, 3.0, 4.0, 5.0, 4.0, 5.0, 6.0 })]
        [Row("sum(sum(x y) z))", new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0, 3.0 }, new[] { -1.0, -2.0, -3.0 }, new[] { 1.0, 2.0, 3.0, 1.0, 2.0, 3.0, 1.0, 2.0, 3.0 })]
        public void TestEvaluateSeriesIn2DProjection(string formula, double[] xVariableValues, double[] yVariableValues, double[] zVariableValues, double[] expectedResults)
        {
            FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(formula);
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            List<IEnumerable<double>> variablesValues = new List<IEnumerable<double>>();
            if (xVariableValues != null)
                variablesValues.Add(xVariableValues);

            if (yVariableValues != null)
                variablesValues.Add(yVariableValues);

            if (zVariableValues != null)
                variablesValues.Add(zVariableValues);

            double[] results = formulaTree.EvaluateSeriesIn2DProjection(variablesValues.ToArray()).ToArray();
            CollectionAssert.AreElementsEqual(expectedResults, results);
        }

        [RowTest]
        [Row("sum(x x)", 1, 3, -1, -1, -1, -1, new[] { 2.0, 4.0, 6.0 })]
        [Row("sum(x y)", 1, 3, 1, 3, -1, -1, new[] { 2.0, 3.0, 4.0, 3.0, 4.0, 5.0, 4.0, 5.0, 6.0 })]
        [Row("sum(sum(x y) z))",  1, 3, 1, 3, -3, 3, new[] { -1.0, 0.0, 1.0, 1.0, 2.0, 3.0, 3.0, 4.0, 5.0 })]
        public void TestEvaluateRangesIn2DProjection(string formula, double rangeXStart, int rangeXCount, double rangeYStart, int rangeYCount, double rangeZStart, int rangeZCount, double[] expectedResults)
        {
            FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(formula);
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            List<Range> variablesValueRanges = new List<Range>();
            if (!rangeXStart.Equals(-1))
                variablesValueRanges.Add(new Range(rangeXStart, rangeXCount));

            if (!rangeYStart.Equals(-1))
                variablesValueRanges.Add(new Range(rangeYStart, rangeYCount));

            if (!rangeZStart.Equals(-1))
                variablesValueRanges.Add(new Range(rangeZStart, rangeZCount));

            double[] results = formulaTree.EvaluateRangesIn2DProjection(variablesValueRanges.ToArray()).ToArray();
            CollectionAssert.AreElementsEqual(expectedResults, results);
        }
    }
}
