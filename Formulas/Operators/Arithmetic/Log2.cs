namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Log2 : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Log(operands[0], 2);
        }
    }
}
