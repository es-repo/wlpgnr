using System;
using MbUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class FuncUtilitiesTests
    {
        [Test]
        [Row(5, 3)]
        [Row(4, 3)]
        [Row(3, 2)]
        [Row(1, 0)]
        [Row(0, 5, ExpectedException = typeof(ArgumentException))]
        public void TestRepeat(int maxRepeats, int expectedResult)
        {
            int i = 0;
            Func<int> func = () => i++;
            int result = FuncUtilities.Repeat(func, n => n >= 3, maxRepeats);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
