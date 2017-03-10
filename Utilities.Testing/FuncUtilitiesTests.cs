using System;
using NUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class FuncUtilitiesTests
    {
        [TestCase(5, 3)]
        [TestCase(4, 3)]
        [TestCase(3, 2)]
        [TestCase(1, 0)]
        public void TestRepeat(int maxRepeats, int expectedResult)
        {
            int i = 0;
            Func<int> func = () => i++;
            int result = FuncUtilities.Repeat(func, n => n >= 3, maxRepeats);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TestRepeatFailed()
        {
            Assert.Throws<ArgumentException>(() => FuncUtilities.Repeat(() => 0, i => true, 0));
        }
    }
}