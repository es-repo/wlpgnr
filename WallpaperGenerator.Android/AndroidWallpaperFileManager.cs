using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Utilities;
using UI.Shared;
using WallpaperGenerator.FormulaRendering;
using Environment = Android.OS.Environment;

namespace WallpaperGenerator.UI.Android
{
    public class AndroidWallpaperFileManager : WallpaperFileManager
    {
        private readonly Context _context;

        public AndroidWallpaperFileManager(Context context)
            : base(System.IO.Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures).AbsolutePath, 
                context.Resources.GetString(Resource.String.ApplicationName)))
        {
            _context = context;
        }

        protected override void WriteImageAsPng(RenderedFormulaImage image, Stream stream)
        {
            Bitmap bitmap = image.ToBitmap();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }

        protected override void AddFileToGallery(string path)
        {
            IntentShortcuts.AddFileToGallery(_context, path);
        }
    }
}
