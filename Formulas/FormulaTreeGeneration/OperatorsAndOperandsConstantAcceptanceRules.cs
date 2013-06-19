namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class OperatorsAndOperandsConstantAcceptanceRules
    {
        public virtual OperandsConstantAcceptanceRule this[Operator op]
        {
            get { return OperandsConstantAcceptanceRule.NoRestrictionsRule; }
        }

        public OperandsConstantAcceptanceRuleCheck CreateCheck(Operator op)
        {
            return new OperandsConstantAcceptanceRuleCheck(this[op]);
        }
    }
}
