using System;
using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Trees;

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

        public TreeNode<Operator> Create(Operator op, IEnumerable<TreeNode<Operator>> children)
        {
            return CreateInternal(op, children);
        }

        public TreeNode<Operator> Create(Operator op, params TreeNode<Operator>[] children)
        {
            return CreateInternal(op, children);
        }
        
        public TreeNode<Operator> Create(Operator op, OperatorGuard operatorGuard, IEnumerable<TreeNode<Operator>> children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        public TreeNode<Operator> Create(Operator op, OperatorGuard operatorGuard, params TreeNode<Operator>[] children)
        {
            return CreateInternal(op, operatorGuard, children);
        }

        private TreeNode<Operator> CreateInternal(Operator op, IEnumerable<TreeNode<Operator>> children)
        {
            OperatorGuards operatorGuards = _operatorsAndGuards != null && _operatorsAndGuards.ContainsKey(op) ? _operatorsAndGuards[op] : null;
            OperatorGuard operatorGuard = operatorGuards != null ? operatorGuards.GetRandom(_random) : null;
            return CreateInternal(op, operatorGuard, children);
        }

        private TreeNode<Operator> CreateInternal(Operator op, OperatorGuard operatorGuard, IEnumerable<TreeNode<Operator>> children)
        {
            if (operatorGuard == null)
            {
                return new TreeNode<Operator>(op, children);
            }
            
            if (operatorGuard.ChildrenWrappers != null)
            {
                children = WrapChildren(children, operatorGuard.ChildrenWrappers);
            }

            TreeNode<Operator> node = new TreeNode<Operator>(op, children);
            if (operatorGuard.RootWrapper != null)
            {
                node = operatorGuard.RootWrapper.Wrap(node);
            }

            return node;
        }

        private IEnumerable<TreeNode<Operator>> WrapChildren(IEnumerable<TreeNode<Operator>> children,
            IDictionary<int, FormulaTreeNodeWrapper> childrenWrappers)
        {
            int i = 0;
            foreach (TreeNode<Operator> child in children)
            {
                yield return childrenWrappers.ContainsKey(i)
                    ? childrenWrappers[i].Wrap(child)
                    : child;

                i++;
            }
        }
    }
}
