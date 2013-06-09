﻿using System;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas
{
    public abstract class Operator
    {
        public int Arity { get; private set; }

        public string Name { get; private set; }

        protected Operator(int arity)
            : this(arity, null)
        {
        }

        protected Operator(int arity, string name)
        {
            Arity = arity;
            Name = name ?? GetType().Name;
        }

        public abstract Func<double> Evaluate(params ZeroArityOperator[] operands);

        public abstract Func<double> Evaluate(params Func<double>[] operands);

        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return false;
        //    }

        //    return obj.GetType() == GetType();
        //}

        //public override int GetHashCode()
        //{
        //    return GetType().GetHashCode();
        //}
    }
}
