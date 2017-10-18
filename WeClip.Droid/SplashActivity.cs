using Android.App;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;
using System.Threading;
using Android.Net;
using Android.Support.V7.App;
using HockeyApp.Android;
using Android.Views;
using Android.Support.V4.Content;
using WeClip.Droid.AsyncTask;
using WeClip.Droid.Helper;
using Android.Webkit;
using Android.Graphics;
using Android.Widget;
using Com.Bumptech.Glide;

namespace WeClip.Droid
{
    [Activity(NoHistory = true, MainLauncher = true, Label = "WeClip", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation)]
    public class SplashActivity : AppComCustomeActivty
    {
        private static int SplashTimeOut = 3000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_screen);
            LoadAnimatedGif();
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }
            CrashManager.Register(this, "fc94917a0cd847b59b9df99d55de376a", new crManagerListner());
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() =>
            {
                Thread.Sleep(SplashTimeOut);
            });

            startupWork.ContinueWith(t =>
            {
                if (IsConnecting() == true)
                {
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    string UserName = prefs.GetString("LoginUserName", null);
                    string Password = prefs.GetString("Password", null);
                    string tokenValue = prefs.GetString("Access_Token", null);
                    string userID = prefs.GetString("UserID", null);
                    bool RegisterEmail = prefs.GetBoolean("RegisterEmail", false);
                    bool RegisterPhone = prefs.GetBoolean("RegisterPhone", false);

                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                    {
                        if (RegisterEmail == true)
                        {
                            StartActivity(new Intent(Application.Context, typeof(AddUserName)));
                            return;
                        }
                        else
                        if (RegisterEmail == true)
                        {
                            StartActivity(new Intent(Application.Context, typeof(AddUserName)));
                            return;
                        }
                        else
                            new LogInWeClip(UserName, Password, this).Execute();
                    }
                    else
                    {
                        StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
                    }
                }
                else
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alert.SetInverseBackgroundForced(true);
                    alert.SetTitle("No Internet connection");
                    alert.SetMessage("Check your Internet connection");
                    alert.SetPositiveButton("CLOSE", (senderAlert, args) =>
                    {
                        alert.Dispose();
                        this.Finish();
                    });
                    Android.App.Dialog dialog = alert.Create();
                    dialog.Show();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
            startupWork.Start();
        }

        public bool IsConnecting()
        {
            ConnectivityManager connectivity = (ConnectivityManager)this
              .GetSystemService(Context.ConnectivityService);

            if (connectivity != null)
            {
                NetworkInfo info = connectivity.ActiveNetworkInfo;
                if (info != null && info.GetState() == NetworkInfo.State.Connected)
                {
                    return true;
                }
            }
            return false;
        }

        private class crManagerListner : CrashManagerListener
        {
            public override bool ShouldAutoUploadCrashes()
            {
                return true;
            }
        }

        void LoadAnimatedGif()
        {
            var ivgif = FindViewById<ImageView>(Resource.Id.ivgif);

            Glide.With(this).Load(Resource.Drawable.balls).Into(ivgif);
            //webLoadingIcon.SetBackgroundColor(Color.Transparent);
            //webLoadingIcon.SetLayerType(LayerType.Software, null);
            //webLoadingIcon.SetWebViewClient(new WebViewController());
            //webLoadingIcon.LoadDataWithBaseURL("file:///android_asset/", "<html><center><img src=\"balls.gif\"></html>", "text/html", "utf-8", "");

        }

        private class WebViewController : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                return true;
            }
        }
    }
}