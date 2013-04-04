using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public class Tree
    {        
        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterInDepth<T>(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
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

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseInDepth<T>(TreeNode<T> root)
        {
            return FilterInDepth(root, null);
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterInBreadth<T>(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
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

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseInBreadth<T>(TreeNode<T> root)
        {
            return FilterInBreadth(root, null);
        }

        public static int GetNodeHeight<T>(TreeNode<T> node)
        {
            return TraverseInDepth(node).Max(ni => ni.Depth + 1);
        }
    }
}
