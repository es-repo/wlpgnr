namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class Conditional : TernaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return operands[0].Equals(0) ? operands[2] : operands[1];
        }
    }
}
