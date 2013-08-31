using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public static class TreeSerializer
    {
        public static string Serialize<T>(TreeNode<T> treeRoot)
        {
            return Serialize(treeRoot, v => v.ToString());
        }

        public static string Serialize<T>(TreeNode<T> treeRoot, Func<T, string> valueToString)
        {
            IEnumerable<TreeNodeInfo<T>> traversedNodes = Tree<T>.Traverse(treeRoot, TraversalOrder.DepthFirstPreOrder);
            IEnumerable<string> serializedNodeValues = traversedNodes.Select(ni => valueToString(ni.Node.Value));
            return string.Join(" ", serializedNodeValues.ToArray());
        }

        public static TreeNode<T> Deserialize<T>(string value, Func<string, T> nodeValueFromString, Func<T, int> getNodeChildrenCount)
        {
            string[] tokens = value.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<T> nodeValues = tokens.Select(nodeValueFromString);
            return Tree<T>.Build(nodeValues, getNodeChildrenCount);
        }
    }
}
