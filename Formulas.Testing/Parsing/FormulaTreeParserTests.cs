using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Parsing;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace Formulas.Testing.Parsing
{
    [TestFixture]
    public class FormulaTreeParserTests
    {
        [Test]
        public void TestParse()
        {
            TestParse("()", null);

            TestParse("(2)", new FormulaTreeNode(OperatorsLibrary.C2));

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
                        new FormulaTreeNode(OperatorsLibrary.C2),
                        new FormulaTreeNode(new Variable("x"))),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(OperatorsLibrary.C7),
                        new FormulaTreeNode(OperatorsLibrary.Sum,
                            new FormulaTreeNode(OperatorsLibrary.Minus,
                                new FormulaTreeNode(new Variable("y"))),
                            new FormulaTreeNode(OperatorsLibrary.C5)))));
        }

        private static void TestParse(string value, FormulaTreeNode expectedRoot)
        {
            FormulaTreeNode root = FormulaTreeParser.Parse(value);
            Assert.IsTrue(AreFormulaTreesEqual(root, expectedRoot));
        }

        private static bool AreFormulaTreesEqual(FormulaTreeNode rootA, FormulaTreeNode rootB)
        {
            if (rootA == null && rootB == null)
                return true;

            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedNodesA = Tree.TraverseBredthFirstPreOrder(rootA);
            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedNodesB = Tree.TraverseBredthFirstPreOrder(rootB);
            IEnumerator<TraversedTreeNodeInfo<Operator>> nodesInfoBEnumerator = traversedNodesB.GetEnumerator();
            foreach (var nodeInfoA in traversedNodesA)
            {
                nodesInfoBEnumerator.MoveNext();
                var nodeInfoB = nodesInfoBEnumerator.Current;
                if (nodeInfoB == null || !AreFormulaTreeNodesEqual((FormulaTreeNode)nodeInfoA.Node, (FormulaTreeNode)nodeInfoB.Node))
                    return false;
            }

            return true;
        }

        private static bool AreFormulaTreeNodesEqual(FormulaTreeNode nodeA, FormulaTreeNode nodeB)
        {
            Operator opA = nodeA.Value;
            Operator opB = nodeB.Value;
            
            if (opA is Constant && opB is Constant)
            {
                return ((Constant) opA).Value.Equals(((Constant) opB).Value);
            }

            if (opA is Variable && opB is Variable)
            {
                return ((Variable)opA).Name == ((Variable)opB).Name;
            }
            
            return opA.GetType() == opB.GetType();
        }
    }
}
