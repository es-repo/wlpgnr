namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class And : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.And(op1, op2);
        }
    }
}
