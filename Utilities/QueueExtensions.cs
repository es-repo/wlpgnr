using System.Collections.Generic;

namespace WallpaperGenerator.Utilities
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
        {
            for (int i = 0; i < count; i++)
                yield return queue.Dequeue();
        }
    }
}
