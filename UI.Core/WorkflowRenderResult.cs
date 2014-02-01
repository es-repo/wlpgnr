using System;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Core
{
    public class WorkflowRenderResult
    {
        public FormulaRenderArguments FormulaRenderArguments { get; private set; }
        public FormulaRenderResult FormulaRenderResult { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public WorkflowRenderResult(FormulaRenderArguments formulaRenderArguments, FormulaRenderResult formulaRenderResult, TimeSpan elapsedTime)
        {
            FormulaRenderArguments = formulaRenderArguments;
            FormulaRenderResult = formulaRenderResult;
            ElapsedTime = elapsedTime;
        }
    }
}
