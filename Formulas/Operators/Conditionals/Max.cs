namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class Max : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Max(operands[0], operands[1]);
        }
    }
}
