﻿namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Div : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Div(operands[0], operands[1]);
        }
    }
}