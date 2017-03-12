using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Ln : UnaryOperator
    {
        private static readonly Expression<Func<double, double>> _evalExpr = v => Math.Log(v >= 0 ? v : -v, Math.E);

        public Ln() : base (_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () =>
            {
                double v = op0();
                return Math.Log(v >= 0 ? v : -v, Math.E);
            };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => 
            {
                double v = op0.Value;
                return Math.Log(v >= 0 ? v : -v, Math.E);
            };
        }
    }
}
