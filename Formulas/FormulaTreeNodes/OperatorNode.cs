using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas.FormulaTreeNodes
{
    public class OperatorNode : FormulaTreeNode
    {
        public Operator Operator { get; private set; }

        public OperatorNode(Operator @operator, IEnumerable<FormulaTreeNode> children)
            : base(children)
        {
            Operator = @operator;
        }

        public OperatorNode(Operator @operator, params FormulaTreeNode[] children)
            : base(children)
        {
            Operator = @operator;
        }

        public OperatorNode(Operator @operator)
            : this(@operator, Enumerable.Empty<FormulaTreeNode>())
        {            
        }
    }
}
