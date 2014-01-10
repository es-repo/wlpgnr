using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderResult
    {
        public RenderedFormulaImage Image { get; private set; }
        public double[] FormulaEvaluatedValues { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public FormulaRenderResult(RenderedFormulaImage image, double[] formulaEvaluatedValues, TimeSpan elapsedTime)
        {
            Image = image;
            FormulaEvaluatedValues = formulaEvaluatedValues;
            ElapsedTime = elapsedTime;
        }
    }
}
