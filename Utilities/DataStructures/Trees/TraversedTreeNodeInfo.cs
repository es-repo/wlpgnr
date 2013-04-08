namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TraversedTreeNodeInfo<T>
    {
        public TreeNode<T> Node { get; private set; }

        public TreeNode<T> ParentNode { get; private set; }

        public int IndexAmongSiblings { get; private set; }

        public int Depth { get; private set; }

        public TraversedTreeNodeInfo(TreeNode<T> node, TreeNode<T> parentNode, int indexAmongSiblings, int depth)
        {
            Node = node;
            ParentNode = parentNode;
            IndexAmongSiblings = indexAmongSiblings;
            Depth = depth;
        }
    }
}
