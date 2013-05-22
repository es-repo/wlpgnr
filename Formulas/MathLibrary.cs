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
    }
}
