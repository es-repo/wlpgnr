using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Abs : UnaryOperator 
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Math.Abs(op1);
        }
    }
}
