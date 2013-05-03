namespace WallpaperGenerator.Formulas
{
    public class Range
    {
        public int Start { get; private set; }
        public int Count { get; private set; }

        public Range(int start, int count)
        {
            Start = start;
            Count = count;
        }
    }
}
