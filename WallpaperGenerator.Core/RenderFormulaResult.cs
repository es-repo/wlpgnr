using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.Core
{
    public class RenderFormulaResult
    {
        public RenderedFormulaImage Image { get; private set; }
        public double[] FormulaEvaluatedValues { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public RenderFormulaResult(RenderedFormulaImage image, double[] formulaEvaluatedValues, TimeSpan elapsedTime)
        {
            Image = image;
            FormulaEvaluatedValues = formulaEvaluatedValues;
            ElapsedTime = elapsedTime;
        }
    }
}
