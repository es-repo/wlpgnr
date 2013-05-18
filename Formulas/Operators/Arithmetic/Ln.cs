using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Ln : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Log(operands[0], Math.E);
        }
    }
}
