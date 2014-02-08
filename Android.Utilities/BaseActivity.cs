using Android.App;
using Android.OS;
using Android.Runtime;
using Com.Crittercism.App;
using Java.Lang;

namespace Android.Utilities
{
    public abstract class BaseActivity : Activity
    {
        private readonly string _crittercismId;
        protected string CrushReportEmail;

        protected ExceptionHandler ExceptionHandler { get; private set; }

        protected BaseActivity(string crittercismId)
        {
            _crittercismId = crittercismId;
            ExceptionHandler = new ExceptionHandler(this);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Crittercism.Initialize(this, _crittercismId);

            AndroidEnvironment.UnhandledExceptionRaiser += (s, a) =>
            {
                a.Handled = false;
                Crittercism.LogHandledException(Throwable.FromException(a.Exception));
                //StringBuilder errorReport = new StringBuilder();
                //errorReport.Append("Error\n===============\n");
                //errorReport.Append(a.Exception.ToDetailString());

                //errorReport.Append("\nApplication Info\n===============\n");
                //errorReport.AppendFormat("Package: {0}\n", ApplicationContext.PackageName);
                //errorReport.AppendFormat("Version: {0}\n", PackageManager.GetPackageInfo(PackageName, 0).VersionName);

                //errorReport.Append("\nDevice Information\n===============\n");
                //errorReport.Append(Device.Info);

                //Intent intent = new Intent(this, typeof(CrashReportActivity));
                //intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
                //intent.AddFlags(ActivityFlags.ClearTop);
                //intent.PutExtra("error", errorReport.ToString());
                //intent.PutExtra("crushReportEmail", CrushReportEmail);
                //StartActivity(intent);
            };
        }
    }
}