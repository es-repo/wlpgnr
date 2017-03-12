using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sin : UnaryOperator
    {
        private readonly Expression<Func<double, double>> _evalExpr = a => Math.Sin(a);

        public Sin()// : base(_evalExpr)
        {
            EvalExpr = _evalExpr;
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0]; 
            return () => Math.Sin(op0());
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Sin(op0.Value);
        }
    }
}
