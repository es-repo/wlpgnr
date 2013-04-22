using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {
        #region Constants

        public static readonly double C05 = 0.5;
        public static readonly double C1 = 1;
        public static readonly double C2 = 2;
        public static readonly double C3 = 3;
        public static readonly double C5 = 5;
        public static readonly double C7 = 7;
        public static readonly double PI = Math.PI;
        public static readonly double E = Math.E;

        #endregion

        #region Arithmetic functions

        public static double Minus(double a)
        {
            return -a;
        }

        public static double Abs(double a)
        {
            return Math.Abs(a);
        }

        public static double Sum(double a, double b)
        {
            return a + b;
        }

        public static double Mul(double a, double b)
        {
            return a * b;
        }

        #endregion

        #region Trigonometric functions

        public static double Sin(double a)
        {
            return Math.Sin(a);
        }

        public static double Cos(double a)
        {
            return Math.Cos(a);
        }

        #endregion
    }
}
