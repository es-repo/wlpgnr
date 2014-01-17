using System;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.Testing;

namespace WallpaperGenerator.UI.Core.Testing
{
    [TestFixture]
    public class FormulaGenerationArgumentsTests
    {
        [Test]
        public void TestCreateRandom()
        {
            Random random = RandomMock.Setup(new []{0.1});
            Bounds<int> dimensionCountBounds = new Bounds<int>(4, 5);
            Bounds<int> minimalDepthBounds = new Bounds<int>(7, 8);
            Bounds leafProbabilityBounds = new Bounds(0.2, 0.3);
            Bounds constantProbabilityBounds = new Bounds(0.4, 0.5);
            Bounds constantsBounds = new Bounds(5, 10);
            Bounds unaryVsBinaryOperatorsProbabilityBounds = new Bounds(0.5, 0.5);
            Dictionary<Operator, Bounds> operatorAndProbabilityBoundsMap = new Dictionary<Operator, Bounds>
            {
                { OperatorsLibrary.Sin, new Bounds(1, 1) },
                { OperatorsLibrary.Sum, new Bounds(0.1, 0.1) }
            };
            FormulaGenerationArguments args = FormulaGenerationArguments.CreateRandom(random, dimensionCountBounds, minimalDepthBounds, leafProbabilityBounds, 
                constantProbabilityBounds, constantsBounds, operatorAndProbabilityBoundsMap, unaryVsBinaryOperatorsProbabilityBounds);

            Assert.AreEqual(0.1, args.OperatorAndProbabilityMap[OperatorsLibrary.Sum]);
            Assert.AreEqual(0.1, args.OperatorAndProbabilityMap[OperatorsLibrary.Sin]);
            Assert.AreEqual(5.5, args.CreateConstant());
            Assert.AreEqual(4, args.DimensionsCount);
            Assert.AreEqual(7, args.MinimalDepth);
            Assert.AreApproximatelyEqual(0.21, args.LeafProbability, 0.01);
            Assert.AreApproximatelyEqual(0.41, args.ConstantProbability, 0.01);
        }
    }
}
