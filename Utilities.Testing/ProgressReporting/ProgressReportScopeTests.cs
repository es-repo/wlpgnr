using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using TestingUtilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    [TestFixture]
    public class ProgressReportScopeTests
    {
        [Test]
        public void Test()
        {
            List<double> progress = new List<double>();
            Observer<double> progressObserver = new Observer<double>(progress.Add);

            ProgressReportScope progressReportScope = new ProgressReportScope();
            progressReportScope.Subscribe(progressObserver);

            using (ProgressReportScope childProgressReportScope = progressReportScope.CreateChildScope(0.5, "Test08"))
                DumbFuncA(childProgressReportScope);

            AssertExtensions.ExceptionExpected<ArgumentException>(() =>
            {
                using (var childScope = progressReportScope.CreateChildScope(0.7))
                    DumbFuncC(childScope);
            });

            using (progressReportScope.CreateChildScope(0.3))
            {
                AssertExtensions.ExceptionExpected<InvalidOperationException>(() =>
                {
                    using (progressReportScope.CreateChildScope(0.1))
                        Assert.Fail();
                });
            }

            using (var childScope = progressReportScope.CreateChildScope(0.2, "Test02"))
            {
                using (var childScope2 = childScope.CreateChildScope(0.25, "Test02025"))
                {
                    DumbFuncC(childScope2);
                    ProgressFullInfo progressFullInfo = progressReportScope.GetProgressFullInfo();
                    double[] expectedScopeProgresses = new [] { 0.85, 0.25, 1.0 };
                    CollectionAssert.AreEqual(expectedScopeProgresses, progressFullInfo.Select(sp => sp.Progress).ToArray());
                    string[] expectedScopeNames = new [] {"Test", "Test02", "Test02025" };
                    CollectionAssert.AreEqual(expectedScopeNames, progressFullInfo.Select(sp => sp.Name).ToArray());
                }

                using (var childScope2 = childScope.CreateChildScope(0.5))
                    DumbFuncC(childScope2);
            }

            progressReportScope.Complete();

            AssertExtensions.ExceptionExpected<InvalidOperationException>(progressReportScope.Complete);

            double[] expectedProgress = new [] {0.5, 0.8, 0.85, 0.95, 1};
            double[] progressNormilized = progress.Distinct().Select(p => Math.Round(p, 2)).ToArray();
            CollectionAssert.AreEqual(expectedProgress, progressNormilized);
        }

        private static void DumbFuncA(ProgressReportScope progressReportScope)
        {
            using (progressReportScope.CreateChildScope()) 
                DumbFuncB();
        }

        private static void DumbFuncB()
        {
        }

        private static void DumbFuncC(ProgressReportScope progressReportScope)
        {
            using (progressReportScope.CreateChildScope())
                DumbFuncB();
        }
    }
}
