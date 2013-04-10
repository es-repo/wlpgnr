using System;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Parsing;

namespace Formulas.Testing.Parsing
{
    [TestFixture]
    public class FormulaTreeTokenizerTests
    {
        [RowTest]
        [Row("", new FormulaTreeTokenType[] { })]
        [Row("()", new[] 
                    { 
                        FormulaTreeTokenType.OpenningBracket, 
                        FormulaTreeTokenType.ClosingBracket 
                    })]
        [Row("(x)", new[]
            {
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.ClosingBracket
            })]
        [Row("(xyz   123)", new[]
            {
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.ClosingBracket
            })]
        [Row(
@"(
Sum(
    Minus(
        x)
    Mul(
        2
        _y12)))", new[]
            {
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //Sum
                FormulaTreeTokenType.OpenningBracket,
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //Minus
                FormulaTreeTokenType.OpenningBracket,
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //x
                FormulaTreeTokenType.ClosingBracket,
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //Mul 
                FormulaTreeTokenType.OpenningBracket,
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //2 
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, //y12
                FormulaTreeTokenType.ClosingBracket,
                FormulaTreeTokenType.ClosingBracket,
                FormulaTreeTokenType.ClosingBracket
            })]
        [Row("(x y _e% )", new FormulaTreeTokenType[] { }, ExpectedException = typeof(ArgumentException))]
        public void TestTokinize(string value, FormulaTreeTokenType[] expectedTokenTypes)
        {
            FormulaTreeTokenType[] tokenTypes = FormulaTreeTokenizer.Tokenize(value).Select(t => t.Type).ToArray();
            CollectionAssert.AreEqual(expectedTokenTypes, tokenTypes);
        }
    }
}
