namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class And : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.And(operands[0], operands[1]);
        }
    }
}
