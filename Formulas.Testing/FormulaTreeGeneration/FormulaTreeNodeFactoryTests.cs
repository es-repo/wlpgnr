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
            Variable variableX = new Variable("x");
            FormulaTreeNode variableXNode = FormulaTreeNodeFactory.Create(variableX, null);
            Assert.AreEqual(variableX, variableXNode.Operator);
            Assert.AreEqual(0, variableXNode.Children.Count);

            Variable variableY = new Variable("y");
            FormulaTreeNode variableYNode = FormulaTreeNodeFactory.Create(variableY, null);

            OperatorGuard sumRootGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan));
            FormulaTreeNode sumNodeWithRootGuard = FormulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootGuard.Operator, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootGuard.Operands.First().Operator, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootGuard.Operands.First().Children.Count);

            OperatorGuard sumRootAndSecondChildGuard = new OperatorGuard(new FormulaTreeNodeWrapper(OperatorsLibrary.Atan), 
                new Dictionary<int, FormulaTreeNodeWrapper>{  {1, new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh) }});
            FormulaTreeNode sumNodeWithRootAndSecondChildGuard = 
                FormulaTreeNodeFactory.Create(OperatorsLibrary.Sum, sumRootAndSecondChildGuard, variableXNode, variableYNode);

            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Operator, OperatorsLibrary.Atan);
            Assert.AreEqual(1, sumNodeWithRootAndSecondChildGuard.Children.Count);
            Assert.AreEqual(sumNodeWithRootAndSecondChildGuard.Operands.First().Operator, OperatorsLibrary.Sum);
            Assert.AreEqual(2, sumNodeWithRootAndSecondChildGuard.Operands.First().Children.Count);

            FormulaTreeNode sumSecondOperandGuarded = sumNodeWithRootAndSecondChildGuard.Operands.First().Operands.Skip(1).First();
            Assert.AreEqual(sumSecondOperandGuarded.Operator, OperatorsLibrary.Tanh);
            Assert.AreEqual(sumSecondOperandGuarded.Operands.First().Operator, variableY);
        }
    }
}
