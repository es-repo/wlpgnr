namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Minus : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Minus(operands[0]);
        }
    }
}
