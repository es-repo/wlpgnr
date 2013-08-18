using System.Linq;  
using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTreeNode : TreeNode<Operator>
    {
        public FormulaTreeNode(Operator op, IEnumerable<FormulaTreeNode> children)
            : base(op, children)
        {
        }

        public FormulaTreeNode(Operator op, params FormulaTreeNode[] children)
            : base(op, children.Cast<TreeNode<Operator>>())
        {
        }

        public Operator Operator 
        {
            get { return Value; }
        }

        public IEnumerable<FormulaTreeNode> Operands 
        { 
            get { return Children.Cast<FormulaTreeNode>(); } 
        } 
    }
}
