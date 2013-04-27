namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Pow(operands[0], operands[1]);
        }
    }
}
