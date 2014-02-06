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
            return IsVariablesCountOk(formulaRenderArguments);
        }

        public bool IsVariablesCountOk(FormulaRenderArguments formulaRenderArguments)
        {
            return formulaRenderArguments.FormulaTree.Variables.Length >= MinVariablesCount;
        }
    }
}
