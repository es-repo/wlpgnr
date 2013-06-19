using MbUnit.Framework;
using WallpaperGenerator.Formulas.FormulaTreeGeneration;

namespace WallpaperGenerator.Formulas.Testing.FormulaTreeGeneration
{
    [TestFixture]
    public class OperandsConstantAcceptanceCheckTests
    {
        [Test]
        public void TestConstantAllowed()
        {
            OperandsConstantAcceptanceRuleCheck noRestrictionsRuleCheck = new OperandsConstantAcceptanceRuleCheck(OperandsConstantAcceptanceRule.NoRestrictionsRule);
            Assert.IsTrue(noRestrictionsRuleCheck.ConstantAllowed(0));
            Assert.IsTrue(noRestrictionsRuleCheck.ConstantAllowed(30));

            OperandsConstantAcceptanceRuleCheck noConstantsAllowedRuleCheck = new OperandsConstantAcceptanceRuleCheck(OperandsConstantAcceptanceRule.NoConstantsAllowedRule);
            Assert.IsFalse(noConstantsAllowedRuleCheck.ConstantAllowed(0));

            OperandsConstantAcceptanceRuleCheck oneConstantAllowedRuleCheck = new OperandsConstantAcceptanceRuleCheck(OperandsConstantAcceptanceRule.OneConstantAllowedRule);
            Assert.IsTrue(oneConstantAllowedRuleCheck.ConstantAllowed(0));
            Assert.IsFalse(oneConstantAllowedRuleCheck.ConstantAllowed(1));
        }
    }
}
