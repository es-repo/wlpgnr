using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Utilities;
using Android.Views;
using Android.Widget;
using Com.Google.Ads;
using WallpaperGenerator.App.Core;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.App.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, ScreenOrientation = ScreenOrientation.Nosensor)]
    public class MainActivity : BaseActivity
    {
        private AdView _adView;
        private Button _generateButton;
        private Button _changeColorsButton;
        private Button _transformButton;
        private Button _setAsWallpaperButton;
        private LinearLayout _technicalInfoLayout;
        private TextView _formulaTextView;
        private TextView _renderTimeTextView;
        private TextView _coresCountTextView;
        private HorizontalScrollView _horizontalScrollView;
        private ImageView _imageView;
        private FormulaRenderWorkflow _workflow;
        private AndroidWallpaperFileManager _wallpaperFileManager;
        
        private Bitmap ImageBitmap
        {
            get { return ((BitmapDrawable)_imageView.Drawable).Bitmap; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            CrushReportEmail = Resources.GetString(Resource.String.CrushReportEmail);
            
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            InitButtonBar();

            _adView = FindViewById<AdView>(Resource.Id.adView);
            InitAdView();

            _horizontalScrollView = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView);
            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);
            _coresCountTextView = FindViewById<TextView>(Resource.Id.coresCountTextView);
            TextView sizeTextView = FindViewById<TextView>(Resource.Id.sizeTextView);
            TextView launcherTextView = FindViewById<TextView>(Resource.Id.launcherTextView);
            _technicalInfoLayout = FindViewById<LinearLayout>(Resource.Id.technicalInfoLayout);

            launcherTextView.Text = "launcher: " + IntentShortcuts.GetLauncherPackageName(this);
            WallpaperManager wallpaperManager = WallpaperManager.GetInstance(this);
            Point wallpaperSize = wallpaperManager.GetDesiredSize(WindowManager.DefaultDisplay, Resources.Configuration);
            Size imageSize = new Size(wallpaperSize.X, wallpaperSize.Y);
            sizeTextView.Text = "image size: " + imageSize.Width + "x" + imageSize.Height;
            _workflow = new FormulaRenderWorkflow(new FormulaRenderArgumentsGenerationParams(), imageSize, s => new AndroidFormulaBitmap(s));
            
            if (_workflow.FormulaRenderArguments != null)
                _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();

            _wallpaperFileManager = new AndroidWallpaperFileManager(this);

            AdjustButtons();
            ClearImage();
        }

        private void InitAdView()
        {
            AdRequest adRequest = new AdRequest();
            _adView.LoadAd(adRequest);
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

                case Resource.Id.benchmarkMenuItem:
                    t = OnBenchmarkMenuItemSelected();
                    break;

                case Resource.Id.openGalleryMenuItem:
                    OnOpenGalleryMenuItemSelected();
                    return true;

                case Resource.Id.shareMenuItem:
                    OnShareMenuItemSelected();
                    return true;

                case Resource.Id.feedbackMenuItem:
                    OnFeedbackMenuItemSelected();
                    return true;

                case Resource.Id.rateAppMenuItem:
                    OnRateAppMenuItemSelected();
                    return true;

                case Resource.Id.displayTechnicalInfoMenuItem:
                    _technicalInfoLayout.Visibility = _technicalInfoLayout.Visibility == ViewStates.Gone ? ViewStates.Visible : ViewStates.Gone;
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
            await OnGenerateOrBenchmarkClicked(false);
        }

        private async Task OnBenchmarkMenuItemSelected()
        {
            await OnGenerateOrBenchmarkClicked(true);
        }

        private async Task OnGenerateOrBenchmarkClicked(bool benchmark)
        {
            if (_workflow.IsImageRendering)
                return;
            
            await DrawImageAsync(true, benchmark);
            _formulaTextView.Text = _workflow.FormulaRenderArguments.ToString();
            AdjustButtons();
        }

        private async Task OnChangeColorsClicked()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.ChangeColors();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync(false, false);
        }

        private async Task OnTransformClicked()
        {
            if (_workflow.FormulaRenderArguments == null || _workflow.IsImageRendering)
                return;

            FormulaRenderArguments formulaRenderArguments = _workflow.TransformRanges();
            _formulaTextView.Text = formulaRenderArguments.ToString();
            await DrawImageAsync(false, false);
        }

        private async void OnSetAsWallpaperClicked()
        {
            if (!_workflow.IsImageReady)
                return;

            ProgressDialog progressDialog = CreateProgressDialog(Resources.GetString(Resource.String.SettingWallpaper));
            progressDialog.Show();
            try
            {
                await SetWallpaperAsync();
                IntentShortcuts.GoHome(this);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleExpected(e);
            }
            finally
            {
                progressDialog.Dismiss();
            }
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
            try
            {
                var filesPath = await _wallpaperFileManager.SaveAsync(_workflow.LastWorkflowRenderResult, false);
                if (filesPath == null)
                    throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleExpected(e);
            }
            finally
            {
                progressDialog.Dismiss();
            }
        }

        private void OnOpenGalleryMenuItemSelected()
        {
            IntentShortcuts.OpenGallery(this);
        }

        private void OnShareMenuItemSelected()
        {
            if (!_workflow.IsImageReady)
                return;

            string message = Resources.GetString(Resource.String.ShareMessage);
            string packageName = ApplicationContext.PackageName;
            message = message.Replace("{packageName}", packageName);
            try
            {
                IntentShortcuts.Share(this, ImageBitmap, System.IO.Path.Combine(_wallpaperFileManager.Path, "sharedwallpaper.png"),
                    Resources.GetString(Resource.String.ShareTitle),
                    Resources.GetString(Resource.String.ShareSubject),
                    message);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleExpected(e);
            }
        }

        private void OnRateAppMenuItemSelected()
        {
            string packageName = ApplicationContext.PackageName;
            StartActivity(new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("market://details?id=" + packageName)));
        }

        private void OnFeedbackMenuItemSelected()
        {
            string subject = Resources.GetString(Resource.String.FeedbackSubject);
            string appVersion = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
            string deviceInfo = string.Format("{0} {1} {2}", Build.Brand, Build.Model, Build.VERSION.Sdk);
            subject = subject.Replace("{appVersion}", appVersion).Replace("{deviceInfo}", deviceInfo);
            IntentShortcuts.Email(this, Resources.GetString(Resource.String.ContactEmail), subject);
        }

        private void ClearImage()
        {
            _imageView.LayoutParameters.Width = _workflow.ImageSize.Width;
            _imageView.LayoutParameters.Height = _workflow.ImageSize.Height;
            _imageView.SetBackgroundColor(Color.Black);
        }

        private async Task DrawImageAsync(bool generateNew, bool benchmark)
        {
            int coresCount = Java.Lang.Runtime.GetRuntime().AvailableProcessors();
            _coresCountTextView.Text = "available cpu cores: " + coresCount.ToInvariantString();

            ProgressDialog progressDialog = CreateProgressDialog(
                benchmark ? Resources.GetString(Resource.String.Benchmark) : Resources.GetString(Resource.String.WallpaperWillBeReady), 
                benchmark ? "" : Resources.GetString(Resource.String.Wait), false);
            progressDialog.Max = 100;
            progressDialog.Show();

            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => RunOnUiThread(() =>
                {
                    progressDialog.Progress = (int) (p.Progress*progressDialog.Max);
                }), TimeSpan.FromMilliseconds(100),
                () => progressDialog.Progress = progressDialog.Max);

            WorkflowRenderResult result = benchmark
                ? await _workflow.BenchmarkAsync(coresCount, renderingProgressObserver) 
                : await _workflow.RenderFormulaAsync(generateNew, coresCount, renderingProgressObserver);

            result.Bitmap.Update(result.FormulaRenderResult);
            _renderTimeTextView.Text = "rendering time: " + result.ElapsedTime;
            _imageView.SetImageBitmap((Bitmap)result.Bitmap.PlatformBitmap);
            _horizontalScrollView.ScrollTo(_horizontalScrollView.Width / 2, 0);
            
            progressDialog.Dismiss();

            if (benchmark)
            {
                AlertDialog dialog = CreateAlertDialog(result.ElapsedTime.ToString("hh':'mm':'ss"));
                dialog.Show();
            }
        }

        private ProgressDialog CreateProgressDialog(string message, string title = null, bool indeterminate = true)
        {
            ProgressDialog progressDialog = new ProgressDialog(this) {Indeterminate = indeterminate};
            if (indeterminate)
            {
                progressDialog.SetProgressPercentFormat(new EmptyNumberFormat());
            }
            progressDialog.SetProgressNumberFormat("");
            progressDialog.SetCancelable(false);
            progressDialog.SetCanceledOnTouchOutside(false);
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            return progressDialog;
        }

        private AlertDialog CreateAlertDialog(string message)
        {
            TextView dialogMessage = new TextView(this) { Text = message, Gravity = GravityFlags.CenterHorizontal, TextSize = 30};
            AlertDialog dialog = new AlertDialog.Builder(this)
                .SetView(dialogMessage)
                .SetCancelable(true)
                .Create();
            dialog.SetCanceledOnTouchOutside(true);
            return dialog;
        }

        protected override void OnDestroy()
        {
            _adView. Destroy();
            base.OnDestroy();
        }
    }
}