namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class IfG0 : TernaryOperator
    {
        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return op1 > 0 ? op3 : op2;
        }
    }
}
