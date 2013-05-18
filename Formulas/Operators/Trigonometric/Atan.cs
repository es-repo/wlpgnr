using System;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Atan : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Atan(operands[0]);
        }
    }
}
