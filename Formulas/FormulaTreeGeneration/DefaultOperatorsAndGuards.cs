using System.Collections.Generic;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public static class DefaultOperatorsAndGuards
    {
        private static readonly Dictionary<Operator, OperatorGuards> _map;

        static DefaultOperatorsAndGuards()
        {
            _map = new Dictionary<Operator, OperatorGuards>
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
                    OperatorsLibrary.Pow2,
                    new OperatorGuards(
                        new[]
                        {
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                        })
                },

                {
                    OperatorsLibrary.Pow3,
                    new OperatorGuards(
                        new[]
                        {
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Atan),
                            new FormulaTreeNodeWrapper(OperatorsLibrary.Tanh)
                        })
                },

                {
                    OperatorsLibrary.Pow,
                    new OperatorGuards(
                        null,
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
                    OperatorsLibrary.Mod,
                    new OperatorGuards(null,
                        new Dictionary<int, FormulaTreeNodeWrapper[]>
                        {
                            {
                                1, 
                                new[]
                                {
                                    new FormulaTreeNodeWrapper(
                                        node => 
                                            new FormulaTreeNode(OperatorsLibrary.Sum,
                                                new FormulaTreeNode(OperatorsLibrary.Mul, node, node),
                                                new FormulaTreeNode(new Constant(0.0001))))
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

        public static IDictionary<Operator, OperatorGuards> Get()
        {
            return _map;
        }
    }
}
