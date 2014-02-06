using System.Linq;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.App.Core
{
    public class FormulaRenderArgumentsGoodnessAnalyzer
    {
        private readonly FormulaRenderResult _formulaRenderResult;

         public int MinVariablesCount { get; private set; }

        public FormulaRenderArgumentsGoodnessAnalyzer(int minVariablesCount)
        {
            MinVariablesCount = minVariablesCount;
            Size emptynessCheckImageSize = new Size(8, 8);
           _formulaRenderResult = new FormulaRenderResult(emptynessCheckImageSize);
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
            FormulaRender.Render(formulaRenderArguments.FormulaTree, formulaRenderArguments.Ranges, _formulaRenderResult.Size, formulaRenderArguments.ColorTransformation, true, 1, _formulaRenderResult);
            return _formulaRenderResult.BlueChannel.Any(b => b != _formulaRenderResult.BlueChannel[0]) &&
                _formulaRenderResult.RedChannel.Any(b => b != _formulaRenderResult.RedChannel[0]) &&
                _formulaRenderResult.GreenChannel.Any(b => b != _formulaRenderResult.GreenChannel[0]);
        }
    }
}
