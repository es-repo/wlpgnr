using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTreeNode formulaTreeRoot, int widthInPixels, int heightInPixels)
        {
            FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);

            Rgb[] data = new Rgb[widthInPixels * heightInPixels];
            for (int y = 0; y < heightInPixels; y++)
            {
                for (int x = 0; x < widthInPixels; x++)
                {
                    formulaTree.Variables[0].Value = x;
                    formulaTree.Variables[1].Value = y;

                    double value = formulaTree.Evaluate();
                    
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
