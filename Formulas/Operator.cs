using System;

namespace WallpaperGenerator.Formulas
{
    public abstract class Operator
    {
        public int Arity { get; private set; }

        protected Operator(int arity)
        {
            Arity = arity;
        }

        public double Evaluate(params double[] operands)
        {
            if (operands.Length != Arity)
                throw new ArgumentException();

            return EvaluateCore(operands);
        }

        protected abstract double EvaluateCore(params double[] operands);
    }
}
