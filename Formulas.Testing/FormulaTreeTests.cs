using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                            new TreeNode<Operator>(OperatorsLibrary.Neg,
                                new TreeNode<Operator>(_yVariable)),
                            new TreeNode<Operator>(_xVariable))));
        }

        [Test]
        public void TestSelectVariables()
        {
            string[] variableNames = FormulaTree.SelectVariables(_formulaRoot).Select(v => v.Name).ToArray();
            string[] expectedVariableNames = { _xVariable.Name, _yVariable.Name };
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

        [Test]
        public void TestEvalPerformance()
        {
            //string formula = "Sub Sqrt Sqrt Cos Sub Sub Sub Sum Cos Atan Ln Cos Sub x5 x0 Cos Atan Pow3 Cbrt Sum x5 x5 Sin Cbrt Cbrt Cos Sqrt Mul x5 x0 Cbrt Sub Mul Sub Mul Mul Sum x3 x5 Sum x4 x6 Sum Sub x1 x0 Sin x3 Sub Cbrt Sub x2 x3 Sum Sum x3 x2 Sub x1 x0 Cbrt Mul Sub Cbrt x6 Sub x2 x2 Sub Cbrt x6 Mul x3 x1 Sub Cbrt Sub Sub Sub x3 x2 Sub x4 x0 Atan Pow3 x6 Max Cos Cbrt Mul x6 x1 Sum Cbrt Mul x4 x3 Atan Ln x3 Sub Sub Sin Sum Mul Sum Atan Ln x5 Mul Sum x1 x0 Sin x4 Cbrt Sub Cos x1 Sum x3 x3 Mul Sin Max Mul x2 x0 Sin x0 Sub Sqrt Sum x2 x5 Cbrt Sub x2 x1 Sub Cbrt Sub Sin Sub Cbrt x4 Sub x6 x3 Sub Cbrt Sub x2 x0 Sub Cos x0 Sum x0 x2 Cos Sum Mul Sqrt Sub x6 x0 Sqrt Cos x5 Sub Sqrt Cos x3 Max Cos x1 Mul x2 x2 Sum Sum Cbrt Sub Cos Cbrt Sub x1 x5 Sub Atan Atan Ln x2 Sub Sub x6 x2 Sum x6 x4 Max Sub Sum Sub Sub x1 x6 Sub x5 x2 Cbrt Sub x3 x1 Sum Sum Cos x0 Sub x2 x2 Cos Cos x4 Sin Sqrt Cos Cos x6 Atan Sub Sqrt Sin Sub Cos x1 Sum x6 x0 Sub Sub Cbrt Cos x2 Sum Sub x1 x4 Cos x2 Sum Mul Sum x2 x1 Sum x2 x0 Sub Sum x1 x4 Sum x2 x5 Sub Cbrt Sub Sum Sin Cos Sub Sub Sqrt Max Max Sub x3 x4 Sum x0 x0 Sum Atan Ln x5 Atan Pow3 x4 Mul Sub Sqrt Atan Ln x5 Sub Cbrt x2 Sub x3 x4 Sub Sub Sub x3 x6 Sub x1 x6 Sub Sub x1 x4 Sin x3 Max Cbrt Sqrt Cos Sub x3 x5 Sum Sub Cbrt Sub x4 x1 Sum Cos x6 Sin x5 Sub Atan Atan Pow3 x4 Cbrt Sum x5 x3 Atan Ln Sum Sin Cbrt Cos Cos Sum x4 x3 Cbrt Atan Pow3 Sqrt Sub x4 x2 Cbrt Sub Sum Sub 8.3 Atan Pow3 Sqrt Sub Sum x3 x2 Cos x3 Sum Cos Cbrt Sin Atan Cos x4 Cbrt Sum Cos Sqrt Cos x6 Sub Sub Sum x5 x2 Sum x4 x5 Sum Sub x5 x4 Cbrt x6 Sub Cbrt Cos Sub Sub Cos Sub x6 x0 Sin Cos x0 Sum Mul Sum x0 x4 Cos x6 Sqrt Mul x6 x3 Sub Cbrt Sum Sum Max Sub x6 x6 Cos x1 Sin Mul x6 x4 Sub Sum Sub x4 x5 Sqrt x6 Cos Sum x4 x5 Sin Cbrt Sum Atan Ln x1 Sqrt Sub x3 x4 Sub Cbrt Sqrt Sum Mul Sum Sum Atan Ln Cbrt Cos x3 Max Sum Sub Sum x6 x1 Mul x3 x4 Cbrt Sum x2 x6 Atan Pow3 Mul x4 x2 Mul Sub Sub Cos Sum x4 x2 Sub Cos x4 Sub x0 x3 Cbrt Sqrt Sum x0 x0 Sub Cos Sum Sum x3 x3 Atan Ln x6 Sub Cbrt Sub x1 x1 Sum Sum x3 x2 Cbrt x4 Cbrt Atan Sqrt Mul Cos Sub x3 x6 Sub Sub x1 x5 Sqrt x2 Atan Mul Sum Cos Cbrt Sub Sqrt x4 Atan Ln x4 Sub Mul Sqrt Sub x5 x4 Max Mul x2 x3 Sum x5 x4 Sum Sub Sub x6 x4 Cos x6 Sum Max x5 x6 Sub x3 x1 Cbrt Sub Mul Sub Sqrt x3 Sum x6 x4 Atan Ln x5 Sum Sum Sin x5 Sum x5 x1 Cos Sqrt x6 Max Sum Sin Sub Cos Atan Ln Cbrt Sub Sqrt x2 Cos x2 Sum Sum Sum Atan Ln Mul x1 x0 Sub Sum Atan Ln x3 Cbrt x6 Cbrt Cos x3 Sum Cos Cbrt Sum x6 x0 Sum Cos Sub x4 x1 Sub Sub x6 x5 Cbrt x5 Sqrt Atan Ln Sub Sub x4 x4 Max x5 x5 Sum Sqrt Cbrt Cos Sub Sub Atan Pow3 x4 Sin Cbrt x6 Sub Cos Cbrt x1 Cbrt Sub x6 x0 Sqrt Sub Sum Cbrt Max Sqrt Mul x4 x4 Cbrt Cbrt x1 Sum Sub Cos Sqrt x6 Cbrt Cbrt x6 Sub Mul Sqrt x6 Sub x3 x0 Sum Sub x2 x2 Sub x5 x0 Sqrt Cbrt Sum Sub Sum x5 x1 Max x1 x0 Sin Cos x4 Sum Sub Sum Sum Sin Sum Sub Atan Pow3 x4 Sum Mul x2 x5 Sqrt x1 Sin Sqrt Cos x5 Cbrt Mul Sub Cbrt Cos x4 Mul Sub x6 x1 Sqrt x3 Mul Atan Ln x2 Atan Sub x6 x6 Cos Sin Cos Sum Sub Sub x5 x3 Sum x6 x6 Cbrt Sum x0 x6 Cos Sub Sin Sub Sqrt Sub Sub x4 x3 Sqrt x6 Sub Cos Atan Pow3 x5 Sqrt Cos x4 Sub Sqrt Sub Atan Pow3 x1 Cbrt Sub x4 x6 Sub Sub Sub Sub x0 x6 Atan Ln x0 Sqrt Sqrt x1 Sum Sub Sqrt x5 Sub x6 x1 Cos Atan Ln x4 Cbrt Cbrt Sqrt Sum Sin Sub Sub Atan Pow3 x6 Atan Pow3 x4 Sum Sum x4 x4 Mul x4 x3 Sub Sin Mul Sub x2 x6 Atan Ln x4 Sum Sub Sin x1 Atan Pow3 x6 Sub Atan Ln x4 Sub x2 x6";
            //FormulaTree ft = FormulaTreeSerializer.Deserialize(formula);
            //Random rnd = new Random();
            //Variable[] vars = ft.Variables.ToArray();
            //foreach (Variable v in ft.Variables)
            //    v.Value = rnd.Next();

            //const int attempts = 10;
            //for (int j = 0; j < attempts; j++)
            //{
            //    const int repeat = 100000;
            //    double[] res = new double[repeat];
            //    var sw = new System.Diagnostics.Stopwatch();
            //    sw.Start();
            //    for (int i = 0; i < repeat; i++)
            //    {
            //        res[i] = ft.Evaluate();
            //    }
            //    sw.Stop();
            // }
        }

        [TestCase("sum x x", 1, 3, -1, 3, -1, -1, 3, 3, new[] { 2.0f, 2.0f, 2.0f, 4.0f, 4.0f, 4.0f, 6.0f, 6.0f, 6.0f })]
        [TestCase("sum x y", 1, 3, 1, 3, -1, -1, 3, 3, new[] { 2.0f, 3.0f, 4.0f, 3.0f, 4.0f, 5.0f, 4.0f, 5.0f, 6.0f })]
        [TestCase("sum sum x y z", 1, 3, 1, 3, -3, 3, 3, 3, new[] { -1.0f, 0.0f, 1.0f, 1.0f, 2.0f, 3.0f, 3.0f, 4.0f, 5.0f })]
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
