using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas
{
    public abstract class Operator
    {
        protected Expression EvalExpr;

        public int Arity { get; private set; }

        public string Name { get; private set; }

        protected Operator(int arity, Expression evalExpr = null)
            : this(arity, null, evalExpr)
        {
        }

        protected Operator(int arity, string name, Expression evalExpr = null)
        {
            Arity = arity;
            Name = name ?? GetType().Name;

            EvalExpr = evalExpr;
        }

        public abstract Func<double> Evaluate(params ZeroArityOperator[] operands);

        public abstract Func<double> Evaluate(params Func<double>[] operands);

        public virtual Expression Evaluate(Stack<Expression> operands)
        {
            if (EvalExpr == null)
                throw new InvalidOperationException("Evaluating expression is not defined or \"Evaluate\" method is not overloaded.");

            if (Arity == 0)
                return EvalExpr;

            var ops = new Expression[Arity];
            for (int i = Arity - 1; i >= 0; i--)
                ops[i] = operands.Pop();

            return Expression.Invoke(EvalExpr, ops);
        }
    }
}