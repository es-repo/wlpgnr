namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Cos : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Cos(operands[0]);
        }
    }
}
