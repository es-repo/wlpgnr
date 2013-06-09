using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class FormulaGuard
    {
        private readonly Func<FormulaTreeNode, FormulaTreeNode> _overriding;

        public FormulaTreeNode Override(FormulaTreeNode formulaTreeNode)
        {
            return _overriding(formulaTreeNode);
        }

        public FormulaGuard(Func<FormulaTreeNode, FormulaTreeNode> overriding)
        {
            _overriding = overriding;
        }

        public FormulaGuard(UnaryOperator guardingOperator)
            : this(node => new FormulaTreeNode(guardingOperator, node)) 
        {
        }
    }
    
    public class OperatorGuard
    {
        public Operator Operator { get; private set; }

        public FormulaGuard[] OperatorOverrides { get; private set; }

        public IDictionary<int, FormulaGuard[]> OperandOverrides { get; private set; }

        public OperatorGuard(Operator op, IEnumerable<FormulaGuard> operatorOverrides, IDictionary<int, IEnumerable<FormulaGuard>> operandOverrides)
        {
            Operator = op;
            OperatorOverrides = operatorOverrides.ToArray();
            OperandOverrides = new Dictionary<int, FormulaGuard[]>();
            foreach (int operandIndex in operandOverrides.Keys)
            {
                OperandOverrides.Add(operandIndex, operandOverrides[operandIndex].ToArray());
            }
        }
    }

    public static class OperatorGuards
    {
        private static readonly IDictionary<Operator, OperatorGuard> _operatorGuards;

        static OperatorGuards()
        {
            _operatorGuards = new Dictionary<Operator, OperatorGuard>
            {
                { 
                    OperatorsLibrary.Div, 
                        new OperatorGuard(OperatorsLibrary.Div, 
                            new []
                                {
                                    new FormulaGuard(OperatorsLibrary.Atan), 
                                    new FormulaGuard(OperatorsLibrary.Tanh)
                                }, null)
                },

                { 
                    OperatorsLibrary.Ln, 
                        new OperatorGuard(OperatorsLibrary.Ln, 
                            new []
                                {
                                    new FormulaGuard(OperatorsLibrary.Atan), 
                                    new FormulaGuard(OperatorsLibrary.Tanh)
                                }, null)
                },

                { 
                    OperatorsLibrary.Sinh, 
                        new OperatorGuard(OperatorsLibrary.Sinh, 
                            new []
                                {
                                    new FormulaGuard(OperatorsLibrary.Atan), 
                                    new FormulaGuard(OperatorsLibrary.Tanh)
                                }, null)
                },
            };
        }

        public static OperatorGuard Get(Operator op)
        {
            return _operatorGuards[op];
        }
    }
}
