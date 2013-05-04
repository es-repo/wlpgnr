namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Csc : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Csc(operands[0]);
        }
    }
}
