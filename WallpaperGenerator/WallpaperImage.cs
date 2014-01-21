using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WallpaperGenerator.FormulaRendering;
using Size = WallpaperGenerator.Utilities.Size;

namespace WallpaperGenerator.UI.Windows
{
    public class WallpaperImage
    {
        #region Fields

        private readonly WriteableBitmap _bitmap;
        private readonly Int32Rect _rect;
        private readonly int _stride;
        
        #endregion

        #region Properties

        public Size Size { get; private set; }

        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
        }

        #endregion

        #region Constructors

        public WallpaperImage(Size size)
        {
            Size = size;
            _bitmap = new WriteableBitmap(Size.Width, Size.Height, 96, 96, PixelFormats.Bgra32, null);
            _rect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            int bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7) / 8;
            _stride = _bitmap.PixelWidth * bytesPerPixel;
        }

        #endregion

        #region Public Methods

        public void Update(RenderedFormulaImage renderedFormulaImage)
        {
            byte[] colors = new byte[renderedFormulaImage.RedChannel.Length*4];
            for (int i = 0, j = 0; i < renderedFormulaImage.RedChannel.Length; i++, j+=4)
            {
                colors[j] = renderedFormulaImage.BlueChannel[i];
                colors[j + 1] = renderedFormulaImage.GreenChannel[i];
                colors[j + 2] = renderedFormulaImage.RedChannel[i];
                colors[j + 3] = 255;
            }
           _bitmap.WritePixels(_rect, colors, _stride, 0);
        }

        #endregion
    }
}
