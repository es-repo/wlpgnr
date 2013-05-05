namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class IfG0 : TernaryOperator
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return operands[0] > 0 ? operands[2] : operands[1];
        }
    }
}
