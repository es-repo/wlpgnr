using Android.Content;
using Android.Graphics;
using Android.Net;
using File = Java.IO.File;

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
            intent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        public static void Share(Context context, Bitmap bitmap, string bitmapSavePath, string title, string subject, string message)
        {
            bitmap.SaveToFile(bitmapSavePath);

            Intent i = new Intent(Intent.ActionSend);
            i.SetType("image/png");
            i.PutExtra(Intent.ExtraSubject, subject);
            i.PutExtra(Intent.ExtraText, message);
            File f = new File(bitmapSavePath);
            i.PutExtra(Intent.ExtraStream, Uri.FromFile(f));

            context.StartActivity(Intent.CreateChooser(i, title));
        }
    }
}