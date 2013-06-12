using System.Collections.Generic;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public static class FormulaTreeNodeFactory
    {
        public static FormulaTreeNode Create(Operator op, OperatorGuard operatorGuard, IEnumerable<FormulaTreeNode> children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        public static FormulaTreeNode Create(Operator op, OperatorGuard operatorGuard, params FormulaTreeNode[] children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        private static FormulaTreeNode CreateInternal(Operator op, IEnumerable<FormulaTreeNode> children)
        {
            return new FormulaTreeNode(op, children);
        }

        private static FormulaTreeNode CreateInternal(Operator op, OperatorGuard operatorGuard, IEnumerable<FormulaTreeNode> children)
        {
            if (operatorGuard == null)
            {
                return CreateInternal(op, children);
            }
            
            if (operatorGuard.ChildrenWrappers != null)
            {
                children = WrapChildren(children, operatorGuard.ChildrenWrappers);
            }

            FormulaTreeNode node = new FormulaTreeNode(op, children);
            if (operatorGuard.RootWrapper != null)
            {
                node = operatorGuard.RootWrapper.Wrap(node);
            }

            return node;
        }

        private static IEnumerable<FormulaTreeNode> WrapChildren(IEnumerable<FormulaTreeNode> children,
            IDictionary<int, FormulaTreeNodeWrapper> childrenWrappers)
        {
            int i = 0;
            foreach (FormulaTreeNode child in children)
            {
                yield return childrenWrappers.ContainsKey(i)
                    ? childrenWrappers[i].Wrap(child)
                    : child;

                i++;
            }
        }
    }
}
