using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

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

            return Create(operators.Concat(variables), minimalDepth);
        }

        public static FormulaTree Create(IEnumerable<Operator> operators, int minimalDepth)
        {
            //Rule<Operator>[] rules = new[]
            //{
            //    // V -> x1|x2...|xn
            //    new OrRule<Operator>("V", 0, operators.OfType<Variable>()),

            //    // C -> x1|x2...|xn
            //    new OrRule<Operator>("C", 0, operators.OfType<Constant>()),

            //    // Op0 -> V|C
            //    new OrRule<Operator>("Op0", new [] { "V", "C" }),

            //    // Op1 -> abs|sin|...
            //    new OrRule<Operator>("Op1", 0, operators.Where(op => op.Arity == 1)),

            //    // Op2 -> +|-|...
            //    new OrRule<Operator>("Op2", 0, operators.Where(op => op.Arity == 2)),

            //    // Op3 -> ifg0|...
            //    new OrRule<Operator>("Op3", 0, operators.Where(op => op.Arity == 3)),

            //    // Op4 -> ifelse|...
            //    new OrRule<Operator>("Op4", 0, operators.Where(op => op.Arity == 4)),

            //    // Node0 -> Op0
            //    new Rule<Operator>("Node0", new[] { "Op0" }),

            //    // Node1 -> Op1 Node
            //    new Rule<Operator>("Node1", new[] { "Op1", "Node" }),

            //    // Node2 -> Op1 Node Node
            //    new Rule<Operator>("Node1",  new[] { "Op2", "Node", "Node" }),

            //    // Node -> Node0|Node1|Node2
            //    //new OrRule<Operator>("Node", rs => new TreeGenerationRuleSelector<Operator>(minimalDepth, new Rule<Operator>[]{}}))

            //};
            
            
            
            
            return null;
        }
    }
}
