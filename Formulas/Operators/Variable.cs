﻿using System;
using System.Text.RegularExpressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public class Variable : ZeroArityOperator
    {
        private const string VariableNamePattern = "^[a-zA-Z_]+[a-zA-Z_0-9]*$";
        private static readonly Regex _variableNamePatternRegex = new Regex(VariableNamePattern, RegexOptions.Compiled);

        public double Value { get; set; }
        
        public Variable(string name)
            : base (name)
        {
            if (!IsNameValid(name))
                throw new ArgumentException("Variable name is invalid.");
        }

        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Value;
        }

        private static bool IsNameValid(string value)
        {
            return value != null && _variableNamePatternRegex.IsMatch(value);
        }
    }
}
