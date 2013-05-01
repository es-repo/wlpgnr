namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Cosh : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Cosh(operands[0]);
        }
    }
}
