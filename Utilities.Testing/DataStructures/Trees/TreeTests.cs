using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeTests
    {
        private Tree<int> _treeForTraverseInDepth;
        private Tree<int> _treeForTraverseInBreadth;
        
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //                      0
            //      1               5               9
            //  2   3   4       6   7   8       10  11  12           
            TreeNode<int> rootForTraverseInDepth = new TreeNode<int>(
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
            _treeForTraverseInDepth = new Tree<int>(rootForTraverseInDepth);

            //                      0
            //      1               2               3
            //  4   5   6       7   8   9       10  11  12           
            TreeNode<int> rootForTraverseInBreadth = new TreeNode<int>(
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
            _treeForTraverseInBreadth = new Tree<int>(rootForTraverseInBreadth);
        }

        [Test]
        public void TestTraverseInDepth()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = _treeForTraverseInDepth.TraverseInDepth();
            
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
        public void TestFilterInDepth()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = _treeForTraverseInDepth.FilterInDepth(ni => ni.Node.Value % 2 == 0);

            const int expectedNodesCount = 7;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = new[] { 0, 2, 4, 6, 8, 10, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 0, 2, 0, 2, 0, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedDepthes = new[] { 0, 2, 2, 2, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }

        [Test]
        public void TestTraverseInBreadth()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = _treeForTraverseInBreadth.TraverseInBreadth();

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
        public void TestFilterInBreadth()
        {
            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = _treeForTraverseInBreadth.FilterInBreadth(ni => ni.Node.Value % 2 == 0);

            const int expectedNodesCount = 7;
            Assert.AreEqual(expectedNodesCount, traversedNodes.Count());

            int[] expectedValues = new[] { 0, 2, 4, 6, 8, 10, 12 };
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 1, 0, 2, 1, 0, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedDepthes = new[] { 0, 1, 2, 2, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
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

            IEnumerable<TraversedTreeNodeInfo<int>> traversedNodes = Tree<int>.TraverseInDepth(root);
            
            int[] expectedHeights = new [] {5, 3, 1, 2, 1, 1, 2, 1, 1, 1, 4, 1, 1, 3, 1, 2, 1};
            int[] heights = traversedNodes.Select(ni => Tree<int>.GetNodeHeight(ni.Node)).ToArray();
            Assert.AreEqual(expectedHeights, heights);
        }
    }
}
