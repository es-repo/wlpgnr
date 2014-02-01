using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core.Testing
{
    public class FormulaBitmapMock : FormulaBitmap
    {
        public FormulaBitmapMock(Size size) : base(size)
        {
        }

        public override void Update(FormulaRendering.FormulaRenderResult formulaRenderResult)
        {
        }

        public override void WriteAsPng(System.IO.Stream stream)
        {
        }
    }
}
