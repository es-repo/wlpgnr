namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sub : BinaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return op1 - op2;
        }
    }
}
