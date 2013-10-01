﻿using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    [TestFixture]
    public class ProgressReportScopeTests
    {
        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestTooBigChildScope()
        {
            using (ProgressReportScope scope = new ProgressReportScope())
            {
                using (scope.CreateChildScope(0.7))
                    DumbFunc();

                using (scope.CreateChildScope(0.7))
                    DumbFunc();
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestChildSCopeIsAlreadeCreated()
        {
            using (ProgressReportScope scope = new ProgressReportScope())
            {
                using (scope.CreateChildScope(0.7))
                {
                    using(scope.CreateChildScope(0.1))
                        DumbFunc();
                }
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestScopeIsAlreadyCompleted()
        {
            ProgressReportScope scope = new ProgressReportScope();
            scope.Complete();
            scope.Complete();
        }

        [Test]
        public void TestNestedScopes()
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(progress.Add);
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

        [RowTest]
        [Row(5, 5, new []{0.2, 0.4, 0.6, 0.8, 1.0})]
        [Row(5, 2, new[] { 0.2, 0.4, 1.0 })]
        public void TestScopeWithSteps(int stepsCount, int stepsPassed, double[] expectedProgress)
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(progress.Add);
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

        [RowTest]
        [Row(5, 1, 2, new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 })]
        [Row(2, 0.5, 2, new[] { 0.125, 0.25, 0.5, 0.625, 0.75, 1.0 })]
        public void TestNestedScopesWithSteps(int stepsCount, double childScopeSpan, int childScopeStepsCount, double[] expectedProgress)
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(progress.Add);
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
        public void TestGetProgressFullInfo()
        {
            using (ProgressReportScope scope = new ProgressReportScope(name: "s1"))
            {
                using (scope.CreateChildScope(0.5))
                    DumbFunc();

                using (ProgressReportScope child = scope.CreateChildScope(0.2, "s2"))
                {
                    using (child.CreateChildScope(0.5))
                        DumbFunc();

                    using (child.CreateChildScope(0.5, "s3"))
                    {
                        DumbFunc();
                        ProgressFullInfo progressFullInfo = scope.GetProgressFullInfo();
                        double[] expectedScopeProgresses = new[] { 0.6, 0.5, 0.0 };
                        CollectionAssert.AreEqual(expectedScopeProgresses, progressFullInfo.Select(sp => sp.Progress).ToArray());
                        string[] expectedScopeNames = new[] { "s1", "s2", "s3" };
                        CollectionAssert.AreEqual(expectedScopeNames, progressFullInfo.Select(sp => sp.Name).ToArray());
                    }
                }
            }
        }

        [Test]
        public void TestChildScopeNotCompleted()
        {
            List<double> progress = new List<double>();
            ProgressObserver progressObserver = new ProgressObserver(progress.Add);
            using (ProgressReportScope scope = new ProgressReportScope())
            {
                scope.Subscribe(progressObserver);
                scope.CreateChildScope(0.5);
            }
            ProgressAssert.AreEqual(new[] { 0.5, 1.0 }, progress);
        }

        private static void DumbFunc()
        {
        }
    }
}
