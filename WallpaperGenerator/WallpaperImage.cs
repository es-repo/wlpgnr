using System;
using System.Collections.Generic;
using System.Linq;  
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WallpaperGenerator.FormulaRendering;

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

        #region Properties

        public int WidthInPixels { get; private set; }

        public int HeightInPixels { get; private set; }

        public ImageSource Source
        {
            get { return _bitmap; }
        }

        #endregion

        #region Constructors

        public WallpaperImage(int widthInPixels, int heightInPixels)
        {
            WidthInPixels = widthInPixels;
            HeightInPixels = heightInPixels;

            _bitmap = new WriteableBitmap(WidthInPixels, HeightInPixels, 96, 96, PixelFormats.Bgra32, null);
            _rect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            _bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7) / 8;
            _stride = _bitmap.PixelWidth * _bytesPerPixel;
            int arraySize = _stride * _bitmap.PixelHeight;
            _colorArray = new byte[arraySize];
        }

        #endregion

        #region Public Methods

        public void Update(RenderedFormulaImage renderedFormulaImage)
        {
            IEnumerable<byte> colors = renderedFormulaImage.Data.Select(rgb => new byte[] {rgb.B, rgb.G, rgb.R, 255}).SelectMany(b => b); 
           _bitmap.WritePixels(_rect, colors.ToArray(), _stride, 0);
        }

        #endregion
    }
}
