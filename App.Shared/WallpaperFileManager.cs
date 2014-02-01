using System;
using System.IO;
using System.Threading.Tasks;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Shared
{
    public class WallpaperFileManager
    {
        private const string FileNamePrefix = "wlp";

        public string Path { get; private set; }

        public WallpaperFileManager(string path)
        {
            Path = path;
        }

        public Task<Tuple<string, string>> SaveAsync(WorkflowRenderResult workflowRenderResult, bool withFormulaRenderArguments)
        {
            return Task.Run(() => Save(workflowRenderResult, withFormulaRenderArguments));
        }

        public Tuple<string, string> Save(WorkflowRenderResult workflowRenderResult, bool withFormulaRenderArguments)
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            string imagePath = GetNextFilePath();
            using (FileStream stream = new FileStream(imagePath, FileMode.CreateNew))
            {
                workflowRenderResult.Bitmap.WriteAsPng(stream);
                stream.Flush();
            }
            AddFileToGallery(imagePath);

            string dataPath = null;
            if (withFormulaRenderArguments)
            {
                dataPath = imagePath.Replace(".png", ".txt");
                File.WriteAllText(dataPath, workflowRenderResult.FormulaRenderArguments.ToString());
            }

            return new Tuple<string, string>(imagePath, dataPath);
        }

        protected virtual void AddFileToGallery(string path)
        {
        }

        private string GetNextFilePath()
        {
            return FuncUtilities.Repeat(() => System.IO.Path.Combine(Path, FileNamePrefix + DateTime.Now.Ticks + ".png"), p => !File.Exists(p), 10);
        }
    }
}
