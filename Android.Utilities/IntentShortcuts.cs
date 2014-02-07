using Android.Content;
using Android.Content.PM;
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
            var contentUri = Uri.FromFile(file);
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

        public static void Email(Context context, string to, string subject)
        {
            Email(context, to ,subject, "");
        }

        public static void Email(Context context, string to, string subject, string message)
        {
            Intent i = new Intent(Intent.ActionView);
            string uri = "mailto:" + to +  "?subject=" + subject + "&body=" + message;
            i.SetData(Uri.Parse(uri));
            i.PutExtra(Intent.ExtraEmail, new[] { to });
            context.StartActivity(i);
        }

        public static string GetLauncherPackageName(Context context)
        {
            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryHome);
            ResolveInfo resolveInfo = context.PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly);
            return resolveInfo.ActivityInfo.PackageName;
        }
    }
}