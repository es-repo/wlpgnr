namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Pow(op1, op2);
        }
    }
}
