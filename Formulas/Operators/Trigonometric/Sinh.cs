namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sinh : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Sinh(operands[0]);
        }
    }
}
