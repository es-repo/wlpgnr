using System.Linq;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Parsing;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Testing.Parsing
{
    [TestFixture]
    public class FormulaTreeParserTests
    {
        [Test]
        public void TestParse()
        {
            TestParse("()", null);

            TestParse("(2)", new TreeNode<Operator>(new Constant(2)));

            TestParse("(x)", new TreeNode<Operator>(new Variable("x")));
            
            TestParse("(Sum(x y)",
                new TreeNode<Operator>(
                    OperatorsLibrary.Sum,
                        new TreeNode<Operator>(new Variable("x")),
                        new TreeNode<Operator>(new Variable("y"))));

            TestParse(// 2*x + 7*(-y + 5)            
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
			5))))",
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(2)),
                        new TreeNode<Operator>(new Variable("x"))),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(7)),
                        new TreeNode<Operator>(OperatorsLibrary.Sum,
                            new TreeNode<Operator>(OperatorsLibrary.Minus,
                                new TreeNode<Operator>(new Variable("y"))),
                            new TreeNode<Operator>(new Constant(5))))));
        }

        [Test]
        public void TestParseWithSameVariableSeveralOccurences()
        {
            const string formulaString = "Sum(x x)";
            TreeNode<Operator> formulaTreeRoot = FormulaTreeParser.Parse(formulaString);
            IEnumerable<TreeNodeInfo<Operator>> traversedTree = Tree<Operator>.Traverse(formulaTreeRoot, TraversalOrder.DepthFirstPostOrder);
            Variable[] xVariables = traversedTree.Where(ni => ni.Node.Value is Variable).Select(ni => (Variable)ni.Node.Value).ToArray();
            Assert.IsTrue(ReferenceEquals(xVariables[0], xVariables[1]));
        }

        private static void TestParse(string value, TreeNode<Operator> expectedRoot)
        {
            TreeNode<Operator> root = FormulaTreeParser.Parse(value);
            Assert.IsTrue(FormulaTree.Equal(root, expectedRoot));
        }
    }
}
