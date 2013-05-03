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
                        new FormulaTreeNode(OperatorsLibrary.C2),
                        new FormulaTreeNode(_xVariable)),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(OperatorsLibrary.C7),
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
            double result = new FormulaTree(_formulaRoot).Evaluate(xVariableValue, yVariableValue, 0);
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
    }
}
