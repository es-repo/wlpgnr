namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class Or : BinaryOperator 
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Or(operands[0], operands[1]);
        }
    }
}
