using System;
using System.Text.RegularExpressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public class Variable : ZeroArityOperator
    {
        private const string VariableNamePattern = "^[a-zA-Z_]+[a-zA-Z_0-9]*$";
        private static readonly Regex _variableNamePatternRegex = new Regex(VariableNamePattern, RegexOptions.Compiled);

        public string Name { get; private set; }

        public double? Value { get; set; }
        
        public Variable(string name)
        {
            if (!IsNameValid(name))
                throw new ArgumentException("Variable name is invalid.");
            
            Name = name;
        }

        protected override double EvaluateCore(params double[] operands)
        {
            if (Value == null)
                    throw new InvalidOperationException("Variable doesn't have assigned value.");
            
            return Value.Value;
        }

        private static bool IsNameValid(string value)
        {
            return value != null && _variableNamePatternRegex. IsMatch(value);
        }
    }
}
