using System;
using System.Collections.Generic;
using NUnit.Framework;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    [TestFixture]
    public class ProgressReportScopeTests
    {
        [Test]
        public void TestTooBigChildScope()
        {
            Assert.That(() =>
            {
                using (ProgressReportScope scope = new ProgressReportScope())
                {
                    using (scope.CreateChildScope(0.7))
                        DumbFunc();

                    using (scope.CreateChildScope(0.7))
                        DumbFunc();
                }
            }, Throws.ArgumentException);
        }

        [Test]
        public void TestChildScopeIsAlreadeCreated()
        {
            Assert.That(() =>
            {
                using (ProgressReportScope scope = new ProgressReportScope())
                {
                    using (scope.CreateChildScope(0.7))
                    {
                        using (scope.CreateChildScope(0.1))
                            DumbFunc();
                    }
                }
            }, Throws.InvalidOperationException);
        }

        [Test]
        public void TestScopeIsAlreadyCompleted()
        {
            Assert.That(() =>
            {
                ProgressReportScope scope = new ProgressReportScope();
                scope.Complete();
                scope.Complete();
            }, Throws.InvalidOperationException);
        }

        [Test]
        public void TestNestedScopes()
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(s => progress.Add(s.Progress));
            using (ProgressReportScope scope = new ProgressReportScope())
            {
                scope.Subscribe(progressObserver);
                using (scope.CreateChildScope(0.5)) 
                    DumbFunc();

                using (ProgressReportScope child = scope.CreateChildScope(0.2))
                {
                    using (child.CreateChildScope(0.5))
                        DumbFunc();

                    using (child.CreateChildScope(0.5))
                        DumbFunc();
                }
            }
            ProgressAssert.AreEqual(new[] { 0.5, 0.6, 0.7, 1.0 }, progress);
        }

        [Test]
        [TestCase(5, 5, new []{0.2, 0.4, 0.6, 0.8, 1.0})]
        [TestCase(5, 2, new[] { 0.2, 0.4, 1.0 })]
        public void TestScopeWithSteps(int stepsCount, int stepsPassed, double[] expectedProgress)
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(s => progress.Add(s.Progress));
            using (ProgressReportScope scope = new ProgressReportScope(stepsCount))
            {
                scope.Subscribe(progressObserver);
                for (int i = 0; i < stepsPassed; i++)
                {
                    scope.Increase();
                }
            }
            ProgressAssert.AreEqual(expectedProgress, progress);
        }

        [Test]
        [TestCase(5, 1, 2, new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 })]
        [TestCase(2, 0.5, 2, new[] { 0.125, 0.25, 0.5, 0.625, 0.75, 1.0 })]
        public void TestNestedScopesWithSteps(int stepsCount, double childScopeSpan, int childScopeStepsCount, double[] expectedProgress)
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(s => progress.Add(s.Progress));
            using (ProgressReportScope scope = new ProgressReportScope(stepsCount))
            {
                scope.Subscribe(progressObserver);
                for (int i = 0; i < stepsCount; i++)
                {
                    using (ProgressReportScope childScope = scope.CreateChildScope(childScopeStepsCount, childScopeSpan))
                    {
                        for (int j = 0; j < childScopeStepsCount; j++)
                        {
                            childScope.Increase();
                        }
                    }
                    
                    scope.Increase();
                }
            }
            ProgressAssert.AreEqual(expectedProgress, progress);
        }
        
        [Test]
        public void TestChildScopeNotCompleted()
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(s => progress.Add(s.Progress));
            using (ProgressReportScope scope = new ProgressReportScope())
            {
                scope.Subscribe(progressObserver);
                scope.CreateChildScope(0.5);
            }
            ProgressAssert.AreEqual(new[] { 0.5, 1.0 }, progress);
        }

        [Test]
        public void TestScopeWithSpanAndInitProgress()
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(s => progress.Add(s.Progress));
            const int steps = 3;
            using (ProgressReportScope scope = new ProgressReportScope(steps, 0.3, 0.65))
            {
                scope.Subscribe(progressObserver);
                for ( int i = 0; i < steps; i++) scope.Increase();
            }
            ProgressAssert.AreEqual(new[] { 0.75, 0.85, 0.95 }, progress);
        }

        private static void DumbFunc()
        {
        }
    }
}
