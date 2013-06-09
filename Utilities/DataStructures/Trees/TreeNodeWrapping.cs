using System.Linq;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public static class TreeNodeWrapping
    {
        public static TreeNode<T> Wrap<T>(TreeNode<T> treeNode, T value)
        {
            return new TreeNode<T>(value, treeNode);
        }

        public static TreeNode<T> WrapChildren<T>(TreeNode<T> treeNode, IDictionary<int, T> childrenIndexesAndValues)
        {
            Dictionary<int, TreeNode<T>> wrappedChildern = new Dictionary<int, TreeNode<T>>();
            foreach (int childIndex in childrenIndexesAndValues.Keys)
            {
                TreeNode<T> wrappedChild = Wrap(treeNode.Children[childIndex], childrenIndexesAndValues[childIndex]);
                wrappedChildern.Add(childIndex, wrappedChild);
            }

            IEnumerable<TreeNode<T>> children = treeNode.Children.
                Select((node, i) => childrenIndexesAndValues.ContainsKey(i) ? wrappedChildern[i] : node);

            return new TreeNode<T>(treeNode.Value, children);
        }
    }
}
