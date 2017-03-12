using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow3 : UnaryOperator 
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () =>
            {
                double v0 = op0();
                return v0*v0*v0;
            };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => op0.Value * op0.Value * op0.Value;
        }

        public override Expression Evaluate(Stack<Expression> operands)
        {
            Expression a = operands.Pop();
            return Expression.Multiply(Expression.Multiply(a, a), a);
        }
    }
}
