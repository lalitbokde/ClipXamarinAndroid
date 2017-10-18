using System;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "FriendRequestActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class FriendRequestActivity : AppCompatActivity
    {
        private ListView lvFriendRequest;
        private ImageView ivBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.FriendRequest);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "";
                toolbar_title.Text = "Friend Request";
                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }
                lvFriendRequest = FindViewById<ListView>(Resource.Id.lvFriendRequest);
                ivBack = FindViewById<ImageView>(Resource.Id.btnFRback);
                ivBack.Click += IvBack_Click;
                new GetFriendRequest(lvFriendRequest, this).Execute();
            }
            catch (Exception ex)
            {
                new CrashReportAsync("EventGalleryView", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void IvBack_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        public override void OnBackPressed()
        {
            this.Finish();
        }
    }
}