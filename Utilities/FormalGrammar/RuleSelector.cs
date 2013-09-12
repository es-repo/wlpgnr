using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T> : IEnumerable<Rule<T>>
    {
        private readonly IEnumerator<Rule<T>> _enumerator;

        protected IEnumerable<Rule<T>> Rules { get; private set; }

        protected RuleSelector(IEnumerable<Rule<T>> rules)
        {
            Rules = rules;
            _enumerator = EnumerableExtensions.Repeat(Next).GetEnumerator();
        }

        public abstract Rule<T> Next();
    
        public IEnumerator<Rule<T>> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
