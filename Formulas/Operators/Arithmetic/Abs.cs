namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Abs : UnaryOperator 
    {
        protected override double EvaluateCore(params double[] operands)
        {
            return MathLibrary.Abs(operands[0]) ;
        }
    }
}
