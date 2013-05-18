using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {        
        #region Arithmetic functions
        
        public static double Pow(double a, double b)
        {
            if (b < 1)
                a = Math.Abs(a);
            return Math.Pow(a, b);
        }

        #endregion

        #region Trigonometric functions

        public static double Sin(double a)
        {
            return Math.Sin(a);
        }

        public static double Sec(double a)
        {
            return 1 / Math.Cos(a);
        }

        public static double Cos(double a)
        {
            return Math.Cos(a);
        }

        public static double Csc(double a)
        {
            return 1/ Math.Sin(a);
        }

        public static double Tan(double a)
        {
            return Math.Tan(a);
        }

        public static double Atan(double a)
        {
            return Math.Atan(a);
        }

        public static double Sinh(double a)
        {
            return Math.Sinh(a);
        }

        public static double Cosh(double a)
        {
            return Math.Cosh(a);
        }

        public static double Tanh(double a)
        {
            return Math.Tanh(a);
        }

        #endregion

        #region Bitwise

        public static double And(double a, double b)
        {
            return (long)a & (long)b;
        }

        public static double Or(double a, double b)
        {
            return (long)a | (long)b;
        }

        public static double Xor(double a, double b)
        {
            return (long)a ^ (long)b;
        }

        #endregion

        #region Conditionals

        public static double Max(double a, double b)
        {
            return Math.Max(a, b);
        }

        #endregion
    }
}
