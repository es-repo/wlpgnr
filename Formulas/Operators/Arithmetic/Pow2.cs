namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow3 : UnaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return operands[0] * operands[0] * operands[0];
        }
    }
}
