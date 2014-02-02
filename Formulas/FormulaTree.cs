using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Formulas
{
    public class FormulaTree : Tree<Operator>
    {
        #region Fields

        private readonly Func<double> _compiledFormula;

        #endregion

        #region Properties

        public Variable[] Variables { get; private set; }

        #endregion

        #region Constructors

        public FormulaTree(TreeNode<Operator> root)
            : base(root)
        {
            Variables = SelectVariables(Root).ToArray();
            _compiledFormula = CompileFormula();
        }

        #endregion

        #region  Methods

        public static bool Equal(FormulaTree a, FormulaTree b)
        {
            return Equal(a.Root, b.Root, OperatorsEqual);
        }

        private static bool OperatorsEqual(Operator opA, Operator opB)
        {
            if (opA is Constant && opB is Constant)
            {
                return ((Constant)opA).Value.Equals(((Constant)opB).Value);
            }

            return opA.Name == opB.Name;
        }

        public static IEnumerable<Variable> SelectVariables(TreeNode<Operator> node)
        {
            return Traverse(node, TraversalOrder.BredthFirstPreOrder)
                       .Where(ni => ni.Node.Value is Variable)
                       .Select(ni => (Variable) ni.Node.Value)
                       .Distinct();
        }

        public void EvaluateRangesIn2DProjection(Range[] ranges, int xCount, int yStart, int yCount, float[] resultBuffer)
        {
            for (int i = 0; i < Variables.Length; i += 2)
            {
                Variables[i].Value = ranges[i].Start + ranges[i].Step * yStart;
            }

            int r = xCount * yStart;
            using (ProgressReporter.CreateScope(yCount))
            for (int y = yStart; y < yCount + yStart; y++)
            {
                for (int i = 1; i < Variables.Length; i += 2)
                {
                    Variables[i].Value = ranges[i].Start;
                }

                for (int x = 0; x < xCount; x++)
                {
                    resultBuffer[r++] = (float)Evaluate();

                    for (int i = 1; i < Variables.Length; i += 2)
                    {
                        Variables[i].Value += ranges[i].Step;
                    }
                }

                for (int i = 0; i < Variables.Length; i += 2)
                {
                    Variables[i].Value += ranges[i].Step;
                }
                ProgressReporter.Increase();
            }
        }

        public double Evaluate()
        {
            return _compiledFormula();
        }

        //public double Evaluate()
        //{
        //    return Fold((TreeNodeInfo<Operator> ni, double[] c) => ni.Node.Value.Evaluate(c));
        //}

        public Func<double> CompileFormula()
        {
            return Fold(
                (TreeNodeInfo<Operator> ni, Func<double>[] operands) =>
                    {
                        Operator op = ni.Node.Value;
                        return GetNodeHeight(ni.Node) == 2 
                            ? op.Evaluate(ni.Node.Children.Select(n => (ZeroArityOperator)n.Value).ToArray()) 
                            : op.Evaluate(operands);
                    });
        }

        public static FormulaTree Build(IEnumerable<Operator> operators, TraversalOrder nodeValuesOrder = TraversalOrder.DepthFirstPreOrder)
        {
            TreeNode<Operator> root = Build(operators, op => op.Arity, nodeValuesOrder);
            return new FormulaTree(root);
        }

        #endregion
    }
}
