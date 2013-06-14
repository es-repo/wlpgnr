using System;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class FormulaTreeNodeWrapper
    {
        public Func<FormulaTreeNode, FormulaTreeNode> Wrap { get; private set; }

        public FormulaTreeNodeWrapper(Func<FormulaTreeNode, FormulaTreeNode> wrap)
        {
            Wrap = wrap;
        }

        public FormulaTreeNodeWrapper(UnaryOperator unaryOperator)
            : this(node => new FormulaTreeNode(unaryOperator, node))
        {
        }
    }
}
