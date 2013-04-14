using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeSerializerTests
    {
        [Test]
        public void TestSerialize()
        {
            TestSerialize(new FormulaTreeNode(ConstantsLibrary.C05), "(0.5)",
@"(
0.5)");

            TestSerialize(new FormulaTreeNode(new Variable("x")), "(x)",
@"(
x)");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(ConstantsLibrary.C2),
                    new FormulaTreeNode(new Variable("y"))), "(Sum(2 y))", 
@"(
Sum(
	2
	y))");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(ConstantsLibrary.C2),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Variable("y")),
                        new FormulaTreeNode(new Variable("z")))), "(Sum(2 Mul(y z)))", 
@"(
Sum(
	2
	Mul(
		y
		z)))");

            TestSerialize(
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                        new TreeNode<Operator>(OperatorsLibrary.Mul,
                            new TreeNode<Operator>(ConstantsLibrary.C3),
                            new TreeNode<Operator>(OperatorsLibrary.Minus,
                                new TreeNode<Operator>(OperatorsLibrary.Minus,
                                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                                        new TreeNode<Operator>(ConstantsLibrary.C05)))))),
                    new TreeNode<Operator>(OperatorsLibrary.Minus,
                         new TreeNode<Operator>(OperatorsLibrary.Minus,
                            new TreeNode<Operator>(OperatorsLibrary.Minus,
                                new TreeNode<Operator>(ConstantsLibrary.C7))))),
                "(Sum(Minus(Mul(3 Minus(Minus(Minus(0.5))))) Minus(Minus(Minus(7)))))",
@"(
Sum(
	Minus(
		Mul(
			3
			Minus(
				Minus(
					Minus(
						0.5)))))
	Minus(
		Minus(
			Minus(
				7)))))");

            TestSerialize(// 2*x + 7*(-y + 5)            
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(ConstantsLibrary.C2),
                        new FormulaTreeNode(new Variable("x"))),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(ConstantsLibrary.C7),
                        new FormulaTreeNode(OperatorsLibrary.Sum,
                            new FormulaTreeNode(OperatorsLibrary.Minus,
                                new FormulaTreeNode(new Variable("y"))),
                            new FormulaTreeNode(ConstantsLibrary.C5)))), "(Sum(Mul(2 x) Mul(7 Sum(Minus(y) 5))))",
@"(
Sum(
	Mul(
		2
		x)
	Mul(
		7
		Sum(
			Minus(
				y)
			5))))");
        }

        private static void TestSerialize(FormulaTreeNode formulaTree, string expectedNoIndent, string expectedWithIndent)
        {
            string noIndentResult = FormulaTreeSerializer.Serialize(formulaTree, new FormulaTreeSerializationOptions { WithIndentation = false });
            Assert.AreEqual(expectedNoIndent, noIndentResult);

            string withIndentResult = FormulaTreeSerializer.Serialize(formulaTree, new FormulaTreeSerializationOptions { WithIndentation = true });
            expectedWithIndent = expectedWithIndent.Replace("\r\n", "\n");
            Assert.AreEqual(expectedWithIndent, withIndentResult);
        }
    }
}
