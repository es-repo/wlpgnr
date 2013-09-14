using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Formulas.Operators.Arithmetic;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeGenerator2Tests
    {
        [Test]
        public void TestCreateGrammar()
        {
            TestCreateGrammar(new[] { new Variable("x"), new Variable("y") });
            TestCreateGrammar(new Operator[] { new Variable("x"), new Variable("y"), OperatorsLibrary.Abs, OperatorsLibrary.Sin });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.IfG0 });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.IfG });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum });
            TestCreateGrammar(new Operator[] { new Variable("x"), OperatorsLibrary.Abs, OperatorsLibrary.Sin, OperatorsLibrary.Sum,
                OperatorsLibrary.Pow, OperatorsLibrary.IfG, OperatorsLibrary.Max, OperatorsLibrary.Mod, OperatorsLibrary.IfG0 });

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
            IEnumerable<string> expectedFromSymbols = new [] { "V", "C", "InfGuard", "RegOp2Operands", "OpNode", "NoConstOpNode" };
            expectedFromSymbols = expectedFromSymbols.Concat(operators.GroupBy(op => op.Arity).Select(g => "Op" + g.Key + "Node"))
                .Concat(operators.Where(op => op.Arity > 0).Select(op => op.Name + "Node"));
            if (operators.Any(op => op is Pow))
            {
                expectedFromSymbols = expectedFromSymbols.Concat("PowAltNode");
            }
            expectedFromSymbols = expectedFromSymbols.OrderBy(s => s);

            Grammar<Operator> grammar = FormulaTreeGenerator2.CreateGrammar(operators, () => 0, 1);
            IEnumerable<string> fromSymbols = grammar.Rules.Select(r => r.From.Name).OrderBy(s => s);
            CollectionAssert.AreEqual(expectedFromSymbols.ToArray(), fromSymbols.ToArray());
        }

        [Test]
        public void TestGenerate()
        {
            Func<double> createConstans = new EnumerableNext<double>(new double[] { 1, 2, 3 }.Repeat()).Next;
            TestGenerate(new[] { new Variable("x"), new Variable("y") }, createConstans, 1,
                "x");
        }

        private static void TestGenerate(IEnumerable<Operator> operators, Func<double> createConstant,  int minimalTreeDepth, string expectedSerializedTree)
        {
            FormulaTree formulaTree = FormulaTreeGenerator2.Generate(operators, createConstant, minimalTreeDepth);
            Assert.AreEqual(expectedSerializedTree, FormulaTreeSerializer.Serialize(formulaTree));
        }
    }
}
