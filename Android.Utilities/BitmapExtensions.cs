using System.IO;
using Android.Graphics;

namespace Android.Utilities
{
    public static class BitmapExtensions
    {
        public static void SaveToFile(this Bitmap bitmap, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Flush();
            }
        }
    }
}