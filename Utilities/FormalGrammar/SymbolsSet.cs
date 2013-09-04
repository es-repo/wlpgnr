using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class SymbolsSet<T> : KeyedSet<string, Symbol<T>>
    {
        public  SymbolsSet(IEnumerable<Symbol<T>> symbols)
            : base(s => s.Name, symbols)
        {
        }
    }
}
