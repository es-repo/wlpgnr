using System.Collections.Generic;
using System.Linq;  
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using System;
using WallpaperGenerator.Utilities.DataStructures.Trees;

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
        [Row(null, null)]
        [Row(2.0, null)]
        [Row(null, 3.0)]
        [Row(2.0, 3.0)]
        public void TestSetVariableValues(double? xVariableValue, double? yVariableValue)
        {
            IDictionary<string, double?> variableNamesAndValues = new Dictionary<string, double?>();
            if (xVariableValue != null)
                variableNamesAndValues.Add(_xVariable.Name, xVariableValue);

            if (yVariableValue != null)
                variableNamesAndValues.Add(_yVariable.Name, yVariableValue);

            FormulaTree.SetVariableValues(_formulaRoot, variableNamesAndValues); 

            Assert.AreEqual(xVariableValue, _xVariable.Value);
            Assert.AreEqual(yVariableValue, _yVariable.Value);
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

        [Test]
        public void TestSerialize()
        {
            TestSerialize(new FormulaTreeNode(OperatorsLibrary.C05), "(0.5)");

            TestSerialize(new FormulaTreeNode(new Variable("x")), "(x)");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum, 
                    new TreeNode<Operator>(OperatorsLibrary.C2),
                    new FormulaTreeNode(new Variable("y"))), "(Sum(2,y))");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.C2),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Variable("y")),
                        new FormulaTreeNode(new Variable("z")))), "(Sum(2,Mul(y,z)))");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                        new TreeNode<Operator>(OperatorsLibrary.Mul,
                            new TreeNode<Operator>(OperatorsLibrary.C3),
                            new TreeNode<Operator>(OperatorsLibrary.Minus, 
                                new TreeNode<Operator>(OperatorsLibrary.Minus,
                                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                                        new TreeNode<Operator>(OperatorsLibrary.C05)))))),
                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                         new TreeNode<Operator>(OperatorsLibrary.Minus,
                            new TreeNode<Operator>(OperatorsLibrary.Minus,
                                new TreeNode<Operator>(OperatorsLibrary.C7))))),
                "(Sum(Minus(Mul(3,Minus(Minus(Minus(0.5))))),Minus(Minus(Minus(7)))))");

            TestSerialize(_formulaRoot, "(Sum(Mul(2,x),Mul(7,Sum(Minus(y),5))))");
        }

        private static void TestSerialize(FormulaTreeNode formulaTree, string expected)
        {
            string str = FormulaTree.Serialize(formulaTree);
            Assert.AreEqual(expected, str);
        }

        [Test]
        public void TestDeserialize()
        {
        }
    }
}
