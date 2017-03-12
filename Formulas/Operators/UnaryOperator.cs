using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public abstract class UnaryOperator : Operator
    {
        protected UnaryOperator(Expression evalExpr = null) : base(1, evalExpr)
        {
        }
    }
}