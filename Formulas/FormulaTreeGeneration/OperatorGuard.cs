using System;
using System.Collections.Generic;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    // public delegate FormulaTreeNodeWrapper = Func<FormulaTreeNode, FormulaTreeNode>;
    //{
    //    /*private readonly Func<FormulaTreeNode, FormulaTreeNode> _wrapper;

    //    public FormulaTreeNode Wrap(FormulaTreeNode formulaTreeNode)
    //    {
    //        return _wrapper(formulaTreeNode);
    //    }

    //    public FormulaTreeNodeRootWrapper(Func<FormulaTreeNode, FormulaTreeNode> wrapper)
    //    {
    //        _wrapper = wrapper;
    //    }

    //    public FormulaTreeNodeRootWrapper(UnaryOperator guardingOperator)
    //        : this(node => new FormulaTreeNode(guardingOperator, node)) 
    //    {
    //    }*/
    //}


    //public class FormulaTreeNodeChildrenWrapper
    //{
    //    private readonly IDictionary<int, Func<FormulaTreeNode, FormulaTreeNode>> _childrenIndexesAndWrappers;

    //    public FormulaTreeNode Wrap(FormulaTreeNode formulaTreeNode)
    //    {
    //        return null;
    //        ;_wrapper(formulaTreeNode);
    //    }

    //    public FormulaTreeNodeChildrenWrapper(IDictionary<int, Func<FormulaTreeNode, FormulaTreeNode>> childrenIndexesAndWrappers)
    //    {
    //        _childrenIndexesAndWrappers = childrenIndexesAndWrappers;
    //    }

    //    public FormulaTreeNodeChildrenWrapper(IDictionary<int, UnaryOperator> childrenIndexesAndguardingOperators)
    //    {
    //        _childrenIndexesAndWrappers = new Dictionary<int, Func<FormulaTreeNode, FormulaTreeNode>>();
    //        foreach (int childIndex in childrenIndexesAndguardingOperators.Keys)
    //        {
    //            UnaryOperator guardingOperator = childrenIndexesAndguardingOperators[childIndex];
    //            Func<FormulaTreeNode, FormulaTreeNode> wrapper = node => new FormulaTreeNode(guardingOperator, node);
    //            _childrenIndexesAndWrappers.Add(childIndex, wrapper);
    //        }
    //    }
    //}

    public class FormulaTreeNodeWrapper
    {
        public Func<FormulaTreeNode, FormulaTreeNode> Wrap { get; private set; }

        public FormulaTreeNodeWrapper(Func<FormulaTreeNode, FormulaTreeNode> wrap)
        {
            Wrap = wrap;
        }

        public FormulaTreeNodeWrapper(UnaryOperator unaryOperator)
            : this(node => new FormulaTreeNode(unaryOperator, node))
        {
        }
    }

    public class OperatorGuard
    {
        public FormulaTreeNodeWrapper RootWrapper { get; private set; }

        public IDictionary<int, FormulaTreeNodeWrapper> ChildrenWrappers { get; private set; }

        public OperatorGuard(FormulaTreeNodeWrapper rootWrapper)
            : this (rootWrapper, null)
        {
        }

        public OperatorGuard(FormulaTreeNodeWrapper rootWrapper,
                             IDictionary<int, FormulaTreeNodeWrapper> childrenWrappers)
        {
            RootWrapper = rootWrapper;
            ChildrenWrappers = childrenWrappers;
        }
    }

    public class OperatorGuards
    {
        public FormulaTreeNodeWrapper[] RootWrappers { get; private set; }

        public IDictionary<int, FormulaTreeNodeWrapper[]> ChildrenWrappers { get; private set; }

        public OperatorGuards(FormulaTreeNodeWrapper[] rootWrappers)
            : this (rootWrappers, null)
        {
        }

        public OperatorGuards(FormulaTreeNodeWrapper[] rootWrappers,
                              IDictionary<int, FormulaTreeNodeWrapper[]> childrenWrappers)
        {
            RootWrappers = rootWrappers;
            ChildrenWrappers = childrenWrappers;
        }
    }

    public class OperatorsAndGuards
    {
        private static readonly IDictionary<Operator, OperatorGuards> _operatorsAndGuards;

        static OperatorsAndGuards()
        {
            _operatorsAndGuards = new Dictionary<Operator, OperatorGuards>
            {
                {
                    OperatorsLibrary.Div,
                    new OperatorGuards(
                        new[]                                          
                        {
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                        })
                },

                {
                    OperatorsLibrary.Ln,
                    new OperatorGuards(
                        new[]
                        {
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                        })
                },

                {
                    OperatorsLibrary.Pow,
                    new OperatorGuards(null,
                        new Dictionary<int, FormulaTreeNodeWrapper[]>
                        {
                            {
                                1, 
                                new[]
                                {
                                    new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                                    new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                                }}
                        })
                },  

                {
                    OperatorsLibrary.Sinh,
                    new OperatorGuards(new[]
                                      {
                                          new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                                          new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                                      })
                },
            };
        }

        public static OperatorGuards Get(Operator op)
        {
            return _operatorsAndGuards[op];
        }

        public static OperatorGuard GetRandomGuard(Operator op)
        {
            return null;
        }
    }
}
