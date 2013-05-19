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

        public abstract double Evaluate(double op1, double op2, double op3, double op4);
    }
}
