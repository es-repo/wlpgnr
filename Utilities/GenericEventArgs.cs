using System;

namespace WallpaperGenerator.Utilities
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Object { get; set; }

        public GenericEventArgs(T @object)
        {
            Object = @object;
        }
    }
}
