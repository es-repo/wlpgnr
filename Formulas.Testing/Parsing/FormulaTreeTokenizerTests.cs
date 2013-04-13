using System;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Parsing;
using System.Collections.Generic;

namespace Formulas.Testing.Parsing
{
    [TestFixture]
    public class FormulaTreeTokenizerTests
    {
        [RowTest]
        [Row("", new FormulaTreeTokenType[] { }, new string[] { })]
        [Row("()",
            new[] 
            { 
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.ClosingBracket 
            },
            new[] { "(", ")" })]
        [Row("(x)", 
            new[]
            {
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.ClosingBracket
            }, 
            new[] { "(", "x", ")" })]
        [Row("(xyz   123)", 
            new[]
            {
                FormulaTreeTokenType.OpenningBracket, 
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.Whitespace,
                FormulaTreeTokenType.Word, 
                FormulaTreeTokenType.ClosingBracket
            },
            new[] { "(", "xyz", "   ", "123", ")" })]
        [Row(
@"(
Sum(
    Minus(
        x)
    Mul(
        2
        _y12)))", 
            new[]
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
            },
            new[] { "(", "\r\n", "Sum", "(", "\r\n    ", "Minus", "(", "\r\n        ", "x", ")", "\r\n    ", "Mul", "(", "\r\n        ", "2", "\r\n        ", "_y12", ")", ")", ")" })]
        [Row("(x y _e% )", new FormulaTreeTokenType[] { }, new string[] { }, ExpectedException = typeof(ArgumentException))]
        public void TestTokinize(string value, FormulaTreeTokenType[] expectedTokenTypes, string[] expectedTokenValues)
        {
            IEnumerable<FormulaTreeToken> tokens = FormulaTreeTokenizer.Tokenize(value);
            
            FormulaTreeTokenType[] tokenTypes = tokens.Select(t => t.Type).ToArray();
            CollectionAssert.AreEqual(expectedTokenTypes, tokenTypes);

            string[] tokenValues = tokens.Select(t => t.Value).ToArray();
            CollectionAssert.AreEqual(expectedTokenValues, tokenValues);
        }
    }
}
