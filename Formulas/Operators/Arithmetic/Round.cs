using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Round : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Round(operands[0]);
        }
    }
}
