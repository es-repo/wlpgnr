using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public abstract class ZeroArityOperator : Operator
    {
        public double Value { get; set; }

        protected ZeroArityOperator(string name, Expression evalExpr = null)
            : base(0, name, evalExpr)
        {
        }
    }
}