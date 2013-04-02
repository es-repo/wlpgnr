namespace WallpaperGenerator.Utilities.DataStructures.Trees
{
    public class TraversedTreeNodeInfo<T>
    {
        public TreeNode<T> Node { get; private set; }

        public int IndexAmongSiblings { get; private set; }

        public int TraversedIndex { get; private set; }

        public int Depth { get; private set; }

        public TraversedTreeNodeInfo(TreeNode<T> node, int indexAmongSiblings, int traversedIndex, int depth)
        {
            Node = node;
            IndexAmongSiblings = indexAmongSiblings;
            TraversedIndex = traversedIndex;
            Depth = depth;
        }
    }
}
