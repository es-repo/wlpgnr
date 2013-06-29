namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class Symbol<T>
    {
        public string Name { get; private set; }

        public T Value { get; private set; }

        public bool IsTerminal { get; private set; }

        public Symbol(string name)
            : this(name, default(T), false)
        {
        }

        public Symbol(string name, T value)
            : this(name, value, true)
        {
        }

        private Symbol(string name, T value, bool isTerminal)
        {
            Name = name;
            Value = value;
            IsTerminal = isTerminal;
        }
    }
}
