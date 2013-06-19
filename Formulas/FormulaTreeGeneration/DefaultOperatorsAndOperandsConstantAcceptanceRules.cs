using WallpaperGenerator.Formulas.Operators.Conditionals;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class DefaultOperatorsAndOperandsConstantAcceptanceRules : OperatorsAndOperandsConstantAcceptanceRules
    {
        public override OperandsConstantAcceptanceRule this[Operator op]
        {
            get
            {
                return op.Arity != 2 || op is Max
                    ? OperandsConstantAcceptanceRule.NoConstantsAllowedRule
                    : OperandsConstantAcceptanceRule.OneConstantAllowedRule;
            }
        }
    }
}
