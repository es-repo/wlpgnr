using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Utilities;
using WallpaperGenerator.Utilities;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;

namespace WallpaperGenerator.UI.Android
{
    public class WallpaperFileManager
    {
        private const string FileNamePrefix = "wlp";
        private readonly string _path;
        private readonly Context _context;

        public WallpaperFileManager(Context context, string folderName)
        {
            _path = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, folderName);
            _context = context;
        }

        public Task<String> SaveAsync(Bitmap bitmap)
        {
            return Task.Run(() => Save(bitmap));
        }

        private string Save(Bitmap bitmap)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            string imagePath = GetNextFilePath();
            using (FileStream stream = new FileStream(imagePath, FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Flush();
            }
            IntentShortcuts.AddFileToGallery(_context, imagePath);
            return imagePath;
        }

        private string GetNextFilePath()
        {
            return FuncUtilities.Repeat(() => Path.Combine(_path, FileNamePrefix + DateTime.Now.Ticks + ".png"), p => !File.Exists(p), 10);
        }
    }
}
