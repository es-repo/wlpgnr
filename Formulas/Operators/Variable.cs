using System;
using System.Text.RegularExpressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public class Variable : ZeroArityOperator
    {
        private const string VariableNamePattern = "^[a-zA-Z_]+[a-zA-Z_0-9]*$";
        private static readonly Regex VariableNamePatternRegex = new Regex(VariableNamePattern, RegexOptions.Compiled);

        public Variable(string name)
            : base (name)
        {
            if (!IsNameValid(name))
                throw new ArgumentException("Variable name is invalid.");
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            return () => Value;
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            throw new InvalidOperationException();
        }

        private static bool IsNameValid(string value)
        {
            return value != null && VariableNamePatternRegex.IsMatch(value);
        }
    }
}
