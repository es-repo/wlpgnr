using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T> : IEnumerable<Rule<T>>
    {
        private IEnumerator<Rule<T>> _enumerator;

        protected IEnumerable<Rule<T>> Rules { get; private set; }

        protected RuleSelector(IEnumerable<Rule<T>> rules)
        {
            Rules = rules;
        }

        public abstract Rule<T> Next();
    
        public IEnumerator<Rule<T>> GetEnumerator()
        {
            return _enumerator ?? (_enumerator = Enumerate().GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<Rule<T>> Enumerate()
        {
            return EnumerableExtensions.Repeat(Next);
        }
    }
}
