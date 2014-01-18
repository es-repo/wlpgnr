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
    public class FormulaRenderWorkflowTests
    {
        private Random _random;
        private FormulaRenderWorkflow _workflow;

        [SetUp]
        public void SetUp()
        {
            _random = RandomMock.Setup(EnumerableExtensions.Repeat(i => i * 0.1, 10));
            FormulaRenderArgumentsGenerationParams generationParams = new FormulaRenderArgumentsGenerationParams
            {
                WidthInPixels = 3,
                HeightInPixels = 3,
                DimensionCountBounds = new Bounds<int>(2,3),
                MinimalDepthBounds = new Bounds<int>(4, 4),
                OperatorAndMaxProbabilityBoundsMap = new Dictionary<Operator, Bounds>{ {OperatorsLibrary.Sum, new Bounds(1, 1)}, {OperatorsLibrary.Sin, new Bounds(1, 1)} }
            };
            _workflow = new FormulaRenderWorkflow(generationParams, _random);
        }

        [Test]
        public void TestGenerateFormulaRenderArguments()
        {
            FormulaRenderArguments args = _workflow.GenerateFormulaRenderArguments();
            const string expectedArgsString = "3;3;0,15.36;-1.92,0;1;1.35\r\n4.48,0,-0.56,0.4;2.52,-5.4,-0.36,0.3;1.2,3.2,0,0.2\r\nSum Sin Sin x1 Sum Sin x0 0.01";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestChangeColors()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.ChangeColors();
            const string expectedArgsString = "3;3;0,15.36;-1.92,0;1;1.35\r\n0.4,1.68,-3.6,0.1;0,0.72,1.92,0;-0.12,0.2,0.84,0.9\r\nSum Sin Sin x1 Sum Sin x0 0.01";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestTransformRanges()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.TransformRanges();
            const string expectedArgsString = "3;3;1.2,5.04;-10.8,-0.72;1;1.2\r\n4.48,0,-0.56,0.4;2.52,-5.4,-0.36,0.3;1.2,3.2,0,0.2\r\nSum Sin Sin x1 Sum Sin x0 0.01";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestRenderFormulaAsync()
        {
            _workflow.GenerateFormulaRenderArguments();
            Assert.IsFalse(_workflow.IsImageReady);
            FormulaRenderResult formulaRenderResult = _workflow.RenderFormulaAsync(null).Result;
            Assert.IsNotNull(formulaRenderResult.Image);
            Assert.AreNotEqual(TimeSpan.Zero, formulaRenderResult.ElapsedTime);
        }

        [Test]
        public void TestState()
        {
            Assert.IsFalse(_workflow.IsImageReady);
            _workflow.GenerateFormulaRenderArguments();
            Assert.IsFalse(_workflow.IsImageReady);
            _workflow.ChangeColors();
            Assert.IsFalse(_workflow.IsImageReady);
            _workflow.TransformRanges();
            Assert.IsFalse(_workflow.IsImageReady);
            FormulaRenderResult formulaRenderResult = _workflow.RenderFormulaAsync(null).Result;
            Assert.IsNotNull(formulaRenderResult);
            Assert.IsTrue(_workflow.IsImageReady);
            _workflow.ChangeColors();
            Assert.IsTrue(_workflow.IsImageReady);
            _workflow.TransformRanges();
            Assert.IsFalse(_workflow.IsImageReady);
            formulaRenderResult = _workflow.RenderFormulaAsync(null).Result;
            Assert.IsNotNull(formulaRenderResult);
            _workflow.GenerateFormulaRenderArguments();
            Assert.IsFalse(_workflow.IsImageReady);
        }
    }
}
