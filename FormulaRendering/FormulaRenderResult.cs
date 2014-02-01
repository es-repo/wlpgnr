using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class FormulaRenderResult
    {
        public Size Size { get; private set; }

        public double[] EvaluatedValuesBuffer { get; private set; }
        public double[] ColorTranformedValuesBuffer { get; private set; }

        public byte[] RedChannel { get; private set; }
        public byte[] GreenChannel { get; private set; }
        public byte[] BlueChannel { get; private set; }

        public FormulaRenderResult(Size size)
        {
            Size = size;
            int l = size.Width*size.Height;
            EvaluatedValuesBuffer = new double[l];
            ColorTranformedValuesBuffer = new double[l];
            RedChannel = new byte[l];
            GreenChannel = new byte[l];
            BlueChannel = new byte[l];
        }
    }
}
