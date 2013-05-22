using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Minus : UnaryOperator 
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => -op0();
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => -op0.Value;
        }
    }
}
