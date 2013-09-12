using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class RuleSelector<T> : IEnumerable<Rule<T>>
    {
        private readonly IEnumerator<Rule<T>> _enumerator;

        protected IEnumerable<Rule<T>> Rules { get; private set; }

        public RuleSelector(IEnumerable<Rule<T>> rules, IEnumerable<Rule<T>> rulesSequence = null)
        {
            Rules = rules;
            rulesSequence = rulesSequence ?? EnumerableExtensions.Repeat(Next);
            _enumerator = rulesSequence.GetEnumerator();
        }

        public virtual Rule<T> Next()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }
    
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
