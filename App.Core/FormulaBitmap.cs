using System.IO;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public abstract class FormulaBitmap
    {
        public Size Size { get; private set; }

        public object PlatformBitmap { get; protected set; }

        protected FormulaBitmap(Size size)
        {
            Size = size;
        }

        public abstract void Update(FormulaRenderResult formulaRenderResult);

        public abstract void WriteAsPng(Stream stream);
    }
}
