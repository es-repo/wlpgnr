using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.App.Core
{
    public class FormulaGoodnessAnalyzer
    {
        public int MinVariablesCount { get; private set; }

        public FormulaGoodnessAnalyzer(int minVariablesCount)
        {
            MinVariablesCount = minVariablesCount;
        }

        public virtual bool Analyze(FormulaTree formulaTree)
        {
            return formulaTree.Variables.Length >= MinVariablesCount;
        }
    }
}
