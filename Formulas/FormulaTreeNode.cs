using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas
{
    public abstract class FormulaTreeNode
    {
        public IList<FormulaTreeNode> Children { get; private set; }

        public bool IsLeaf 
        {
            get { return Children.Count == 0; }
        }

        protected FormulaTreeNode(IEnumerable<FormulaTreeNode> children)
        {
            Children = children.ToList();
        }

        protected FormulaTreeNode(params FormulaTreeNode[] children)
        {
            Children = children.ToList();
        }

        protected FormulaTreeNode()
            : this(Enumerable.Empty<FormulaTreeNode>())
        {            
        }
    }
}
