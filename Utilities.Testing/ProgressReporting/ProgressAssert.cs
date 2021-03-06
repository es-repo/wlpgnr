﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing.ProgressReporting
{
    public static class ProgressAssert
    {
        public static void AreEqual(double[] expectedProgress, IEnumerable<double> progress)
        {
            double[] progressNormilized = progress.Distinct().Select(p => Math.Round(p, 3)).ToArray();
            CollectionAssert.AreEqual(expectedProgress, progressNormilized);
        }
    }
}
