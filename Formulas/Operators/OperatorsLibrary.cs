using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Conditionals;
using WallpaperGenerator.Formulas.Operators.Trigonometric;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {                
        #region Arithmetic Operators

        public static readonly Minus Minus = new Minus();
        public static readonly Abs Abs = new Abs();
        public static readonly Sum Sum = new Sum();
        public static readonly Sub Sub = new Sub();
        public static readonly Mul Mul = new Mul();
        public static readonly Div Div = new Div();
        public static readonly Mod Mod = new Mod();
        public static readonly Pow Pow = new Pow();
        public static readonly Pow2 Pow2 = new Pow2();
        public static readonly Pow3 Pow3= new Pow3();
        public static readonly Sqrt Sqrt = new Sqrt();
        public static readonly Cbrt Cbrt = new Cbrt();
        public static readonly Ln Ln = new Ln();

        public static IEnumerable<Operator> Arithmetic
        {
            get
            {
                yield return Abs;
                yield return Sum;
                yield return Sub;
                yield return Mul;
                yield return Div;
                yield return Mod;
                yield return Pow;
                yield return Pow2;
                yield return Pow3;
                yield return Sqrt;
                yield return Cbrt;
                yield return Ln;
            }
        }

        #endregion

        #region Trigonometric Operators

        public static readonly Sin Sin = new Sin();
        public static readonly Cos Cos = new Cos();
        public static readonly Tan Tan = new Tan();
        public static readonly Atan Atan = new Atan();
        public static readonly Sinh Sinh = new Sinh();
        public static readonly Cosh Cosh = new Cosh();
        public static readonly Tanh Tanh = new Tanh();
        
        public static IEnumerable<Operator> Trigonometric
        {
            get
            {
                yield return Sin;
                yield return Cos;
                yield return Tan;
                yield return Atan;
                yield return Sinh;
                yield return Cosh;
                yield return Tanh;
            }
        }

        #endregion

        #region IfG0 Operators

        public static readonly IfG0 IfG0 = new IfG0();
        public static readonly Max Max = new Max();

        public static IEnumerable<Operator> Conditionals
        {
            get
            {
                yield return IfG0;
                yield return Max;
            }
        }

        #endregion

        public static IEnumerable<Operator> All
        {
            get
            {
                return AllByCategories.SelectMany(e => e.Value);
            }
        }

        public static IEnumerable<KeyValuePair<string, IEnumerable<Operator>>> AllByCategories
        {
            get
            {
                yield return new KeyValuePair<string, IEnumerable<Operator>>("Arithmetic", Arithmetic);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("Trigonometric", Trigonometric);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("Conditionals", Conditionals);
            }
        }
    }
}
