using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Pow : BinaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1];
            return () =>
            {
                double v = op0();
                return Math.Pow(v >= 0 ? v : -v, op1());
            };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            return () =>
            {
                double v = op0.Value;
                return Math.Pow(v >= 0 ? v : -v, op1.Value);
            };
        }
    }
}
