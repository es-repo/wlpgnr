﻿using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Abs : UnaryOperator 
    {
        private static readonly Expression<Func<double, double>> _evalExpr = a => a >=0 ? a : -a;

        public Abs() : base(_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () =>
                {
                    double v = op0();
                    return v > 0 ? v : -v;
                };
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () =>
            {
                double v = op0.Value;
                return v > 0 ? v : -v;
            };
        }
    }
}
