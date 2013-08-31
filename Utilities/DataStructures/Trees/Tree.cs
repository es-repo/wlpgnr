using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public class Tree<T>
    {   
        #region Fields

        private TreeNodeInfo<T>[] _traversedDepthFirstPreOrder;
        private TreeNodeInfo<T>[] _traversedDepthFirstPostOrder;
        private TreeNodeInfo<T>[] _traversedBredthFirstPreOrder;
        
        #endregion

        #region Properties

        public TreeNode<T> Root { get; private set; }

        #endregion

        #region Constructors

        public Tree(TreeNode<T> root)
        {
            Root = root;
        }

        #endregion

        #region Traversal

        public TreeNodeInfo<T>[] Traverse(TraversalOrder order)
        {
            switch (order)
            {
                case TraversalOrder.DepthFirstPreOrder:
                    return TraverseDepthFirstPreOrder();

                case TraversalOrder.DepthFirstPostOrder:
                    return TraverseDepthFirstPostOrder();

                case TraversalOrder.BredthFirstPreOrder:
                    return TraverseBredthFirstPreOrder();
            }

            throw new NotImplementedException();
        }

        private TreeNodeInfo<T>[] TraverseDepthFirstPreOrder()
        {
            return _traversedDepthFirstPreOrder ??
                (_traversedDepthFirstPreOrder = TraverseDepthFirstPreOrder(Root).ToArray());
        }

        private TreeNodeInfo<T>[] TraverseDepthFirstPostOrder()
        {
            return _traversedDepthFirstPostOrder ??
                (_traversedDepthFirstPostOrder = TraverseDepthFirstPostOrder(Root).ToArray());
        }

        private TreeNodeInfo<T>[] TraverseBredthFirstPreOrder()
        {
            return _traversedBredthFirstPreOrder ??
                (_traversedBredthFirstPreOrder = TraverseBredthFirstPreOrder(Root).ToArray());
        }

        public static IEnumerable<TreeNodeInfo<T>> Traverse(TreeNode<T> root, TraversalOrder order)
        {
            switch (order)
            {
                case TraversalOrder.DepthFirstPreOrder:
                    return TraverseDepthFirstPreOrder(root);

                case TraversalOrder.DepthFirstPostOrder:
                    return TraverseDepthFirstPostOrder(root);

                case TraversalOrder.BredthFirstPreOrder:
                    return TraverseBredthFirstPreOrder(root);
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<TreeNodeInfo<T>> TraverseDepthFirstPreOrder(TreeNode<T> root)
        {
            TreeNodeInfo<T> rootNodeInfo = new TreeNodeInfo<T>(root, null, 0, 0);
            Stack<TreeNodeInfo<T>> stack = new Stack<TreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (stack.Count > 0)
            {
                TreeNodeInfo<T> nodeInfo = stack.Pop();
                yield return nodeInfo;

                int indexAmongSiblings = nodeInfo.Node.Children.Count - 1;
                foreach (TreeNode<T> n in nodeInfo.Node.Children.Reverse())
                {
                    TreeNodeInfo<T> childNodeInfo = new TreeNodeInfo<T>(n, nodeInfo.Node, indexAmongSiblings, nodeInfo.Depth + 1);
                    stack.Push(childNodeInfo);
                    indexAmongSiblings--;
                }
            }
        }

        class TraversedTreeNodeInfo
        {
            public bool IsVisited { get; set; }
            public TreeNodeInfo<T> NodeInfo { get; set; }
        }

        private static IEnumerable<TreeNodeInfo<T>> TraverseDepthFirstPostOrder(TreeNode<T> root)
        {
            TreeNodeInfo<T> rootNodeInfo = new TreeNodeInfo<T>(root, null, 0, 0);
            Stack<TraversedTreeNodeInfo> stack = new Stack<TraversedTreeNodeInfo>(new[]
                {
                    new TraversedTreeNodeInfo { NodeInfo = rootNodeInfo }
                });

            while (stack.Count > 0)
            {
                TraversedTreeNodeInfo vni = stack.Peek();
                if (vni.IsVisited)
                {
                    stack.Pop();
                    yield return vni.NodeInfo;
                }
                else
                {
                    vni.IsVisited = true;
                    int indexAmongSiblings = vni.NodeInfo.Node.Children.Count - 1;
                    foreach (TreeNode<T> n in vni.NodeInfo.Node.Children.Reverse())
                    {
                        TreeNodeInfo<T> childNodeInfo = new TreeNodeInfo<T>(n, vni.NodeInfo.Node, indexAmongSiblings, vni.NodeInfo.Depth + 1);
                        stack.Push(new TraversedTreeNodeInfo { NodeInfo = childNodeInfo });
                        indexAmongSiblings--;
                    }
                }
            }
        }

        private static IEnumerable<TreeNodeInfo<T>> TraverseBredthFirstPreOrder(TreeNode<T> root)
        {
            TreeNodeInfo<T> rootNodeInfo = new TreeNodeInfo<T>(root, null, 0, 0);
            Queue<TreeNodeInfo<T>> queue = new Queue<TreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (queue.Count > 0)
            {
                TreeNodeInfo<T> nodeInfo = queue.Dequeue();
                yield return nodeInfo;

                int indexAmongSiblings = 0;
                foreach (TreeNode<T> n in nodeInfo.Node.Children)
                {
                    TreeNodeInfo<T> childNodeInfo = new TreeNodeInfo<T>(n, nodeInfo.Node, indexAmongSiblings, nodeInfo.Depth + 1);
                    queue.Enqueue(childNodeInfo);
                    indexAmongSiblings++;
                }
            }
        }

        #endregion

        public static TreeNode<T> Build(IEnumerable<T> nodeValues, Func<T, int> getNodeChildrenCount, TraversalOrder nodeValuesOrder = TraversalOrder.DepthFirstPreOrder)
        {
            return new TreeBuilder<T>(nodeValuesOrder).Append(nodeValues, getNodeChildrenCount);
        }

        public static int GetNodeHeight(TreeNode<T> node)
        {
            return TraverseDepthFirstPreOrder(node).Max(ni => ni.Depth + 1);
        }

        public R Fold<R>(Func<TreeNodeInfo<T>, R[], R> func)
        {
            return FoldCore(TraverseDepthFirstPostOrder(), func);
        }

        public static R Fold<R>(TreeNode<T> node, Func<TreeNodeInfo<T>, R[], R> func)
        {
            IEnumerable<TreeNodeInfo<T>> traversedDepthFirstPostOrderNodes = TraverseDepthFirstPostOrder(node);
            return FoldCore(traversedDepthFirstPostOrderNodes, func);
        }

        private static R FoldCore<R>(IEnumerable<TreeNodeInfo<T>> traversedDepthFirstPostOrder, Func<TreeNodeInfo<T>, R[], R> func)
        {
            Stack<R> foldedChildrenQueue = new Stack<R>();
            foreach (TreeNodeInfo<T> nodeInfo in traversedDepthFirstPostOrder)
            {
                int childrenCount = nodeInfo.Node.Children.Count;
                R[] foldedChildren = new R[childrenCount];
                for (int i = childrenCount - 1; i >= 0; i--)
                {
                    foldedChildren[i] = foldedChildrenQueue.Pop();
                }

                R r = func(nodeInfo, foldedChildren);
                foldedChildrenQueue.Push(r);
            }

            return foldedChildrenQueue.Pop();
        }

        public static bool Equal(TreeNode<T> rootA, TreeNode<T> rootB, Func<T, T, bool> nodeValuesEqual)
        {
            if (rootA == null && rootB == null)
                return true;

            IEnumerable<TreeNodeInfo<T>> traversedNodesA = Traverse(rootA, TraversalOrder.BredthFirstPreOrder);
            IEnumerable<TreeNodeInfo<T>> traversedNodesB = Traverse(rootB, TraversalOrder.BredthFirstPreOrder);
            IEnumerator<TreeNodeInfo<T>> nodesInfoBEnumerator = traversedNodesB.GetEnumerator();
            foreach (var nodeInfoA in traversedNodesA)
            {
                nodesInfoBEnumerator.MoveNext();
                var nodeInfoB = nodesInfoBEnumerator.Current;
                if (nodeInfoB == null || !nodeValuesEqual(nodeInfoA.Node.Value, nodeInfoA.Node.Value))
                    return false;
            }

            return true;
        }

        public static bool Equal(TreeNode<T> rootA, TreeNode<T> rootB)
        {
            return Equal(rootA, rootB, (va, vb) => va.Equals(vb));
        }
    }
}
