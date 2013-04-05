using MbUnit.Framework;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeNodeTests
    {
        [Test]
        public void TestNodeCreation()
        {
            //// c2*x + Pi*y
            //FormulaTreeNode formulaTree = 
            //    new OperatorNode(OperatorsLibrary.Sum,
            //        new OperatorNode(OperatorsLibrary.Mul,
            //            new OperandNode(Constants.C2),
            //            new OperandNode(new Variable("x"))),
            //        new OperatorNode(OperatorsLibrary.Mul,
            //            new OperandNode(Constants.C2),
            //            new OperandNode(new Variable("y"))));

            //Assert.IsFalse(formulaTree.IsLeaf);
        }
    }
}
