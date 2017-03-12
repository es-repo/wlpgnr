using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Sum : BinaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1];
            return () => op0() + op1();
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            return () => op0.Value + op1.Value;
        }

        public override Expression Evaluate(Stack<Expression> operands)
        {
            Expression op1 = operands.Pop();
            return Expression.Add(operands.Pop(), op1);
        }
    }
}
