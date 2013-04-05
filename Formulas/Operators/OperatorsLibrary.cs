using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {
        #region Constants

        #region Base Constants

        public static readonly Operator C05 = new Constant(MathLibrary.C05);
        public static readonly Operator C2 = new Constant(MathLibrary.C2);
        public static readonly Operator C3 = new Constant(MathLibrary.C3);
        public static readonly Operator C5 = new Constant(MathLibrary.C5);
        public static readonly Operator C7 = new Constant(MathLibrary.C7);
        public static readonly Operator Pi = new Constant(MathLibrary.PI);
        public static readonly Operator E = new Constant(MathLibrary.E);

        public static IEnumerable<Operator> ConstantsBase
        {
            get
            {
                yield return C05;
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
        public static readonly Operator Sum = new Sum();
        public static readonly Operator Mul = new Mul();

        public static IEnumerable<Operator> ArithmeticBase
        {
            get
            {
                yield return Minus;
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

        public static IEnumerable<Operator> All
        {
            get
            {
                return  Constants.Concat(Arithmetic);
            }
        }
    }
}
