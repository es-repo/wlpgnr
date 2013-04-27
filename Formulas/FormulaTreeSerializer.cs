using System.Collections.Generic;
using System.Text;
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
            IEnumerable<TraversedTreeNodeInfo<Operator>> nodes = Tree<Operator>.TraverseDepthFirstPreOrder(node);
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
                sb.Append(ni.Node.Value.Name);
                depth = ni.Depth;
            }
            sb.Append(new string(')', depth + 1));
            return sb.ToString();
        }

        public static FormulaTreeNode Deserialize(string value)
        {
            return FormulaTreeParser.Parse(value);
        }
    }
}
