using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class Symbol<T>
    {
        public string Name { get; private set; }

        public T Value { get; private set; }

        public bool IsTerminal { get; private set; }

        public Symbol(string name)
            : this(default(T), name, false)
        {
        }

        public Symbol(T value, string name = "")
            : this(value, name, true)
        {
        }

        private Symbol(T value, string name, bool isTerminal)
        {
            Name = name;
            Value = value;
            IsTerminal = isTerminal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Symbol<T>) obj);
        }

        protected bool Equals(Symbol<T> other)
        {
            return string.Equals(Name, other.Name) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }
    }
}
