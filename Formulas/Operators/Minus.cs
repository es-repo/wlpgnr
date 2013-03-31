namespace WallpaperGenerator.Formulas.Operators
{
    public class Minus : UnaryOperator, IArithmeticOperation
    {
        protected override double CalculateCore(params Operand[] operands)
        {
            double a = operands[0].Value;
            return -a;
        }
    }
}
