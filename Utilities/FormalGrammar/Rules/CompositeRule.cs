namespace WallpaperGenerator.Utilities.FormalGrammar.Rules
{
    public abstract class CompositeRule<T> : Rule<T>
    {
        public Rule<T>[] Rules { get; private set; }

        protected CompositeRule(Symbol<T> from, params Rule<T>[] rules)
            : base(from)
        {
            Rules = rules;
        }
    }
}
