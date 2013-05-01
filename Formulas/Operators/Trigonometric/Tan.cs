namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Tan : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Tan(operands[0]);
        }
    }
}
