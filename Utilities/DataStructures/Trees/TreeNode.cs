using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TreeNode<T>
    {
        private readonly List<TreeNode<T>> _children;

        public T Value { get; private set; }

        public IList<TreeNode<T>> Children { get { return _children; } }

        public bool IsLeaf 
        {
            get { return Children.Count == 0; }
        }

        private TreeNode(T value, IEnumerable<TreeNode<T>> children, bool overloadedConstructorMarker)
        {
            Value = value;
            _children = children.ToList();
        }

        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
            : this(value, children, false)
        {            
        }

        public TreeNode(T value, params TreeNode<T>[] children)
            : this(value, children, false)
        {            
        }

        public void AddChild(TreeNode<T> node)
        {
            _children.Add(node);
        }

        public void RemoveChild(TreeNode<T> node)
        {
            _children.Remove(node);
        }
    }
}
