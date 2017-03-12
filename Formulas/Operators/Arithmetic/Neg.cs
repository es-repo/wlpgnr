using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Neg : UnaryOperator 
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => -op0();
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => -op0.Value;
        }

        public override Expression Evaluate(Stack<Expression> operands)
        {
            return Expression.Negate(operands.Pop());
        }
    }
}
