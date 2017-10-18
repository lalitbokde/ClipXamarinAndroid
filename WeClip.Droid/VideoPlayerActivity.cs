using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using WeClip.Core.Model;
using WeClip.Core.Common;
using Android.Content.Res;
using Android.Media;
using Android.Runtime;
using Android.Graphics;
using WeClip.Droid.Helper;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WeClip.Droid.Services;
using Android.Util;

namespace WeClip.Droid
{
    [Activity(Label = "VideoPlayerActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VideoPlayerActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener, MediaPlayer.IOnErrorListener,
                                       ISurfaceHolderCallback, MediaPlayer.IOnBufferingUpdateListener, MediaController.IMediaPlayerControl
    {

        LinearLayout llVideoView;
        private MediaPlayer mediaPlayer;
        private MediaController mediaController;
        private ISurfaceHolder vidHolder;
        private SurfaceView playerSurface;
        private int buffered = 0;
        private Handler handler = new Handler();
        Android.Net.Uri videoUri;
        ProgressBar pDialog;
        private WeClipVideo videoInfo;
        private EventFiles EventvideoInfo;
        private string EventName;
        private DateTime? EventDate;
        private string videoFileName;
        private string videoUrl;
        private Android.Net.Uri localFileUrl;

        private string urlFromTheme;

        private string shareTitle, shareDate, sharePath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.VideoPlayer);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)FindViewById(Resource.Id.toolbar_title);
                pDialog = FindViewById<ProgressBar>(Resource.Id.progessbar);
                SetSupportActionBar(toolbar);

                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                var url = Intent.GetStringExtra("VideoUrl") ?? null;
                var urlFromEvent = Intent.GetStringExtra("VideoUrlFromEvent") ?? null;
                urlFromTheme = Intent.GetStringExtra("VideoUrlFromTheme") ?? null;


                if (url != null && url != "")
                {
                    SupportActionBar.Title = "";
                    toolbar_title.Text = "WeClip";
                    videoInfo = JsonConvert.DeserializeObject<WeClipVideo>(url);
                    videoUri = Android.Net.Uri.Parse(videoInfo.VideoFileUrl);
                    EventName = videoInfo.EventName;
                    EventDate = videoInfo.EventDate;
                    videoFileName = videoInfo.VideoFileName;
                    videoUrl = videoInfo.VideoFileUrl;

                    shareTitle = videoInfo.EventName;
                    shareDate = videoInfo.EventDate != null ? videoInfo.EventDate.ToString() : "";
                    sharePath = videoInfo.VideoFileUrl;
                }
                else if (!string.IsNullOrEmpty(urlFromEvent))
                {
                    SupportActionBar.Title = "";
                    toolbar_title.Text = "Event Video";
                    EventvideoInfo = JsonConvert.DeserializeObject<EventFiles>(urlFromEvent);
                    videoUri = Android.Net.Uri.Parse(EventvideoInfo.FileUrl);
                    EventName = EventvideoInfo.EventName;
                    videoFileName = EventvideoInfo.FileName;
                    videoUrl = EventvideoInfo.FileUrl;
                    shareTitle = EventvideoInfo.EventName;
                    shareDate = "";
                    sharePath = EventvideoInfo.FileUrl;
                }

                else if (!string.IsNullOrEmpty(urlFromTheme))
                {
                    SupportActionBar.Title = "";
                    videoInfo = JsonConvert.DeserializeObject<WeClipVideo>(urlFromTheme);
                    toolbar_title.Text = videoInfo.EventName;
                    videoUri = Android.Net.Uri.Parse(videoInfo.VideoFileUrl);
                    videoFileName = videoInfo.VideoFileName;
                    videoUrl = videoInfo.VideoFileUrl;

                    shareTitle = videoInfo.EventName;
                    shareDate = videoInfo.EventDate != null ? videoInfo.EventDate.ToString() : "";
                    sharePath = videoInfo.VideoFileUrl;

                }

                if (videoUri == null)
                {
                    AlertBox.Create("Alert", "Error in WeClip source.", this);
                    return;
                }
                if (getfileFromPath(videoFileName))
                {
                    videoUri = localFileUrl;
                }

                llVideoView = FindViewById<LinearLayout>(Resource.Id.llVideoView);
                playerSurface = FindViewById<SurfaceView>(Resource.Id.playerSurface);

                vidHolder = playerSurface.Holder;
                vidHolder.AddCallback(this);

                pDialog.Visibility = ViewStates.Visible;

                mediaController = new Android.Widget.MediaController(this);

            }
            catch (System.Exception ex)
            {

            }
        }

        private bool getfileFromPath(string videoFileName)
        {
            string extr = Android.OS.Environment.ExternalStorageDirectory.ToString();
            Java.IO.File file = new Java.IO.File(extr + "/TMMFOLDER", videoFileName);

            if (file.Exists())
            {
                localFileUrl = Android.Net.Uri.Parse(file.AbsolutePath);
                return true;
            }
            else
                return false;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflator = MenuInflater;
            inflator.Inflate(Resource.Menu.VideoPlayerMenu, menu);

            if (!string.IsNullOrEmpty(urlFromTheme))
            {
                menu.FindItem(Resource.Id.action_shareVideo).SetVisible(false);
                menu.FindItem(Resource.Id.action_DownloadVideo).SetVisible(false);
            }

            else
            {
                menu.FindItem(Resource.Id.action_shareVideo).SetVisible(true);
                menu.FindItem(Resource.Id.action_DownloadVideo).SetVisible(true);
            }

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;

                case Resource.Id.action_shareVideo:
                    Intent intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Android.Content.Intent.ExtraText, shareTitle + "\n" + shareDate + "\n" + sharePath + "\n" + "Download weclip from http://www.weclip.com");
                    this.StartActivity(Intent.CreateChooser(intent, "Share To"));
                    break;
                case Resource.Id.action_DownloadVideo:
                    install();
                    break;

                default:
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void install()
        {
            var IsDownloaded = getfileFromPath(videoFileName);

            if (IsDownloaded == false)
            {
                List<VideoDownloadInfo> mediaFiles = new List<VideoDownloadInfo>();

                mediaFiles.Add(new VideoDownloadInfo
                {
                    VideoUrl = videoUrl,
                    VideoFileName = videoFileName
                });

                Intent intent = new Intent(this, typeof(WeClipDownloadService));
                intent.PutExtra("UploadList", JsonConvert.SerializeObject(mediaFiles));
                this.StartService(intent);
                mediaFiles.Clear();
            }
            else
            {
                Toast.MakeText(this, "Video is already downloaded.", ToastLength.Short).Show();
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                SupportActionBar.Hide();
            }
            else
            {
                SupportActionBar.Show();
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (mediaController != null)
            {
                mediaController.Hide();
            }
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
                mediaPlayer.Release();
            }

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            mediaController.Show();
            return base.OnTouchEvent(e);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.SetDisplay(vidHolder);
            mediaPlayer.SetDataSource(this, videoUri);
            mediaPlayer.SetOnPreparedListener(this);
            mediaPlayer.SetAudioStreamType(Stream.Music);
            mediaPlayer.SetOnBufferingUpdateListener(this);
            mediaPlayer.SetOnErrorListener(this);
            mediaPlayer.PrepareAsync();

            mediaController = new Android.Widget.MediaController(this);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }

        public void OnBufferingUpdate(MediaPlayer mp, int percent)
        {
            buffered = percent;
            //       pDialog.SetMessage("Buffered " + percent + "%");
        }

        public void OnPrepared(MediaPlayer mp)
        {
            //   pDialog.Dismiss();

            pDialog.Visibility = ViewStates.Gone;
            setVideoSize();
            mediaController.SetMediaPlayer(this);
            mediaController.SetAnchorView(llVideoView);
            handler.Post(new RunnableMediaControllerHelper(this, mediaController));
            mp.Start();
            //SetDelayedHideForStatusBar();
        }

        public bool OnError(MediaPlayer mp, [GeneratedEnum] MediaError what, int extra)
        {
            //   pDialog.Dismiss();
            pDialog.Visibility = ViewStates.Gone;
            return false;
        }

        public int AudioSessionId
        {
            get
            {
                return 0;
            }
        }

        public int BufferPercentage
        {
            get
            {
                return buffered;
            }
        }

        public int CurrentPosition
        {
            get
            {
                if (mediaPlayer != null)
                {
                    return mediaPlayer.CurrentPosition;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Duration
        {
            get
            {
                if (mediaPlayer != null)
                {
                    return mediaPlayer.Duration;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void setVideoSize()
        {
            // // Get the dimensions of the video
            int videoWidth = mediaPlayer.VideoWidth;
            int videoHeight = mediaPlayer.VideoHeight;
            float videoProportion = (float)videoWidth / (float)videoHeight;

            // Get the width of the screen

            DisplayMetrics metrics = this.Resources.DisplayMetrics;
            int width = metrics.WidthPixels;
            int height = metrics.HeightPixels;

            float screenProportion = (float)width / (float)height;

            // Get the SurfaceView layout parameters
            Android.Views.ViewGroup.LayoutParams lp = playerSurface.LayoutParameters;
            if (videoProportion > screenProportion)
            {
                lp.Width = width;
                lp.Height = (int)((float)width / videoProportion);
            }
            else
            {
                lp.Width = (int)(videoProportion * (float)height);
                lp.Height = height;
            }
            // Commit the layout parameters
            playerSurface.LayoutParameters = (lp);
        }

        public bool IsPlaying
        {
            get
            {
                return mediaPlayer.IsPlaying;
            }
        }

        public bool CanPause()
        {
            return true;
        }

        public bool CanSeekBackward()
        {
            return true;
        }

        public bool CanSeekForward()
        {
            return true;
        }

        public void Pause()
        {
            mediaPlayer.Pause();
        }

        public void Start()
        {
            mediaPlayer.Start();
        }

        public void SeekTo(int pos)
        {
            mediaPlayer.SeekTo(pos);
        }

        // Runnable for displaying media controller once playing starts

        private class RunnableMediaControllerHelper : Java.Lang.Object, Java.Lang.IRunnable
        {
            private readonly Context _context;
            private MediaController _mediaController;

            public RunnableMediaControllerHelper(Context context, MediaController mediaController)
            {
                this._context = context;
                this._mediaController = mediaController;
            }

            public void Run()
            {
                _mediaController.Enabled = true;
                _mediaController.Show();
            }
        }

    }
}