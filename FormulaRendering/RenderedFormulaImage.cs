using System;

namespace WallpaperGenerator.FormulaRendering
{
    public class RenderedFormulaImage
    {
        public int WidthInPixels { get; private set; }

        public int HeightInPixels { get; private set; }

        public Rgb[] Data { get; private set; }

        public RenderedFormulaImage(Rgb[] data, int widthInPixels, int heightInPixels)
        {
            if (widthInPixels*heightInPixels != data.Length)
                throw new ArgumentException("Width and height values doesn't correspond to data array.");
            
            WidthInPixels = widthInPixels;
            HeightInPixels = heightInPixels;
            Data = data;
        }
    }
}
