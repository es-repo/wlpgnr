namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class Xor : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Xor(operands[0], operands[1]);
        }
    }
}
