using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Parsing;

namespace Formulas.Testing.Parsing
{
    [TestFixture]
    public class FormulaTreeParserTests
    {
        [Test]
        public void TestParse()
        {
            TestParse("()", null);

            TestParse("(2)", new FormulaTreeNode(ConstantsLibrary.C2));

            TestParse("(x)", new FormulaTreeNode(new Variable("x")));

            TestParse("(Sum(x y)", 
                new FormulaTreeNode(
                    OperatorsLibrary.Sum, 
                        new FormulaTreeNode(new Variable("x")),
                        new FormulaTreeNode(new Variable("y"))));

            TestParse(// 2*x + 7*(-y + 5)            
@"(
Sum(
	Mul(
		2
		x)
	Mul(
		7
		Sum(
			Minus(
				y)
			5))))",
                new FormulaTreeNode(OperatorsLibrary.Sum,
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(ConstantsLibrary.C2),
                        new FormulaTreeNode(new Variable("x"))),
                    new FormulaTreeNode(OperatorsLibrary.Mul,
                        new FormulaTreeNode(ConstantsLibrary.C7),
                        new FormulaTreeNode(OperatorsLibrary.Sum,
                            new FormulaTreeNode(OperatorsLibrary.Minus,
                                new FormulaTreeNode(new Variable("y"))),
                            new FormulaTreeNode(ConstantsLibrary.C5)))));
        }

        private static void TestParse(string value, FormulaTreeNode expectedRoot)
        {
            FormulaTreeNode root = FormulaTreeParser.Parse(value);
            Assert.IsTrue(Utilities.AreFormulaTreesEqual(root, expectedRoot));
        }
    }
}
