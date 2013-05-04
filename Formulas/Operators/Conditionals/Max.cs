namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Max : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Max(operands[0], operands[1]);
        }
    }
}
