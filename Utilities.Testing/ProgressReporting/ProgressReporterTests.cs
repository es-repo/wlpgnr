using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using TestingUtilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    [TestFixture]
    public class ProgressReporterTests
    {
        [Test]
        public void Test()
        {
            List<double> progress = new List<double>();
            Observer<double> progressObserver = new Observer<double>(progress.Add);

            ProgressReportScope progressReportScope = ProgressReporter.CreateScope();
            progressReportScope.Subscribe(progressObserver);

            using (ProgressReporter.CreateScope(0.5, "Test08")) 
                DumbFuncA();

            AssertExtensions.ExceptionExpected<ArgumentException>(() =>
            {
                using (ProgressReporter.CreateScope(0.7)) DumbFuncC();
            });

            using (progressReportScope.CreateChildScope(0.3)) DumbFuncC();
            
            using (ProgressReporter.CreateScope(0.2))
            {
                using (ProgressReporter.CreateScope(0.25)) DumbFuncC();

                using (ProgressReporter.CreateScope(0.5)) DumbFuncC();
            }

            progressReportScope.Complete();

            AssertExtensions.ExceptionExpected<InvalidOperationException>(progressReportScope.Complete);

            double[] expectedProgress = new[] { 0.5, 0.8, 0.85, 0.95, 1 };
            double[] progressNormilized = progress.Distinct().Select(p => Math.Round(p, 2)).ToArray();
            CollectionAssert.AreEqual(expectedProgress, progressNormilized);
        }

        private static void DumbFuncA()
        {
            using (ProgressReporter.CreateScope())
                DumbFuncB();
        }

        private static void DumbFuncB()
        {
        }

        private static void DumbFuncC()
        {
            using (ProgressReporter.CreateScope())
                DumbFuncB();
        }
    }
}
