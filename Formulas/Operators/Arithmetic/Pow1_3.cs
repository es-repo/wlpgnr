namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow1_3 : UnaryOperator
    {
        private const double c1_3 = 1.0/3.0;

        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Pow(operands[0], c1_3);
        }
    }
}
