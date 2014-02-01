using Android.App;
using Android.OS;
using Android.Widget;

namespace Android.Utilities
{
    [Activity(Label = "@string/CrashReport")]
    public class CrashReportActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CrashReport);
            TextView errorTextView = FindViewById<TextView>(Resource.Id.errorTextView);
            string error = Intent.GetStringExtra("error");
            if (!string.IsNullOrEmpty(error))
            {
                errorTextView.Text = error;
                string crushReportEmail = Intent.GetStringExtra("crushReportEmail");
                if (!string.IsNullOrEmpty(crushReportEmail))
                    IntentShortcuts.Email(this, "wallpapergenerator@gmail.com", "Crush Report", error);
            }
        }
    }
}