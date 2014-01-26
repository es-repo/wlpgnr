using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
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
        private Button _generateButton;
        private Button _changeColorsButton;
        private Button _transformButton;
        private Button _setAsWallpaperButton;
        private TextView _formulaTextView;
        private TextView _renderTimeTextView;
        private ImageView _imageView;
        private FormulaRenderWorkflow _workflow;
        private AndroidWallpaperFileManager _wallpaperFileManager;

        private Bitmap ImageBitmap
        {
            get { return ((BitmapDrawable)_imageView.Drawable).Bitmap; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            InitButtonBar();

            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);

            Point displaySize = DisplayExtensions.GetSize(WindowManager.DefaultDisplay);

            _workflow = new FormulaRenderWorkflow(new FormulaRenderArgumentsGenerationParams(), new Size(displaySize.X, displaySize.Y));

            if (_workflow.FormulaRenderArguments != null)
                _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();

            _wallpaperFileManager = new AndroidWallpaperFileManager(this);

            AdjustButtons();
            ClearImage();
        }

        private void InitButtonBar()
        {
            _generateButton = FindViewById<Button>(Resource.Id.generateButton);
            _generateButton.Click += async (s, a) => await OnGenerateClicked();

            _changeColorsButton = FindViewById<Button>(Resource.Id.changeColorsButton);
            _changeColorsButton.Click += async (s, a) => await OnChangeColorsClicked();

            _transformButton = FindViewById<Button>(Resource.Id.transformButton);
            _transformButton.Click += async (s, a) => await OnTransformClicked();

            _setAsWallpaperButton = FindViewById<Button>(Resource.Id.setAsWallpaperButton);
            _setAsWallpaperButton.Click += (s, a) => OnSetAsWallpaperClicked();
        }

        private void AdjustButtons()
        {
            _changeColorsButton.Enabled = _workflow.FormulaRenderArguments != null;
            _transformButton.Enabled = _workflow.FormulaRenderArguments != null;
            _setAsWallpaperButton.Enabled = _workflow.FormulaRenderArguments != null;
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

        private async Task OnGenerateClicked()
        {
            if (_workflow.IsImageRendering)
                return;

            await DrawImageAsync(true);
            _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();
            AdjustButtons();
        }

        private async Task OnChangeColorsClicked()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.ChangeColors();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync(false);
        }

        private async Task OnTransformClicked()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.TransformRanges();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync(false);
        }

        private async void OnSetAsWallpaperClicked()
        {
            if (!_workflow.IsImageReady)
                return;

            ProgressDialog progressDialog = CreateProgressDialog(Resources.GetString(Resource.String.SettingWallpaper));
            progressDialog.Show();
            // TODO: wrap in try..catch block
            await SetWallpaperAsync();
            IntentShortcuts.GoHome(this);
        }

        private Task SetWallpaperAsync()
        {
            return Task.Run(() =>
            {
                WallpaperManager wallpaperManager = WallpaperManager.GetInstance(this);
                wallpaperManager.SetBitmapWithExactScreenSize(ImageBitmap);
            });
        }

        private async Task OnSaveMenuItemSelected()
        {
            if (!_workflow.IsImageReady)
                return;

            ProgressDialog progressDialog = CreateProgressDialog(Resources.GetString(Resource.String.SavingWallpaper));
            progressDialog.Show();
            // TODO: wrap in try..catch block
            var filesPath = await _wallpaperFileManager.SaveAsync(_workflow.LastFormulaRenderResult, false);
            if (filesPath == null)
                throw new InvalidOperationException();
            progressDialog.Dismiss();
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

        private async Task DrawImageAsync(bool generateNew)
        {
            ProgressDialog progressDialog = CreateProgressDialog(Resources.GetString(Resource.String.WallpaperWillBeReady), Resources.GetString(Resource.String.Wait), false);
            progressDialog.Max = 100;
            progressDialog.Show();

            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => RunOnUiThread(() =>
                {
                    progressDialog.Progress = (int) (p.Progress*progressDialog.Max);
                }), TimeSpan.FromMilliseconds(100),
                () => progressDialog.Progress = progressDialog.Max);

            FormulaRenderResult formulaRenderResult = await _workflow.RenderFormulaAsync(generateNew, renderingProgressObserver);
            _renderTimeTextView.Text = formulaRenderResult.ElapsedTime.ToString();
            _imageView.SetImageBitmap(formulaRenderResult.Image.ToBitmap());
            
            progressDialog.Dismiss();
        }

        private ProgressDialog CreateProgressDialog(string message, string title = null, bool indeterminate = true)
        {
            ProgressDialog progressDialog = new ProgressDialog(this) {Indeterminate = indeterminate};
            if (indeterminate)
            {
                progressDialog.SetProgressPercentFormat(new EmptyNumberFormat());
            }
            progressDialog.SetProgressNumberFormat("");

            progressDialog.SetCanceledOnTouchOutside(false);
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            return progressDialog;
        }
    }
}