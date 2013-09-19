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
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Formulas
{
    public static class FormulaTreeGenerator
    {
        public static FormulaTree Generate(IEnumerable<Operator> operators, Func<double> createConstant, int dimensionsCount, int minimalDepth,
            Random random, double constantProbability, IDictionary<int, double> arityAndOpNodeProbabiltyMap)
        {
            IEnumerable<string> variableNames = EnumerableExtensions.Repeat(i => "x" + i.ToString(CultureInfo.InvariantCulture), dimensionsCount);
            IEnumerable<Operator> variables = variableNames.Select(n => new Variable(n));
            return Generate(operators.Concat(variables), createConstant, minimalDepth, random, constantProbability, arityAndOpNodeProbabiltyMap);
        }

        public static FormulaTree Generate(IEnumerable<Operator> operators, Func<double> createConstant, int minimalDepth, Random random,
            double constantProbability, IDictionary<int, double> arityAndOpNodeProbabiltyMap)
        {
            Grammar<Operator> grammar = CreateGrammar(operators, createConstant, minimalDepth, random, constantProbability, arityAndOpNodeProbabiltyMap); 
            TreeNode<Operator> treeRoot = TreeGenerator.Generate(grammar, "OpNode", op => op.Arity);
            return new FormulaTree(treeRoot);
        }

        public static Grammar<Operator> CreateGrammar(IEnumerable<Operator> operators, Func<double> createConstant, int minimalDepth, Random random,
            double constantProbability, IDictionary<int, double> arityAndOpNodeProbabiltyMap)
        {
            if (!operators.OfType<Variable>().Any())
                throw new ArgumentException("Operators should have at least one variable.", "operators");

            // V -> x1|x2|...
            // C -> c1|c2|...
            // Op0Node -> V

            // AbsNode -> abs OpNode
            // SqrtNode -> sqrt OpNode
            // CbrtNode -> cbrt OpNode
            // SinNode -> sin OpNode
            // CosNode -> cos OpNode
            // AtanNode -> atan OpNode
            // TanhNode -> tanh OpNode
            // InfGuard -> atan|tanh
            // Pow2Node -> InfGuard pow2 OpNode
            // Pow3Node -> InfGuard pow3 OpNode
            // LnNode -> InfGuard ln OpNode
            // SinhNode -> InfGuard sinh OpNode
            // CoshNode -> InfGuard cosh OpNode
            // Op1Node -> AbsNode|SqrtNode|CbrtNode|SinNode|CosNode|AtanNode|TanhNode|Pow2Node|Pow3Node|LnNode|SinhNode|CoshNode

            // OpOrConstOperands -> (OpNode C)|(C OpNode)
            // RegOp2Operands -> (OpNode OpNode)|OpOrConstOperands
            // SumNode -> sum RegOp2Operands
            // SubNode -> sub RegOp2Operands
            // MulNode -> mul RegOp2Operands
            // DivNode -> (InfGuard div OpNode OpNode)|(div OpOrConstOperands)
            // PowNode -> (InfGuard pow RegOp2Operands)|(pow (OpNode InfGuard C)|(C InfGuard OpNode))
            // MaxNode -> max OpNode OpNode
            // ModNode -> mod (OpNode sum abs OpNode 0.0001)|(OpNode sum abs OpNode 0.01)
            // Op2Node -> DivNode|PowNode|MaxNode|ModNode

            // Ifg0Node -> ifg0 OpNode OpNode OpNode
            // Op3Node -> Ifg0Node

            // IfgNode -> ifg OpNode OpNode OpNode OpNode 
            // Op4Node -> IfgNode

            // OpNode -> Op0Node|Op1Node|Op2Node|Op3Node|Op4Node

            SymbolsSet<Operator> s = CreateSymbols(operators);

            IEnumerable<Symbol<Operator>> opArityNodeSymbols = GetOpArityNodeSymbolNames(operators).Select(n => s[n]).ToArray();
            IEnumerable<double> opNodesProbabilities = NormalizeOpNodeProbabilities(operators, arityAndOpNodeProbabiltyMap);
            Func<IEnumerable<Rule<Operator>>, RuleSelector<Operator>> createConstRuleSelector = rs => new RandomRuleSelector<Operator>(random, rs, new[] { 1 - constantProbability, constantProbability });
            List<Rule<Operator>> rules = new List<Rule<Operator>>
            {
                new Rule<Operator>(s["C"], () => new[] { new Symbol<Operator>(new Constant(createConstant()))}),
                new OrRule<Operator>(s["V"], rs => new RandomRuleSelector<Operator>(random, rs), operators.OfType<Variable>().Select(v => s[v.Name])),
                new OrRule<Operator>(s["Op0Node"], 
                    new []{ s["V"] }), 

                new OrRule<Operator>(s["InfGuard"], rs => new RandomRuleSelector<Operator>(random, rs),
                    new []{s[GetOpSymbolName(OperatorsLibrary.Atan)], s[GetOpSymbolName(OperatorsLibrary.Tanh)]}), 
                
                new OrRule<Operator>(s["RegOp2Operands"], 
                    createConstRuleSelector,
                    new Rule<Operator>(new[]{s["OpNode"], s["OpNode"]}), 
                    new Rule<Operator>(new[]{s["OpOrConstOperands"]})), 

                new OrRule<Operator>(s["OpOrConstOperands"], 
                    rs => new RandomRuleSelector<Operator>(random, rs),
                    new Rule<Operator>(new[]{s["C"], s["OpNode"]}), 
                    new Rule<Operator>(new[]{s["OpNode"], s["C"]})),

                new OrRule<Operator>(s["OpNode"], 
                    rs => new TreeGeneratingRuleSelector<Operator>(minimalDepth, rs, 
                        rls => new RandomRuleSelector<Operator>(random, rls, opNodesProbabilities)),
                    opArityNodeSymbols), 
            };

            // AbsNode -> abs OpNode, 
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["OpNode"] }),
                typeof(Abs), typeof(Sqrt), typeof(Cbrt), typeof(Sin), typeof(Cos), typeof(Atan), typeof(Tanh)));

            // Pow2Node -> InfGuard pow2 OpNode
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["OpNode"] }),
                typeof(Pow2), typeof(Pow3), typeof(Ln), typeof(Sinh), typeof(Cosh)));

            // SumNode -> sum RegOp2Operands
            // ...
            rules.AddRange(CreateOpNodeRules(operators, 
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["RegOp2Operands"] }),
                typeof(Sum), typeof(Sub), typeof(Mul)));

            // DivNode -> (InfGuard div OpNode OpNode)|(div OpOrConstOperands)
            rules.AddRange(CreateOpNodeRules(operators,
                op =>
                    new OrRule<Operator>(s[GetOpNodeSymbolName(op)],
                        createConstRuleSelector,
                        new Rule<Operator>(new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["OpNode"], s["OpNode"] }),
                        new Rule<Operator>(new[] { s[GetOpSymbolName(op)], s["OpOrConstOperands"] })),
                typeof(Div)));

            // PowNode -> (InfGuard pow RegOp2Operands)|(pow (OpNode InfGuard C)|(C InfGuard OpNode))
            rules.AddRange(CreateOpNodeRules(operators,
                op => new OrRule<Operator>(s[GetOpNodeSymbolName(op)],
                    createConstRuleSelector,
                    new Rule<Operator>(new[] { s["InfGuard"], s[GetOpSymbolName(op)], s["RegOp2Operands"] }),
                    new AndRule<Operator>(new Rule<Operator>(new[]{s[GetOpSymbolName(op)]}),
                        new OrRule<Operator>(
                            new Rule<Operator>(new []{s["OpNode"], s["InfGuard"], s["C"]}),
                            new Rule<Operator>(new []{s["C"], s["InfGuard"], s["OpNode"]})))),
                typeof(Pow)));

            // ModNode -> mod (OpNode sum abs OpNode 0.01)|OpOrConstOperands
            rules.AddRange(CreateOpNodeRules(operators,
                op => new AndRule<Operator>(s[GetOpNodeSymbolName(op)], 
                    new Rule<Operator>(new[] { s[GetOpSymbolName(op)] }),
                    new OrRule<Operator>(createConstRuleSelector,
                        new Rule<Operator>(new[] { s["OpNode"], s[GetOpSymbolName(OperatorsLibrary.Sum)], s[GetOpSymbolName(OperatorsLibrary.Abs)], s["OpNode"], new Symbol<Operator>(new Constant(0.01))}),
                        new Rule<Operator>(new[] { s["OpOrConstOperands"] }))),
                typeof(Mod)));

            // MaxNode -> max OpNode OpNode
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["OpNode"], s["OpNode"] }),
                typeof(Max)));

            // Ifg0Node -> ifg0 OpNode OpNode OpNode
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["OpNode"], s["OpNode"], s["OpNode"] }),
                typeof(IfG0)));

            // IfgNode -> ifg OpNode OpNode OpNode OpNode 
            rules.AddRange(CreateOpNodeRules(operators,
                op => new Rule<Operator>(s[GetOpNodeSymbolName(op)], new[] { s[GetOpSymbolName(op)], s["OpNode"], s["OpNode"], s["OpNode"], s["OpNode"] }),
                typeof(IfG)));

            // Op1Node -> AbsNode|SqrtNode|...
            // Op2Node -> DivNode|PowNode|...
            // ...
            IEnumerable<IGrouping<int, Operator>> operatorsByArity = operators.GroupBy(op => op.Arity).Where(g => g.Key > 0 && g.Any());
            rules.AddRange(operatorsByArity.Select(g =>
                new OrRule<Operator>(s[GetOpArityNodeSymbolName(g.Key)], rls => new RandomRuleSelector<Operator>(random, rls), g.Select(op => s[GetOpNodeSymbolName(op)]))));
            
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
                "V", "C", "InfGuard", "RegOp2Operands", "OpOrConstOperands", "OpNode"
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

        public static IEnumerable<double> NormalizeOpNodeProbabilities(IEnumerable<Operator> operators, 
            IEnumerable<KeyValuePair<int, double>> arityAndProbabiltyMap)
        {
            IEnumerable<double> probabilities = arityAndProbabiltyMap.Where(e => operators.Any(op => op.Arity == e.Key)).Select(e => e.Value);
            return MathUtilities.Normalize(probabilities);
        }
    }
}
