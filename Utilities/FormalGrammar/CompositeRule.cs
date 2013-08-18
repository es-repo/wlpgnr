namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class CompositeRule<T> : Rule<T>
    {
        protected Rule<T>[] Rules { get; private set; }

        protected CompositeRule(params Rule<T>[] rules)
            : this(null, rules: rules)
        {
        }

        protected CompositeRule(Symbol<T> from, string name = "", params Rule<T>[] rules)
            : base(from, name)
        {
            Rules = rules;
        }
    }
}
