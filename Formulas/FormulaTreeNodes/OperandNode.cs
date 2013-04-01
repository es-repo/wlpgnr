using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas.FormulaTreeNodes
{
    public class OperandNode : FormulaTreeNode
    {
        public Operand Operand { get; private set; }

        public OperandNode(Operand operand, IEnumerable<FormulaTreeNode> children)
            : base(children)
        {
            Operand = operand;
        }

        public OperandNode(Operand operand, params FormulaTreeNode[] children)
            : base(children)
        {
            Operand = operand;
        }

        public OperandNode(Operand operand)
            : this(operand, Enumerable.Empty<FormulaTreeNode>())
        {            
        }
    }
}
