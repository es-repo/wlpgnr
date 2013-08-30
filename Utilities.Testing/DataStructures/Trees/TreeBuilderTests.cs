using System;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeBuilderTests
    {
        [Test]
        public void TestAppend()
        {
            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();
            Assert.IsFalse(treeBuilder.IsTreeReady);
            Assert.IsNull(treeBuilder.Root);
            Assert.IsNotNull(treeBuilder.NextAppendingNodeInfo);
            AssertTreeNodeInfosEqual(new TreeNodeInfo<int>(null, null, 0, 1), treeBuilder.NextAppendingNodeInfo);

            TreeNodeInfo<int> treeNodeInfo = treeBuilder.Append(0, 0);
            AssertTreeNodeInfosEqual(new TreeNodeInfo<int>(treeNodeInfo.Node, null, 0, 1), treeNodeInfo);
            Assert.IsTrue(treeBuilder.IsTreeReady);
            Assert.IsTrue(Tree<int>.Equal(treeBuilder.Root, new TreeNode<int>(0)));
            Assert.IsNull(treeBuilder.NextAppendingNodeInfo);

            try
            {
                treeBuilder.Append(0, 0);
                Assert.Fail("InvalidOperationException is expected.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static void AssertTreeNodeInfosEqual<T>(TreeNodeInfo<T> expectedTreeNodeInfo, TreeNodeInfo<T> treeNodeInfo)
        {
            Assert.AreEqual(expectedTreeNodeInfo.Node, treeNodeInfo.Node);
            Assert.AreEqual(expectedTreeNodeInfo.ParentNode, treeNodeInfo.ParentNode);
            Assert.AreEqual(expectedTreeNodeInfo.IndexAmongSiblings, treeNodeInfo.IndexAmongSiblings);
            Assert.AreEqual(expectedTreeNodeInfo.Depth, treeNodeInfo.Depth);
        }
    }
}
