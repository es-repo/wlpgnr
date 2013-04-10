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
            FormulaTreeTokenType? tokenType = null;
            foreach (char ch in value)
            {
                FormulaTreeTokenType? nextTokenType;
                if (ch == '(')
                {
                    nextTokenType = FormulaTreeTokenType.OpenningBracket;
                }
                else if (ch == ')')
                {
                    nextTokenType = FormulaTreeTokenType.ClosingBracket;
                }
                else if (IsWordCharacter(ch))
                {
                    nextTokenType = FormulaTreeTokenType.Word;
                    tokenSb.Append(ch);
                }
                else if (IsWhitespaceCharacter(ch))
                {
                    nextTokenType = FormulaTreeTokenType.Whitespace;
                    tokenSb.Append(ch);
                }
                else
                {
                    throw new ArgumentException(string.Format("Unexpected character \"{0}\"", ch));
                }

                if (tokenType != null)
                {
                    bool isTokenProcessed = false;
                    switch (tokenType)
                    {
                        case FormulaTreeTokenType.OpenningBracket:
                            isTokenProcessed = true;
                            break;

                        case FormulaTreeTokenType.ClosingBracket:
                            isTokenProcessed = true;
                            break;

                        case FormulaTreeTokenType.Whitespace:
                        case FormulaTreeTokenType.Word:
                            if (tokenType != nextTokenType)
                                isTokenProcessed = true;
                            break;
                    }

                    if (isTokenProcessed)
                    {
                        yield return new FormulaTreeToken(tokenType.Value, tokenSb.ToString());
                        tokenSb = new StringBuilder();
                    }
                }

                tokenSb.Append(ch);
                tokenType = nextTokenType;
            }

            if (tokenType != null)
                yield return new FormulaTreeToken(tokenType.Value, tokenSb.ToString());
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
