using System;

namespace WallpaperGenerator.Formulas.Operators
{
    public class Variable : ZeroArityOperator
    {
        public string Name { get; private set; }

        public double? Value { get; set; }
        
        public Variable(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Variable name can't be null or empty.");
            
            Name = name;
        }

        protected override double EvaluateCore(params double[] operands)
        {
            if (Value == null)
                    throw new InvalidOperationException("Variable doesn't have assigned value.");
            
            return Value.Value;
        }
    }
}
