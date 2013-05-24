using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Ln : UnaryOperator
    {
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
