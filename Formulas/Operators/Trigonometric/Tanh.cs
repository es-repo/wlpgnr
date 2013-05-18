using System;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Tanh : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Tanh(operands[0]);
        }
    }
}
