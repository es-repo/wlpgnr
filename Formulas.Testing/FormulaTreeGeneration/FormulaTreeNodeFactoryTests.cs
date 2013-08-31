using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.FormulaTreeGeneration;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Testing.FormulaTreeGeneration
{
    [TestFixture]
    public class FormulaTreeNodeFactoryTests
    {
        [Test]
        public void TestCreate()
        {
            FormulaTreeNodeFactory formulaTreeNodeFactory = new FormulaTreeNodeFactory(new Random(), null);
            
            Variable variableX = new Variable("x");
            TreeNode<Operator> variableXNode = formulaTreeNodeFactory.Create(variableX);
            Assert.AreEqual(variableX, variableXNode.Value);
            Assert.AreEqual(0, variableXNode.Children.Count);

            Variable variableY = new Variable("y");
            TreeNode<Operator> variableYNode = formulaTreeNodeFactory.Create(variableY);

            OperatorGuard sumRootGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan));
            TreeNode<Operator> sumNodeWithRootGuard = formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootGuard.Value, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootGuard.Children.First().Value, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootGuard.Children.First().Children.Count);

            OperatorGuard sumRootAndSecondChildGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan), 
                new Dictionary<int, FormulaTreeNodeWrapper>{  {1, new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh) }});
            TreeNode<Operator> sumNodeWithRootAndSecondChildGuard =
                formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootAndSecondChildGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Value, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootAndSecondChildGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Children.First().Value, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootAndSecondChildGuard.Children.First().Children.Count);

            TreeNode<Operator> sumSecondOperandGuarded = sumNodeWithRootAndSecondChildGuard.Children.First().Children.Skip(1).First();
            Assert.AreEqual(sumSecondOperandGuarded.Value, OperatorsLibrary.Tanh);
            Assert.AreEqual(sumSecondOperandGuarded.Children.First().Value, variableY);
        }

        [Test]
        public void TestCreateWithPredefinedGuards()
        {
            FormulaTreeNodeFactory formulaTreeNodeFactory = new FormulaTreeNodeFactory(new Random(), 
                new Dictionary<Operator, OperatorGuards>
                {
                    {
                        OperatorsLibrary.Sum,
                        new OperatorGuards(
                            new[] { new FormulaTreeNodeWrapper(OperatorsLibrary.Atan) },
                            new Dictionary<int, FormulaTreeNodeWrapper[]>
                            {
                                {
                                    1, 
                                    new[] { new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh) }
                                }
                            })
                    }
                 });

            Variable variableX = new Variable("x");
            TreeNode<Operator> variableXNode = formulaTreeNodeFactory.Create(variableX);
            Assert.AreEqual(variableX, variableXNode.Value);
            
            Variable variableY = new Variable("y");
            TreeNode<Operator> variableYNode = formulaTreeNodeFactory.Create(variableY);

            TreeNode<Operator> guardedSumNode = formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, variableXNode, variableYNode);
            Assert.AreEqual(OperatorsLibrary.Atan, guardedSumNode.Value);

            TreeNode<Operator> sumNode = guardedSumNode.Children.First();
            Assert.AreEqual(OperatorsLibrary.Sum, sumNode.Value);

            TreeNode<Operator> guardedYVariableNode = sumNode.Children.Skip(1).First();
            Assert.AreEqual(OperatorsLibrary.Tanh, guardedYVariableNode.Value);

            TreeNode<Operator> yVariableNode = guardedYVariableNode.Children.First();
            Assert.AreEqual(variableY, yVariableNode.Value);
        }
    }
}
