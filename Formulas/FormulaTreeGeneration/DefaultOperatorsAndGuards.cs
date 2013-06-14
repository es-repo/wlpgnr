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

        public static IDictionary<Operator, OperatorGuards> Get()
        {
            return _map;
        }
    }
}
