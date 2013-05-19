using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Cbrt : UnaryOperator
    {
        private const double c1_3 = 1.0/3.0;

        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Math.Pow(Math.Abs(op1), c1_3);
        }
    }
}
