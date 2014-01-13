using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Utilities;
using Android.Views;
using Android.Widget;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : BaseActivity
    {
        private TextView _formulaTextView;
        private TextView _progressTextView;
        private TextView _renderTimeTextView;
        private ImageView _imageView;
        private FormulaRenderWorkflow _workflow;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _progressTextView = FindViewById<TextView>(Resource.Id.progressTextView);
            _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            _workflow = new FormulaRenderWorkflow(
                new FormulaRenderArgumentsGenerationParams
                {
                    WidthInPixels = displayMetrics.WidthPixels,  
                    HeightInPixels = displayMetrics.HeightPixels
                });

            if (_workflow.FormulaRenderArguments != null)
                _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();

            ClearImageView();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Task t;
            switch (item.ItemId)
            {
                case Resource.Id.generateMenuItem:
                    t = OnGenerateMenuItemSelected();
                    break;

                case Resource.Id.renderMenuItem:
                    t = OnRenderMenuItemSelected();
                    break;

                case Resource.Id.changeColorMenuItem:
                    t = OnChangeColorMenuItemSelected();
                    break;

                case Resource.Id.transformMenuItem:
                    t = OnTransformMenuItemSelected();
                    break;

                case Resource.Id.setAsWallpaperMenuItem:
                    OnSetAsWallpaperMenuItem();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

            if (t == null)
                throw new InvalidOperationException();
            return true;
        }

        private async Task OnGenerateMenuItemSelected()
        {
            FormulaRenderArguments formulaRenderArguments = await _workflow.GenerateFormulaRenderArgumentsAsync();
            _formulaTextView.Text = formulaRenderArguments.ToString();
        }

        private async Task OnRenderMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null)
                return;

            ClearImageView();
            await DrawImageAsync();
        }

        private async Task OnChangeColorMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.ChangeColors();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync();
        }

        private async Task OnTransformMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.TransformRanges();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync();
        }

        private void OnSetAsWallpaperMenuItem()
        {
            if (!_workflow.IsImageReady)
                return;

            Bitmap bitmap = ((BitmapDrawable)_imageView.Drawable).Bitmap;
            WallpaperManager wallpaperManager = WallpaperManager.GetInstance(this);
            
            // TODO: wrap in try..catch block
            wallpaperManager.SetBitmapWithExactScreenSize(bitmap);
            GoHome();
        }

        private void GoHome()
        {
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            StartActivity(startMain);
        }

        private void ClearImageView()
        {
            int width = _workflow.GenerationParams.WidthInPixels;
            int height = _workflow.GenerationParams.HeightInPixels;  
            int[] pixels = new int[width * height];
            Bitmap blankBitmap = Bitmap.CreateBitmap(pixels, width, height, Bitmap.Config.Argb8888);
            _imageView.SetImageBitmap(blankBitmap);
        }

        private async Task DrawImageAsync()
        {
            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => RunOnUiThread(() => _progressTextView.Text = p.Progress.ToString("P1")), TimeSpan.FromMilliseconds(100));

            FormulaRenderResult formulaRenderResult = await _workflow.RenderFormulaAsync(renderingProgressObserver);
            _progressTextView.Text = 1.ToString("P1");
            _renderTimeTextView.Text = formulaRenderResult.ElapsedTime.ToString();
            _imageView.SetImageBitmap(formulaRenderResult.Image.ToBitmap());
        }
    }
}

