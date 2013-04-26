using System;
using System.Linq;  
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

namespace Formulas.Testing
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

            // 2*x + 7*(-y + 5)
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
                            new FormulaTreeNode(OperatorsLibrary.C5))));
        }
        
        [Test]
        public void TestSelectVariables()
        {
            string[] variableNames = FormulaTree.SelectVariables(_formulaRoot).Select(v => v.Name).ToArray();
            string[] expectedVariableNames = new []{_xVariable.Name, _yVariable.Name};
            CollectionAssert.AreElementsEqual(expectedVariableNames, variableNames);
        }
        
        [RowTest]
        [Row(null, null, 0, ExpectedException = typeof(InvalidOperationException))]
        [Row(5.0, null, 0, ExpectedException = typeof(InvalidOperationException))]
        [Row(null, 3.0, 0, ExpectedException = typeof(InvalidOperationException))]
        [Row(5.0, 3.0, 24)]
        [Row(0.0, 0.0, 35)]
        public void TestEvaluation(double? xVariableValue, double? yVariableValue, double expectedResult)
        {
            _xVariable.Value = xVariableValue;
            _yVariable.Value = yVariableValue;
            double result = FormulaTree.Evaluate(_formulaRoot);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
