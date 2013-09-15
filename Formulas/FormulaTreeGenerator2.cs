using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Formulas.Operators.Conditionals;
using WallpaperGenerator.Formulas.Operators.Trigonometric;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating;
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
            return Generate(operators.Concat(variables), null, minimalDepth);
        }

        public static FormulaTree Generate(IEnumerable<Operator> operators, Func<double> createConstant, int minimalDepth)
        {
            Grammar<Operator> grammar = CreateGrammar(operators, createConstant, minimalDepth); 
            TreeNode<Operator> treeRoot = TreeGenerator.Generate(grammar, "OpNode", op => op.Arity);
            return new FormulaTree(treeRoot);
        }

        public static Grammar<Operator> CreateGrammar(IEnumerable<Operator> operators, Func<double> createConstant, int minimalDepth)
        {
            if (!operators.OfType<Variable>().Any())
                throw new ArgumentException("Operators should have at least one variable.", "operators");
            
            // V -> x1|x2|...
            // C -> c1|c2|...
            // Op0Node -> V|C

            // AbsNode -> abs NoConstOpNode
            // SqrtNode -> sqrt NoConstOpNode
            // CbrtNode -> cbrt NoConstOpNode
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
            // Op1Node -> AbsNode|SqrtNode|CbrtNode|SinNode|CosNode|AtanNode|TanhNode|Pow2Node|Pow3Node|LnNode|SinhNode|CoshNode

            // RegOp2Operands -> (OpNode NoConstOpNode)|(NoConstOpNode OpNode)
            // SumNode -> sum RegOp2Operands
            // SubNode -> sub RegOp2Operands
            // MulNode -> mul RegOp2Operands
            // DivNode -> InfGuard div RegOp2Operands
            // PowNode -> (InfGuard pow RegOp2Operands)|(pow (NoConstOpNode InfGuard OpNode)|(OpNode InfGuard NoConstOpNode))
            // MaxNode -> max NoConstOpNode NoConstOpNode
            // ModNode -> mod (NoConstOpNode sum abs OpNode 0.0001)|(OpNode sum abs NoConstOpNode 0.0001)
            // Op2Node -> DivNode|PowNode|MaxNode|ModNode

            // Ifg0Node -> ifg0 NoConstOpNode NoConstOpNode NoConstOpNode
            // Op3Node -> Ifg0Node

            // IfgNode -> ifg NoConstOpNode NoConstOpNode NoConstOpNode NoConstOpNode 
            // Op4Node -> IfgNode

            // NoConstOpNode -> V|Op1Node|Op2Node|Op3Node|Op4Node
            // OpNode -> Op0Node|Op1Node|Op2Node|Op3Node|Op4Node

            SymbolsSet<Operator> s = CreateSymbols(operators);

            IEnumerable<Symbol<Operator>> opArityNodeSymbols = GetOpArityNodeSymbolNames(operators).Select(n => s[n]).ToArray();

            List<Rule<Operator>> rules = new List<Rule<Operator>>
            {
                new Rule<Operator>(s["C"], () => new[] { new Symbol<Operator>(new Constant(createConstant()))}),
                new OrRule<Operator>(s["V"], operators.OfType<Variable>().Select(v => s[v.Name])),
                new OrRule<Operator>(s["Op0Node"], new []{ s["V"], s["C"] }), // TODO: apply probability

                new OrRule<Operator>(s["InfGuard"], 
                    new []{s[GetOpSymbolName(OperatorsLibrary.Atan)], s[GetOpSymbolName(OperatorsLibrary.Tanh)]} /*rs => new RandomRuleSelector<Operator>()*/ ), // TODO: apply randomness
                
                new OrRule<Operator>(s["RegOp2Operands"], 
                    new Rule<Operator>(new[]{s["OpNode"], s["NoConstOpNode"]}), 
                    new Rule<Operator>(new[]{s["NoConstOpNode"], s["OpNode"]})), // TODO: apply randomness

                new OrRule<Operator>(s["NoConstOpNode"], rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs), // TODO: apply probability
                    new []{ s["V"] }.Concat(opArityNodeSymbols.Skip(1))), 

                new OrRule<Operator>(s["OpNode"], rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs), // TODO: apply probability
                    opArityNodeSymbols), 
            };

            // AbsNode -> abs NoConstOpNode, 
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["NoConstOpNode"] }),
                typeof(Abs), typeof(Sqrt), typeof(Cbrt), typeof(Sin), typeof(Cos), typeof(Atan), typeof(Tanh)));

            // Pow2Node -> InfGuard pow2 NoConstOpNode
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["NoConstOpNode"] }),
                typeof(Pow2), typeof(Pow3), typeof(Ln), typeof(Sinh), typeof(Cosh)));

            // SumNode -> sum RegOp2Operands
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["RegOp2Operands"] }),
                typeof(Sum), typeof(Sub), typeof(Mul)));

            // DivNode -> InfGuard div RegOp2Operands
            // ...
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["RegOp2Operands"] }),
                typeof(Div)));

            // PowNode -> (InfGuard pow RegOp2Operands)|(pow (NoConstOpNode InfGuard OpNode)|(OpNode InfGuard NoConstOpNode))
            rules.AddRange(CreateOpNodeRules(operators,
                op => new OrRule<Operator>(s[GetOpNodeSymbolName(op)],
                    new Rule<Operator>(new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["RegOp2Operands"] }),
                    new AndRule<Operator>(new Rule<Operator>(new[]{s[GetOpSymbolName(op)]}),
                        new OrRule<Operator>(
                            new Rule<Operator>(new []{s["NoConstOpNode"], s["InfGuard"], s["OpNode"]}),
                            new Rule<Operator>(new []{s["OpNode"], s["InfGuard"], s["NoConstOpNode"]})))),
                typeof(Pow)));

            // ModNode -> mod (NoConstOpNode sum abs OpNode 0.0001)|(OpNode sum abs NoConstOpNode 0.0001)
            rules.AddRange(CreateOpNodeRules(operators,
                op => new AndRule<Operator>(s[GetOpNodeSymbolName(op)], 
                    new Rule<Operator>(new[] { s[GetOpSymbolName(op)] }),
                    new OrRule<Operator>(
                        new Rule<Operator>(new[] { s["NoConstOpNode"], s[GetOpSymbolName(OperatorsLibrary.Sum)], s[GetOpSymbolName(OperatorsLibrary.Abs)], s["OpNode"], new Symbol<Operator>(new Constant(0.0001))}),
                        new Rule<Operator>(new[] { s["OpNode"], s[GetOpSymbolName(OperatorsLibrary.Sum)], s[GetOpSymbolName(OperatorsLibrary.Abs)], s["NoConstOpNode"], new Symbol<Operator>(new Constant(0.0001))}))),
                typeof(Mod)));

            // MaxNode -> max NoConstOpNode NoConstOpNode
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["NoConstOpNode"], s["NoConstOpNode"] }),
                typeof(Max)));

            // Ifg0Node -> ifg0 NoConstOpNode NoConstOpNode NoConstOpNode
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["NoConstOpNode"], s["NoConstOpNode"], s["NoConstOpNode"] }),
                typeof(IfG0)));

            // IfgNode -> ifg NoConstOpNode NoConstOpNode NoConstOpNode NoConstOpNode 
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["NoConstOpNode"], s["NoConstOpNode"], s["NoConstOpNode"], s["NoConstOpNode"] }),
                typeof(IfG)));

            // Op1Node -> AbsNode|SqrtNode|...
            // Op2Node -> DivNode|PowNode|...
            // ...
            IEnumerable<IGrouping<int, Operator>> operatorsByArity = operators.GroupBy(op => op.Arity).Where(g => g.Key > 0 && g.Any());
            rules.AddRange(operatorsByArity.Select(g => 
                new OrRule<Operator>(s[GetOpArityNodeSymbolName(g.Key)], g.Select(op => s[GetOpNodeSymbolName(op)]))));
            
            return new Grammar<Operator>(rules);
        }

        private static IEnumerable<Rule<Operator>> CreateOpNodeRules(IEnumerable<Operator> operators,
            Func<Operator, Rule<Operator>> createRule, params Type[] operatorTypes)
        {
            IEnumerable<Operator> availableOperators = operators.Where(op => operatorTypes.Contains(op.GetType()));
            return availableOperators.Select(createRule);
        }
       
        private static SymbolsSet<Operator> CreateSymbols(IEnumerable<Operator> operators)
        {
            List<string> nonTerminalsNames = new List<string> 
            { 
                "V", "C", "InfGuard", "RegOp2Operands", "OpNode", "NoConstOpNode"
            };

            // Op0Node, Op1Node, ...
            nonTerminalsNames.AddRange(GetOpArityNodeSymbolNames(operators));

            // SumNode, SinNode, ...
            nonTerminalsNames.AddRange(operators.Where(op => op.Arity > 0).Select(GetOpNodeSymbolName));
            
            IEnumerable<Operator> terminalGuards = new Operator[] {OperatorsLibrary.Atan, OperatorsLibrary.Tanh, OperatorsLibrary.Sum, OperatorsLibrary.Abs};
            IEnumerable<Symbol<Operator>> terminals = operators.Concat(terminalGuards).Distinct().Select(op => new Symbol<Operator>(op, GetOpSymbolName(op)));
            IEnumerable<Symbol<Operator>> nonTerminals = nonTerminalsNames.Select(n => new Symbol<Operator>(n));
            return new SymbolsSet<Operator> { terminals, nonTerminals };
        }

        private static IEnumerable<string> GetOpArityNodeSymbolNames(IEnumerable<Operator> operators)
        {
            IEnumerable<int> availableArities = operators.Select(op => op.Arity).Distinct().OrderBy(a => a);
            return availableArities.Select(GetOpArityNodeSymbolName);
        }

        private static string GetOpArityNodeSymbolName(int arity)
        {
            return "Op" + arity + "Node";
        }

        private static string GetOpNodeSymbolName(Operator op)
        {
            return GetOpSymbolName(op) + "Node";
        }

        private static string GetOpSymbolName(Operator op)
        {
            return op.Name;
        }
    }
}
