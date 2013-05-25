using System;

namespace WallpaperGenerator.Formulas
{
    public static class MathLibrary
    {        
        #region Arithmetic functions
        
        public static double Pow(double a, double b)
        {
            if (b < 0)
                b = -b;
            return Math.Pow(b >= 1 ? a : a >= 0 ? a : -a, b);
        }

        #endregion
    }
}
