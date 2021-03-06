﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class RuleTests
    {
        [Test]
        public void TestProduce()
        {
            Rule<string> rule = new Rule<string>(new Symbol<string>("A"), new[]
                {
                    new Symbol<string>("a", ""), 
                    new Symbol<string>("b", ""), 
                    new Symbol<string>("c", "")
                });
            IEnumerable<Symbol<string>> to = rule.Produce();
            string[] sequence = to.Select(s => s.Value).ToArray();
            string[] expectedSequence = { "a", "b", "c"};
            CollectionAssert.AreEqual(expectedSequence, sequence);
        }

        [Test]
        public void TestApplyWithSpecificFunc()
        {
            Rule<int> rule = new Rule<int>(new Symbol<int>("A"), 
                () => Enumerable.Repeat(0, 5).Select((e, i) => (i * 2)).Select(v => new Symbol<int>(v)));

            IEnumerable<Symbol<int>> to = rule.Produce();
            int[] sequence = to.Select(s => s.Value).ToArray();
            int[] expectedSequence = {0, 2, 4, 6, 8};
            CollectionAssert.AreEqual(expectedSequence, sequence);
        }

        [Test]
        public void TestCompositeRules()
        {            
            Symbol<string> a = new Symbol<string>("a", "a");
            Symbol<string> b = new Symbol<string>("b", "b");
            Symbol<string> c = new Symbol<string>("c", "c");
            Symbol<string> d = new Symbol<string>("d", "d");

            RuleAssert.AssertGeneratedSequences(

                // R -> (aa|bb)(cc|dd)
                new AndRule<string>(
                    new OrRule<string>(
                        new Rule<string>(new[] { a, a }),
                        new Rule<string>(new[] { b, b })),
                    new OrRule<string>(
                        new Rule<string>(new[] { c, c }),
                        new Rule<string>(new[] { d, d }))),

                    new[]
                    {
                        new[] {a, a, c, c},
                        new[] {b, b, d, d}
                    });
        }
    }
}
