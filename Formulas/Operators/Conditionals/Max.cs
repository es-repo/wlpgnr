using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class Max : BinaryOperator
    {
        private static readonly Expression<Func<double, double, double>> _evalExpr = (a, b) => a > b ? a : b;

        public Max() : base (_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1]; 
            return () =>
                {
                    double v0 = op0();
                    double v1 = op1();
                    return v0 > v1 ? v0 : v1;
                };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            return () => op0.Value > op1.Value ? op0.Value : op1.Value;
        }
    }
}
