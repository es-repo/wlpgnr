using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Log2 : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Log(operands[0], 2);
        }
    }
}
