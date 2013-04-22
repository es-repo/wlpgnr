namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sin : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Sin(operands[0]);
        }
    }
}
