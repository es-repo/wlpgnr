using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Trigonometric;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {        
        #region Arithmetic Operators

        #region Base Arithmetic Operators

        public static readonly Operator Minus = new Minus();
        public static readonly Operator Abs = new Abs();
        public static readonly Operator Sum = new Sum();
        public static readonly Operator Mul = new Mul();

        public static IEnumerable<Operator> ArithmeticBase
        {
            get
            {
                yield return Minus;
                yield return Abs;
                yield return Sum;
                yield return Mul;
            }
        }

        #endregion

        #region Extra Arithmetic Operators

        public static IEnumerable<Operator> ArithmeticExtra
        {
            get
            {
                return Enumerable.Empty<Operator>();
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
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ArithmeticBase", ArithmeticBase);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("ArithmeticExtra", ArithmeticExtra);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("TrigonometricBase", TrigonometricBase);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("TrigonometricExtra", TrigonometricExtra);
                yield return new KeyValuePair<string, IEnumerable<Operator>>("Conditionals", Conditionals);
            }
        }
    }
}
