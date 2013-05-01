namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow1_2 : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Pow(operands[0], 0.5);
        }
    }
}
