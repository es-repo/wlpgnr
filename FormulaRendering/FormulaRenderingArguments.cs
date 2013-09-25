using System;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public class FormulaRenderingArguments
    {
        public FormulaTree FormulaTree { get; private set; }
        public RangesForFormula2DProjection Ranges { get; private set; }
        public ColorTransformation ColorTransformation { get; private set; }

        public FormulaRenderingArguments(FormulaTree formulaTree, RangesForFormula2DProjection ranges, 
            ColorTransformation colorTransformation)
        {
            FormulaTree = formulaTree;
            Ranges = ranges;
            ColorTransformation = colorTransformation; 
        }

        public override string ToString()
        {            
            string formulaString = FormulaTreeSerializer.Serialize(FormulaTree);
            string variableRangesString = Ranges.ToString();
            string colorTransformationString = ColorTransformation.ToString();
            string[] lines = new[] { variableRangesString, colorTransformationString, formulaString };
            return string.Join("\r\n", lines);
        }

        public static FormulaRenderingArguments FromString(string value)
        {
            string[] lines = value.Split(new[] { "\r\n" }, 3, StringSplitOptions.RemoveEmptyEntries);
            RangesForFormula2DProjection ranges = RangesForFormula2DProjection.FromString(lines[0]);
            ColorTransformation colorTransformation = ColorTransformation.FromString(lines[1]);
            FormulaTree formulaTree = FormulaTreeSerializer.Deserialize(lines[2]);
            return new FormulaRenderingArguments(formulaTree, ranges, colorTransformation);
        }
    }
}
