using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T>
    {
        protected IEnumerable<Rule<T>> Rules { get; private set; }

        protected RuleSelector(IEnumerable<Rule<T>> rules)
        {
            Rules = rules;
        }

        public abstract Rule<T> Select();
    }
}
