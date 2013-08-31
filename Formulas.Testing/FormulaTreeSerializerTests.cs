using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeSerializerTests
    {
        [Test]
        public void TestSerialize()
        {
            TestSerialize(new TreeNode<Operator>(new Constant(0.5)), "0.5");

            TestSerialize(new TreeNode<Operator>(new Variable("x")), "x");

            TestSerialize(
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(new Constant(2)),
                    new TreeNode<Operator>(new Variable("y"))), 
                "Sum 2 y");

            TestSerialize(
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(new Constant(2)),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Variable("y")),
                        new TreeNode<Operator>(new Variable("z")))), 
                "Sum 2 Mul y z");

            TestSerialize(
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Abs,
                        new TreeNode<Operator>(OperatorsLibrary.Mul,
                            new TreeNode<Operator>(new Constant(3)),
                            new TreeNode<Operator>(OperatorsLibrary.Abs,
                                new TreeNode<Operator>(OperatorsLibrary.Abs,
                                    new TreeNode<Operator>(OperatorsLibrary.Abs,
                                        new TreeNode<Operator>(new Constant(0.5))))))),
                    new TreeNode<Operator>(OperatorsLibrary.Abs,
                         new TreeNode<Operator>(OperatorsLibrary.Abs,
                            new TreeNode<Operator>(OperatorsLibrary.Abs,
                                new TreeNode<Operator>(new Constant(7)))))),
                "Sum Abs Mul 3 Abs Abs Abs 0.5 Abs Abs Abs 7");

            TestSerialize(// 2*x + 7*(|y| + 5)            
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(2)),
                        new TreeNode<Operator>(new Variable("x"))),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(7)),
                        new TreeNode<Operator>(OperatorsLibrary.Sum,
                            new TreeNode<Operator>(OperatorsLibrary.Abs,
                                new TreeNode<Operator>(new Variable("y"))),
                            new TreeNode<Operator>(new Constant(5))))), "Sum Mul 2 x Mul 7 Sum Abs y 5");
        }

        private static void TestSerialize(TreeNode<Operator> formulaTreeRoot, string expected)
        {
            string serialized = FormulaTreeSerializer.Serialize(new FormulaTree(formulaTreeRoot));
            Assert.AreEqual(expected, serialized);
        }

        [Test]
        public void TestDeserialize()
        {
            TestDeserialize("2", new TreeNode<Operator>(new Constant(2)));

            TestDeserialize("x", new TreeNode<Operator>(new Variable("x")));

            TestDeserialize("Sum x y",
                new TreeNode<Operator>(
                    OperatorsLibrary.Sum,
                        new TreeNode<Operator>(new Variable("x")),
                        new TreeNode<Operator>(new Variable("y"))));
            
            TestDeserialize(// 2*x + 7*(|y| + 5)            
                "Sum Mul 2 x Mul 7 Sum Abs y 5",
                new TreeNode<Operator>(OperatorsLibrary.Sum,
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(2)),
                        new TreeNode<Operator>(new Variable("x"))),
                    new TreeNode<Operator>(OperatorsLibrary.Mul,
                        new TreeNode<Operator>(new Constant(7)),
                        new TreeNode<Operator>(OperatorsLibrary.Sum,
                            new TreeNode<Operator>(OperatorsLibrary.Abs,
                                new TreeNode<Operator>(new Variable("y"))),
                            new TreeNode<Operator>(new Constant(5))))));
        }

        private static void TestDeserialize(string serialized, TreeNode<Operator> expectedFormulaTreeRoot)
        {
            FormulaTree formulaTree = FormulaTreeSerializer.Deserialize(serialized);
            Assert.IsTrue(FormulaTree.Equal(new FormulaTree(expectedFormulaTreeRoot), formulaTree));
        }

        [Test]
        public void TestDeserializeWithSameVariableSeveralOccurences()
        {
            const string serializedFormula = "Sum x x";
            FormulaTree formulaTree = FormulaTreeSerializer.Deserialize(serializedFormula);
            IEnumerable<TreeNodeInfo<Operator>> traversedTree = Tree<Operator>.Traverse(formulaTree.Root, TraversalOrder.DepthFirstPostOrder);
            Variable[] xVariables = traversedTree.Where(ni => ni.Node.Value is Variable).Select(ni => (Variable)ni.Node.Value).ToArray();
            Assert.IsTrue(ReferenceEquals(xVariables[0], xVariables[1]));
        }
    }
}
