namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class Max : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Max(op1, op2);
        }
    }
}
