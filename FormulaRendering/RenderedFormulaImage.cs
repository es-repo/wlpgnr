using System;

namespace WallpaperGenerator.FormulaRendering
{
    public class RenderedFormulaImage
    {
        public int WidthInPixels { get; private set; }

        public int HeightInPixels { get; private set; }

        public byte[] RedChannel { get; private set; }
        public byte[] GreenChannel { get; private set; }
        public byte[] BlueChannel { get; private set; }

        public RenderedFormulaImage(byte[] redChannel, byte[] greenChannel, byte[] blueChannel, int widthInPixels, int heightInPixels)
        {
            if (widthInPixels * heightInPixels != redChannel.Length)
                throw new ArgumentException("Width and height values doesn't correspond to data array.");
            
            WidthInPixels = widthInPixels;
            HeightInPixels = heightInPixels;
            RedChannel = redChannel;
            GreenChannel = greenChannel;
            BlueChannel = blueChannel;
        }
    }
}
