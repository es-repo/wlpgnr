using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Collections
{
    [TestFixture]
    public class CircularEnumerationTests
    {
        [Test]
        public void TestEnumerator()
        {
            CircularEnumeration<int> circularEnumeration = new CircularEnumeration<int>(new[] {1, 2, 3});
            CollectionAssert.AreEqual(new[] {1, 2, 3, 1, 2, 3, 1}, circularEnumeration.Take(7).ToArray()); 
        }
    }
}
