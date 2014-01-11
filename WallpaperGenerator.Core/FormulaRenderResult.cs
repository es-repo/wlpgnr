using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderResult
    {
        public RenderedFormulaImage Image { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public FormulaRenderResult(RenderedFormulaImage image, TimeSpan elapsedTime)
        {
            Image = image;
            ElapsedTime = elapsedTime;
        }
    }
}
