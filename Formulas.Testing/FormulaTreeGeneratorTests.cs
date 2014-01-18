using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Collections;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.Testing;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeGeneratorTests
    {
        [Test]
        public void TestCreateGrammar()
        {
            TestCreateGrammar(new[] { new Variable("x"), new Variable("y") });
            TestCreateGrammar(new Operator[] { new Variable("x"), new Variable("y"), OperatorsLibrary.Abs, OperatorsLibrary.Sin });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.IfG });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum,
                OperatorsLibrary.Pow, OperatorsLibrary.IfG, OperatorsLibrary.Max, OperatorsLibrary.Mod });

            try
            {
                TestCreateGrammar(new Operator[] { OperatorsLibrary.Abs, OperatorsLibrary.Sin });
                Assert.Fail(typeof(ArgumentException).Name + " is expected.");
            }
            catch (ArgumentException)
            {
            }
        }

        private static void TestCreateGrammar(IEnumerable<Operator> operators)
        {
            IEnumerable<string> expectedFromSymbols = new[] { "V", "C", "InfGuard", "RegOp2Operands", "OpNode", "OpOrOp0NodeOperands", "OpOrVNodeOperands", "OpOrVNode" };
            expectedFromSymbols = expectedFromSymbols.Concat(operators.GroupBy(op => op.Arity).Select(g => "Op" + g.Key + "Node"))
                .Concat(operators.Where(op => op.Arity > 0).Select(op => op.Name + "Node"));
            
            expectedFromSymbols = expectedFromSymbols.OrderBy(s => s);

            Random random = RandomMock.Setup(EnumerableExtensions.Repeat(i => i * 0.1, 10));
            IDictionary<Operator, double> operatorAndProbabilityMap = new DictionaryExt<Operator, double>(operators.Select((op, i) => new KeyValuePair<Operator, double>(op, i % 2 + 1)));
            Grammar<Operator> grammar = FormulaTreeGenerator.CreateGrammar(random, operatorAndProbabilityMap, () => 0, 1, 0.3, 0.3);
            IEnumerable<string> fromSymbols = grammar.Rules.Select(r => r.From.Name).OrderBy(s => s);
            Assert.AreElementsEqual(expectedFromSymbols.ToArray(), fromSymbols.ToArray());
        }

        [Test]
        public void TestGenerate()
        {
            Func<double> createConstans = new EnumerableNext<double>(new double[] { 1, 2, 3 }.Repeat()).Next;

            TestGenerate(new[] { new Variable("x"), new Variable("y") }, createConstans, 3,
                "x");

            TestGenerate(new Operator[] { new Variable("x"), new Variable("y"), OperatorsLibrary.Abs, OperatorsLibrary.Sin }, createConstans, 5,
                "abs abs sin sin y");

            TestGenerate(new Operator[] { new Variable("x"), new Variable("y"), 
                OperatorsLibrary.Sum, OperatorsLibrary.Mul, OperatorsLibrary.Sin, OperatorsLibrary.Abs }, createConstans, 5,
                "sin sin abs mul x x");
            
            TestGenerate(new Operator[] { new Variable("x"), new Variable("y"), new Variable("z"),
                OperatorsLibrary.Pow, OperatorsLibrary.IfG }, createConstans, 6,
                "atan pow pow 1 atan atan pow z z 2");

            TestGenerate(new Operator[] { new Variable("x"), new Variable("y"), new Variable("z"),
                OperatorsLibrary.Div, OperatorsLibrary.Max }, createConstans, 4,
                    "atan div atan div y z max x x");

            TestGenerate(new Operator[] { new Variable("x"), new Variable("y"), new Variable("z"),
                OperatorsLibrary.Mod}, createConstans, 2,
                    "mod x sum abs y 0.01");
        }

        private static void TestGenerate(IEnumerable<Operator> operators, Func<double> createConstant,  int minimalTreeDepth, string expectedSerializedTree)
        {
            Random random = RandomMock.Setup(EnumerableExtensions.Repeat(i => i * 0.1, 10));
            IDictionary<Operator, double> operatorAndProbabilityMap = new DictionaryExt<Operator, double>(operators.Select((op, i) => new KeyValuePair<Operator, double>(op, i % 2 + 1)));
            FormulaTree formulaTree = FormulaTreeGenerator.Generate(random, operatorAndProbabilityMap, createConstant, minimalTreeDepth, 0.3, 0.3);
            Assert.AreEqual(expectedSerializedTree, FormulaTreeSerializer.Serialize(formulaTree).ToLower());
        }

        [Test]
        public void TestGetOpNodeProbabilities()
        {
            Dictionary<Operator, double> operatorAndProbabilityMap = new Dictionary<Operator, double>
            {
                { OperatorsLibrary.Sin, 30 },
                { OperatorsLibrary.Ln, 50 },
                { OperatorsLibrary.Sum, 20 },
                { OperatorsLibrary.Sub, 30 },
                { OperatorsLibrary.IfG, 0 },
            };
            double[] expectedOpNodeProbabilities = {80.0, 50};
            double[] opNodeProbabilities = FormulaTreeGenerator.GetOpNodeProbabilities(operatorAndProbabilityMap);
            Assert.AreElementsEqual(expectedOpNodeProbabilities, opNodeProbabilities);
        }
    }
}
