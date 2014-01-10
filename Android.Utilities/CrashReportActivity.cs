using System;
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
            String error = Intent.GetStringExtra("error");
            if (error != null)
                errorTextView.Text = error;
        }
    }
}