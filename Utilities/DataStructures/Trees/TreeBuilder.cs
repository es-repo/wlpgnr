using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TreeBuilder<T>
    {
        private readonly Stack<Tuple<TreeNode<T>, int>> _stack;
        private readonly Func<T, int, TreeNodeInfo<T>> _appendFunc;

        public TraversalOrder NodeValuesOrder { get; private set; }

        public TreeNode<T> Root { get; private set; }

        public TreeBuilder(TraversalOrder nodeValuesOrder = TraversalOrder.DepthFirstPreOrder)
        {
            NodeValuesOrder = nodeValuesOrder;

            switch (NodeValuesOrder)
            {
                case TraversalOrder.DepthFirstPreOrder:
                    _appendFunc = AppendDepthFirstPreOrder;
                    break;
            }

            _stack = new Stack<Tuple<TreeNode<T>, int>>();
        }

        public TreeNode<T> Append(IEnumerable<T> nodeValues, Func<T, int> getNodeChildrenCount)
        {
            foreach (T value in nodeValues)
            {
                Append(value, getNodeChildrenCount(value));
            }

            return Root;
        }

        public TreeNodeInfo<T> Append(T nodeValue, int nodeChildrenCount)
        {
            return _appendFunc(nodeValue, nodeChildrenCount);
        }

        private TreeNodeInfo<T> AppendDepthFirstPreOrder(T nodeValue, int nodeChildrenCount)
        {
            TreeNode<T> parentNode = null;
            if (_stack.Count > 0)
            {
                Tuple<TreeNode<T>, int> parentNodeAndChildrenCount = _stack.Pop();
                parentNode = parentNodeAndChildrenCount.Item1;
                int parentChildrenCount = parentNodeAndChildrenCount.Item2;
                if (parentChildrenCount > 1)
                {
                    _stack.Push(new Tuple<TreeNode<T>, int>(parentNode, parentChildrenCount - 1));
                }
            }

            TreeNode<T> node = new TreeNode<T>(nodeValue);

            if (Root == null)
            {
                Root = node;
            }

            int nodeIndexAmongSiblings = parentNode != null ? parentNode.Children.Count : 0;
            int nodeDepth = _stack.Count + 1;
            TreeNodeInfo<T> nodeInfo = new TreeNodeInfo<T>(node, parentNode, nodeIndexAmongSiblings, nodeDepth);

            if (parentNode != null)
            {
                parentNode.AddChild(node);
            }

            if (nodeChildrenCount > 0)
            {
                _stack.Push(new Tuple<TreeNode<T>, int>(node, nodeChildrenCount));
            }

            return nodeInfo;
        }
    }
}
