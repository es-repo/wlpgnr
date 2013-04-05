namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Mul : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Mul(operands[0], operands[1]);
        }
    }
}
