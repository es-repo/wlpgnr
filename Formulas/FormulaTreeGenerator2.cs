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
            return Generate(operators.Concat(variables), null, minimalDepth);
        }

        public static FormulaTree Generate(IEnumerable<Operator> operators, Func<double> createConstant, int minimalDepth)
        {
            if (!operators.OfType<Variable>().Any())
                throw new ArgumentException("Operators should have at least one variable.", "operators");
            
            SymbolsSet<Operator> s = CreateSymbols(operators);
            
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

            IEnumerable<int> availableArities = operators.Select(op => op.Arity).Distinct().ToArray();

            List<Rule<Operator>> rules = new List<Rule<Operator>>
            {
                new Rule<Operator>(s["C"], () => new[] { new Symbol<Operator>(new Constant(createConstant()))}),
                new Rule<Operator>(s["V"], operators.OfType<Variable>().Select(v => s[v.Name])),
                new OrRule<Operator>(s["Op0Node"], new []{ s["C"], s["V"] }), // TODO: apply probability

                new OrRule<Operator>(s["InfGuard"], new []{s["Atan"], s["Tanh"]} /*rs => new RandomRuleSelector<Operator>()*/ ), // TODO: apply randomness
                new OrRule<Operator>(s["RegOp2Operands"], 
                    new Rule<Operator>(new[]{s["OpNode"], s["NoConstOpNode"]}), 
                    new Rule<Operator>(new[]{s["NoConstOpNode"], s["OpNode"]})), // TODO: apply randomness

                new OrRule<Operator>(s["NoConstOpNode"], rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs), // TODO: apply probability
                    new []{ s["V"] }.Concat(availableArities.Where(a => a > 0).Select(a => s["Op" + a + "Node"]))), 

                new OrRule<Operator>(s["OpNode"], rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs), // TODO: apply probability
                    new []{ s["V"] }.Concat(availableArities.Select(a => s["Op" + a + "Node"]))), 
            };

            Grammar<Operator> grammar = new Grammar<Operator>(rules);
            TreeNode<Operator> treeRoot = TreeGenerator.Generate(grammar, "OpNode", op => op.Arity);
            return new FormulaTree(treeRoot);
        }
       
        private static SymbolsSet<Operator> CreateSymbols(IEnumerable<Operator> operators)
        {
            List<string> nonTerminalsNames = new List<string> 
            { 
                "V", "C", "InfGuard", "RegOp2Operands", "OpNode", "NoConstOpNode"
            };

            IEnumerable<int> availableArities = operators.Select(op => op.Arity).Distinct();
            nonTerminalsNames.AddRange(availableArities.Select(a => "Op" + a + "Node"));

            Func<Operator, string> getOpNodeName = op => op.Name + "Node";
            nonTerminalsNames.AddRange(operators.Where(op => op.Arity > 0).Select(getOpNodeName));

            IEnumerable<Symbol<Operator>> terminals = operators.Select(op => new Symbol<Operator>(op, op.Name));
            IEnumerable<Symbol<Operator>> nonTerminals = nonTerminalsNames.Select(n => new Symbol<Operator>(n));
            return new SymbolsSet<Operator> { terminals, nonTerminals };
        }
    }
}
