﻿using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators.Trigonometric
{
    public class Cosh : UnaryOperator
    {
        private static readonly Expression<Func<double, double>> _evalExpr = a => Math.Cosh(a);

        public Cosh() : base(_evalExpr)
        {
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => Math.Cosh(op0());
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Cosh(op0.Value);
        }
    }
}
