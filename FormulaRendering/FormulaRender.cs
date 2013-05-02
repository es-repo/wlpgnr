using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            IEnumerable<double> formulaEvaluatedField = GetFormulaEvaluatedField(formulaTreeRoot, width, height);
            IEnumerable<Rgb> data = MapToRgb(formulaEvaluatedField);
            return new RenderedFormulaImage(data.ToArray(), width, height);
        }

        private static IEnumerable<double> GetFormulaEvaluatedField(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            double[] field = new double[width * height];
            
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    formulaTree.Variables[0].Value = x;
                    if (formulaTree.Variables.Length > 1)
                        formulaTree.Variables[1].Value = y;
                    int index = y * width + x;
                    field[index] = formulaTree.Evaluate();
                }
            }
            return field;
        }
        
        private static IEnumerable<Rgb> MapToRgb(IEnumerable<double> values)
        {
            IEnumerable<double> significantValues = GetSignificantValues(values);

            double mathExpectation = MathUtilities.MathExpectation(significantValues);
            double threeSigmas = MathUtilities.ThreeSigmas(significantValues);
            if (double.IsNegativeInfinity(threeSigmas))
                threeSigmas = double.MinValue;
            if (double.IsPositiveInfinity(threeSigmas))
                threeSigmas = double.MaxValue;  

            double rangeStart = mathExpectation - threeSigmas;
            double rangeEnd = mathExpectation + threeSigmas;
            if (rangeStart > rangeEnd)
            {
                double tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }

            IEnumerable<byte> colors = values.Select(v => (byte)MathUtilities.Map(v, rangeStart, rangeEnd, 0, 255));
            return colors.Select(c => new Rgb(c, c, c));
        }

        private static IEnumerable<double> GetSignificantValues(IEnumerable<double> values)
        {
            const double factor = 1e175;
            const double lowBound = double.MinValue * factor;
            const double highBound = double.MaxValue / factor;
            return values.Where(v => !double.IsNaN(v) && (v > lowBound) && (v < highBound));
        }
    }
}
