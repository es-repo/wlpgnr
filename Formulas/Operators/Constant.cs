namespace WallpaperGenerator.Formulas.Operators
{
    public sealed class Constant : ZeroArityOperator
    {
        public double Value { get; private set; }

        public Constant(double value)
        {
            Value = value;
        }

        protected override double EvaluateCore(params double[] operands)
        {
            return Value;
        }
    }
}
