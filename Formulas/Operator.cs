using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas
{
    public abstract class Operator
    {
        public int Arity { get; private set; }

        protected Operator(int arity)
        {
            Arity = arity;
        }

        public double Evaluate(IEnumerable<double> operands)
        {
            return EvaluateCore(operands.ToArray());
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
