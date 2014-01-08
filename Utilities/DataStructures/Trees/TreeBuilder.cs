using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TreeBuilder<T>
    {
        private readonly Stack<Tuple<TreeNodeInfo<T>, int>> _stack;
        private readonly Func<T, int, TreeNodeInfo<T>> _appendFunc;

        public TraversalOrder NodeValuesOrder { get; private set; }

        public TreeNode<T> Root { get; private set; }

        public TreeNodeInfo<T> NextAppendingNodeInfo { get; private set; }

        public bool IsTreeReady 
        {
            get { return NextAppendingNodeInfo == null; }
        }

        public TreeBuilder(TraversalOrder nodeValuesOrder = TraversalOrder.DepthFirstPreOrder)
        {
            NodeValuesOrder = nodeValuesOrder;

            switch (NodeValuesOrder)
            {
                case TraversalOrder.DepthFirstPreOrder:
                    _appendFunc = AppendDepthFirstPreOrder;
                    break;
            }

            _stack = new Stack<Tuple<TreeNodeInfo<T>, int>>();

            NextAppendingNodeInfo = new TreeNodeInfo<T>(null, null, 0, 1);
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
            if (IsTreeReady)
            {
                throw new InvalidOperationException("Tree is already built.");
            }

            TreeNodeInfo<T> parentNodeInfo = null;
            if (_stack.Count > 0)
            {
                Tuple<TreeNodeInfo<T>, int> parentNodeInfoAndChildrenCount = _stack.Pop();
                parentNodeInfo = parentNodeInfoAndChildrenCount.Item1;
                int parentChildrenCount = parentNodeInfoAndChildrenCount.Item2;
                if (parentChildrenCount > 1)
                {
                    _stack.Push(new Tuple<TreeNodeInfo<T>, int>(parentNodeInfo, parentChildrenCount - 1));
                }
            }

            TreeNode<T> node = new TreeNode<T>(nodeValue);

            TreeNode<T> parentNode = parentNodeInfo != null ? parentNodeInfo.Node : null;
            if (parentNode != null)
            {
                parentNode.AddChild(node);
            }

            TreeNodeInfo<T> nodeInfo = new TreeNodeInfo<T>(node, parentNode, NextAppendingNodeInfo.IndexAmongSiblings, NextAppendingNodeInfo.Depth);
            if (nodeChildrenCount > 0)
            {
                _stack.Push(new Tuple<TreeNodeInfo<T>, int>(nodeInfo, nodeChildrenCount));
            }

            if (Root == null)
            {
                Root = node;
            }

            Tuple<TreeNodeInfo<T>, int> nextParentNodeInfoAndChildrenCount = _stack.Count > 0 ? _stack.Peek() : null;
            TreeNodeInfo<T> nextParentNodeInfo = nextParentNodeInfoAndChildrenCount != null ? nextParentNodeInfoAndChildrenCount.Item1 : null;
            NextAppendingNodeInfo = nextParentNodeInfoAndChildrenCount == null 
                ? null 
                : new TreeNodeInfo<T>(null, nextParentNodeInfo.Node, nextParentNodeInfo.Node.Children.Count, nextParentNodeInfo.Depth + 1);

            return nodeInfo;
        }
    }
}
