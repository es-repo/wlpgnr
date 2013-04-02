using System;

namespace WallpaperGenerator.Formulas.Operands
{
    public class Variable : Operand
    {
        private double? _value;

        public string Name { get; private set; }

        public override double Value
        {
            get
            {
                if (_value == null)
                    throw new InvalidOperationException("Variable doesn't have assigned value.");
                
                return _value.Value;
            }
            protected set
            {
                _value = value; 
            }
        }
        
        public Variable(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Variable name can't be null or empty.");
            
            Name = name;
        }

        public void SetValue(double value)
        {
            Value = value;
        }
    }
}
