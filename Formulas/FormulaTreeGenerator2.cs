using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;
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

            return Generate(operators.Concat(variables), minimalDepth);
        }

        public static FormulaTree Generate(IEnumerable<Operator> operators, int minimalDepth)
        {
            if (operators.Count(op => op.Arity == 0) == 0)
                throw new ArgumentException("Operators should have at least one variable or constant.", "operators");

            IEnumerable<Symbol<Operator>> terminals = operators.Select(op => new Symbol<Operator>(op, op.Name));

            List<string> nonTerminalsNames = new List<string> { "Node" };
           
            bool hasVariables = operators.Any(op => op is Variable);
            if (hasVariables)
            {
                nonTerminalsNames.Add("V"); 
            }

            bool hasConstants = operators.Any(op => op is Constant);
            if (hasConstants)
            {
                nonTerminalsNames.Add("C");
            }

            IEnumerable<int> arities = operators.Select(op => op.Arity); 
            foreach (int a in arities)
            {
                nonTerminalsNames.Add("Op" + a.ToString(CultureInfo.InvariantCulture));
                nonTerminalsNames.Add("Node" + a.ToString(CultureInfo.InvariantCulture));
            }


            IEnumerable<Symbol<Operator>> nonTerminals = nonTerminalsNames.Select(n => new Symbol<Operator>(n));

            SymbolsSet<Operator> s = new SymbolsSet<Operator>
            {
                terminals, 
                nonTerminals
            };
            
            // V -> x1|x2|...
            // C -> c1|c2|...
            // Op0Node -> V|C

            // InfGuard -> atan|tanh

            // RegOp1 -> abs|sqrt|cbrt|sin|cos|atan|tanh
            // RegOp1Node -> RegOp1 NoConstOpNode
            // InfGuardOp1 -> pow2|pow3|ln|sinh|cosh
            // InfGuardOp1Node -> InfGuard InfGuardOp1 NoConstOpNode
            // Op1Node -> RegOp1Node|InfGuardOp1Node

            // RegOp2 -> sum|sub|mul
            // RegOp2Operands -> (OpNode NoConstOpNode)|(NoConstOpNode OpNode)
            // RegOp2Node -> RegOp2 RegOp2Operands
            // InfGuardOp2 -> div|pow
            // InfGuardOp2Node -> InfGuard InfGuardOp2 Op2Operands
            // SndOpndInfGuardOp2 -> pow
            // SndOpndInfGuardOp2Node -> SndOpndInfGuardOp2 (NoConstOpNode InfGuard OpNode)|(OpNode InfGuard NoConstOpNode)
            // NoConstOp2 -> max
            // NoConstOp2Node -> NoConstOp2 NoConstOpNode NoConstOpNode
            // ModNode -> mod (NoConstOpNode sum abs OpNode 0.0001)|(OpNode sum abs NoConstOpNode 0.0001)
            // Op2Node -> RegOp2Node|InfGuardOp1Node|SndOpndInfGuardOp2Node|NoConstOp2Node|ModNode
            
            // NoConstOp3 -> ifg0
            // NoConstOp3Node -> NoConstOp3 NoConstOpNode NoConstOpNode NoConstOpNode
            // Op3Node -> NoConstOp3Node
            
            // NoConstOp4 -> ifg
            // NoConstOp4Node -> NoConstOp4 NoConstOpNode NoConstOpNode NoConstOpNode NoConstOpNode 
            // Op4Node -> NoConstOp4Node

            // NoConstOpNode -> V|Op1Node|Op2Node|Op3Node|Op4Node
            // OpNode -> Op0Node|Op1Node|Op2Node|Op3Node|Op4Node

            Rule<Operator>[] rules = new[]
            {
                // V -> x1|x2...|xn
                new OrRule<Operator>(s["V"], terminals.Where(ts => ts.Value is Variable)),

                // C -> x1|x2...|xn
                new OrRule<Operator>(s["C"],  terminals.Where(ts => ts.Value is Constant)),

                // Op0 -> V|C
                new OrRule<Operator>(s["Op0"], new [] { s["V"], s["C"] }),

                // Op1 -> abs|sin|...
                new OrRule<Operator>(s["Op1"], terminals.Where(ts => ts.Value.Arity == 1)),

                // Op2 -> +|-|...
                new OrRule<Operator>(s["Op2"], terminals.Where(ts => ts.Value.Arity == 2)),

            //    // Op3 -> ifg0|...
            //    new OrRule<Operator>("Op3", 0, operators.Where(op => op.Arity == 3)),

            //    // Op4 -> ifelse|...
            //    new OrRule<Operator>("Op4", 0, operators.Where(op => op.Arity == 4)),

                // Node0 -> Op0
            
                new Rule<Operator>(s["Node0"], new[] { s["Op0"] }),

                // NodeNon0 -> Node1|Node2
                // Node1 -> Op1 (V|NodeNon0)


                // Node1 -> Op1 Node
                new Rule<Operator>(s["Node1"], new[] { s["Op1"], s["Node"] }),

                // Node2 -> Op1 Node Node
                new Rule<Operator>(s["Node2"],  new[] { s["Op2"], s["Node"], s["Node"] }),

                // Node -> Node0|Node1|Node2
                //new OrRule<Operator>(s["Node"], rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs),
                //    new [] { s["Node0"], s["Node1"], s["Node2"] })
            };

            Grammar<Operator> grammar = new Grammar<Operator>(rules);
            TreeNode<Operator> treeRoot = TreeGenerator.Generate(grammar, "OpNode", op => op.Arity);
            return new FormulaTree(treeRoot);
        }
    }
}
