namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Minus : UnaryOperator, IArithmeticOperator
    {
        protected override double CalculateCore(params Operand[] operands)
        {
            double a = operands[0].Value;
            return -a;
        }
    }
}
