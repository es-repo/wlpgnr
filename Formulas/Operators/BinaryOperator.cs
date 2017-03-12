using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public abstract class BinaryOperator : Operator 
    {
        protected BinaryOperator(Expression evalExpr = null) : base(2, evalExpr)
        {
        }
    }
}