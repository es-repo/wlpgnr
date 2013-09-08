using System;
using Moq;

namespace WallpaperGenerator.Utilities.Testing
{
    public static class RandomMock
    {
        public static Random Setup(params double[] nextDoubles)
        {
            int i = 0;
            Mock<Random> rndMock = new Mock<Random>();
            rndMock.Setup(r => r.NextDouble()).Returns(() => nextDoubles[i++]);
            return rndMock.Object;
        }
    }
}
