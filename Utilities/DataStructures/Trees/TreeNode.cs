using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TreeNode<T>
    {
        public T Value { get; private set; }

        public IList<TreeNode<T>> Children { get; private set; }

        public bool IsLeaf 
        {
            get { return Children.Count == 0; }
        }

// ReSharper disable UnusedParameter.Local
        private TreeNode(T value, IEnumerable<TreeNode<T>> children, bool overloadedConstructorMarker)
// ReSharper restore UnusedParameter.Local
        {
            Value = value;
            Children = new ReadOnlyCollection<TreeNode<T>>(children.ToList());
        }

        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
            : this(value, children, false)
        {            
        }

        public TreeNode(T value, params TreeNode<T>[] children)
            : this(value, children, false)
        {            
        }

        public TreeNode(T value)
            : this(value, Enumerable.Empty<TreeNode<T>>())
        {                
        }
    }
}
