using Android.App;
using Android.Content;
using Com.Crittercism.App;
using Com.Flurry.Android;
using Com.Google.Analytics.Tracking.Android;
using Java.Lang;
using Exception = System.Exception;
using Log = Android.Util.Log;

namespace Android.Utilities
{
    public class ExceptionHandler
    {
        private readonly Context _context;

        public ExceptionHandler(Context context)
        {
            _context = context;
        }

        public void HandleExpected(Exception ex)
        {
            EasyTracker.Tracker.SendException(ex.Message, Throwable.FromException(ex), false);
            FlurryAgent.OnError(ex.GetType().Name, ex.Message, Throwable.FromException(ex));
            Crittercism.LogHandledException(Throwable.FromException(ex));
            Log.Error(_context.GetType().Name, ex.Message);
            string errorMessage = _context.Resources.GetString(Resource.String.CommonErrorMessage);
            AlertDialog dialog = new AlertDialog.Builder(_context).SetMessage(errorMessage).Create();
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Show();
        }
    }
}