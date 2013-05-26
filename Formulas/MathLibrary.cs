using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {
        private const double TwoPi = 2 * Math.PI;

        private static double[] _precalculatedSinuses;
 
        public static void Init(int precalculatedSinusesCount)
        {
            _precalculatedSinuses = new double[precalculatedSinusesCount];
            double value = 0;
            double step = TwoPi / precalculatedSinusesCount;
            for (int i = 0; i < precalculatedSinusesCount; i++)
            {
                _precalculatedSinuses[i] = Math.Sin(value);
                value += step;
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
            if (value >= 0)
            {
                while (value > TwoPi)
                    value -= TwoPi;
            }
            else
            {
                while (value < 0)
                    value += TwoPi;
            }
            int i = (int)((value / TwoPi) * _precalculatedSinuses.Length);
            return _precalculatedSinuses[i];
        }
    }
}
