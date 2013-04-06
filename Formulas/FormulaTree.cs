using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public static class FormulaTree 
    {
        public static double Evaluate(FormulaTreeNode node)
        {
            return Tree.Fold<Operator, double>(node, (ni, c) => ni.Node.Value.Evaluate(c));
        }

        public static double Evaluate2(FormulaTreeNode node)
        {
            IEnumerable<double> operands = node.Children.Cast<FormulaTreeNode>().Select(n => Evaluate(n));
            return node.Value.Evaluate(operands);
        }
    }
}
