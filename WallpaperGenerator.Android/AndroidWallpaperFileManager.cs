using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Utilities;
using UI.Shared;
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

        public virtual Task<String> SaveAsync(Bitmap bitmap)
        {
            return SaveAsync(new AndroidBitmap(bitmap));
        }

        protected override void AddFileToGallery(string path)
        {
            IntentShortcuts.AddFileToGallery(_context, path);
        }
    }
}
