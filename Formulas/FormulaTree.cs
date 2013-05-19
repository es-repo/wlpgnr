﻿using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
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

        public IEnumerable<double> EvaluateRangesIn2DProjection(params Range[] variableValueRanges)
        {
            IEnumerable<double>[] series = variableValueRanges.Select(r => r.Values).ToArray();
            return EvaluateSeriesIn2DProjection(series);
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
            return Fold((TraversedTreeNodeInfo<Operator> ni, Func<double>[] operands)
                => () =>
                    {                        
                        if (operands.Length == 0)
                        {
                            return ni.Node.Value.Evaluate(0, 0, 0, 0);
                        }
                        if (operands.Length == 1)
                        {
                            return ni.Node.Value.Evaluate(operands[0](), 0, 0, 0);
                        }
                        if (operands.Length == 2)
                        {
                            return ni.Node.Value.Evaluate(operands[0](), operands[1](), 0, 0);
                        }
                        if (operands.Length == 3)
                        {
                            return ni.Node.Value.Evaluate(operands[0](), operands[1](), operands[2](), 0);
                        }
                        
                        return ni.Node.Value.Evaluate(operands[0](), operands[1](), operands[2](), operands[3]());
                    });
        }

        #endregion
    }
}
