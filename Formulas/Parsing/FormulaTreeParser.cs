using System.Linq;  
using System.Collections.Generic;
using System.Globalization;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas.Parsing
{
    public static class FormulaTreeParser
    {
        public static FormulaTreeNode Parse(string value)
        {
            Stack<FormulaTreeNode> parentNodes = new Stack<FormulaTreeNode>();
            FormulaTreeNode currentNode = null;
            IEnumerable<FormulaTreeToken> tokens = FormulaTreeTokenizer.Tokenize(value);
            foreach (FormulaTreeToken token in tokens)
            {
                switch (token.Type)
                {
                    case FormulaTreeTokenType.OpenningBracket:
                        if (currentNode != null)
                        {
                            parentNodes.Push(currentNode);
                        }
                        break;

                    case FormulaTreeTokenType.ClosingBracket:
                        if (parentNodes.Count > 0)
                            currentNode = parentNodes.Pop(); 
                        break;

                    case FormulaTreeTokenType.Word:
                        currentNode = CreateNode(token.Value);
                        FormulaTreeNode parentNode = parentNodes.Count > 0 ? parentNodes.Peek() : null;  
                        if (parentNode != null)
                        {
                            parentNode.AddChild(currentNode);    
                        }
                        break;

                    case FormulaTreeTokenType.Whitespace:
                        break;
                }
            }

            return currentNode;
        }

        private static FormulaTreeNode CreateNode(string token)
        {
            Operator op = OperatorFromString(token);
            return new FormulaTreeNode(op);
        }

        private static Operator OperatorFromString(string str)
        {
            Operator knownOperator = GetKnownOperator(str);
            if (knownOperator != null)
                return knownOperator;

            double constant;
            if (double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out constant))
            {
                return new Constant(constant);
            }

            return new Variable(str);
        }

        private static Operator GetKnownOperator(string str)
        {
            return OperatorsLibrary.All.FirstOrDefault(op => op.GetType().Name == str);
        }
    }
}
