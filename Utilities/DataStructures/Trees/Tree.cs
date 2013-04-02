using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{    
    public class Tree<T>
    {
        public TreeNode<T> Root { get; private set; } 

        public Tree(TreeNode<T> root)
        {
            Root = root;
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterInDepth(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            int depth = 0;
            int traversedIndex = 0;
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, 0, traversedIndex++, depth++);
            Stack<TraversedTreeNodeInfo<T>> stack = new Stack<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (stack.Any())
            {
                TraversedTreeNodeInfo<T> nodeInfo = stack.Pop();
                if (predicate == null || predicate(nodeInfo))
                    yield return nodeInfo;

                int indexAmongSiblings = nodeInfo.Node.Children.Count - 1;
                foreach (TreeNode<T> n in nodeInfo.Node.Children.Reverse())
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, indexAmongSiblings, traversedIndex, depth);
                    stack.Push(childNodeInfo);
                    indexAmongSiblings--;
                    traversedIndex++;
                }
                
                depth++;
            }
        }

        public IEnumerable<TraversedTreeNodeInfo<T>> FilterInDepth(Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            return FilterInDepth(Root, predicate);
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseInDepth(TreeNode<T> root)
        {
            return FilterInDepth(root, null);
        }

        public IEnumerable<TraversedTreeNodeInfo<T>> TraverseInDepth()
        {
            return TraverseInDepth(Root);
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> FilterInBreadth(TreeNode<T> root, Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            int depth = 0;
            int traversedIndex = 0;
            TraversedTreeNodeInfo<T> rootNodeInfo = new TraversedTreeNodeInfo<T>(root, 0, traversedIndex++, depth++);
            Queue<TraversedTreeNodeInfo<T>> queue = new Queue<TraversedTreeNodeInfo<T>>(new[] { rootNodeInfo });
            while (queue.Any())
            {
                TraversedTreeNodeInfo<T> nodeInfo = queue.Dequeue();
                if (predicate == null || predicate(nodeInfo))
                    yield return nodeInfo;
                
                int indexAmongSiblings = 0;
                foreach (TreeNode<T> n in nodeInfo.Node.Children)
                {
                    TraversedTreeNodeInfo<T> childNodeInfo = new TraversedTreeNodeInfo<T>(n, indexAmongSiblings, traversedIndex, depth);
                    queue.Enqueue(childNodeInfo);
                    indexAmongSiblings++;
                    traversedIndex++;
                }

                traversedIndex++;
                depth++;
            }
        }

        public IEnumerable<TraversedTreeNodeInfo<T>> FilterInBreadth(Predicate<TraversedTreeNodeInfo<T>> predicate)
        {
            return FilterInBreadth(Root, predicate);
        }

        public static IEnumerable<TraversedTreeNodeInfo<T>> TraverseInBreadth(TreeNode<T> root)
        {
            return FilterInBreadth(root, null);
        }

        public IEnumerable<TraversedTreeNodeInfo<T>> TraverseInBreadth()
        {
            return TraverseInBreadth(Root);
        }

        public static int GetNodeHeight(TreeNode<T> node)
        {
            return TraverseInDepth(node).Max(ni => ni.Depth);
        }
    }
}
