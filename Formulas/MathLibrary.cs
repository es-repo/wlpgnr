using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {
        #region Constants

        public static readonly double C1_3 = 1.0/3.0;
        public static readonly double C05 = 0.5;
        public static readonly double C1 = 1;
        public static readonly double C2 = 2;
        public static readonly double C3 = 3;
        public static readonly double C5 = 5;
        public static readonly double C7 = 7;
        public static readonly double C11 = 11;
        public static readonly double PI = Math.PI;
        public static readonly double E = Math.E;
        public static readonly double Sqrt2 = Math.Sqrt(2);
        public static readonly double Sqrt3 = Math.Sqrt(3);
        public static readonly double AperysC = 1.202056903159594;
        public static readonly double GoldenRatio = 1.61803398874;
        public static readonly double EulerMascheroniC = 0.57721;
        public static readonly double KhinchinsC = 2.6854520010;

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

        public static double Sub(double a, double b)
        {
            return a - b;
        }

        public static double Mul(double a, double b)
        {
            return a * b;
        }

        public static double Div(double a, double b)
        {
            return a / b;
        }

        public static double DivRem(double a, double b)
        {
            return a % b;
        }

        public static double Pow(double a, double b)
        {
            if ((1/ b % 2).Equals(0))
                a = Math.Abs(a);
            return Math.Pow(a, b);
        }

        public static double Log(double a, double b)
        {
            return Math.Log(Math.Abs(a), b);
        }

        public static double Round(double a)
        {
            return Math.Round(a);
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
