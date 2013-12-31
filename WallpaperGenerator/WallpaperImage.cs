using System.IO;
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
        private readonly int _stride;
        
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

        public void SaveToFile(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_bitmap.Clone()));
                encoder.Save(stream);
                stream.Close();
            }
        }

        #endregion
    }
}
