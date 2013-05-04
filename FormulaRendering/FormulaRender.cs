using System;
using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        private static readonly Random _random = new Random();

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
        
        private static IEnumerable<Rgb> MapToRgb(double[] values)
        {
            Func<double, double> redChannelTransformingFunction = CreateChannelTransformingFunction();
            IEnumerable<byte> redChannel = MapToColorChannel(values, redChannelTransformingFunction);

            Func<double, double> greenChannelTransformingFunction = CreateChannelTransformingFunction();
            IEnumerable<byte> greenChannel = MapToColorChannel(values, greenChannelTransformingFunction);

            Func<double, double> blueChannelTransformingunction = CreateChannelTransformingFunction();
            IEnumerable<byte> blueChannel = MapToColorChannel(values, blueChannelTransformingunction);

            IEnumerator<byte> greenChannelEnumerator = greenChannel.GetEnumerator();
            IEnumerator<byte> blueChannelEnumerator = blueChannel.GetEnumerator();
            foreach (byte r in redChannel)
            {
                greenChannelEnumerator.MoveNext();
                byte g = greenChannelEnumerator.Current;

                blueChannelEnumerator.MoveNext();
                byte b = blueChannelEnumerator.Current;

                yield return new Rgb(r, g, b);
            }
        }

        private static Func<double, double> CreateChannelTransformingFunction()
        {
            int aSign = _random.Next(-2, 2);
            int bSign = _random.Next(-2, 2);
            int cSign = _random.Next(-2, 2);

            double a = _random.NextDouble() * aSign;
            double b = _random.NextDouble() * bSign;
            double c = _random.NextDouble() * cSign;
            
            return v => v*v*v*a + v*v*b + v*c;
        }

        private static IEnumerable<byte> MapToColorChannel(IEnumerable<double> values, Func<double, double> channelTransformingFunction)
        {
            double[] channelValues = TransformChannelValues(values, channelTransformingFunction).ToArray();
            
            IEnumerable<double> significantValues = GetSignificantValues(channelValues);

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

            return channelValues.Select(v => (byte)MathUtilities.Map(v, rangeStart, rangeEnd, 0, 255));
        }

        private static IEnumerable<double> TransformChannelValues(IEnumerable<double> values, Func<double, double> channelTransformingFunction)
        {
            return values.Select(channelTransformingFunction);
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
