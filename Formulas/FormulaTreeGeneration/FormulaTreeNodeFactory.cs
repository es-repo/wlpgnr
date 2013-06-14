using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class FormulaTreeNodeFactory
    {
        private readonly Random _random;

        private readonly IDictionary<Operator, OperatorGuards> _operatorsAndGuards;

        public FormulaTreeNodeFactory(Random random)
            : this(random, DefaultOperatorsAndGuards.Get())
        {
        }

        public FormulaTreeNodeFactory(Random random, IDictionary<Operator, OperatorGuards> operatorsAndGuards)
        {
            _random = random;
            _operatorsAndGuards = operatorsAndGuards;
        }

        public FormulaTreeNode Create(Operator op, IEnumerable<FormulaTreeNode> children)
        {
            return CreateInternal(op, children);
        }

        public FormulaTreeNode Create(Operator op, params FormulaTreeNode[] children)
        {
            return CreateInternal(op, children);
        }
        
        public FormulaTreeNode Create(Operator op, OperatorGuard operatorGuard, IEnumerable<FormulaTreeNode> children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        public FormulaTreeNode Create(Operator op, OperatorGuard operatorGuard, params FormulaTreeNode[] children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        private FormulaTreeNode CreateInternal(Operator op, IEnumerable<FormulaTreeNode> children)
        {
            OperatorGuards operatorGuards = _operatorsAndGuards != null && _operatorsAndGuards.ContainsKey(op) ? _operatorsAndGuards[op] : null;
            OperatorGuard operatorGuard = operatorGuards != null ? operatorGuards.GetRandom(_random) : null;
            return CreateInternal(op, operatorGuard, children);
        }

        private FormulaTreeNode CreateInternal(Operator op, OperatorGuard operatorGuard, IEnumerable<FormulaTreeNode> children)
        {
            if (operatorGuard == null)
            {
                return new FormulaTreeNode(op, children);
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

        private IEnumerable<FormulaTreeNode> WrapChildren(IEnumerable<FormulaTreeNode> children,
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
