using System;
using MbUnit.Framework;

namespace TestingUtilities
{
    public static class AssertExtensions
    {
        public static void ExceptionExpected<T>(Action codeToFail) where T : Exception
        {
            try
            {
                codeToFail();
                Assert.Fail(string.Format("{0} is expected,", typeof(T)));
            }
            catch (T)
            {
            }
        }
    }
}
