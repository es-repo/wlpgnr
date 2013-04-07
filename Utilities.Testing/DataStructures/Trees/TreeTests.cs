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
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = Tree.TraverseDepthFirstPreOrder(_rootForTraverseDepthFirstPreOrder);
            
            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 0, 0, 1, 2, 1, 0, 1, 2, 2, 0, 1, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);
            
            int[] expectedDepthes = new[] { 0, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestTraverseDepthFirstPostOrder()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = Tree.TraverseDepthFirstPostOrder(_rootForTraverseDepthFirstPostOrder);

            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 1, 2, 0, 0, 1, 2, 1, 0, 1, 2, 2, 0 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedDepthes = new[] { 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 0 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestTraverseBreadthFirstPreOrder()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = Tree.TraverseBredthFirstPreOrder(_rootForTraverseBredthFirstPreOrder);

            const int expectedNodesCount = 13;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedDepthes = new[] { 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestFold()
        {
            int result = Tree.Fold<int, int>(_rootForTraverseDepthFirstPostOrder, (ni, c) => ni.Node.Value + c.Sum(i => i));
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

            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = Tree.TraverseDepthFirstPreOrder(root);
            
            int[] expectedHeights = new [] {5, 3, 1, 2, 1, 1, 2, 1, 1, 1, 4, 1, 1, 3, 1, 2, 1};
            int[] heights = traversedNodes.Select(ni => Tree.GetNodeHeight(ni.Node)).ToArray();
            Assert.AreEqual(expectedHeights, heights);
        }
    }
}
