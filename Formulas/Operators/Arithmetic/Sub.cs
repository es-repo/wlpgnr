namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sub : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Sub(operands[0], operands[1]);
        }
    }
}
