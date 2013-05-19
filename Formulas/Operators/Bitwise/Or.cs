namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class Or : BinaryOperator 
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Or(op1, op2);
        }
    }
}
