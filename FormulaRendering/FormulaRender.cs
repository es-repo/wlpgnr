using System;
using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

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

            FormulaTreeNode redChannelDilutingFormulaTreeRoot = CreateChannelDilutingFormulaTreeRoot();
            IEnumerable<byte> redChannel = MapToColorChannel(values, redChannelDilutingFormulaTreeRoot);

            FormulaTreeNode greenChannelDilutingFormulaTreeRoot = CreateChannelDilutingFormulaTreeRoot();
            IEnumerable<byte> greenChannel = MapToColorChannel(values, greenChannelDilutingFormulaTreeRoot);

            FormulaTreeNode blueChannelDilutingFormulaTreeRoot = CreateChannelDilutingFormulaTreeRoot();
            IEnumerable<byte> blueChannel = MapToColorChannel(values, blueChannelDilutingFormulaTreeRoot);

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

        private static FormulaTreeNode CreateChannelDilutingFormulaTreeRoot()
        {
            //IEnumerable<Operator> operatorsForChannelDilutingFormula = new[]
            //    {
            //        OperatorsLibrary.Sum, 
            //        OperatorsLibrary.Sub,
            //        OperatorsLibrary.Mul
            //    };
            //operatorsForChannelDilutingFormula = operatorsForChannelDilutingFormula.Concat(OperatorsLibrary.Constants);
            //return FormulaTreeGenerator.CreateRandomFormulaTree(1, 3, 3, 0, operatorsForChannelDilutingFormula);

            Random random = new Random();
            double a = random.NextDouble();
            double b = random.NextDouble();
            double c = random.NextDouble();
            return FormulaTreeSerializer.Deserialize(string.Format("sum(sum(mul(mul(x x) {0}) mul(x {1})) {2})", a, b, c));
        }

        private static IEnumerable<byte> MapToColorChannel(IEnumerable<double> values, FormulaTreeNode channelDilutingFormulaTreeRoot)
        {
            FormulaTree channelDilutingFormulaTree = new FormulaTree(channelDilutingFormulaTreeRoot);
            IEnumerable<double> channelValues = channelDilutingFormulaTree.EvaluateSeries(values);

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

        private static IEnumerable<double> GetSignificantValues(IEnumerable<double> values)
        {
            const double factor = 1e175;
            const double lowBound = double.MinValue * factor;
            const double highBound = double.MaxValue / factor;
            return values.Where(v => !double.IsNaN(v) && (v > lowBound) && (v < highBound));
        }
    }
}
