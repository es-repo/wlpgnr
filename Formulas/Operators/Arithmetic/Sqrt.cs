using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sqrt : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Sqrt(Math.Abs(operands[0]));
        }
    }
}
