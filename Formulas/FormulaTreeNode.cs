using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTreeNode : TreeNode<Operator>
    {
        public FormulaTreeNode(Operator value, IEnumerable<TreeNode<Operator>> children) 
            : base(value, children)
        {
        }

        public FormulaTreeNode(Operator value, params TreeNode<Operator>[] children) 
            : base(value, children)
        {
        }

        public FormulaTreeNode(Operator value) : base(value)
        {
        }

        public Operator Operator 
        {
            get { return Value; }
        }
    }
}
