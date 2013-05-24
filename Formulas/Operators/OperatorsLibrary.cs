using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Conditionals;
using WallpaperGenerator.Formulas.Operators.Trigonometric;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {
        //#region Constants

        //public static readonly Operator C1_3 = new Constant(MathLibrary.C1_3, "1_3");
        //public static readonly Operator C05 = new Constant(MathLibrary.C05, "1_2");
        //public static readonly Operator C1 = new Constant(MathLibrary.C1);
        //public static readonly Operator C2 = new Constant(MathLibrary.C2);
        //public static readonly Operator C3 = new Constant(MathLibrary.C3);
        //public static readonly Operator C5 = new Constant(MathLibrary.C5);
        //public static readonly Operator C7 = new Constant(MathLibrary.C7);
        //public static readonly Operator C11 = new Constant(MathLibrary.C11);
        //public static readonly Operator Pi = new Constant(MathLibrary.PI, "Pi");
        //public static readonly Operator E = new Constant(MathLibrary.E, "E");
        //public static readonly Operator Sqrt2 = new Constant(MathLibrary.Sqrt2, "Sqrt2");
        //public static readonly Operator Sqrt3 = new Constant(MathLibrary.Sqrt3, "Sqrt3");
        //public static readonly Operator AperysC = new Constant(MathLibrary.AperysC, "Zita");
        //public static readonly Operator GoldenRatio = new Constant(MathLibrary.GoldenRatio, "Fi");
        //public static readonly Operator EulerMascheroniC = new Constant(MathLibrary.EulerMascheroniC, "Gamma");
        //public static readonly Operator KhinchinsC = new Constant(MathLibrary.KhinchinsC, "K");

        //public static IEnumerable<Operator> Constants
        //{
        //    get
        //    {
        //        yield return C1_3;
        //        yield return C05;
        //        yield return C1;
        //        yield return C2;
        //        yield return C3;
        //        yield return C5;
        //        yield return C7;
        //        yield return C11;
        //        yield return Pi;
        //        yield return E;
        //        yield return Sqrt2;
        //        yield return Sqrt3;
        //        yield return AperysC;
        //        yield return GoldenRatio;
        //        yield return EulerMascheroniC;
        //        yield return KhinchinsC;
        //    }
        //}

        //#endregion
        
        #region Arithmetic Operators

        public static readonly Operator Minus = new Minus();
        public static readonly Operator Abs = new Abs();
        public static readonly Operator Sum = new Sum();
        public static readonly Operator Sub = new Sub();
        public static readonly Operator Mul = new Mul();
        public static readonly Operator Div = new Div();
        public static readonly Operator DivRem = new DivRem();
        public static readonly Operator Pow = new Pow();
        public static readonly Operator Sqrt = new Sqrt();
        public static readonly Operator Cbrt = new Cbrt();
        public static readonly Operator Log = new Log();
        public static readonly Operator Ln = new Ln();

        public static IEnumerable<Operator> Arithmetic
        {
            get
            {
                yield return Abs;
                yield return Sum;
                yield return Sub;
                yield return Mul;
                yield return Div;
                yield return DivRem;
                yield return Pow;
                yield return Sqrt;
                yield return Cbrt;
                yield return Log;
                yield return Ln;
            }
        }

        #endregion

        #region Trigonometric Operators
        
        public static readonly Operator Sin = new Sin();
        public static readonly Operator Cos = new Cos();
        public static readonly Operator Tan = new Tan();
        public static readonly Operator Atan = new Atan();
        public static readonly Operator Sinh = new Sinh();
        public static readonly Operator Cosh = new Cosh();
        public static readonly Operator Tanh = new Tanh();
        
        public static IEnumerable<Operator> Trigonometric
        {
            get
            {
                yield return Sin;
                //yield return Cos;
                //yield return Tan;
                yield return Atan;
                //yield return Sinh;
                //yield return Cosh;
                yield return Tanh;
            }
        }

        #endregion

        #region Conditional Operators

        public static readonly Operator Conditional = new IfG0();
        public static readonly Operator Max = new Max();

        public static IEnumerable<Operator> Conditionals
        {
            get
            {
                yield return Conditional;
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
