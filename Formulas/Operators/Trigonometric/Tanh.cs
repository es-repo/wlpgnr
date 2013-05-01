namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Tanh : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Tanh(operands[0]);
        }
    }
}
