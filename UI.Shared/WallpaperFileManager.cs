using System;
using System.IO;
using System.Threading.Tasks;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Shared
{
    public class WallpaperFileManager
    {
        private const string FileNamePrefix = "wlp";
        private readonly string _path;

        public WallpaperFileManager(string path)
        {
            _path = path;
        }

        public Task<Tuple<string, string>> SaveAsync(FormulaRenderResult formulaRenderResult, bool withFormulaRenderArguments)
        {
            return Task.Run(() => Save(formulaRenderResult, withFormulaRenderArguments));
        }

        public Tuple<string, string> Save(FormulaRenderResult formulaRenderResult, bool withFormulaRenderArguments)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            string imagePath = GetNextFilePath();
            using (FileStream stream = new FileStream(imagePath, FileMode.CreateNew))
            {
                WriteImageAsPng(formulaRenderResult.Image, stream);
                stream.Flush();
            }
            AddFileToGallery(imagePath);

            string dataPath = null;
            if (withFormulaRenderArguments)
            {
                dataPath = imagePath.Replace(".png", ".txt");
                File.WriteAllText(dataPath, formulaRenderResult.FormulaRenderArguments.ToString());
            }

            return new Tuple<string, string>(imagePath, dataPath);
        }

        protected virtual void WriteImageAsPng(RenderedFormulaImage image, Stream stream)
        {
        }

        protected virtual void AddFileToGallery(string path)
        {
        }

        private string GetNextFilePath()
        {
            return FuncUtilities.Repeat(() => Path.Combine(_path, FileNamePrefix + DateTime.Now.Ticks + ".png"), p => !File.Exists(p), 10);
        }
    }
}
