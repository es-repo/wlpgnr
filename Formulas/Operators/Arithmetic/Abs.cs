using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Abs : UnaryOperator 
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () =>
                {
                    double v = op0();
                    return v > 0 ? v : -v;
                };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () =>
            {
                double v = op0.Value;
                return v > 0 ? v : -v;
            };
        }
    }
}
