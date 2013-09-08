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
    public static class FormulaTreeGenerator2d
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

            // AbsNode -> abs NoConstOpNode
            // SqrtNode -> sqrt NoConstOpNode
            // CqrtNode -> cbrt NoConstOpNode
            // SinNode -> sin NoConstOpNode
            // CosNode -> cos NoConstOpNode
            // AtanNode -> atan NoConstOpNode
            // TanhNode -> tanh NoConstOpNode
            // InfGuard -> atan|tanh
            // Pow2Node -> InfGuard pow2 NoConstOpNode
            // Pow3Node -> InfGuard pow3 NoConstOpNode
            // LnNode -> InfGuard ln NoConstOpNode
            // SinhNode -> InfGuard sinh NoConstOpNode
            // CoshNode -> InfGuard cosh NoConstOpNode
            // Op1Node -> AbsNode|SqrtNode|CqrtNode|SinNode|CosNode|AtanNode|TanhNode|Pow2Node|Pow3Node|LnNode|SinhNode|CoshNode

            // RegOp2Operands -> (OpNode NoConstOpNode)|(NoConstOpNode OpNode)
            // SumNode -> sum RegOp2Operands
            // SubNode -> sub RegOp2Operands
            // MulNode -> mul RegOp2Operands
            // DivNode -> InfGuard div RegOp2Operands
            // PowNode -> InfGuard pow RegOp2Operands
            // PowAltNode -> pow (NoConstOpNode InfGuard OpNode)|(OpNode InfGuard NoConstOpNode)
            // MaxNode -> max NoConstOpNode NoConstOpNode
            // ModNode -> mod (NoConstOpNode sum abs OpNode 0.0001)|(OpNode sum abs NoConstOpNode 0.0001)
            // Op2Node -> DivNode|PowNode|PowAltNode|MaxNode|ModNode

            // Ifg0Node -> ifg0 NoConstOpNode NoConstOpNode NoConstOpNode
            // Op3Node -> Ifg0Node

            // IfgNode -> ifg NoConstOpNode NoConstOpNode NoConstOpNode NoConstOpNode 
            // Op4Node -> IfgNode

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
