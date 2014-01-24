using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Utilities;
using Android.Views;
using Android.Widget;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;
using AndroidEnvironment = Android.OS.Environment;

namespace WallpaperGenerator.UI.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Nosensor)]
    public class MainActivity : BaseActivity
    {
        private TextView _formulaTextView;
        private TextView _progressTextView;
        private TextView _renderTimeTextView;
        private ImageView _imageView;
        private FormulaRenderWorkflow _workflow;
        private AndroidWallpaperFileManager _wallpaperFileManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _progressTextView = FindViewById<TextView>(Resource.Id.progressTextView);
            _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);

            Point displaySize = DisplayExtensions.GetSize(WindowManager.DefaultDisplay);

            _workflow = new FormulaRenderWorkflow(new FormulaRenderArgumentsGenerationParams(), new Size(displaySize.X, displaySize.Y));

            if (_workflow.FormulaRenderArguments != null)
                _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();

            _wallpaperFileManager = new AndroidWallpaperFileManager(this);

            ClearImage();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);
            menu.FindItem(Resource.Id.renderMenuItem).SetVisible(false);
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
                    OnSetAsWallpaperMenuItemSelected();
                    return true;

                case Resource.Id.saveMenuItem:
                    t = OnSaveMenuItemSelected();
                    break;

                case Resource.Id.openGalleryMenuItem:
                    OnOpenGalleryMenuItemSelected();
                    return true;

                case Resource.Id.shareMenuItem:
                    OnShareMenuItemSelected();
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
            if (_workflow.IsImageRendering)
                return;
            
            ClearImage();
            FormulaRenderArguments formulaRenderArguments = await _workflow.GenerateFormulaRenderArgumentsAsync();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync();
        }

        private async Task OnRenderMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            ClearImage();
            await DrawImageAsync();
        }

        private async Task OnChangeColorMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.ChangeColors();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync();
        }

        private async Task OnTransformMenuItemSelected()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.TransformRanges();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync();
        }

        private void OnSetAsWallpaperMenuItemSelected()
        {
            if (!_workflow.IsImageReady)
                return;

            WallpaperManager wallpaperManager = WallpaperManager.GetInstance(this);
            
            // TODO: wrap in try..catch block
            wallpaperManager.SetBitmapWithExactScreenSize(ImageBitmap);
            IntentShortcuts.GoHome(this);
        }

        private async Task OnSaveMenuItemSelected()
        {
            if (!_workflow.IsImageReady)
                return;

            // TODO: wrap in try..catch block
            var filesPath = await _wallpaperFileManager.SaveAsync(_workflow.LastFormulaRenderResult, false);
            if (filesPath == null)
                throw new InvalidOperationException();
            Toast.MakeText(this, Resources.GetString(Resource.String.WallpaperIsSaved), ToastLength.Long).Show();
        }

        private void OnOpenGalleryMenuItemSelected()
        {
            IntentShortcuts.OpenGallery(this);
        }

        private void OnShareMenuItemSelected()
        {
            if (!_workflow.IsImageReady)
                return;

            // TODO: wrap in try..catch block
            IntentShortcuts.Share(this, ImageBitmap, System.IO.Path.Combine(_wallpaperFileManager.Path, "sharedwallpaper.png"),
                Resources.GetString(Resource.String.ShareTitle),
                Resources.GetString(Resource.String.ShareSubject),
                Resources.GetString(Resource.String.ShareMessage));
        } 

        private void ClearImage()
        {
            int width = _workflow.ImageSize.Width;
            int height = _workflow.ImageSize.Height;  
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

        private Bitmap ImageBitmap
        {
            get { return ((BitmapDrawable) _imageView.Drawable).Bitmap; }
        }
    }
}