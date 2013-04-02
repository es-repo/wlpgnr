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
            
            int[] expectedValues = Enumerable.Range(0, expectedNodesCount).ToArray();
            int[] values = traversedNodes.Select(ni => ni.Node.Value).ToArray();
            CollectionAssert.AreEqual(expectedValues, values);

            int[] expectedIndexesAmongSiblings = new[] { 0, 0, 0, 1, 2, 1, 0, 1, 2, 2, 0, 1, 2 };
            int[] indexesAmongSiblings = traversedNodes.Select(ni => ni.IndexAmongSiblings).ToArray();
            CollectionAssert.AreEqual(expectedIndexesAmongSiblings, indexesAmongSiblings);

            int[] expectedTraversedIndexes = new[] { 0, 1, 4, 5, 6, 2, 7, 8, 9, 3, 10, 11, 12 };
            int[] traversedIndexes = traversedNodes.Select(ni => ni.TraversedIndex).ToArray();
            CollectionAssert.AreEqual(expectedTraversedIndexes, traversedIndexes);

            int[] expectedDepthes = new[] { 0, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2 };
            int[] depthes = traversedNodes.Select(ni => ni.Depth).ToArray();
            CollectionAssert.AreEqual(expectedDepthes, depthes);
        }
    }
}
