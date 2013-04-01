using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas.Operators.Arithmetic;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class OperatorsLibrary
    {
        public static readonly Operator Minus = new Minus();
        public static readonly Operator Sum = new Sum();
        public static readonly Operator Mul = new Mul();

        public static IEnumerable<Operator> All
        {
            get
            {
                return  Arithmetic.Concat(
                        ArithmeticExtended);
            }
        }
              
        public static IEnumerable<Operator> Arithmetic
        {
            get
            {
                yield return Minus;
                yield return Sum;
                yield return Mul;
            }
        }

        public static IEnumerable<Operator> ArithmeticExtended
        {
            get
            {
                return Enumerable.Empty<Operator>();
            }
        }
    }
}
