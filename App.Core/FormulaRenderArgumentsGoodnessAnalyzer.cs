using System.Linq;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.App.Core
{
    public class FormulaRenderArgumentsGoodnessAnalyzer
    {
        public int MinVariablesCount { get; private set; }

        public FormulaRenderArgumentsGoodnessAnalyzer(int minVariablesCount)
        {
            MinVariablesCount = minVariablesCount;
        }

        public virtual bool Analyze(FormulaRenderArguments formulaRenderArguments)
        {
            return IsVariablesCountOk(formulaRenderArguments) && IsRenderedImageNotEmpty(formulaRenderArguments);
        }

        public bool IsVariablesCountOk(FormulaRenderArguments formulaRenderArguments)
        {
            return formulaRenderArguments.FormulaTree.Variables.Length >= MinVariablesCount;
        }

        public bool IsRenderedImageNotEmpty(FormulaRenderArguments formulaRenderArguments)
        {
            Size imageSize = new Size(10, 10);
            FormulaRenderResult formulaRenderResult = new FormulaRenderResult(imageSize);
            FormulaRender.Render(formulaRenderArguments.FormulaTree, formulaRenderArguments.Ranges, imageSize, formulaRenderArguments.ColorTransformation, true, 1, formulaRenderResult);
            return formulaRenderResult.BlueChannel.All(b => b == formulaRenderResult.BlueChannel[0]) &&
                formulaRenderResult.RedChannel.All(b => b == formulaRenderResult.RedChannel[0]) &&
                formulaRenderResult.GreenChannel.All(b => b == formulaRenderResult.GreenChannel[0]);
        }
    }
}
