using System;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class RenderedFormulaImage
    {
        public Size Size { get; private set; }
        
        public byte[] RedChannel { get; private set; }
        public byte[] GreenChannel { get; private set; }
        public byte[] BlueChannel { get; private set; }

        public RenderedFormulaImage(byte[] redChannel, byte[] greenChannel, byte[] blueChannel, Size size)
        {
            Size = size;
            if (Size.Width * Size.Height != redChannel.Length)
                throw new ArgumentException("Width and height values doesn't correspond to data array.");
            
            RedChannel = redChannel;
            GreenChannel = greenChannel;
            BlueChannel = blueChannel;
        }
    }
}
