using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using WeClip.Core.Model;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using WeClip.Droid.Helper;
using WeClip.Droid.AsyncTask;
using WeClip.Droid.Adapters;
using Android.Graphics;

namespace WeClip.Droid
{
    [Activity(Label = "ThemeActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ThemeActivity : AppCompatActivity
    {
        protected GridView gvTheme;
        private string strEventData;
        private WeClipInfo wcHolder;
        private ImageView imageViewNext, imageViewBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.ThemeView);
                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);

                SetSupportActionBar(toolbar);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                SupportActionBar.Title = "";
                toolbar_title.Text = "Select theme";
                imageViewNext = FindViewById<ImageView>(Resource.Id.ivSelectThemeNext);
                imageViewBack = FindViewById<ImageView>(Resource.Id.ivSelectThemeBack);
                //strEventData = this.Intent.GetStringExtra("strMusicView") ?? null;

                strEventData = this.Intent.GetStringExtra("strEventGalleryView") ?? null;


                if (!string.IsNullOrEmpty(strEventData))
                {
                    wcHolder = JsonConvert.DeserializeObject<WeClipInfo>(strEventData);
                    gvTheme = FindViewById<GridView>(Resource.Id.gvTheme);
                    gvTheme.ChoiceMode = ChoiceMode.Single;
                    gvTheme.SetDrawSelectorOnTop(true);
                    imageViewNext.Click += ImageViewNext_Click;
                    imageViewBack.Click += ImageViewBack_Click;
                    new GetThemeFile(this, gvTheme, wcHolder).Execute();
                }
                else
                {
                    AlertBox.Create("Error", "Error occured", this);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("Theme", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void ImageViewBack_Click(object sender, EventArgs e)
        {
            this.OnBackPressed();
        }

        private void ImageViewNext_Click(object sender, EventArgs e)
        {

            if (wcHolder.ThemeID == 0)
            {
                AlertBox.Create("Error", "Please select theme", this);
                return;
            }

            var data = JsonConvert.SerializeObject(wcHolder);
            Intent intent = new Intent(this, typeof(PostWeClip));
            intent.PutExtra("strMusicView", data);
            StartActivity(intent);
            this.Finish();
        }

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
            //this.Finish();
            //Intent intent = new Intent(this, typeof(MusicView));
            //intent.PutExtra("strEventGalleryView", strEventData);
            //this.StartActivity(intent);

            Intent intent = new Intent(this, typeof(EventGalleryView));
            intent.PutExtra("strEventGallery", wcHolder.EventID);
            intent.PutExtra("strDefultEvent", wcHolder.IsDefault);
            this.StartActivity(intent);
            this.Finish();
        }

        private class GetThemeFile : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<Theme>>
        {
            private GridView gvTheme;
            private Activity activity;
            private WeClipInfo wcHolder;
            private List<Theme> themelist;
            ProgressDialog p;

            public GetThemeFile(ThemeActivity activity, GridView gvTheme, WeClipInfo wcHolder)
            {
                this.activity = activity;
                this.gvTheme = gvTheme;
                this.wcHolder = wcHolder;
                themelist = new List<Theme>();
                p = ProgressDialog.Show(activity, "Please wait", "Loading data");
            }

            protected override List<Theme> RunInBackground(params Java.Lang.Void[] @params)
            {
                themelist = RestSharpCall.GetList<Theme>("Event/GetWeClipThemes");
                return themelist;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (themelist != null)
                {
                    gvTheme.Adapter = new ThemeViewAdapter(themelist, activity);
                    gvTheme.OnItemClickListener = new grvOnClickListner(activity, wcHolder, themelist, gvTheme);
                }
            }

            private class grvOnClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
            {
                private Activity activity;
                private List<Theme> themeList;
                private WeClipInfo wcHolder;
                private int backposition = -1;
                private GridView gvTheme;

                public grvOnClickListner(Activity activity, WeClipInfo wcHolder, List<Theme> themeList, GridView gvTheme)
                {
                    this.gvTheme = gvTheme;
                    this.activity = activity;
                    this.themeList = themeList;
                    this.wcHolder = wcHolder;
                }

                public void OnItemClick(AdapterView parent, View view, int position, long id)
                {
                    wcHolder.ThemeID = themeList[position].ID;

                    var selectedItem = parent.GetItemAtPosition(position).ToString();
                    var GridViewItems = (LinearLayout)view;
                    GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                    var BackSelectedItem = (LinearLayout)gvTheme.GetChildAt(backposition);
                    if (backposition != -1)
                    {
                        BackSelectedItem.Selected = (false);
                        BackSelectedItem.SetBackgroundColor(Color.Transparent);
                    }

                    if (backposition == position)
                    {
                        GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                    }

                    backposition = position;
                }
            }
        }
    }
}
