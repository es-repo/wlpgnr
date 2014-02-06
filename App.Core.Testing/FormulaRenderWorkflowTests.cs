using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Moq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.Testing;

namespace WallpaperGenerator.App.Core.Testing
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
                DimensionCountBounds = new Bounds<int>(2,3),
                MinimalDepthBounds = new Bounds<int>(4, 4),
                OperatorAndMaxProbabilityBoundsMap = new Dictionary<Operator, Bounds>{ {OperatorsLibrary.Sum, new Bounds(1, 1)}, {OperatorsLibrary.Sin, new Bounds(1, 1)} },
                FirstPredefinedFormulaRenderingArgumentsCount = 0,
                PredefinedFormulaRenderingArgumentsEnabled = false,
                UnBinOpBalancedTreeProbability = 0
            };

            Mock<FormulaRenderArgumentsGoodnessAnalyzer> formulaGoodnessAnalyzerMock = new Mock<FormulaRenderArgumentsGoodnessAnalyzer>(0, 0);
            formulaGoodnessAnalyzerMock.Setup(fa => fa.Analyze(It.IsAny<FormulaRenderArguments>())).Returns(false);

            _workflow = new FormulaRenderWorkflow(generationParams, new Size(3, 3), formulaGoodnessAnalyzerMock.Object, s => new FormulaBitmapMock(s),  1, _random);
        }

        [Test]
        public void TestGenerateFormulaRenderArguments()
        {
            FormulaRenderArguments args = _workflow.GenerateFormulaRenderArguments();
            const string expectedArgsString = "-21.6,-1.44\r\n0.4,1.68,-3.6,0.03;0,0.72,1.92,0;0,0,0,0\r\nSin Sin Sin x2";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestChangeColors()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.ChangeColors();
            const string expectedArgsString = "-21.6,-1.44\r\n0,0.72,1.92,0;0,0,0,0;0,0.72,1.92,0\r\nSin Sin Sin x2";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestTransformRanges()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.TransformRanges();
            const string expectedArgsString = "-0.36,0.64\r\n0.4,1.68,-3.6,0.03;0,0.72,1.92,0;0,0,0,0\r\nSin Sin Sin x2";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestRenderFormula()
        {
            Assert.IsFalse(_workflow.IsImageReady);
            WorkflowRenderResult result = _workflow.RenderFormula(true);
            Assert.IsNotNull(_workflow.FormulaRenderArguments);
            Assert.IsNotNull(result.FormulaRenderResult);
            Assert.AreNotEqual(TimeSpan.Zero, result.ElapsedTime);
        }

        [Test]
        public void TestState()
        {
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            _workflow.GenerateFormulaRenderArguments();
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            _workflow.ChangeColors();
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            _workflow.TransformRanges();
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            WorkflowRenderResult result = _workflow.RenderFormula(false);
            Assert.IsNotNull(result);
            Assert.IsFalse(_workflow.IsImageRendering);
            Assert.IsTrue(_workflow.IsImageReady);
            _workflow.ChangeColors();
            Assert.IsTrue(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            _workflow.TransformRanges();
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
            result = _workflow.RenderFormula(false);
            Assert.IsNotNull(result);
            _workflow.GenerateFormulaRenderArguments();
            Assert.IsFalse(_workflow.IsImageReady);
            Assert.IsFalse(_workflow.IsImageRendering);
        }

        [Test]
        public void TestGenerateFormulaRenderArgumentsWithPredefinedFormulas()
        {
            _workflow.GenerationParams.PredefinedFormulaRenderingArgumentsEnabled = true;
            _workflow.GenerationParams.FirstPredefinedFormulaRenderingArgumentsCount = 1;
            _workflow.GenerationParams.RepeatPredefinedFormulaRenderingArgumentsAfterEvery = 2;
            _workflow.GenerationParams.PredefinedFormulaRenderingFormulaRenderArgumentStrings = new[] {
@"0,10;0,10
0,0,0,0;0,0,0,0;1,1,1,0
Sum Cos x Sin y"};

            FormulaRenderArguments args = _workflow.GenerateFormulaRenderArguments();
            Assert.AreEqual("-0.24,0.76;0.72,1.92\r\n0,0,0,0;0,0,0,0;1,1,1,0\r\nSum Cos x Sin y", args.ToString());

            args = _workflow.GenerateFormulaRenderArguments();
            Assert.AreEqual("-21.6,-1.44\r\n0.4,1.68,-3.6,0.03;0,0.72,1.92,0;0,0,0,0\r\nSin Sin Sin x2", args.ToString());

            args = _workflow.GenerateFormulaRenderArguments();
            Assert.AreEqual("0,2.16;0,5.76\r\n0,0.72,1.92,0;0,0,0,0;0,0.72,1.92,0\r\nSum Cos x Sin y", args.ToString());

            args = _workflow.GenerateFormulaRenderArguments();
            Assert.AreEqual("-0.24,0.76;0.72,1.92\r\n0,0,0,0;0,0,0,0;0,0.72,1.92,0\r\nSin Sin Sum x2 x0", args.ToString());
        }
    }
}
