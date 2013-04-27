namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Log : BinaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Log(operands[0], operands[1]);
        }
    }
}
