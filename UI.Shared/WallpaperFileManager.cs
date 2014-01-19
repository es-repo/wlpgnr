using System;
using System.IO;
using System.Threading.Tasks;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities;

namespace UI.Shared
{
    public class WallpaperFileManager
    {
        private const string FileNamePrefix = "wlp";
        private readonly string _path;
        
        public WallpaperFileManager(string path)
        {
            _path = path;
        }

        public virtual Task<String> SaveAsync(BaseBitmap bitmap)
        {
            return Task.Run(() => Save(bitmap));
        }

        public virtual string Save(BaseBitmap bitmap)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            string imagePath = GetNextFilePath();
            using (FileStream stream = new FileStream(imagePath, FileMode.CreateNew))
            {
                bitmap.WriteAsPng(stream);
                stream.Flush();
            }
            AddFileToGallery(imagePath);
            return imagePath;
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
