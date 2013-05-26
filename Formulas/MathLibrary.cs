using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {
        private const double TwoPi = 2 * Math.PI;

        private static double _precalculatedSinusesStep;
        private static double[] _precalculatedSinuses;
 
        public static void Init(int precalculatedSinusesCount)
        {
            _precalculatedSinuses = new double[precalculatedSinusesCount];
            double value = 0;
            _precalculatedSinusesStep = TwoPi / precalculatedSinusesCount;
            for (int i = 0; i < precalculatedSinusesCount; i++)
            {
                _precalculatedSinuses[i] = Math.Sin(value);
                value += _precalculatedSinusesStep;
            }
        }

        public static double Pow(double a, double b)
        {
            if (b < 0)
                b = -b;
            return Math.Pow(b >= 1 ? a : a >= 0 ? a : -a, b);
        }

        public static double FastSin(double value)
        {
            if (double.IsNaN(value))
                return double.NaN; 

            value = value % TwoPi; 
            if (value < 0)
            {
                value = TwoPi + value;
            }
            int i = (int)(value / _precalculatedSinusesStep);
            if (i == _precalculatedSinuses.Length)
                i--; 
            return _precalculatedSinuses[i];
        }
    }
}
