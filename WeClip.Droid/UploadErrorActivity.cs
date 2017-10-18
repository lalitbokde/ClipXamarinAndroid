using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Droid.AsyncTask;
using Android.Content;

namespace WeClip.Droid
{
    [Activity(Label = "UploadErrorActivity", LaunchMode = Android.Content.PM.LaunchMode.SingleTop, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UploadErrorActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.UploadError);

                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "";
                toolbar_title.Text = "WeClip";

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetMessage("Upgrade to WeClip Premium and get more space.");
                alertDialog.SetPositiveButton("Go PREMIUM", (senderAlert, args) =>
                {
                    Intent intent1 = new Intent(this, typeof(PackageActivity));
                    StartActivity(intent1);
                });
                alertDialog.SetNegativeButton("NO THANKS", (senderAlert, args) =>
                {
                    alertDialog.Dispose();
                    this.Finish();
                });
                alertDialog.Show();
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("UploadErrorActivity", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;
            }
            return true;
        }
    }
}