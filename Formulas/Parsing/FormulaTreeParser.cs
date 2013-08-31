using System;
using System.Linq;  
using System.Collections.Generic;
using System.Globalization;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas.Parsing
{
    public static class FormulaTreeParser
    {
        public static TreeNode<Operator> Parse(string value)
        {
            Stack<TreeNode<Operator>> parentNodes = new Stack<TreeNode<Operator>>();
            TreeNode<Operator> currentNode = null;
            IEnumerable<FormulaTreeToken> tokens = FormulaTreeTokenizer.Tokenize(value);
            List<Operator> variables = new List<Operator>();
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
                        currentNode = CreateNode(token.Value, variables);
                        if (currentNode.Value is Variable)
                        {
                            variables.Add(currentNode.Value);
                        }

                        TreeNode<Operator> parentNode = parentNodes.Count > 0 ? parentNodes.Peek() : null;  
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

        private static TreeNode<Operator> CreateNode(string token, IEnumerable<Operator> availableVariables)
        {
            Operator op = OperatorFromString(token, availableVariables);
            return new TreeNode<Operator>(op);
        }


        private static Operator OperatorFromString(string str, IEnumerable<Operator> availableVariables)
        {
            Operator knownOperator = GetKnownOperator(str);
            if (knownOperator != null)
                return knownOperator;

            double constant;
            if (double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out constant))
            {
                return new Constant(constant);
            }

            Operator availableVariable = availableVariables.FirstOrDefault(op => op.Name == str);
            return availableVariable ?? new Variable(str);
        }

        private static Operator GetKnownOperator(string str)
        {
            return OperatorsLibrary.All.FirstOrDefault(op => op.Name.Equals(str, StringComparison.OrdinalIgnoreCase));
        }
    }
}
