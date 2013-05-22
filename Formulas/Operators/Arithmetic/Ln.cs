using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Ln : UnaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => Math.Log(op0(), Math.E);
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Log(op0.Value, Math.E);
        }
    }
}
