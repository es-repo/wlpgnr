namespace WallpaperGenerator.Utilities
{
    public class Size
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Square { get; private set; }

        public Size(int width = 0, int height = 0)
        {
            Width = width;
            Height = height;
            Square = width*height;
        }

        public static bool operator ==(Size a, Size b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(Size a, Size b)
        {
            return !(a == b);
        }

        protected bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (Width * 397) ^ Height; }
        }
    }
}
