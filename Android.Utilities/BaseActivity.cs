using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using WallpaperGenerator.Utilities;

namespace Android.Utilities
{
    public abstract class BaseActivity : Activity
    {
        protected string CrushReportEmail;

        protected ExceptionHandler ExceptionHandler { get; private set; }

        protected BaseActivity()
        {
            ExceptionHandler = new ExceptionHandler(this);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AndroidEnvironment.UnhandledExceptionRaiser += (s, a) =>
            {
                a.Handled = true;
                StringBuilder errorReport = new StringBuilder();
                errorReport.Append("Error\n===============\n");
                errorReport.Append(a.Exception.ToDetailString());

                errorReport.Append("\nApplication Info\n===============\n");
                errorReport.AppendFormat("Package: {0}\n", ApplicationContext.PackageName);
                errorReport.AppendFormat("Version: {0}\n", PackageManager.GetPackageInfo(PackageName, 0).VersionName);

                errorReport.Append("\nDevice Information\n===============\n");
                errorReport.Append(Device.Info);
                
                Intent intent = new Intent(this, typeof(CrashReportActivity));
                intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("error", errorReport.ToString());
                intent.PutExtra("crushReportEmail", CrushReportEmail);
                StartActivity(intent);
            };
        }
    }
}