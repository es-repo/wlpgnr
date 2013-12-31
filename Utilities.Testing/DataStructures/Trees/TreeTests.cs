using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeTests
    {
        private TreeNode<int> _rootForTraverseDepthFirstPreOrder;
        private TreeNode<int> _rootForTraverseDepthFirstPostOrder;
        private TreeNode<int> _rootForTraverseBredthFirstPreOrder;
        
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //                      0
            //      1               5               9
            //  2   3   4       6   7   8       10  11  12           
            _rootForTraverseDepthFirstPreOrder = new TreeNode<int>(
                0, 
                new TreeNode<int>(1, 
                    new TreeNode<int>(2),
                    new TreeNode<int>(3),
                    new TreeNode<int>(4)),
                new TreeNode<int>(5, 
                    new TreeNode<int>(6),
                    new TreeNode<int>(7),
                    new TreeNode<int>(8)),
                new TreeNode<int>(9, 
                    new TreeNode<int>(10),
                    new TreeNode<int>(11),
                    new TreeNode<int>(12)));

            //                      12
            //      3               7               11
            //  0   1   2       4   5   6       8  9  10           
            _rootForTraverseDepthFirstPostOrder = new TreeNode<int>(
                12,
                new TreeNode<int>(3,
                    new TreeNode<int>(0),
                    new TreeNode<int>(1),
                    new TreeNode<int>(2)),
                new TreeNode<int>(7,
                    new TreeNode<int>(4),
                    new TreeNode<int>(5),
                    new TreeNode<int>(6)),
                new TreeNode<int>(11,
                    new TreeNode<int>(8),
                    new TreeNode<int>(9),
                    new TreeNode<int>(10)));
            
            //                      0
            //      1               2               3
            //  4   5   6       7   8   9       10  11  12           
            _rootForTraverseBredthFirstPreOrder = new TreeNode<int>(
                0,
                new TreeNode<int>(1,
                    new TreeNode<int>(4),
                    new TreeNode<int>(5),
                    new TreeNode<int>(6)),
                new TreeNode<int>(2,
                    new TreeNode<int>(7),
                    new TreeNode<int>(8),
                    new TreeNode<int>(9)),
                new TreeNode<int>(3,
                    new TreeNode<int>(10),
                    new TreeNode<int>(11),
                    new TreeNode<int>(12)));
        }

        [Test]
        public void TestTraverseDepthFirstPreOrder()
        {
            IEnumerable<TreeNodeInfo<int>> traversedNodes = Tree<int>.Traverse(_rootForTraverseDepthFirstPreOrder);
            
            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = { 0, 0, 0, 1, 2, 1, 0, 1, 2, 2, 0, 1, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedParentValues = { -1, 0, 1, 1, 1, 0, 5, 5, 5, 0, 9, 9, 9 };
            int[] parentValues = traversedNodes.Select(ni => ni.ParentNode == null ? -1 : ni.ParentNode.Value).ToArray();
            CollectionAssert.AreEqual(expectedParentValues, parentValues);
            
            int[] expectedDepthes = { 0, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestTraverseDepthFirstPostOrder()
        {
            IEnumerable<TreeNodeInfo<int>> traversedNodes = Tree<int>.Traverse(_rootForTraverseDepthFirstPostOrder, TraversalOrder.DepthFirstPostOrder);

            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = { 0, 1, 2, 0, 0, 1, 2, 1, 0, 1, 2, 2, 0 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedParentValues = { 3, 3, 3, 12, 7, 7, 7, 12, 11, 11, 11, 12, -1 };
            int[] parentValues = traversedNodes.Select(ni => ni.ParentNode == null ? -1 : ni.ParentNode.Value).ToArray();
            CollectionAssert.AreEqual(expectedParentValues, parentValues);

            int[] expectedDepthes = { 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 0 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestTraverseBreadthFirstPreOrder()
        {
            IEnumerable<TreeNodeInfo<int>> traversedNodes = Tree<int>.Traverse(_rootForTraverseBredthFirstPreOrder, TraversalOrder.BredthFirstPreOrder);

            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = { 0, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedParentValues = { -1, 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3 };
            int[] parentValues = traversedNodes.Select(ni => ni.ParentNode == null ? -1 : ni.ParentNode.Value).ToArray();
            CollectionAssert.AreEqual(expectedParentValues, parentValues);

            int[] expectedDepthes = { 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestBuild()
        {
            TestBuild(new int[] { }, null);

            TestBuild(new[] { 0 }, new TreeNode<int>(0));

            TestBuild(new[] { 2, 2, 0, 0, 2, 0, 0 },
                new TreeNode<int>(2,
                    new TreeNode<int>(2,
                        new TreeNode<int>(0),
                        new TreeNode<int>(0)),
                    new TreeNode<int>(2,
                        new TreeNode<int>(0),
                        new TreeNode<int>(0))));
        }

        private static void TestBuild(IEnumerable<int> values, TreeNode<int> expectedTreeRoot)
        {
            TreeNode<int> treeRoot = Tree<int>.Build(values, v => v);
            Assert.IsTrue(Tree<int>.Equal(treeRoot, expectedTreeRoot));
        }

        [Test]
        public void TestFold()
        {
            int result = Tree<int>.Fold<int>(_rootForTraverseDepthFirstPostOrder, (ni, c) => ni.Node.Value + c.Sum(i => i));
            Assert.AreEqual(78, result);
        }

        [Test]
        public void TestGetNodeHeight()
        {
            //                      0
            //      1               6               10
            //  2   3   5       7   8   9       11  12  13           
            //      4                                   14  15
            //                                              16
            TreeNode<int> root = new TreeNode<int>(
                0,
                new TreeNode<int>(1,
                    new TreeNode<int>(2),
                    new TreeNode<int>(3,
                        new TreeNode<int>(4)),
                    new TreeNode<int>(5)),
                new TreeNode<int>(6,
                    new TreeNode<int>(7),
                    new TreeNode<int>(8),
                    new TreeNode<int>(9)),
                new TreeNode<int>(10,
                    new TreeNode<int>(11),
                    new TreeNode<int>(12),
                    new TreeNode<int>(13,
                        new TreeNode<int>(14),
                        new TreeNode<int>(15, 
                            new TreeNode<int>(16)))));

            IEnumerable<TreeNodeInfo<int>> traversedNodes = Tree<int>.Traverse(root);
            
            int[] expectedHeights = {5, 3, 1, 2, 1, 1, 2, 1, 1, 1, 4, 1, 1, 3, 1, 2, 1};
            int[] heights = traversedNodes.Select(ni => Tree<int>.GetNodeHeight(ni.Node)).ToArray();
            Assert.AreEqual(expectedHeights, heights);
        }
    }
}
