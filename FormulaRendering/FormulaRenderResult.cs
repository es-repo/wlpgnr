using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class FormulaRenderResult
    {
        public Size Size { get; private set; }

        public float[] EvaluatedValuesBuffer { get; private set; }
        public float[] ColorTranformedValuesBuffer { get; private set; }

        public byte[] RedChannel { get; private set; }
        public byte[] GreenChannel { get; private set; }
        public byte[] BlueChannel { get; private set; }

        public FormulaRenderResult(Size size)
        {
            Size = size;
            int l = size.Width*size.Height;
            EvaluatedValuesBuffer = new float[l];
            ColorTranformedValuesBuffer = new float[l];
            RedChannel = new byte[l];
            GreenChannel = new byte[l];
            BlueChannel = new byte[l];
        }
    }
}
