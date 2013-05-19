using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Log : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Math.Log(op1, op2);
        }
    }
}
