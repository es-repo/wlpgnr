namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TreeNodeInfo<T>
    {
        public TreeNode<T> Node { get; private set; }

        public TreeNode<T> ParentNode { get; private set; }

        public int IndexAmongSiblings { get; private set; }

        public int Depth
        {
            get; private set;
        }

        public TreeNodeInfo(TreeNode<T> node, TreeNode<T> parentNode, int indexAmongSiblings, int depth)
        {
            Node = node;
            ParentNode = parentNode;
            IndexAmongSiblings = indexAmongSiblings;
            Depth = depth;
        }
    }
}
