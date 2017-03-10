using NUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeSerializerTests
    {
        [Test]
        public void TestSerialize()
        {
            TestSerialize(
                new TreeNode<int>(0),
                "0");

            TestSerialize(
                new TreeNode<int>(1,
                    new TreeNode<int>(2,
                        new TreeNode<int>(2,
                            new TreeNode<int>(1,
                                new TreeNode<int>(0)),
                            new TreeNode<int>(0)),
                        new TreeNode<int>(0))),
                "1 2 2 1 0 0 0");
        }

        private void TestSerialize<T>(TreeNode<T> treeRoot, string expectedSerializedTree)
        {
            string serializedTree = TreeSerializer.Serialize(treeRoot);
            Assert.AreEqual(expectedSerializedTree, serializedTree);
        }

        [Test]
        public void TestDeserialize()
        {
            TestDeserialize(
                "0",
                new TreeNode<int>(0));

            TestDeserialize(
                "1 2 2 1 0 0 0",
                new TreeNode<int>(1,
                    new TreeNode<int>(2,
                        new TreeNode<int>(2,
                            new TreeNode<int>(1,
                                new TreeNode<int>(0)),
                            new TreeNode<int>(0)),
                        new TreeNode<int>(0))));
        }

        private void TestDeserialize(string serializedTree, TreeNode<int> expectedTreeRoot)
        {
            TreeNode<int> treeRoot = TreeSerializer.Deserialize(serializedTree, int.Parse, v => v);
            Assert.IsTrue(Tree<int>.Equal(treeRoot, expectedTreeRoot));
        }
    }
}
