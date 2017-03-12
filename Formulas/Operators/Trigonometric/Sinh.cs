using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sinh : UnaryOperator
    {
        private static readonly Expression<Func<double, double>> _evalExpr = a => Math.Sinh(a);

        public Sinh() : base(_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => Math.Sinh(op0());
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Sinh(op0.Value);
        }
    }
}
