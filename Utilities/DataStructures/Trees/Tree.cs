using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public class Tree<T>
    {   
        #region Fields

        private TraversedTreeNodeInfo<T>[] _traversedDepthFirstPreOrder;
        private TraversedTreeNodeInfo<T>[] _traversedDepthFirstPostOrder;
        private TraversedTreeNodeInfo<T>[] _traversedBredthFirstPreOrder;
        
        #endregion

        #region Properties

        public TreeNode<T> Root { get; private set; }

        public TraversedTreeNodeInfo<T>[] TraversedDepthFirstPreOrder
        {
            get
            {
                return _traversedDepthFirstPreOrder ?? 
                    (_traversedDepthFirstPreOrder = TraverseDepthFirstPreOrder(Root).ToArray());
            }
        }

        public TraversedTreeNodeInfo<T>[] TraversedDepthFirstPostOrder
        {
            get
            {
                return _traversedDepthFirstPostOrder ??
                    (_traversedDepthFirstPostOrder = TraverseDepthFirstPostOrder(Root).ToArray());
            }
        }

        public TraversedTreeNodeInfo<T>[] TraversedBredthFirstPreOrder
        {
            get
            {
                return _traversedBredthFirstPreOrder ??
                    (_traversedBredthFirstPreOrder = TraverseBredthFirstPreOrder(Root).ToArray());
            }
        }

        #endregion

        #region Constructors

        public Tree(TreeNode<T> root)
        {
            Root = root;
        }

        #endregion

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPreOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Stack<TraversedTreeNodeInfo<T>> stack = new Stack<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (stack.Any())
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

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPostOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Stack<VisitableTraversedTreeNodeInfo> stack = new Stack<VisitableTraversedTreeNodeInfo>(new[]
                {
                    new VisitableTraversedTreeNodeInfo { NodeInfo = rootNodeInfo }
                });

            while (stack.Any())
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

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseBredthFirstPreOrder(TreeNode<T> root)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, null, 0, 0);
            Queue<TraversedTreeNodeInfo<T>> queue = new Queue<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (queue.Any())
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

        public static int GetNodeHeight(TreeNode<T> node)
        {
            return TraverseDepthFirstPreOrder(node).Max(ni => ni.Depth + 1);
        }

        public R Fold<R>(Func<TraversedTreeNodeInfo<T>, R[], R> func)
        {
            return FoldCore(TraversedDepthFirstPostOrder, func);
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
    }
}
