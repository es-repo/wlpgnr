using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Log : BinaryOperator
    {
        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            Func<double> op1 = operands[1];
            return () =>
                {
                    double v0 = op0();
                    double v1 = op1();
                    return Math.Log(v0 >= 0 ? v0 : -v0, v1 >= 0 ? v1 : -v1);
                };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            ZeroArityOperator op1 = operands[1];
            return () => 
            {
                double v0 = op0.Value;
                double v1 = op1.Value;
                return Math.Log(v0 >= 0 ? v0 : -v0, v1 >= 0 ? v1 : -v1);
            };
        }
    }
}
