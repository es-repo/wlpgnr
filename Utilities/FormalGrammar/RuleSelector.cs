using System;
using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class RuleSelector<T> : IEnumerable<Rule<T>>
    {
        private IEnumerator<Rule<T>> _enumerator;

        public RuleSelector(IEnumerable<Rule<T>> rules)
            : this(rules, rules.Repeat())
        {
            Init(rules.Repeat());
        }

// ReSharper disable UnusedParameter.Local
        public RuleSelector(IEnumerable<Rule<T>> rules, IEnumerable<Rule<T>> rulesSequence)
// ReSharper restore UnusedParameter.Local
        {
            Init(rulesSequence);
        }

        public RuleSelector(Func<Rule<T>> next = null)
        {
            Init(EnumerableExtensions.Repeat(next ?? Next));
        }

        private void Init(IEnumerable<Rule<T>> rulesSequence)
        {
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
