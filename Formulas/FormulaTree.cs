using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public static string Serialize(FormulaTreeNode node)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<TraversedTreeNodeInfo<Operator>> nodes = Tree.TraverseDepthFirstPreOrder(node);
            int depth = 0;
            foreach (TraversedTreeNodeInfo<Operator> ni in nodes)
            {
                if (depth > ni.Depth)
                {
                    sb.Append(new string(')', depth - ni.Depth));
                }
                sb.Append(ni.IndexAmongSiblings == 0 ? "(" : ",");
                string opStr = OperatorToString(ni.Node.Value);
                sb.Append(opStr);
                depth = ni.Depth;
            }
            sb.Append(new string(')', depth + 1));
            return sb.ToString();
        }

        public static FormulaTreeNode Deserialize(string str)
        {
            return null;
        }

        private static string OperatorToString(Operator op)
        {
            if (op is Constant)
            {
                return ((Constant) op).Value.ToString(CultureInfo.InvariantCulture);
            }
            if (op is Variable)
            {
                return ((Variable)op).Name;
            }

            return op.GetType().Name;
        }
    }
}
