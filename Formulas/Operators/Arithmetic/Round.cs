namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Round : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Round(operands[0]);
        }
    }
}
