using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            double[] formulaEvaluatedField = GetFormulaEvaluatedField(formulaTreeRoot, width, height).ToArray();
            IEnumerable<Rgb> data = MapToRgb(formulaEvaluatedField);
            return new RenderedFormulaImage(data.ToArray(), width, height);
        }

        private static IEnumerable<double> GetFormulaEvaluatedField(FormulaTreeNode formulaTreeRoot, int width, int height)
        {
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            return formulaTree.EvaluateRanges(new Range(0, width), new Range(0, height));
        }
        
        private static IEnumerable<Rgb> MapToRgb(IEnumerable<double> values)
        {
            //FormulaTreeNode redChannelDilutingFormula = FormulaTreeGenerator.CreateRandomFormulaTree(1, 3, 3, 0, OperatorsLibrary.All);
            //FormulaTreeNode greenChannelDilutingFormula = FormulaTreeGenerator.CreateRandomFormulaTree(1, 3, 3, 0, OperatorsLibrary.All);
            //FormulaTreeNode blueChannelDilutingFormula = FormulaTreeGenerator.CreateRandomFormulaTree(1, 3, 3, 0, OperatorsLibrary.All); 
            
            //IEnumerable<double> redChannelValues = 

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
            //return colors.Select(c => new Rgb((byte)(3*c*c + c), (byte)(0.5*c*c), c));
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
