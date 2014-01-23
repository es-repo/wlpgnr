using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Orientation = Android.Content.Res.Orientation;

namespace Android.Utilities
{
    public class DisplayExtensions
    {
        public static Point GetSize(Display display)
        {
            int width;
            int heigth;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.HoneycombMr2)
            {
                Point size = new Point();
                display.GetSize(size);

                width = size.X;
                heigth = size.Y;
            }
            else
            {
                 width = display.Width;
                 heigth = display.Height;
            }
            return new Point(width, heigth);
        }

        public static Point GetNaturalSize(Display display, Configuration configuration)
        {
            Point size = GetSize(display);
            Orientation orientation = GetNaturalOrientation(display, configuration);
            return (size.X > size.Y && orientation == Orientation.Portrait)
                || (size.X < size.Y && orientation == Orientation.Landscape ) 
                ? new Point(size.Y, size.X) 
                : size;
        }

        public static Orientation GetNaturalOrientation(Display display, Configuration configuration)
        {
            SurfaceOrientation rotation = display.Rotation;

            return ((rotation == SurfaceOrientation.Rotation0 || rotation == SurfaceOrientation.Rotation180) &&
                    configuration.Orientation == Orientation.Landscape)
                   || ((rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270) &&
                       configuration.Orientation == Orientation.Portrait)
                ? Orientation.Landscape
                : Orientation.Portrait;
        }
    }
}