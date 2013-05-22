namespace WallpaperGenerator.Formulas.Operators
{
    public abstract class ZeroArityOperator : Operator
    {
        public double Value { get; set; }

        protected ZeroArityOperator(string name)
            : base(0, name)
        {
        }
    }
}