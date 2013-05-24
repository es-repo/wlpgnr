using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator
{
    public class FormulaRenderingArguments
    {
        public FormulaTree FormulaTree { get; private set; }
        public Range[] VariableRanges { get; private set; }
        public ColorTransformation ColorTransformation { get; private set; }

        public FormulaRenderingArguments(FormulaTree formulaTree, IEnumerable<Range> variableRanges, ColorTransformation colorTransformation)
        {
            FormulaTree = formulaTree;
            VariableRanges = variableRanges.ToArray();
            ColorTransformation = colorTransformation; 
        }

        public override string ToString()
        {            
            string formulaString = FormulaTreeSerializer.Serialize(FormulaTree.FormulaRoot);
            string variableRangesString = RangesToString(VariableRanges);
            string colorTransformationString = ColorTransformation.ToString();
            string[] lines = new[] { variableRangesString, colorTransformationString, formulaString };
            return string.Join("\r\n", lines);
        }

        public static FormulaRenderingArguments FromString(string value)
        {
            string[] lines = value.Split(new[] { "\r\n" }, 3, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<Range> ranges = RangesFromString(lines[0]);
            ColorTransformation colorTransformation = ColorTransformation.FromString(lines[1]);
            FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(lines[2]);
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            return new FormulaRenderingArguments(formulaTree, ranges, colorTransformation);
        }

        private static string RangesToString(IEnumerable<Range> ranges)
        {
            string[] strings = ranges.Select(r => r.ToString()).ToArray();
            return string.Join(";", strings);
        }

        private static IEnumerable<Range> RangesFromString(string value)
        {
            string[] rangeString = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return rangeString.Select(s => Range.FromString(s));
         }
    }
}
