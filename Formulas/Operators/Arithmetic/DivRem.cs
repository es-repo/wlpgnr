namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class DivRem : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.DivRem(operands[0], operands[1]);
        }
    }
}
