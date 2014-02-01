using System;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.Testing;

namespace WallpaperGenerator.App.Core.Testing
{
    [TestFixture]
    public class FormulaGenerationArgumentsTests
    {
        [Test]
        public void TestCreateRandom()
        {
            Random random = RandomMock.Setup(new []{0.9});
            Bounds<int> dimensionCountBounds = new Bounds<int>(4, 5);
            Bounds<int> minimalDepthBounds = new Bounds<int>(7, 8);
            Bounds leafProbabilityBounds = new Bounds(0.2, 0.3);
            Bounds constantProbabilityBounds = new Bounds(0.4, 0.5);
            Bounds constantsBounds = new Bounds(5, 10);
            Bounds unaryVsBinaryOperatorsProbabilityBounds = new Bounds(0.5, 0.5);
            Dictionary<Operator, Bounds> operatorAndProbabilityBoundsMap = new Dictionary<Operator, Bounds>
            {
                { OperatorsLibrary.Sin, new Bounds(1, 1) },
                { OperatorsLibrary.Cos, new Bounds(1, 1) },
                { OperatorsLibrary.Ln, new Bounds(1, 1) },
                { OperatorsLibrary.Sum, new Bounds(0.1, 0.1) }
            };
            Operator[] obligatoryOperators = {OperatorsLibrary.Cos};
            FormulaGenerationArguments args = FormulaGenerationArguments.CreateRandom(random, dimensionCountBounds, minimalDepthBounds, leafProbabilityBounds,
                constantProbabilityBounds, constantsBounds, operatorAndProbabilityBoundsMap, obligatoryOperators, unaryVsBinaryOperatorsProbabilityBounds);

            Assert.AreEqual(0.1, args.OperatorAndProbabilityMap[OperatorsLibrary.Sum]);
            Assert.AreEqual(0.05, args.OperatorAndProbabilityMap[OperatorsLibrary.Sin]);
            Assert.AreEqual(0.05, args.OperatorAndProbabilityMap[OperatorsLibrary.Cos]);
            Assert.IsFalse(args.OperatorAndProbabilityMap.ContainsKey(OperatorsLibrary.Ln));
            Assert.AreEqual(9.5, args.CreateConstant());
            Assert.AreEqual(5, args.DimensionsCount);
            Assert.AreEqual(8, args.MinimalDepth);
            Assert.AreApproximatelyEqual(0.29, args.LeafProbability, 0.01);
            Assert.AreApproximatelyEqual(0.49, args.ConstantProbability, 0.01);
        }
    }
}
