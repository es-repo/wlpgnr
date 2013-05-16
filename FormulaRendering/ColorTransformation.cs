namespace WallpaperGenerator.FormulaRendering
{
    public class ColorTransformation
    {
        public ColorChannelTransformation RedChannelTransformation { get; private set; }

        public ColorChannelTransformation GreenChannelTransformation { get; private set; }

        public ColorChannelTransformation BlueChannelTransformation { get; private set; }

        public ColorTransformation(ColorChannelTransformation redChannelTransformation,
            ColorChannelTransformation greenChannelTransformation,
            ColorChannelTransformation blueChannelTransformation)
        {
            RedChannelTransformation = redChannelTransformation;
            GreenChannelTransformation = greenChannelTransformation;
            BlueChannelTransformation = blueChannelTransformation;
        }
    }
}
