using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sqrt : UnaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Math.Sqrt(Math.Abs(op1));
        }
    }
}
