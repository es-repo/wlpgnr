using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class SymbolsSet<T> : DictionaryExt<string, Symbol<T>>
    {
        public SymbolsSet()
            : this(Enumerable.Empty<Symbol<T>>())
        {
        }

        public  SymbolsSet(IEnumerable<Symbol<T>> symbols)
            : base(s => s.Name, symbols)
        {
        }
    }
}
