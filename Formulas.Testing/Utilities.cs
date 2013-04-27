using System.Collections.Generic;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace Formulas.Testing
{
    public class Utilities
    {
        public static bool AreFormulaTreesEqual(FormulaTreeNode rootA, FormulaTreeNode rootB)
        {
            if (rootA == null && rootB == null)
                return true;

            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedNodesA = Tree<Operator>.TraverseBredthFirstPreOrder(rootA);
            IEnumerable<TraversedTreeNodeInfo<Operator>> traversedNodesB = Tree<Operator>.TraverseBredthFirstPreOrder(rootB);
            IEnumerator<TraversedTreeNodeInfo<Operator>> nodesInfoBEnumerator = traversedNodesB.GetEnumerator();
            foreach (var nodeInfoA in traversedNodesA)
            {
                nodesInfoBEnumerator.MoveNext();
                var nodeInfoB = nodesInfoBEnumerator.Current;
                if (nodeInfoB == null || !AreFormulaTreeNodesEqual((FormulaTreeNode)nodeInfoA.Node, (FormulaTreeNode)nodeInfoB.Node))
                    return false;
            }

            return true;
        }

        public static bool AreFormulaTreeNodesEqual(FormulaTreeNode nodeA, FormulaTreeNode nodeB)
        {
            Operator opA = nodeA.Value;
            Operator opB = nodeB.Value;

            if (opA is Constant && opB is Constant)
            {
                return ((Constant)opA).Value.Equals(((Constant)opB).Value);
            }

            return opA.Name == opB.Name;
        }
    }
}
