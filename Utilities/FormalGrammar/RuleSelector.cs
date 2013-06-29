using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T>
    {
        protected IEnumerable<Rule<T>> _rules;

        protected RuleSelector(IEnumerable<Rule<T>> rules)
        {
            _rules = rules;
        }

        public abstract Rule<T> Select();
    }
}
