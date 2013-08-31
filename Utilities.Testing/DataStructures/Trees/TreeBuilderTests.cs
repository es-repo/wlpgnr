using System;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees
{
    [TestFixture]
    public class TreeBuilderTests
    {
        [Test]
        public void TestAppend0()
        {
            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();

            AssertAppend(treeBuilder, null, null,
                new TreeNodeInfo<int>(null, null, 0, 1), false);

            TreeNodeInfo<int> treeNodeInfo0 = treeBuilder.Append(0, 0);
            AssertAppend(treeBuilder, treeNodeInfo0,
                new TreeNodeInfo<int>(treeNodeInfo0.Node, null, 0, 1),
                null, true, new TreeNode<int>(0));

            AssertAppendWithExceptionExpected(treeBuilder, 0, 0);
        }

        [Test]
        public void TestAppend110()
        {
            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();

            TreeNodeInfo<int> treeNodeInfo0 = treeBuilder.Append(1, 1);
            AssertAppend(treeBuilder, treeNodeInfo0,
                new TreeNodeInfo<int>(treeNodeInfo0.Node, null, 0, 1),
                new TreeNodeInfo<int>(null, treeNodeInfo0.Node, 0, 2), false);

            TreeNodeInfo<int> treeNodeInfo1= treeBuilder.Append(1, 1);
            AssertAppend(treeBuilder, treeNodeInfo1,
                new TreeNodeInfo<int>(treeNodeInfo1.Node, treeNodeInfo0.Node, 0, 2),
                new TreeNodeInfo<int>(null, treeNodeInfo1.Node, 0, 3), false);

            TreeNodeInfo<int> treeNodeInfo2 = treeBuilder.Append(0, 0);
            AssertAppend(treeBuilder, treeNodeInfo2,
                new TreeNodeInfo<int>(treeNodeInfo2.Node, treeNodeInfo1.Node, 0, 3),
                null, true,
                new TreeNode<int>(1,
                    new TreeNode<int>(1,
                        new TreeNode<int>(0))));
        }

        [Test]
        public void TestAppend122000()
        {
            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();

            TreeNodeInfo<int> treeNodeInfo0 = treeBuilder.Append(1, 1);
            AssertAppend(treeBuilder, treeNodeInfo0, 
                new TreeNodeInfo<int>(treeNodeInfo0.Node, null, 0, 1),
                new TreeNodeInfo<int>(null, treeNodeInfo0.Node, 0, 2), false);

            TreeNodeInfo<int> treeNodeInfo1 = treeBuilder.Append(2, 2);
            AssertAppend(treeBuilder, treeNodeInfo1,
                new TreeNodeInfo<int>(treeNodeInfo1.Node, treeNodeInfo0.Node, 0, 2),
                new TreeNodeInfo<int>(null, treeNodeInfo1.Node, 0, 3), false);

            TreeNodeInfo<int> treeNodeInfo2 = treeBuilder.Append(2, 2);
            AssertAppend(treeBuilder, treeNodeInfo2,
                new TreeNodeInfo<int>(treeNodeInfo2.Node, treeNodeInfo1.Node, 0, 3),
                new TreeNodeInfo<int>(null, treeNodeInfo2.Node, 0, 4), false);

            TreeNodeInfo<int> treeNodeInfo3 = treeBuilder.Append(0, 0);
            AssertAppend(treeBuilder, treeNodeInfo3,
                new TreeNodeInfo<int>(treeNodeInfo3.Node, treeNodeInfo2.Node, 0, 4),
                new TreeNodeInfo<int>(null, treeNodeInfo2.Node, 1, 4), false);

            TreeNodeInfo<int> treeNodeInfo4 = treeBuilder.Append(0, 0);
            AssertAppend(treeBuilder, treeNodeInfo4,
                new TreeNodeInfo<int>(treeNodeInfo4.Node, treeNodeInfo2.Node, 1, 4),
                new TreeNodeInfo<int>(null, treeNodeInfo1.Node, 1, 3), false);

            TreeNodeInfo<int> treeNodeInfo5 = treeBuilder.Append(0, 0);
            AssertAppend(treeBuilder, treeNodeInfo5,
                new TreeNodeInfo<int>(treeNodeInfo5.Node, treeNodeInfo1.Node, 1, 3),
                null, true, new TreeNode<int>(1,
                    new TreeNode<int>(2,
                        new TreeNode<int>(2,
                            new TreeNode<int>(0),
                            new TreeNode<int>(0)),
                        new TreeNode<int>(0))));

            AssertAppendWithExceptionExpected(treeBuilder, 0, 0);
        }

        [Test]
        public void TestAppendSequence()
        {            
            TestAppendSequence(new int[] {}, null);

            TestAppendSequence(new [] { 0 }, 
                new TreeNode<int>(0));

            TestAppendSequence(new[] { 1, 1, 1, 0 },
                new TreeNode<int>(1,
                    new TreeNode<int>(1,
                        new TreeNode<int>(1,
                            new TreeNode<int>(0)))));

            TestAppendSequence(new[] { 1, 2, 2, 1, 0, 0, 0 },
                new TreeNode<int>(1,
                    new TreeNode<int>(2,
                        new TreeNode<int>(2,
                            new TreeNode<int>(1,
                                new TreeNode<int>(0)),
                            new TreeNode<int>(0)),
                        new TreeNode<int>(0))));

            TestAppendSequence(new[] { 1, 2, 1, 2, 0, 0, 1, 2, 0, 0 },
                new TreeNode<int>(1,
                    new TreeNode<int>(2,
                        new TreeNode<int>(1,
                            new TreeNode<int>(2,
                                new TreeNode<int>(0),
                                new TreeNode<int>(0))),
                        new TreeNode<int>(1,
                            new TreeNode<int>(2,
                                new TreeNode<int>(0),
                                new TreeNode<int>(0))))));
        }

        private static void TestAppendSequence(IEnumerable<int> nodeValues, TreeNode<int> expectedTreeRoot)
        {
            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();
            TreeNode<int> treeRoot = treeBuilder.Append(nodeValues, v => v);
            Assert.IsTrue(Tree<int>.Equal(treeRoot, expectedTreeRoot));
        }

        private static void AssertAppend<T>(TreeBuilder<T> treeBuilder, TreeNodeInfo<T> treeNodeInfo,
            TreeNodeInfo<T> expectedTreeNodeInfo, TreeNodeInfo<T> expectedNextAppendingTreeNodeInfo, 
            bool expectedIsTreeReady, TreeNode<T> expectedTreeRoot = null)
        {
            AssertTreeNodeInfosEqual(expectedTreeNodeInfo , treeNodeInfo);
            AssertTreeNodeInfosEqual(expectedNextAppendingTreeNodeInfo, treeBuilder.NextAppendingNodeInfo);

            Assert.AreEqual(expectedIsTreeReady, treeBuilder.IsTreeReady);
            
            if (treeBuilder.IsTreeReady)
            {
                Assert.IsTrue(Tree<T>.Equal(treeBuilder.Root, expectedTreeRoot));
            }
        }

        private static void AssertAppendWithExceptionExpected<T>(TreeBuilder<T> treeBuilder, T nodeValue, int nodeChildrenCount)
        {
            try
            {
                treeBuilder.Append(nodeValue, nodeChildrenCount);
                Assert.Fail("InvalidOperationException is expected.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static void AssertTreeNodeInfosEqual<T>(TreeNodeInfo<T> expectedTreeNodeInfo, TreeNodeInfo<T> treeNodeInfo)
        {
            if (expectedTreeNodeInfo == null && treeNodeInfo == null)
                return;

            if (expectedTreeNodeInfo == null)
            {
                Assert.Fail("Expected tree node info is null but actual is not.");
                return;
            }

            Assert.AreEqual(expectedTreeNodeInfo.Node, treeNodeInfo.Node);
            Assert.AreEqual(expectedTreeNodeInfo.ParentNode, treeNodeInfo.ParentNode);
            Assert.AreEqual(expectedTreeNodeInfo.IndexAmongSiblings, treeNodeInfo.IndexAmongSiblings);
            Assert.AreEqual(expectedTreeNodeInfo.Depth, treeNodeInfo.Depth);
        }
    }
}
