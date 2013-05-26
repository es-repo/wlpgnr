using System;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sin : UnaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0]; 
            return () => Math.Sin(op0());
            //return () => MathLibrary.FastSin(op0());
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Sin(op0.Value);
            //return () => MathLibrary.FastSin(op0.Value);
        }
    }
}
