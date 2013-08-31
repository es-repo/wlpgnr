using System;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class FormulaTreeNodeWrapper
    {
        public Func<TreeNode<Operator>, TreeNode<Operator>> Wrap { get; private set; }

        public FormulaTreeNodeWrapper(Func<TreeNode<Operator>, TreeNode<Operator>> wrap)
        {
            Wrap = wrap;
        }

        public FormulaTreeNodeWrapper(UnaryOperator unaryOperator)
            : this(node => new TreeNode<Operator>(unaryOperator, node))
        {
        }
    }
}
