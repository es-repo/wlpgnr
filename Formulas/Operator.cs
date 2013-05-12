namespace WallpaperGenerator.Formulas
{
    public abstract class Operator
    {
        public int Arity { get; private set; }

        public string Name { get; private set; }

        protected Operator(int arity)
            : this(arity, null)
        {
        }

        protected Operator(int arity, string name)
        {
            Arity = arity;
            Name = name ?? GetType().Name;
        }

        public double Evaluate(params double[] operands)
        {            
            return EvaluateCore(operands);
        }

        protected abstract double EvaluateCore(params double[] operands);
    }
}
