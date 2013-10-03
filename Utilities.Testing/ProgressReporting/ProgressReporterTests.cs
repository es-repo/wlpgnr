using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    [TestFixture]
    public class ProgressReporterTests
    {
        [Test]
        public void TestNestedScopes()
        {
            List<double> progress = new List<double>();
            ProgressReporter.Subscribe(new ProgressObserver(s => progress.Add(s.Progress)));
            using (ProgressReporter.CreateScope())
            {
                using (ProgressReporter.CreateScope(0.5))
                    DumbFunc();

                using (ProgressReporter.CreateScope(0.2))
                {
                    using (ProgressReporter.CreateScope(0.5))
                        DumbFunc();

                    using (ProgressReporter.CreateScope(0.5))
                        DumbFunc();
                }
            }
            ProgressAssert.AreEqual(new[] { 0.5, 0.6, 0.7, 1.0 }, progress);
        }

        [RowTest]
        [Row(5, 1, 2, new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 })]
        [Row(2, 0.5, 2, new[] { 0.125, 0.25, 0.5, 0.625, 0.75, 1.0 })]
        public void TestNestedScopesWithSteps(int stepsCount, double childScopeSpan, int childScopeStepsCount, double[] expectedProgress)
        {
            List<double> progress = new List<double>();
            ProgressReporter.Subscribe(new ProgressObserver(s => progress.Add(s.Progress)));
            using (ProgressReporter.CreateScope(stepsCount))
            {
                for (int i = 0; i < stepsCount; i++)
                {
                    using (ProgressReporter.CreateScope(childScopeStepsCount, childScopeSpan))
                    {
                        for (int j = 0; j < childScopeStepsCount; j++)
                        {
                            ProgressReporter.Increase();
                        }
                    }

                    ProgressReporter.Increase();
                }
            }
            ProgressAssert.AreEqual(expectedProgress, progress);
        }

        [Test]
        public void TestComplete()
        {
            List<double> progress = new List<double>();
            ProgressReporter.Subscribe(new ProgressObserver(s => progress.Add(s.Progress)));
            ProgressReporter.CreateScope();
                ProgressReporter.CreateScope(0.5);
                    DumbFunc();
                ProgressReporter.Complete();
                ProgressReporter.CreateScope(0.2);
                    ProgressReporter.CreateScope(0.5);
                        DumbFunc();
                    ProgressReporter.Complete();
                    ProgressReporter.CreateScope(0.5);
                        DumbFunc();
                    ProgressReporter.Complete();
                ProgressReporter.Complete();
            ProgressReporter.Complete();
            
            ProgressAssert.AreEqual(new[] { 0.5, 0.6, 0.7, 1.0 }, progress);
        }

        private static void DumbFunc()
        {
        }
    }
}
