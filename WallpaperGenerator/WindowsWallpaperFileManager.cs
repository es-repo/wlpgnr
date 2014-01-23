﻿using System.IO;
using System.Windows.Media.Imaging;
using UI.Shared;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Windows
{
    public class WindowsWallpaperFileManager : WallpaperFileManager
    {
        public WindowsWallpaperFileManager()
            : base("c://Temp")
        {
        }

        protected override void WriteImageAsPng(RenderedFormulaImage image, Stream stream)
        {
            WriteableBitmap bitmap = image.ToBitmap();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }
    }
}
