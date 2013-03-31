namespace WallpaperGenerator.Formulas.Operators
{
    public class Sum : BinaryOperator, IArithmeticOperation
    {
        protected override double CalculateCore(params Operand[] operands)
        {
            double a = operands[0].Value;
            double b = operands[1].Value; 
            return a + b;
        }
    }
}
