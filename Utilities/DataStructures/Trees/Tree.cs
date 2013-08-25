using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public enum TraversalOrder
    {
        DepthFirstPreOrder,
        DepthFirstPostOrder,
        BredthFirstPreOrder
    }

    public class Tree<T>
    {   
        #region Fields

        private TraversedTreeNodeInfo<T>[] _traversedDepthFirstPreOrder;
        private TraversedTreeNodeInfo<T>[] _traversedDepthFirstPostOrder;
        private TraversedTreeNodeInfo<T>[] _traversedBredthFirstPreOrder;
        
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

        public TraversedTreeNodeInfo<T>[] Traverse(TraversalOrder order)
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

        private TraversedTreeNodeInfo<T>[] TraverseDepthFirstPreOrder()
        {
            return _traversedDepthFirstPreOrder ??
                (_traversedDepthFirstPreOrder = TraverseDepthFirstPreOrder(Root).ToArray());
        }

        private TraversedTreeNodeInfo<T>[] TraverseDepthFirstPostOrder()
        {
            return _traversedDepthFirstPostOrder ??
                (_traversedDepthFirstPostOrder = TraverseDepthFirstPostOrder(Root).ToArray());
        }

        private TraversedTreeNodeInfo<T>[] TraverseBredthFirstPreOrder()
        {
            return _traversedBredthFirstPreOrder ??
                (_traversedBredthFirstPreOrder = TraverseBredthFirstPreOrder(Root).ToArray());
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> Traverse(TreeNode<T> root, TraversalOrder order)
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

        private static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPreOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Stack<TraversedTreeNodeInfo<T>> stack = new Stack<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (stack.Count > 0)
            {
                TraversedTreeNodeInfo<T> nodeInfo = stack.Pop();
                yield return nodeInfo;

                int indexAmongSiblings = nodeInfo.Node.Children.Count - 1;
                foreach (TreeNode<T> n in nodeInfo.Node.Children.Reverse())
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, nodeInfo.Node, indexAmongSiblings, nodeInfo.Depth + 1);
                    stack.Push(childNodeInfo);
                    indexAmongSiblings--;
                }
            }
        }

        class VisitableTraversedTreeNodeInfo
        {
            public bool IsVisited { get; set; }
            public TraversedTreeNodeInfo<T> NodeInfo { get; set; }
        }

        private static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPostOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Stack<VisitableTraversedTreeNodeInfo> stack = new Stack<VisitableTraversedTreeNodeInfo>(new[]
                {
                    new VisitableTraversedTreeNodeInfo { NodeInfo = rootNodeInfo }
                });

            while (stack.Count > 0)
            {
                VisitableTraversedTreeNodeInfo vni = stack.Peek();
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
                        TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, vni.NodeInfo.Node, indexAmongSiblings, vni.NodeInfo.Depth + 1);
                        stack.Push(new VisitableTraversedTreeNodeInfo { NodeInfo = childNodeInfo });
                        indexAmongSiblings--;
                    }
                }
            }
        }

        private static IEnumerable<TraversedTreeNodeInfo<T>> TraverseBredthFirstPreOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Queue<TraversedTreeNodeInfo<T>> queue = new Queue<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (queue.Count > 0)
            {
                TraversedTreeNodeInfo<T> nodeInfo = queue.Dequeue();
                yield return nodeInfo;

                int indexAmongSiblings = 0;
                foreach (TreeNode<T> n in nodeInfo.Node.Children)
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, nodeInfo.Node, indexAmongSiblings, nodeInfo.Depth + 1);
                    queue.Enqueue(childNodeInfo);
                    indexAmongSiblings++;
                }
            }
        }

        #endregion

        public static TreeNode<T> Build(IEnumerable<T> traversedTreeValues, Func<T, int> getChildrenCount)
        {
            return Build(traversedTreeValues, getChildrenCount, TraversalOrder.DepthFirstPreOrder);
        }

        public static TreeNode<T> Build(IEnumerable<T> traversedTreeValues, Func<T, int> getChildrenCount, TraversalOrder order)
        {
            switch (order)
            {
                case TraversalOrder.DepthFirstPreOrder:
                    return BuildFromTraversedDepthFirstPreOrder(traversedTreeValues, getChildrenCount);
            }

            throw new NotImplementedException();
        }

        private static TreeNode<T> BuildFromTraversedDepthFirstPreOrder(IEnumerable<T> traversedTreeValues, Func<T, int> getChildrenCount)
        {
            TreeNode<T> treeRoot = null;

            Stack<Tuple<TreeNode<T>, int>> stack = new Stack<Tuple<TreeNode<T>, int>>();
            foreach (T value in traversedTreeValues)
            {
                TreeNode<T> parentNode = null;
                if (stack.Count > 0)
                {
                    Tuple<TreeNode<T>, int> parentNodeAndChildrenCount = stack.Pop();
                    parentNode = parentNodeAndChildrenCount.Item1; 
                    int parentChildrenCount = parentNodeAndChildrenCount.Item2;
                    if (parentChildrenCount > 1)
                    {
                        stack.Push(new Tuple<TreeNode<T>, int>(parentNode, parentChildrenCount - 1));
                    }
                }

                TreeNode<T> node = new TreeNode<T>(value);
                if (treeRoot == null)
                {
                    treeRoot = node;
                }

                if (parentNode != null)
                {
                    parentNode.AddChild(node);
                }
                
                int nodeChildrenCount = getChildrenCount(value);
                if (nodeChildrenCount > 0)
                {
                    stack.Push(new Tuple<TreeNode<T>, int>(node, nodeChildrenCount));
                }
            }

            return treeRoot;
        }

        public static int GetNodeHeight(TreeNode<T> node)
        {
            return TraverseDepthFirstPreOrder(node).Max(ni => ni.Depth + 1);
        }

        public R Fold<R>(Func<TraversedTreeNodeInfo<T>, R[], R> func)
        {
            return FoldCore(TraverseDepthFirstPostOrder(), func);
        }

        public static R Fold<R>(TreeNode<T> node, Func<TraversedTreeNodeInfo<T>, R[], R> func)
        {
            IEnumerable<TraversedTreeNodeInfo<T>> traversedDepthFirstPostOrderNodes = TraverseDepthFirstPostOrder(node);
            return FoldCore(traversedDepthFirstPostOrderNodes, func);
        }

        private static R FoldCore<R>(IEnumerable<TraversedTreeNodeInfo<T>> traversedDepthFirstPostOrder, Func<TraversedTreeNodeInfo<T>, R[], R> func)
        {
            Stack<R> foldedChildrenQueue = new Stack<R>();
            foreach (TraversedTreeNodeInfo<T> nodeInfo in traversedDepthFirstPostOrder)
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

            IEnumerable<TraversedTreeNodeInfo<T>> traversedNodesA = Traverse(rootA, TraversalOrder.BredthFirstPreOrder);
            IEnumerable<TraversedTreeNodeInfo<T>> traversedNodesB = Traverse(rootB, TraversalOrder.BredthFirstPreOrder);
            IEnumerator<TraversedTreeNodeInfo<T>> nodesInfoBEnumerator = traversedNodesB.GetEnumerator();
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
