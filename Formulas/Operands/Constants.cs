using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Formulas.Operands
{
    public static class Constants
    {
        public static readonly Constant C05 = new Constant(0.5);
        public static readonly Constant C2 = new Constant(2);
        public static readonly Constant C3 = new Constant(3);
        public static readonly Constant C5 = new Constant(5);
        public static readonly Constant C7 = new Constant(7);
        public static readonly Constant Pi = new Constant(Math.PI);
        public static readonly Constant E = new Constant(Math.E);

        public static IEnumerable<Constant> All 
        {
            get
            {
                return Base;
            }
        }

        public static IEnumerable<Constant> Base
        {
            get
            {
                yield return C05;
                yield return C2;
                yield return C3;
                yield return C5;
                yield return Pi;
                yield return E;
            }
        }
    }
}
