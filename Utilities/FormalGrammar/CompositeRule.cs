namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class CompositeRule<T> : Rule<T>
    {
        protected Rule<T>[] Rules { get; private set; }

        protected CompositeRule(Symbol<T> from, params Rule<T>[] rules)
            : base(from)
        {
            Rules = rules;
        }
    }
}
