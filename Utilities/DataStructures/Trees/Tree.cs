using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public class Tree
    {        
        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterDepthFirstPreOrder<T>(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, 0, 0);
            Stack<TraversedTreeNodeInfo<T>> stack = new Stack<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (stack.Any())
            {
                TraversedTreeNodeInfo<T> nodeInfo = stack.Pop();
                if (predicate == null || predicate(nodeInfo))
                    yield return nodeInfo;

                int indexAmongSiblings = nodeInfo.Node.Children.Count - 1;
                foreach (TreeNode<T> n in nodeInfo.Node.Children.Reverse())
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, indexAmongSiblings, nodeInfo.Depth + 1);
                    stack.Push(childNodeInfo);
                    indexAmongSiblings--;
                }
            }
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPreOrder<T>(TreeNode<T> root)
        {
            return FilterDepthFirstPreOrder(root, null);
        }

        class VisitableTraversedTreeNodeInfo<T>
        {
            public bool IsVisited { get; set; }
            public TraversedTreeNodeInfo<T> NodeInfo { get; set; }
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterDepthFirstPostOrder<T>(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, 0, 0);
            Stack<VisitableTraversedTreeNodeInfo<T>> stack = new Stack<VisitableTraversedTreeNodeInfo<T>>(new[]
                {
                    new VisitableTraversedTreeNodeInfo<T> { NodeInfo = rootNodeInfo }
                });

            while (stack.Any())
            {
                VisitableTraversedTreeNodeInfo<T> vni = stack.Peek();
                if (vni.IsVisited)
                {
                    stack.Pop();
                    if (predicate == null || predicate(vni.NodeInfo))
                        yield return vni.NodeInfo;
                }
                else
                {
                    vni.IsVisited = true;
                    int indexAmongSiblings = vni.NodeInfo.Node.Children.Count - 1;
                    foreach (TreeNode<T> n in vni.NodeInfo.Node.Children.Reverse())
                    {
                        TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, indexAmongSiblings, vni.NodeInfo.Depth + 1);
                        stack.Push(new VisitableTraversedTreeNodeInfo<T> { NodeInfo = childNodeInfo });
                        indexAmongSiblings--;
                    }
                }
            }
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseDepthFirstPostOrder<T>(TreeNode<T> root)
        {
            return FilterDepthFirstPostOrder(root, null);
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterBredthFirstPreOrder<T>(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, 0, 0);
            Queue<TraversedTreeNodeInfo<T>> queue = new Queue<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (queue.Any())
            {
                TraversedTreeNodeInfo<T> nodeInfo = queue.Dequeue();
                if (predicate == null || predicate(nodeInfo))
                    yield return nodeInfo;
                
                int indexAmongSiblings = 0;
                foreach (TreeNode<T> n in nodeInfo.Node.Children)
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, indexAmongSiblings, nodeInfo.Depth + 1);
                    queue.Enqueue(childNodeInfo);
                    indexAmongSiblings++;
                }
            }
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseBredthFirstPreOrder<T>(TreeNode<T> root)
        {
            return FilterBredthFirstPreOrder(root, null);
        }

        public static int GetNodeHeight<T>(TreeNode<T> node)
        {
            return TraverseDepthFirstPreOrder(node).Max(ni => ni.Depth + 1);
        }

        public static R Fold<T, R>(TreeNode<T> node, Func<TraversedTreeNodeInfo<T>, IEnumerable<R>, R> func)
        {
            Queue<R> foldedChildrenQueue = new Queue<R>();
            IEnumerable<TraversedTreeNodeInfo<T>> traversedDepthFirstPostOrderNodes = TraverseDepthFirstPostOrder(node);
            foreach (TraversedTreeNodeInfo<T> nodeInfo in traversedDepthFirstPostOrderNodes)
            {
                List<R> foldedChildren = new List<R>();
                for (int i = 0; i < nodeInfo.Node.Children.Count; i++)
                {
                    foldedChildren.Add(foldedChildrenQueue.Dequeue());
                }

                R r = func(nodeInfo, foldedChildren);
                foldedChildrenQueue.Enqueue(r);
            }

            return foldedChildrenQueue.Dequeue();
        }
    }
}
