using System;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public abstract class QuaternaryOperator : Operator 
    {
        protected QuaternaryOperator(Expression evalExpr = null) : base(4, evalExpr)
        {
        }
    }
}