using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTreeNode formulaTree, int widthInPixels, int heightInPixels)
        {
            Rgb[] data = new Rgb[widthInPixels*heightInPixels];
            IEnumerable<Variable> variables = FormulaTree.SelectVariables(formulaTree);
            
            for (int y = 0; y < heightInPixels; y++)
            {
                for (int x = 0; x < widthInPixels; x++)
                {
                    variables.First().Value = x;
                    variables.Skip(1).First().Value = y;

                    double value = FormulaTree.Evaluate(formulaTree);
                    
                    int index = y * widthInPixels + x;
                    data[index] = FormulaEvaluetedValueToRgb(value);
                }    
            }

            return new RenderedFormulaImage(data, widthInPixels, heightInPixels);
        }

        private static Rgb FormulaEvaluetedValueToRgb(double value)
        {
            return new Rgb((byte)(value%256), (byte)(value%256), (byte)(value%256));
        }
    }
}
