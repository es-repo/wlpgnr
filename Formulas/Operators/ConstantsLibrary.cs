using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas.Operators
{
    public static class ConstantsLibrary
    {
        #region Base Constants

        public static readonly Operator C05 = new Constant(MathLibrary.C05);
        public static readonly Operator C1 = new Constant(MathLibrary.C1);
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

        public static IEnumerable<Operator> All
        {
            get
            {
                return ConstantsBase.Concat(ConstantsExtra);
            }
        }

    }
}
