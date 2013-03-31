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

        public double Calculate(params Operand[] operands)
        {
            if (operands.Length != Arity)
                throw new ArgumentException();

            return CalculateCore(operands);
        }

        protected abstract double CalculateCore(params Operand[] operands);
    }
}
