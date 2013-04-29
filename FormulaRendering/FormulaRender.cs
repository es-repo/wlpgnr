using System;
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
            IEnumerable<byte> colors = MapToColors(formulaEvaluatedField);
            IEnumerable<Rgb> data = colors.Select(c => new Rgb(c, c, c)).ToArray();
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
        
        private static IEnumerable<byte> MapToColors(IEnumerable<double> values)
        {
            double expectedValue = GetExpectedValue(values);
            double threeSigmas = GetThreeSigmas(values);
            
            double rangeStart = expectedValue - threeSigmas;
            double rangeEnd = expectedValue + threeSigmas;
            if (rangeStart > rangeEnd)
            {
                double tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }

            return values.Select(v => (byte)Map(v, rangeStart, rangeEnd, 0, 255));
        }

        private static double Map(double value, double rangeStart, double rangeEnd, double mappedRangeStart, double mappedRangeEnd)
        {
            if (value < rangeStart)
                value = rangeStart;
            if (value > rangeEnd)
                value = rangeEnd;

            double range = rangeEnd - rangeStart;
            double mappedRange = mappedRangeEnd - mappedRangeStart;
            double scale = mappedRange/range;
            return (value - rangeStart) * scale + mappedRangeStart;
        }

        private static double GetExpectedValue(IEnumerable<double> values)
        {
            return values.Sum()/values.Count();
        }

        private static double GetVariance(IEnumerable<double> values)
        {
            double expectedValueOfSquares = GetExpectedValue(values.Select(v => v*v));
            double expectedValue = GetExpectedValue(values);
            return expectedValueOfSquares - expectedValue * expectedValue;
        }

        private static double GetStandardDeviation(IEnumerable<double> values)
        {
            double varianse = GetVariance(values);
            return Math.Sqrt(varianse);
        }

        private static double GetThreeSigmas(IEnumerable<double> values)
        {
            double standartDeviation = GetStandardDeviation(values);
            return 3*standartDeviation;
        }
    }
}
