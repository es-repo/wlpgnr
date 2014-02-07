using System;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.App.Core
{
    public class FormulaRenderArguments
    {
        public FormulaTree FormulaTree { get; private set; }
        public Range[] Ranges { get; set; }
        public ColorTransformation ColorTransformation { get; private set; }
        
        public FormulaRenderArguments(FormulaTree formulaTree, Range[] ranges, ColorTransformation colorTransformation)
        {
            FormulaTree = formulaTree;
            Ranges = ranges;
            ColorTransformation = colorTransformation;
        }

        public override string ToString()
        {            
            string formulaString = FormulaTreeSerializer.Serialize(FormulaTree);
            string variableRangesString = RangeUtilities.ToString(Ranges);
            string colorTransformationString = ColorTransformation.ToString();
            string[] lines = { variableRangesString, colorTransformationString, formulaString };
            return string.Join("\r\n", lines);
        }

        public static FormulaRenderArguments FromString(string value)
        {
            string[] lines = value.Split(new[] { "\r\n" }, 3, StringSplitOptions.RemoveEmptyEntries);
            Range[] ranges = RangeUtilities.FromString(lines[0]);
            ColorTransformation colorTransformation = ColorTransformation.FromString(lines[1]);
            FormulaTree formulaTree = FormulaTreeSerializer.Deserialize(lines[2]);
            return new FormulaRenderArguments(formulaTree, ranges, colorTransformation);
        }
    }
}
