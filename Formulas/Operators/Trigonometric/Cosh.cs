namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Cosh : UnaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return MathLibrary.Cosh(op1);
        }
    }
}
