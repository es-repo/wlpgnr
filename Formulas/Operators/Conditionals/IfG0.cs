using System;

namespace WallpaperGenerator.Formulas.Operators.Conditionals
{
    public class IfG0 : TernaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1];
            Func<double> op2 = operands[2];
            return () => op0() > 0 ? op1() : op2();
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            ZeroArityOperator op2 = operands[2];
            return () => op0.Value > 0 ? op1.Value : op2.Value;
        }
    }
}
