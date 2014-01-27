using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderResult
    {
        public FormulaRenderArguments FormulaRenderArguments { get; private set; }
        public RenderedFormulaImage Image { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public FormulaRenderResult(FormulaRenderArguments formulaRenderArguments, RenderedFormulaImage image, TimeSpan elapsedTime)
        {
            FormulaRenderArguments = formulaRenderArguments;
            Image = image;
            ElapsedTime = elapsedTime;
        }
    }
}
