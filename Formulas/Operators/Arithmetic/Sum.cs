namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sum : BinaryOperator, IArithmeticOperator
    {
        protected override double CalculateCore(params Operand[] operands)
        {
            double a = operands[0].Value;
            double b = operands[1].Value; 
            return a + b;
        }
    }
}
