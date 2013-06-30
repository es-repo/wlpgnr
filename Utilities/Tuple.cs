﻿namespace WallpaperGenerator.Utilities
{
    public class Tuple<T, T2>
    {
        public Tuple(T item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T2 Item2 { get; set; }

        public T Item1 { get; set; }
    }
}
