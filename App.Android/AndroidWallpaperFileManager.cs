using Android.Content;
using Android.OS;
using Android.Utilities;
using WallpaperGenerator.App.Shared;

namespace WallpaperGenerator.App.Android
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

        protected override void AddFileToGallery(string path)
        {
            IntentShortcuts.AddFileToGallery(_context, path);
        }
    }
}
