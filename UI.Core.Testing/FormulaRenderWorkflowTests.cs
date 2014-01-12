﻿using System;
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
            FormulaRenderConfiguration configuration = new FormulaRenderConfiguration
            {
                DimensionCountBounds = new Bounds<int>(2,3),
                MinimalDepthBounds = new Bounds<int>(4, 4),
                Operators = new Operator[] { OperatorsLibrary.Sum, OperatorsLibrary.Sin }
            };
            _workflow = new FormulaRenderWorkflow(configuration, 3, 3, _random);
        }

        [Test]
        public void TestGenerateFormulaRenderArguments()
        {
            FormulaRenderArguments args = _workflow.GenerateFormulaRenderArguments();
            const string expectedArgsString =
@"3;3;-31.5,-2.1;-2.1,3.5;1;1.4
-36,-2.4,-2.4,0.5;22.4,0,-2.8,0.4;12.6,-27,-1.8,0.3
Sin Sum Sum x0 x0 Sin x1";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestChangeColors()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.ChangeColors();
            const string expectedArgsString =
@"3;3;-31.5,-2.1;-2.1,3.5;1;1.4
6,16,0,0.2;2,8.4,-18,0.1;0,3.6,9.6,0
Sin Sum Sum x0 x0 Sin x1";
            Assert.AreEqual(expectedArgsString, args.ToString());
            Assert.AreEqual(expectedArgsString, _workflow.FormulaRenderArguments.ToString());
        }

        [Test]
        public void TestTransformRanges()
        {
            _workflow.GenerateFormulaRenderArguments();
            FormulaRenderArguments args = _workflow.TransformRanges();
            const string expectedArgsString =
@"3;3;4.8,12.8;-1.6,0;1;1.25
-36,-2.4,-2.4,0.5;22.4,0,-2.8,0.4;12.6,-27,-1.8,0.3
Sin Sum Sum x0 x0 Sin x1";
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
            Assert.IsTrue(_workflow.IsImageReady);
        }
    }
}