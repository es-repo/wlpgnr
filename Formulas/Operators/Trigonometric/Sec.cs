namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Sec : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Sec(operands[0]);
        }
    }
}
