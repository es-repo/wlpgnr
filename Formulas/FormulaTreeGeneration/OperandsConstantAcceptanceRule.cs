using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class OperandsConstantAcceptanceRule
    {
        public IList<int> AllowedConstantsOperandIndexes { get; private set; }
        
        public int ConstantsMaximumCount { get; private set; }

        public bool IsConstantAllowedAtAnyPosition
        {
            get { return AllowedConstantsOperandIndexes == null && ConstantsMaximumCount > 0; }
        }

        public OperandsConstantAcceptanceRule(IEnumerable<int> allowedConstantsOperandIndexes, int constantsMaximumCount)
        {
            AllowedConstantsOperandIndexes = allowedConstantsOperandIndexes != null ? allowedConstantsOperandIndexes.ToList() : null;
            ConstantsMaximumCount = constantsMaximumCount;
        }

        public static OperandsConstantAcceptanceRule NoConstantsAllowedRule = new OperandsConstantAcceptanceRule(new int[] {}, 0);
        
        public static OperandsConstantAcceptanceRule OneConstantAllowedRule  = new OperandsConstantAcceptanceRule(null, 1);

        public static OperandsConstantAcceptanceRule NoRestrictionsRule = new OperandsConstantAcceptanceRule(null, int.MaxValue);
    }
}
