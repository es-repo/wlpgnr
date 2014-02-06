using MbUnit.Framework;

namespace WallpaperGenerator.App.Core.Testing
{
    [TestFixture]
    public class FormulaRenderArgumentsGoodnessAnalyzerTests
    {
        [Test]
        [Row(
@"0.06,1.06;0.23,1.23;-1.58,-0.58;-0.55,0.64
-1.3,2.36,1.53,0;-0.2,-0.09,0.3,0.15;1.33,-2.25,-2.97,0.14
Sum Cbrt Atan Pow Cbrt Cbrt Cbrt Cbrt Cbrt x3 x2 Cbrt Abs Cbrt Sum Cbrt Sin Pow -2.72 Atan x3 Cbrt Sin Cbrt Sub x0 x1", 3, true)]
        [Row(
@"0.06,1.06;0.23,1.23;-1.58,-0.58;-0.55,0.64
-1.3,2.36,1.53,0;-0.2,-0.09,0.3,0.15;1.33,-2.25,-2.97,0.14
Sum Cbrt Atan Pow Cbrt Cbrt Cbrt Cbrt Cbrt x3 x2 Cbrt Abs Cbrt Sum Cbrt Sin Pow -2.72 Atan x3 Cbrt Sin Cbrt Sub x0 x1", 8, false)]
        public void TestIsVariablesCountOk(string argsStr, int variablesCount, bool expectedResult)
        {
            FormulaRenderArguments args = FormulaRenderArguments.FromString(argsStr);
            FormulaRenderArgumentsGoodnessAnalyzer analyzer = new FormulaRenderArgumentsGoodnessAnalyzer(variablesCount);
            bool result = analyzer.IsVariablesCountOk(args);
            Assert.AreEqual(expectedResult, result);                

        }
    }
}
