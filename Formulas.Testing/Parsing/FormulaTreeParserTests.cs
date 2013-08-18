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

            TestParse("(2)", new FormulaTreeNode(new Constant(2)));

            TestParse("(x)", new FormulaTreeNode(new Variable("x")));
            
            TestParse("(Sum(x y)",
                new FormulaTreeNode(
                    OperatorsLibrary.Sum,
                        new FormulaTreeNode(new Variable("x")),
                        new FormulaTreeNode(new Variable("y"))));

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
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Constant(2)),
                        new FormulaTreeNode(new Variable("x"))),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(new Constant(7)),
                        new FormulaTreeNode(OperatorsLibrary.Sum,
                            new FormulaTreeNode(OperatorsLibrary.Minus,
                                new FormulaTreeNode(new Variable("y"))),
                            new FormulaTreeNode(new Constant(5))))));
        }

        [Test]
        public void TestParseWithSameVariableSeveralOccurences()
        {
            const string formulaString = "Sum(x x)";
            FormulaTreeNode formulaTreeRoot = FormulaTreeParser.Parse(formulaString);
            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedTree = Tree<Operator>.TraverseDepthFirstPostOrder(formulaTreeRoot);
            Variable[] xVariables = traversedTree.Where(ni => ni.Node.Value is Variable).Select(ni => (Variable)ni.Node.Value).ToArray();
            Assert.IsTrue(ReferenceEquals(xVariables[0], xVariables[1]));
        }

        private static void TestParse(string value, FormulaTreeNode expectedRoot)
        {
            FormulaTreeNode root = FormulaTreeParser.Parse(value);
            Assert.IsTrue(Utilities.AreFormulaTreesEqual(root, expectedRoot));
        }
    }
}
