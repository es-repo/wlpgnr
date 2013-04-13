using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Parsing;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTreeSerializationOptions
    {
        public bool WithIndentation { get; set; }
    }

    public static class FormulaTreeSerializer
    {
        public static string Serialize(FormulaTreeNode node)
        {
            return Serialize(node, new FormulaTreeSerializationOptions());
        }

        public static string Serialize(FormulaTreeNode node, FormulaTreeSerializationOptions serializationOptions)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<TraversedTreeNodeInfo<Operator>> nodes = Tree.TraverseDepthFirstPreOrder(node);
            int depth = 0;
            bool withIndent = serializationOptions.WithIndentation;
            foreach (TraversedTreeNodeInfo<Operator> ni in nodes)
            {
                if (depth > ni.Depth)
                {
                    sb.Append(new string(')', depth - ni.Depth));
                }
                sb.Append(ni.IndexAmongSiblings == 0 ? "(" : withIndent ? "" : " ");
                if (withIndent)
                {
                    sb.Append("\n" + new string('\t', ni.Depth));
                }
                string opStr = OperatorToString(ni.Node.Value);
                sb.Append(opStr);
                depth = ni.Depth;
            }
            sb.Append(new string(')', depth + 1));
            return sb.ToString();
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

        public static FormulaTreeNode Deserialize(string value)
        {
            return FormulaTreeParser.Parse(value);
        }
    }
}
