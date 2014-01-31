using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;

namespace Android.Utilities
{
    public static class WallpaperManagerExtensions
    {
        public static Point GetDesiredSize(this WallpaperManager wallpaperManager, Display display, Configuration configuration)
        {
            int width = wallpaperManager.DesiredMinimumWidth;
            int height = wallpaperManager.DesiredMinimumHeight;
            bool noWidth = width < 1;
            bool noHeight = height < 1;
            if (noWidth || noHeight)
            {
                Point displaySize = DisplayExtensions.GetNaturalSize(display, configuration);
                if (noWidth)
                {
                    width = displaySize.X;
                }
                if (noHeight)
                {
                    height = displaySize.Y;
                }
            }

            return new Point(width, height);
        }

        public static void SetBitmapWithExactScreenSize(this WallpaperManager wallpaperManager, Bitmap bitmap)
        {
            // On TouchWiz (Samsung Galaxy S3,4 and etc) desired min width is equal to height but not to real screen width.
            // To proper set bitmap on these phones need to create blank bitmap and put specified one into center of the blank.
            //if (bitmap.Width < wallpaperManager.DesiredMinimumWidth)
            //{
            //    bitmap = CreateOverlayedBitmap(bitmap, wallpaperManager.DesiredMinimumWidth, bitmap.Height);
            //}
            wallpaperManager.SetBitmap(bitmap);
        }

        private static Bitmap CreateOverlayedBitmap(Bitmap bitmap, int width, int height)
        {
            int[] originalPixels = new int[bitmap.Width * bitmap.Height];
            bitmap.GetPixels(originalPixels, 0, bitmap.Width, 0, 0, bitmap.Width, bitmap.Height);
            int[] pixels = new int[width * height];
            int xStart = (width - bitmap.Width) / 2;
            int xEnd = (width + bitmap.Width) / 2;
            for (int y = 0; y < height; y++)
            {
                int rowStart = y*width;
                int originalRowStart = y*bitmap.Width;
                for (int ox = 0, x = xStart; x < xEnd; x++, ox++)
                {
                    pixels[rowStart + x] = originalPixels[originalRowStart + ox];
                }
            }
            return Bitmap.CreateBitmap(pixels, width, height, bitmap.GetConfig());
        }
    }
}