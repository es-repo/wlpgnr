using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using WallpaperGenerator.Utilities;

namespace Android.Utilities
{
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AndroidEnvironment.UnhandledExceptionRaiser += (s, a) =>
            {
                a.Handled = true;
                StringBuilder errorReport = new StringBuilder();
                errorReport.Append("Error\n===============\n");
                errorReport.Append(a.Exception.ToDetailString());

                errorReport.Append("\nDevice Information\n===============\n");
                errorReport.Append(Device.Info);
                Intent intent = new Intent(this, typeof(CrashReportActivity));
                intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("error", errorReport.ToString());
                StartActivity(intent);
            };
        }
    }
}