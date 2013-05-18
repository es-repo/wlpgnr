using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Abs : UnaryOperator 
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Abs(operands[0]);
        }
    }
}
