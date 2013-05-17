using System;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator
{
    public class FormulaRenderingArguments
    {
        public FormulaTree FormulaTree { get; private set; }
        public ColorTransformation ColorTransformation { get; private set; }

        public FormulaRenderingArguments(FormulaTreeNode formulaTreeRoot, ColorTransformation colorTransformation)
        {
            FormulaTree = new FormulaTree(formulaTreeRoot);
            ColorTransformation = colorTransformation; 
        }

        public override string ToString()
        {            
            string formulaString = FormulaTreeSerializer.Serialize(FormulaTree.FormulaRoot);
            string colorTransformationString = ColorTransformation.ToString();
            string[] lines = new[] {colorTransformationString, formulaString};
            return string.Join("\r\n", lines);
        }

        public static FormulaRenderingArguments FromString(string value)
        {
            string[] lines = value.Split(new[] { "\r\n" }, 3, StringSplitOptions.RemoveEmptyEntries);
            ColorTransformation colorTransformation = ColorTransformation.FromString(lines[0]);
            FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(lines[1]);
            return new FormulaRenderingArguments(formulaTreeRoot, colorTransformation);
        }
    }
}
