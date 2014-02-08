using Android.App;
using Android.Content;
using Android.Util;
using Com.Crittercism.App;
using Java.Lang;
using Exception = System.Exception;

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
            Log.Error(_context.GetType().Name, ex.Message);
            string errorMessage = _context.Resources.GetString(Resource.String.CommonErrorMessage);
            AlertDialog dialog = new AlertDialog.Builder(_context).SetMessage(errorMessage).Create();
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Show();
        }
    }
}