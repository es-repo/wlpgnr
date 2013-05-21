using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Trigonometric;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTree : Tree<Operator>
    {
        #region Fields

        private readonly Func<double> _compiledFormula;

        #endregion

        #region Properties

        public FormulaTreeNode FormulaRoot
        {
            get { return (FormulaTreeNode) Root; }
        }

        public Variable[] Variables { get; private set; }

        #endregion

        #region Constructors

        public FormulaTree(FormulaTreeNode root)
            : base(root)
        {
            Variables = SelectVariables(FormulaRoot).ToArray();
            _compiledFormula = CompileFormula();
        }

        #endregion

        #region  Methods

        public static IEnumerable<Variable> SelectVariables(FormulaTreeNode node)
        {
            return Tree<Operator>.TraverseBredthFirstPreOrder(node)
                       .Where(ni => ni.Node.Value is Variable)
                       .Select(ni => (Variable) ni.Node.Value)
                       .Distinct();
        }

        public IEnumerable<double> EvaluateRanges(params Range[] variableValueRanges)
        {
            IEnumerable<double>[] series = variableValueRanges.Select(r => r.Values).ToArray();
            return EvaluateSeries(series);
        }

        public double[] EvaluateRangesIn2DProjection(params Range[] variableValueRanges)
        {
            double[] results = new double[variableValueRanges[0].Count*variableValueRanges[1].Count];
            for (int i = 0; i < variableValueRanges.Length; i += 2)
            {
                Variables[i].Value = variableValueRanges[i].Start - variableValueRanges[i].Step;
            }

            int r = 0;
            for (int x = 0; x < variableValueRanges[0].Count; x++)
            {
                for (int i = 0; i < variableValueRanges.Length; i += 2)
                {
                    Variables[i].Value += variableValueRanges[i].Step;
                }
                
                for (int i = 1; i < variableValueRanges.Length; i += 2)
                {
                    Variables[i].Value = variableValueRanges[i].Start - variableValueRanges[i].Step;
                }

                for (int y = 0; y < variableValueRanges[1].Count; y++)
                {
                    for (int i = 1; i < variableValueRanges.Length; i += 2)
                    {
                        Variables[i].Value += variableValueRanges[i].Step;
                    }

                    results[r++] = Evaluate();
                }
            }

            return results; 
        }

        public IEnumerable<double> EvaluateSeriesIn2DProjection(params IEnumerable<double>[] variableValues)
        {
            IEnumerator<double>[] variableValuesEnumerators = variableValues.Select(vv => vv.GetEnumerator()).ToArray();
            while (variableValuesEnumerators[0].MoveNext())
            {
                Variables[0].Value = variableValuesEnumerators[0].Current;
                for (int x = 2; x < variableValuesEnumerators.Length; x+=2)
                {
                    variableValuesEnumerators[x].MoveNext();
                    Variables[x].Value = variableValuesEnumerators[x].Current;
                }

                if (variableValues.Length > 1)
                {
                    while (variableValuesEnumerators[1].MoveNext())
                    {
                        Variables[1].Value = variableValuesEnumerators[1].Current;
                        for (int y = 3; y < variableValuesEnumerators.Length; y += 2)
                        {
                            variableValuesEnumerators[y].MoveNext();
                            Variables[y].Value = variableValuesEnumerators[y].Current;
                        }

                        yield return Evaluate();
                    }
                    for (int y = 1; y < variableValuesEnumerators.Length; y += 2)
                    {
                        variableValuesEnumerators[y] = variableValues[y].GetEnumerator();
                    }
                }
                else
                {
                    yield return Evaluate();
                }
            }
        } 

        public IEnumerable<double> EvaluateSeries(params IEnumerable<double>[] variableValues)
        {
            return EvaluateSeriesCore(variableValues, 0);
        }

        private IEnumerable<double> EvaluateSeriesCore(IEnumerable<double>[] variableValues, int variableIndex)
        {
            foreach (double value in variableValues[variableIndex])
            {
                Variables[variableIndex].Value = value;
                if (variableIndex == variableValues.Length - 1)
                {
                    yield return Evaluate();
                }
                else
                {
                    IEnumerable<double> results = EvaluateSeriesCore(variableValues, variableIndex + 1);
                    foreach (double r in results)
                    {
                        yield return r;
                    }    
                }
            }
        }

        public double Evaluate()
        {
            return _compiledFormula();
        }

        //public double Evaluate()
        //{
        //    return Fold((TraversedTreeNodeInfo<Operator> ni, double[] c) => ni.Node.Value.Evaluate(c));
        //}

        public Func<double> CompileFormula()
        {
            return Fold(
                (TraversedTreeNodeInfo<Operator> ni, Func<double>[] operands) =>
                {
                    Operator op = ni.Node.Value;
                    
                    Func<double> op0 = operands.Length > 0 ? operands[0] : null;
                    Func<double> op1 = operands.Length > 1 ? operands[1] : null;
                    Func<double> op2 = operands.Length > 2 ? operands[2] : null;
                    Func<double> op3 = operands.Length > 3 ? operands[3] : null;

                    if (op is Variable)
                    {
                        Variable v = (Variable)op;
                        return () => v.Value;
                    }
                    if (op is Constant)
                    {
                        Constant c = (Constant)op;
                        return () => c.Value;
                    }
                    if (op is Abs)
                    {
                        return () =>
                        {
                            double v = op0();
                            return v > 0 ? v : -v;
                        };
                    }
                    if (op is Sin)
                    {
                        return () => Math.Sin(op0());
                    }
                    if (op is Atan)
                    {
                        return () => Math.Atan(op0());
                    }
                    if (op is Tanh)
                    {
                        return () => Math.Tanh(op0());
                    }
                    if (op is Sqrt)
                    {
                        return () =>
                        {
                            double v = op0();
                            return Math.Sqrt(v > 0 ? v : -v);
                        };
                    }
                    if (op is Ln)
                    {
                        return () => Math.Log(op0(), Math.E);
                    }
                    if (op is Sum)
                    {
                        return () => op0() + op1();
                    }
                    if (op is Sub)
                    {
                        return () => op0() - op1();
                    }
                    if (op is Mul)
                    {
                        return () => op0() * op1();
                    }
                    if (op is Div)
                    {
                        return () => op0() / op1();
                    }
                    if (op is DivRem)
                    {
                        return () => op0() % op1();
                    }
                    if (op is Pow)
                    {
                        return () => Math.Pow(op0(), op1());
                    }

                    switch (op.Arity)
                    {
                        case 0:
                            return () => op.Evaluate(0, 0, 0, 0);

                        case 1:
                            return () => op.Evaluate(op0(), 0, 0, 0);

                        case 2:
                            return () => op.Evaluate(op0(), op1(), 0, 0);

                        case 3:
                            return () => op.Evaluate(op0(), op1(), op2(), 0);

                        default:
                            return () => op.Evaluate(op0(), op1(), op2(), op3());
                    }
                });
        }

        #endregion
    }
}
