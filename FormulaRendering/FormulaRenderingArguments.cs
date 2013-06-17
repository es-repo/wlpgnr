using System;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public class FormulaRenderingArguments
    {
        public FormulaTree FormulaTree { get; private set; }
        public VariableValuesRangesFor2DProjection VariableValuesRanges { get; private set; }
        public ColorTransformation ColorTransformation { get; private set; }

        public FormulaRenderingArguments(FormulaTree formulaTree, VariableValuesRangesFor2DProjection variableValuesRanges, 
            ColorTransformation colorTransformation)
        {
            FormulaTree = formulaTree;
            VariableValuesRanges = variableValuesRanges;
            ColorTransformation = colorTransformation; 
        }

        public override string ToString()
        {            
            string formulaString = FormulaTreeSerializer.Serialize(FormulaTree.FormulaRoot, new FormulaTreeSerializationOptions{WithIndentation = false } );
            string variableRangesString = VariableValuesRanges.ToString();
            string colorTransformationString = ColorTransformation.ToString();
            string[] lines = new[] { variableRangesString, colorTransformationString, formulaString };
            return string.Join("\r\n", lines);
        }

        public static FormulaRenderingArguments FromString(string value)
        {
            string[] lines = value.Split(new[] { "\r\n" }, 3, StringSplitOptions.RemoveEmptyEntries);
            VariableValuesRangesFor2DProjection variableValuesRanges = VariableValuesRangesFor2DProjection.FromString(lines[0]);
            ColorTransformation colorTransformation = ColorTransformation.FromString(lines[1]);
            FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(lines[2]);
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
            return new FormulaRenderingArguments(formulaTree, variableValuesRanges, colorTransformation);
        }
    }
}
