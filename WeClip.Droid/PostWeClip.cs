using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Content;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "PostWeClip", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PostWeClip : AppCompatActivity
    {
        protected ImageButton ivCreateWeClip, ivPostWeClipBack;
        protected EditText etWeClipTitle;
        protected EditText etWecliptag;
        protected string strWeClipdata;
        protected WeClipInfo wcHolder;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.PostWeClip);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "";
                toolbar_title.Text = "Post WeClip";
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }
                etWeClipTitle = FindViewById<EditText>(Resource.Id.etWeClipTitle);
                etWecliptag = FindViewById<EditText>(Resource.Id.etWeClipTags);
                ivCreateWeClip = FindViewById<ImageButton>(Resource.Id.ivPostWeClipCreate);
                ivPostWeClipBack = FindViewById<ImageButton>(Resource.Id.ivPostWeClipBack);
                strWeClipdata = this.Intent.GetStringExtra("strMusicView") ?? null;

                if (!string.IsNullOrEmpty(strWeClipdata))
                {
                    wcHolder = JsonConvert.DeserializeObject<WeClipInfo>(strWeClipdata);
                    ivCreateWeClip.Click += IvCreateWeClip_Click;
                    ivPostWeClipBack.Click += IvPostWeClipBack_Click;
                }
                else
                {
                    AlertBox.Create("Error", "Error occured.", this);
                    return;
                }
            }
            catch (Exception ex)
            {
                new CrashReportAsync("PostWeClip", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void IvPostWeClipBack_Click(object sender, EventArgs e)
        {
            HideKeyBoard.hideSoftInput(this);
            Intent intent = new Intent(this, typeof(ThemeActivity));
            // intent.PutExtra("strMusicView", strWeClipdata);
            intent.PutExtra("strEventGalleryView", strWeClipdata);
            StartActivity(intent);
            this.Finish();
        }

        private void IvCreateWeClip_Click(object sender, EventArgs e)
        {
            wcHolder.Tag = etWecliptag.Text;
            wcHolder.Title = etWeClipTitle.Text;

            if (wcHolder.Title.Trim() == "")
            {
                AlertBox.Create("Error", "Please enter title.", this);
                return;
            }
            HideKeyBoard.hideSoftInput(this);
            CreateWeClip createWeClip = new CreateWeClip(this, wcHolder);
            createWeClip.Execute();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Intent intent = new Intent(this, typeof(MusicView));
            intent.PutExtra("strEventGalleryView", strWeClipdata);
            StartActivity(intent);
            this.Finish();
        }
    }
}