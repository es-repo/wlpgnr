using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{        
    public class Rule<T>
    {
        public Symbol<T> Left { get; private set; }

        public Symbol<T>[] Right { get; private set; }

        public Rule(Symbol<T> left, params Symbol<T>[] right)
        {
            Left = left;
            Right = right;
        }

        public IEnumerable<Symbol<T>> Apply()
        {
            return Right;
        }
    }
}
