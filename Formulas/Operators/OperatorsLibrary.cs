using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {        
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
                return Arithmetic;
            }
        }
    }
}
