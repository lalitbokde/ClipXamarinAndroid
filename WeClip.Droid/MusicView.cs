using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using WeClip.Core.Model;
using Java.Lang;
using WeClip.Droid.Helper;
using Android.Util;
using Android.Media;
using Android.Views;
using System;
using Newtonsoft.Json;
using Android.Content;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "MusicView", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MusicView : AppCompatActivity
    {
        protected GridView gvMusic;
        MediaPlayer mp;
        private string strEventData;
        private WeClipInfo wcHolder;
        private ImageView imageViewNext, imageViewBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AudioView);
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
                toolbar_title.Text = "Select music";
                imageViewNext = FindViewById<ImageView>(Resource.Id.ivSelectMusicNext);
                imageViewBack = FindViewById<ImageView>(Resource.Id.ivSelectMusicBack);
                strEventData = this.Intent.GetStringExtra("strEventGalleryView") ?? null;

                if (!string.IsNullOrEmpty(strEventData))
                {
                    wcHolder = JsonConvert.DeserializeObject<WeClipInfo>(strEventData);
                    mp = new MediaPlayer();
                    mp.SetAudioStreamType(Stream.Music);
                    gvMusic = FindViewById<GridView>(Resource.Id.gvMusic);
                    gvMusic.ChoiceMode = ChoiceMode.Single;
                    gvMusic.SetDrawSelectorOnTop(true);
                    imageViewNext.Click += ImageViewNext_Click;
                    imageViewBack.Click += ImageViewBack_Click;
                    new GetAudioFile(this, gvMusic, wcHolder).Execute();
                }
                else
                {
                    AlertBox.Create("Error", "Error occured", this);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("MusicView", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void ImageViewBack_Click(object sender, EventArgs e)
        {
            this.OnBackPressed();
        }

        private void ImageViewNext_Click(object sender, EventArgs e)
        {
            mp.Stop();
            var data = JsonConvert.SerializeObject(wcHolder);
            Intent intent = new Intent(this, typeof(ThemeActivity));
            intent.PutExtra("strMusicView", data);
            StartActivity(intent);
            this.Finish();
        }

        public void playAudioFile(string filename, bool playFile)
        {
            try
            {
                Log.Info("File Path", filename);
                if (mp.IsPlaying)
                {
                    mp.Stop();
                    mp.Reset();
                }
                if (filename != null && playFile)
                {
                    mp.Reset();
                    var asyncplayer = new StartPlayer(this, mp);
                    asyncplayer.Execute(filename);
                }
                else if (filename.Equals("") && !playFile)
                {
                    if (mp.IsPlaying)
                    {
                        mp.Stop();
                    }
                }
            }
            catch (IllegalArgumentException ex)
            {
                new CrashReportAsync("MusicView", "playAudioFile", ex.Message + ex.StackTrace).Execute();
            }
            catch (SecurityException ex)
            {
                new CrashReportAsync("MusicView", "playAudioFile", ex.Message + ex.StackTrace).Execute();
            }
            catch (IllegalStateException ex)
            {
                new CrashReportAsync("MusicView", "playAudioFile", ex.Message + ex.StackTrace).Execute();
            }
            catch (Java.IO.IOException ex)
            {
                new CrashReportAsync("MusicView", "playAudioFile", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if (mp.IsPlaying)
            {
                mp.Stop();
            }
            this.Finish();
            Intent intent = new Intent(this, typeof(EventGalleryView));
            intent.PutExtra("strEventGallery", wcHolder.EventID);
            intent.PutExtra("strDefultEvent", wcHolder.IsDefault);
            this.StartActivity(intent);
        }
    }
}