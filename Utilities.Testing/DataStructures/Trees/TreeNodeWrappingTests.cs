using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeNodeWrappingTests
    {
        [Test]
        public void TestWrap()
        {
            TreeNode<int> treeNode = new TreeNode<int>(1, new TreeNode<int>(2, new TreeNode<int>(3)), new TreeNode<int>(4));
            TreeNode<int> wrappedNode = TreeNodeWrapping.Wrap(treeNode, 0);
            Assert.AreEqual(0, wrappedNode.Value);
            Assert.AreEqual(1, wrappedNode.Children.Count);
            Assert.AreEqual(1, wrappedNode.Children[0].Value);
            Assert.AreEqual(2, wrappedNode.Children[0].Children.Count);
        }

        [Test]
        public void TestWrapChildren()
        {
            TreeNode<int> treeNode = new TreeNode<int>(1, new TreeNode<int>(2, new TreeNode<int>(3)), new TreeNode<int>(4), new TreeNode<int>(5));
            TreeNode<int> wrappedChildrenNode = TreeNodeWrapping.WrapChildren(treeNode, new Dictionary<int, int> { { 0, -1 }, { 2, -2 } });
            Assert.AreEqual(treeNode.Value, wrappedChildrenNode.Value);
            Assert.AreEqual(treeNode.Children.Count, wrappedChildrenNode.Children.Count);
            Assert.AreEqual(-1, wrappedChildrenNode.Children[0].Value);
            Assert.AreEqual(treeNode.Children[0].Value, wrappedChildrenNode.Children[0].Children[0].Value);
            Assert.AreEqual(treeNode.Children[1].Value, wrappedChildrenNode.Children[1].Value);
            Assert.AreEqual(-2, wrappedChildrenNode.Children[2].Value);
        }
    }
}
