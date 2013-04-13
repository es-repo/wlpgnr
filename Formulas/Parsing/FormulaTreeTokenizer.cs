using System;
using System.Collections.Generic;
using System.Text;

namespace WallpaperGenerator.Formulas.Parsing
{
    public enum FormulaTreeTokenType
    {
        OpenningBracket,
        ClosingBracket,
        Whitespace,
        Word
    }

    public class FormulaTreeToken
    {
        public FormulaTreeTokenType Type { get; private set; }
        public string Value { get; private set; }

        public FormulaTreeToken(FormulaTreeTokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public static class FormulaTreeTokenizer
    {                
        public static IEnumerable<FormulaTreeToken> Tokenize(string value)
        {
            StringBuilder tokenSb = new StringBuilder();
            FormulaTreeTokenType? prevTokenType = null;
            foreach (char ch in value)
            {
                FormulaTreeTokenType tokenType;
                if (ch == '(')
                {
                    tokenType = FormulaTreeTokenType.OpenningBracket;
                }
                else if (ch == ')')
                {
                    tokenType = FormulaTreeTokenType.ClosingBracket;
                }
                else if (IsWordCharacter(ch))
                {
                    tokenType = FormulaTreeTokenType.Word;
                }
                else if (IsWhitespaceCharacter(ch))
                {
                    tokenType = FormulaTreeTokenType.Whitespace;
                }
                else
                {
                    throw new ArgumentException(string.Format("Unexpected character \"{0}\"", ch));
                }

                if (prevTokenType != null)
                {
                    bool isTokenEndRiched = false;
                    switch (prevTokenType)
                    {
                        case FormulaTreeTokenType.OpenningBracket:
                            isTokenEndRiched = true;
                            break;

                        case FormulaTreeTokenType.ClosingBracket:
                            isTokenEndRiched = true;
                            break;

                        case FormulaTreeTokenType.Whitespace:
                        case FormulaTreeTokenType.Word:
                            if (prevTokenType != tokenType)
                                isTokenEndRiched = true;
                            break;
                    }

                    if (isTokenEndRiched)
                    {
                        yield return new FormulaTreeToken(prevTokenType.Value, tokenSb.ToString());
                        tokenSb = new StringBuilder();
                    }
                }

                tokenSb.Append(ch);
                prevTokenType = tokenType;
            }

            if (prevTokenType != null)
                yield return new FormulaTreeToken(prevTokenType.Value, tokenSb.ToString());
        }

        private static bool IsWordCharacter(char ch)
        {
            return (ch >= 'a' && ch <= 'z')
                || (ch >= 'A' && ch <= 'Z')
                || (ch >= '0' && ch <= '9')
                || ch == '_'
                || ch == '.'
                || ch == '+'
                || ch == '-';
        }

        private static bool IsWhitespaceCharacter(char ch)
        {
            return ch == ' ' || ch == '\r' || ch == '\n' || ch == '\t';
        }
    }
}
