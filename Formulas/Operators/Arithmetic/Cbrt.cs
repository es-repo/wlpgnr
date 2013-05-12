using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Cbrt : UnaryOperator
    {
        private const double c1_3 = 1.0/3.0;

        protected override double EvaluateCore(params double[] operands)
        {
            return Math.Pow(Math.Abs(operands[0]), c1_3);
        }
    }
}
