using NUnit.Framework;

namespace WallpaperGenerator.App.Core.Testing
{
    [TestFixture]
    public class FormulaRenderArgumentsGoodnessAnalyzerTests
    {
        [Test]
        [TestCase(
@"0.06,1.06;0.23,1.23;-1.58,-0.58;-0.55,0.64
-1.3,2.36,1.53,0;-0.2,-0.09,0.3,0.15;1.33,-2.25,-2.97,0.14
Sum Cbrt Atan Pow Cbrt Cbrt Cbrt Cbrt Cbrt x3 x2 Cbrt Abs Cbrt Sum Cbrt Sin Pow -2.72 Atan x3 Cbrt Sin Cbrt Sub x0 x1", 3, true)]
        [TestCase(
@"0.06,1.06;0.23,1.23;-1.58,-0.58;-0.55,0.64
-1.3,2.36,1.53,0;-0.2,-0.09,0.3,0.15;1.33,-2.25,-2.97,0.14
Sum Cbrt Atan Pow Cbrt Cbrt Cbrt Cbrt Cbrt x3 x2 Cbrt Abs Cbrt Sum Cbrt Sin Pow -2.72 Atan x3 Cbrt Sin Cbrt Sub x0 x1", 8, false)]
        public void TestIsVariablesCountOk(string argsStr, int variablesCount, bool expectedResult)
        {
            FormulaRenderArguments args = FormulaRenderArguments.FromString(argsStr);
            FormulaRenderArgumentsGoodnessAnalyzer analyzer = new FormulaRenderArgumentsGoodnessAnalyzer(variablesCount);
            bool result = analyzer.IsVariablesCountOk(args);
            bool result2 = analyzer.Analyze(args);
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, result2);   
        }

        [Test]

        [TestCase(
@"-0.29,1.71;0.02,2.47;0.49,1.49;-1.61,1.78
-0.24,-3.07,-0.27,0.11;3.29,-0.01,0.91,0.3;0,0,0,0
Cbrt Atan Pow Sqrt Pow x4 Atan Sqrt Sub Cbrt Sub x0 x8 Cbrt Sub x0 x8 x3", false)]

        [TestCase(
@"-5.38,5.08;-18.48,-11.29;-5.37,15.19;-7.41,16.98;-8.23,1.9;0.98,2.17
0.3,-1.35,0.6,0.06;-3.15,0.12,-0.6,0.15;-0.07,1.47,-1.05,0.28
Cos Sum Sum Atan Ln Mul Cos Sum Cbrt x4 x8 Mul Mul Mul x1 x11 Mul x7 x1 Sin Sin x9 Sqrt Cbrt Atan Ln Sum Cbrt x1 Sqrt x11 x7", true)]
        
        public void TestIsRenderedImageNotEmpty(string argsStr, bool expectedResult)
        {
            FormulaRenderArguments args = FormulaRenderArguments.FromString(argsStr);
            FormulaRenderArgumentsGoodnessAnalyzer analyzer = new FormulaRenderArgumentsGoodnessAnalyzer(3);
            bool result = analyzer.IsRenderedImageNotEmpty(args);
            bool result2 = analyzer.Analyze(args);
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, result2);     
        }
    }
}
