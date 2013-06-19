namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class OperandsConstantAcceptanceRuleCheck
    {
        private readonly OperandsConstantAcceptanceRule _operandsConstantAcceptanceRule;

        private int _allowedConstantsCount;

        public OperandsConstantAcceptanceRuleCheck(OperandsConstantAcceptanceRule operandsConstantAcceptanceRule)
        {
            _operandsConstantAcceptanceRule = operandsConstantAcceptanceRule;
        }

        public bool ConstantAllowed(int operandIndex)
        {
            if (_allowedConstantsCount >= _operandsConstantAcceptanceRule.ConstantsMaximumCount)
            {
                return false;
            }

            bool allowed = _operandsConstantAcceptanceRule.IsConstantAllowedAtAnyPosition ||
                _operandsConstantAcceptanceRule.AllowedConstantsOperandIndexes.Contains(operandIndex);

            if (allowed)
            {
                _allowedConstantsCount++;
            }
            return allowed;
        }
    }
}
