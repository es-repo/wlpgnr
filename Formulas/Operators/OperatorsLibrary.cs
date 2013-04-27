using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Trigonometric;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {
        #region Constants

        #region Base Constants

        public static readonly Operator C05 = new Constant(MathLibrary.C05);
        public static readonly Operator C1 = new Constant(MathLibrary.C1);
        public static readonly Operator C2 = new Constant(MathLibrary.C2);
        public static readonly Operator C3 = new Constant(MathLibrary.C3);
        public static readonly Operator C5 = new Constant(MathLibrary.C5);
        public static readonly Operator C7 = new Constant(MathLibrary.C7);
        public static readonly Operator Pi = new Constant(MathLibrary.PI, "Pi");
        public static readonly Operator E = new Constant(MathLibrary.E, "e");

        public static IEnumerable<Operator> ConstantsBase
        {
            get
            {
                yield return C05;
                yield return C1;
                yield return C2;
                yield return C3;
                yield return C5;
                yield return Pi;
                yield return E;
            }
        }

        #endregion

        #region Extra Constants

        public static IEnumerable<Operator> ConstantsExtra
        {
            get
            {
                return Enumerable.Empty<Operator>();
            }
        }

        #endregion

        public static IEnumerable<Operator> Constants
        {
            get
            {
                return ConstantsBase.Concat(ConstantsExtra);
            }
        }

        #endregion
        
        #region Arithmetic Operators

        #region Base Arithmetic Operators

        public static readonly Operator Minus = new Minus();
        public static readonly Operator Abs = new Abs();
        public static readonly Operator Sum = new Sum();
        public static readonly Operator Mul = new Mul();
        public static readonly Operator Div = new Div();
        public static readonly Operator DivRem = new DivRem();

        public static IEnumerable<Operator> ArithmeticBase
        {
            get
            {
                yield return Minus;
                yield return Abs;
                yield return Sum;
                yield return Mul;
                yield return Div;
                yield return DivRem;
            }
        }

        #endregion

        #region Extra Arithmetic Operators

        public static readonly Operator Pow = new Pow();
        public static readonly Operator Log = new Log();

        public static IEnumerable<Operator> ArithmeticExtra
        {
            get
            {
                yield return Pow;
                yield return Log;
            }
        }

        #endregion

        public static IEnumerable<Operator> Arithmetic
        {
            get
            {
                return ArithmeticBase.Concat(ArithmeticExtra);
            }
        }

        #endregion

        #region Trigonometric Operators

        #region Base Trigonometric Operators

        public static readonly Operator Sin = new Sin();
        public static readonly Operator Cos = new Cos();
        
        public static IEnumerable<Operator> TrigonometricBase
        {
            get
            {
                yield return Sin;
                yield return Cos;
            }
        }

        #endregion

        #region Extra Trigonometric Operators

        public static IEnumerable<Operator> TrigonometricExtra
        {
            get
            {
                return Enumerable.Empty<Operator>();
            }
        }

        #endregion

        public static IEnumerable<Operator> Trigonometric
        {
            get
            {
                return TrigonometricBase.Concat(TrigonometricExtra);
            }
        }

        #endregion

        #region Conditional Operators

        public static readonly Operator Conditional = new Conditional();

        public static IEnumerable<Operator> Conditionals
        {
            get
            {
                yield return Conditional;
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
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ConstantsBase", ConstantsBase);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ConstantsExtra", ConstantsExtra);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ArithmeticBase", ArithmeticBase);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ArithmeticExtra", ArithmeticExtra);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("TrigonometricBase", TrigonometricBase);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("TrigonometricExtra", TrigonometricExtra);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("Conditionals", Conditionals);
            }
        }
    }
}
