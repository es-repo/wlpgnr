using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow2 : UnaryOperator 
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () =>
            {
                double v0 = op0();
                return v0*v0;
            };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => op0.Value * op0.Value;
        }
    }
}
