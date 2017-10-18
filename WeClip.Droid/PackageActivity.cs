using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "PackageActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PackageActivity : AppCompatActivity
    {
        protected GridView gvPackages;
        //private ImageView imageViewNext, imageViewBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.PremiumActivity);
                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);

                SetSupportActionBar(toolbar);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);


                SupportActionBar.Title = "";
                toolbar_title.Text = "Upgrade to Premium";
                gvPackages = FindViewById<GridView>(Resource.Id.gvPackages);
                gvPackages.ChoiceMode = ChoiceMode.Single;
                gvPackages.SetDrawSelectorOnTop(true);
             //   imageViewNext = FindViewById<ImageView>(Resource.Id.ivSelectPackagesNext);
             //   imageViewBack = FindViewById<ImageView>(Resource.Id.ivSelectPackageBack);
                new GetWeClipPackages(this, gvPackages).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("PackageActivity", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void ImageViewBack_Click(object sender, EventArgs e)
        {
            this.OnBackPressed();
        }

        private void ImageViewNext_Click(object sender, EventArgs e)
        {
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
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