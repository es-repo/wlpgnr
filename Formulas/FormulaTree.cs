using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTree : Tree<Operator>
    {
        #region Fields

        private Variable[] _variables;

        #endregion

        #region Properties

        public FormulaTreeNode FormulaRoot
        {
            get { return (FormulaTreeNode) Root; }
        }

        public Variable[] Variables
        {
            get
            {
                return _variables ?? (_variables = SelectVariables(FormulaRoot).ToArray());  
            }
        }

        #endregion

        #region Constructors

        public FormulaTree(FormulaTreeNode root)
            : base(root)
        {
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

        public IEnumerable<double> EvaluateRangesIn2DProjection(params Range[] variableValueRanges)
        {
            IEnumerable<double>[] series = variableValueRanges.Select(r => r.Values).ToArray();
            return EvaluateSeriesIn2DProjection(series);
        }

        public IEnumerable<double> EvaluateSeriesIn2DProjection(params IEnumerable<double>[] variableValues)
        {
            IEnumerator<double>[] xVariableValuesEnumerators = variableValues.Where((vv, i) => i % 2 == 0).Select(vv => vv.GetEnumerator()).ToArray();
            while (xVariableValuesEnumerators[0].MoveNext())
            {
                for (int i = 0; i < xVariableValuesEnumerators.Length; i++)
                {
                    if (i != 0)
                    {
                        xVariableValuesEnumerators[i].MoveNext();
                    }
                    Variables[i * 2].Value = xVariableValuesEnumerators[i].Current;
                }

                if (variableValues.Length > 1)
                {
                    IEnumerator<double>[] yVariableValuesEnumerators = variableValues.Where((vv, i) => i % 2 == 1).Select(vv => vv.GetEnumerator()).ToArray();
                    while (yVariableValuesEnumerators[0].MoveNext())
                    {
                        for (int i = 0; i < yVariableValuesEnumerators.Length; i++)
                        {
                            if (i != 0)
                            {
                                yVariableValuesEnumerators[i].MoveNext();
                            }
                            Variables[i * 2 + 1].Value = yVariableValuesEnumerators[i].Current;
                        }

                        yield return Evaluate();
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

        public double Evaluate(params double[] variableValues)
        {
            for (int i = 0; i < variableValues.Length; i++)
            {
                if (i < Variables.Length)
                {
                    Variables[i].Value = variableValues[i];
                }
                else 
                    break;
            }
            return EvaluateCore();
        }

        private double EvaluateCore()
        {
            return Fold((TraversedTreeNodeInfo<Operator> ni, double[] c) => ni.Node.Value.Evaluate(c));
        }

        #endregion
    }
}
