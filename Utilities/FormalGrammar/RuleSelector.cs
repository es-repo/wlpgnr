namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T>
    {
        protected Grammar<T> Grammar { get; private set; }

        protected RuleSelector(Grammar<T> grammar)
        {
            Grammar = grammar;
        }

        public abstract Rule<T> SelectRule(Symbol<T> symbol);
    }
}
