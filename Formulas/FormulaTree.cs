using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public static class FormulaTree
    {
        public static IEnumerable<Variable> SelectVariables(FormulaTreeNode node)
        {
            return Tree.TraverseBredthFirstPreOrder(node)
                       .Where(ni => ni.Node.Value is Variable)
                       .Select(ni => (Variable)ni.Node.Value);
        }
        
        public static void SetVariableValues(FormulaTreeNode node, IDictionary<string, double?> variableNamesAndValues)
        {
            IEnumerable<Variable> variables = SelectVariables(node).Where(v => variableNamesAndValues.ContainsKey(v.Name));
            foreach (Variable v in variables)
            {
                v.Value = variableNamesAndValues[v.Name];
            }
        }
        
        public static double Evaluate(FormulaTreeNode node)
        {
            return Tree.Fold<Operator, double>(node, (ni, c) => ni.Node.Value.Evaluate(c));
        }
    }
}
