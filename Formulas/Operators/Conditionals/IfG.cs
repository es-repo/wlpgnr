using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class IfG : QuaternaryOperator
    {
        private static readonly Expression<Func<double, double, double, double, double>> _evalExpr = (a, b, c, d) => a > b ? c : d;

        public IfG() : base (_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1];
            Func<double> op2 = operands[2];
            Func<double> op3 = operands[3];
            return () => op0() > op1() ? op2() : op3();
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            ZeroArityOperator op2 = operands[2];
            ZeroArityOperator op3 = operands[3];
            return () => op0.Value > op1.Value ? op2.Value : op3.Value;
        }
    }
}
