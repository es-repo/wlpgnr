namespace WallpaperGenerator.FormulaRendering
{
    public class Rgb
    {
        public byte R { get; private set; }

        public byte G { get; private set; }

        public byte B { get; private set; }

        public Rgb(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
