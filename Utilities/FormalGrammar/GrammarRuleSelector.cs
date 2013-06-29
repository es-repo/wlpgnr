namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class GrammarRuleSelector<T>
    {
        protected Grammar<T> Grammar { get; private set; }

        protected GrammarRuleSelector(Grammar<T> grammar)
        {
            Grammar = grammar;
        }

        public abstract Rule<T> SelectRule(Symbol<T> symbol);
    }
}
