namespace WallpaperGenerator.Formulas.Operators.Bitwise
{
    public class Xor : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Xor(op1, op2);
        }
    }
}
