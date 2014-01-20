using Android.Content;
using Java.IO;

namespace Android.Utilities
{
    public static class IntentShortcuts
    {
        public static void GoHome(Context context)
        {
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            context.StartActivity(startMain);
        }

        public static void AddFileToGallery(Context context, string filePath)
        {
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            File file = new File(filePath);
            var contentUri = Net.Uri.FromFile(file);
            mediaScanIntent.SetData(contentUri);
            context.SendBroadcast(mediaScanIntent);
        }

        public static void OpenGallery(Context context)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetType("image/*");
            context.StartActivity(intent);
        }
    }
}