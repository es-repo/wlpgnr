using System;
using System.Collections.Generic;
using Moq;

namespace WallpaperGenerator.Utilities.Testing
{
    public static class RandomMock
    {
        public static Random Setup(IEnumerable<double> nextDoubles)
        {
            Mock<Random> rndMock = new Mock<Random>();
            EnumerableNext<double> loopedDoubles = new EnumerableNext<double>(nextDoubles.Repeat());
            rndMock.Setup(r => r.NextDouble()).Returns(loopedDoubles.Next);
            return rndMock.Object;
        }
    }
}
