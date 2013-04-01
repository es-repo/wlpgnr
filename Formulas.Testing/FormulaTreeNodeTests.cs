using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.FormulaTreeNodes;
using WallpaperGenerator.Formulas.Operands;
using WallpaperGenerator.Formulas.Operators;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeNodeTests
    {
        [Test]
        public void TestNodeCreation()
        {
            // c2*x + Pi*y

            Variable x = new Variable();
            Variable y = new Variable();

            FormulaTreeNode formulaTree = 
                new OperatorNode(OperatorsLibrary.Sum,
                    new OperatorNode(OperatorsLibrary.Mul,
                        new OperandNode(Constants.C2),
                        new OperandNode(x)),
                    new OperatorNode(OperatorsLibrary.Mul,
                        new OperandNode(Constants.C2),
                        new OperandNode(y)));

            Assert.IsFalse(formulaTree.IsLeaf);
        }
    }
}
