using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class LogE : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Log(operands[0], Math.E);
        }
    }
}
