using System;
using Android.App;
using Android.Content;
using Android.Util;

namespace Android.Utilities
{
    public class ExceptionHandler
    {
        private readonly Context _context;

        public string CommonErrorMessage { get; set; }

        public ExceptionHandler(Context context)
        {
            _context = context;
        }

        public void HandleExpected(Exception ex)
        {
            Log.Error(_context.GetType().Name, ex.Message);
            AlertDialog dialog = new AlertDialog.Builder(_context).SetMessage(CommonErrorMessage).Create();
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Show();
        }
    }
}