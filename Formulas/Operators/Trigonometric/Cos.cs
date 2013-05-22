using System;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Cos : UnaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => Math.Cos(op0());
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Cos(op0.Value);
        }
    }
}
