using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.FormulaTreeGeneration;
using WallpaperGenerator.Formulas.Operators;

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
            FormulaTreeNode variableXNode = formulaTreeNodeFactory.Create(variableX);
            Assert.AreEqual(variableX, variableXNode.Operator);
            Assert.AreEqual(0, variableXNode.Children.Count);

            Variable variableY = new Variable("y");
            FormulaTreeNode variableYNode = formulaTreeNodeFactory.Create(variableY);

            OperatorGuard sumRootGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan));
            FormulaTreeNode sumNodeWithRootGuard = formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootGuard.Operator, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootGuard.Operands.First().Operator, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootGuard.Operands.First().Children.Count);

            OperatorGuard sumRootAndSecondChildGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan), 
                new Dictionary<int, FormulaTreeNodeWrapper>{  {1, new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh) }});
            FormulaTreeNode sumNodeWithRootAndSecondChildGuard =
                formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootAndSecondChildGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Operator, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootAndSecondChildGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Operands.First().Operator, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootAndSecondChildGuard.Operands.First().Children.Count);

            FormulaTreeNode sumSecondOperandGuarded = sumNodeWithRootAndSecondChildGuard.Operands.First().Operands.Skip(1).First();
            Assert.AreEqual(sumSecondOperandGuarded.Operator, OperatorsLibrary.Tanh);
            Assert.AreEqual(sumSecondOperandGuarded.Operands.First().Operator, variableY);
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
            FormulaTreeNode variableXNode = formulaTreeNodeFactory.Create(variableX);
            Assert.AreEqual(variableX, variableXNode.Operator);
            
            Variable variableY = new Variable("y");
            FormulaTreeNode variableYNode = formulaTreeNodeFactory.Create(variableY);

            FormulaTreeNode guardedSumNode = formulaTreeNodeFactory.Create(OperatorsLibrary.Sum, variableXNode, variableYNode);
            Assert.AreEqual(OperatorsLibrary.Atan, guardedSumNode.Operator);

            FormulaTreeNode sumNode = guardedSumNode.Operands.First();
            Assert.AreEqual(OperatorsLibrary.Sum, sumNode.Operator);

            FormulaTreeNode guardedYVariableNode = sumNode.Operands.Skip(1).First();
            Assert.AreEqual(OperatorsLibrary.Tanh, guardedYVariableNode.Operator);

            FormulaTreeNode yVariableNode = guardedYVariableNode.Operands.First();
            Assert.AreEqual(variableY, yVariableNode.Operator);
        }
    }
}
