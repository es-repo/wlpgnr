using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.App.Core
{
    public class WorkflowRenderResult
    {
        public FormulaRenderArguments FormulaRenderArguments { get; private set; }
        public FormulaRenderResult FormulaRenderResult { get; private set; }
        public FormulaBitmap Bitmap { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public WorkflowRenderResult(FormulaRenderArguments formulaRenderArguments, FormulaRenderResult formulaRenderResult, FormulaBitmap bitmap, TimeSpan elapsedTime)
        {
            FormulaRenderArguments = formulaRenderArguments;
            FormulaRenderResult = formulaRenderResult;
            Bitmap = bitmap;
            ElapsedTime = elapsedTime;
        }
    }
}
