using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WallpaperGenerator
{
    public class WallpaperImage
    {
        #region Fields

        private readonly WriteableBitmap _bitmap;
        private readonly Int32Rect _rect;
        private readonly int _bytesPerPixel;
        private readonly int _stride;
        private readonly byte[] _colorArray;

        #endregion

        #region Constructors

        #region Properties

        public ImageSource Source
        {
            get { return _bitmap; }
        }
        
        #endregion

        public WallpaperImage()
        {
            _bitmap = new WriteableBitmap(800, 800, 96, 96, PixelFormats.Bgra32, null);
            _rect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            _bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7) / 8;
            _stride = _bitmap.PixelWidth * _bytesPerPixel;
            int arraySize = _stride * _bitmap.PixelHeight;
            _colorArray = new byte[arraySize];
        }

        #endregion

        #region Public Methods

        public void Update()
        {
            var value = new Random();
            value.NextBytes(_colorArray);
            _bitmap.WritePixels(_rect, _colorArray, _stride, 0);
        }

        #endregion
    }
}
