namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Csc : UnaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Csc(op1);
        }
    }
}
