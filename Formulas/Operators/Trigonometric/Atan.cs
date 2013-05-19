using System;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Atan : UnaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Math.Atan(op1);
        }
    }
}
