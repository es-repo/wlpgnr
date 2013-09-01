using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Formulas
{
    public static class FormulaTreeGenerator2
    {
        public static FormulaTree CreateRandom(int dimensionsCount, int minimalDepth)
        {
            IEnumerable<string> variableNames = EnumerableExtensions.Repeat(i => "x" + i.ToString(CultureInfo.InvariantCulture), dimensionsCount);
            IEnumerable<Operator> variables = variableNames.Select(n => new Variable(n));
            IEnumerable<Operator> operators = OperatorsLibrary.All;
            
            //SymbolsSet<Operator> symbols = new SymbolsSet<Operator>(new[]
            //{
            //    new Symbol<Operator>( )
            //});

            return Create(operators.Concat(variables));
        }

        public static FormulaTree Create(IEnumerable<Operator> operators)
        {
            return null;
        }
    }
}
