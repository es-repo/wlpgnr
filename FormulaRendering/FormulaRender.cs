using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            double[] formulaEvaluatedField = GetFormulaEvaluatedField(formulaTreeRoot, width, height);
            IEnumerable<double> formulaEvaluatedFieldCountable = formulaEvaluatedField
                .Select(v => double.IsNaN(v) ? 0 : double.IsNegativeInfinity(v) ? double.MinValue : double.IsPositiveInfinity(v) ? double.MaxValue : v);
            IEnumerable<Rgb> data = MapToRgb(formulaEvaluatedFieldCountable);
            return new RenderedFormulaImage(data.ToArray(), width, height);
        }

        private static double[] GetFormulaEvaluatedField(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            double[] field = new double[width * height];
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    formulaTree.Variables[0].Value = x;
                    formulaTree.Variables[1].Value = y;
                    int index = y * width + x;
                    field[index] = formulaTree.Evaluate();
                }
            }
            return field;
        }
        
        private static IEnumerable<Rgb> MapToRgb(IEnumerable<double> values)
        {
            double mathExpectation = MathUtilities.MathExpectation(values);
            double threeSigmas = MathUtilities.ThreeSigmas(values);
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
    }
}
