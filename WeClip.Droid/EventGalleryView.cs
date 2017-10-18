using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using WeClip.Core.Model;
using Android.Views;
using System;
using System.Linq;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content;
using WeClip.Droid.AsyncTask;
using Exception = Java.Lang.Exception;
using static WeClip.Droid.AsyncTask.GetEventFile;
using WeClip.Droid.Helper;
using WeClip.Core.Common;
using static WeClip.Droid.AsyncTask.GetDefultEventFile;
using System.Net;

namespace WeClip.Droid
{
    [Activity(Label = "EventPhotoView", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]

    public class EventGalleryView : AppCompatActivity
    {
        protected GridView gvEventPhoto;
        private ImageView ivNext, ivBack;
        protected TextView noOfImage;
        protected ImageInfo imageInfo;
        private long EventID;
        private bool IsDefaultEvent;
        protected List<ImageInfo> selectedMedia;
        private List<EventFiles> selectedItem;
        private int MaxImageForWeClip, MaxVideoForWeclip;
        private long MaxVideoMinute;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                MaxImageForWeClip = prefs.GetInt("MaxImageForWeClip", 0);
                MaxVideoMinute = prefs.GetLong("MaxVideoDuration", 0);
                MaxVideoForWeclip = prefs.GetInt("MaxVideoForWeclip", 0);

                SetContentView(Resource.Layout.EventGalleryView);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);

                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "";
                Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }
                EventID = Intent.GetLongExtra("strEventGallery", 0);
                IsDefaultEvent = Intent.GetBooleanExtra("strDefultEvent", false);
                if (EventID == 0)
                {
                    AlertBox.Create("Error", "Error occurred!", this);
                    return;
                }

                gvEventPhoto = FindViewById<GridView>(Resource.Id.gvEventPhotos);
                noOfImage = FindViewById<TextView>(Resource.Id.etNoOfImage);
                ivNext = FindViewById<ImageView>(Resource.Id.ivCreateWeClipNext);
                ivBack = FindViewById<ImageView>(Resource.Id.ivCreateWeClipBack);

                ivNext.Click += IvNext_Click;
                ivBack.Click += IvBack_Click;
                gvEventPhoto.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.Multiple;
                if (IsDefaultEvent == false)
                {
                    toolbar_title.Text = "Create WeClip";
                    new GetEventFile(gvEventPhoto, this, noOfImage, EventID).Execute();
                }
                else
                {
                    toolbar_title.Text = "Edit my story";
                    new GetDefultEventFile(gvEventPhoto, this, noOfImage, EventID).Execute();
                }
            }
            catch (Exception ex)
            {
                new CrashReportAsync("EventGalleryView", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void IvBack_Click(object sender, EventArgs e)
        {
            this.OnBackPressed();
        }

        private void IvNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDefaultEvent == false)
                {
                    selectedItem = ListOfEventFiles.listOfEventFiles.Where(x => x.isSelected() == true).ToList();
                }

                else
                {
                    selectedItem = ListOfDefultEventFiles.listOfEventFiles.Where(x => x.isSelected() == true).ToList();
                }

                selectedMedia = new List<ImageInfo>();

                double totalduration = 0;

                var  ImageSelected = (from x in selectedItem where x.IsVideo == false select x).Count();
                var  VideoSelected = (from x in selectedItem where x.IsVideo == true select x).Count();


                foreach (var file in selectedItem)
                {
                    if (file.IsVideo == false)
                    {
                        file.Duration = 0.05;
                    }
                    totalduration = totalduration + file.Duration;
                }

                if (selectedItem.Count < GlobalClass.MinNoOfSelectedFile)
                {
                    AlertBox.Create("Error", "Please select atleast 3 files!", this);
                    return;
                }

                if (totalduration > MaxVideoMinute)
                {
                    Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                    alertDialog.SetMessage("Upgrade to WeClip Premium and create movies with even more photos.");
                    alertDialog.SetPositiveButton("Go PREMIUM", (senderAlert, args) =>
                    {
                        Intent intent1 = new Intent(this, typeof(PackageActivity));
                        StartActivity(intent1);
                    });
                    alertDialog.SetNegativeButton("NO THANKS", (senderAlert, args) =>
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                    return;
                }

                if (ImageSelected > MaxImageForWeClip)
                {

                    Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                    alertDialog.SetMessage("Upgrade to WeClip Premium and create movies with even more photos.");
                    alertDialog.SetPositiveButton("Go PREMIUM", (senderAlert, args) =>
                    {
                        Intent intent1 = new Intent(this, typeof(PackageActivity));
                        StartActivity(intent1);
                    });
                    alertDialog.SetNegativeButton("NO THANKS", (senderAlert, args) =>
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                    return;
                }


                if (VideoSelected > MaxVideoForWeclip)
                {

                    Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                    alertDialog.SetMessage("Upgrade to WeClip Premium and create movies with even more videos.");
                    alertDialog.SetPositiveButton("Go PREMIUM", (senderAlert, args) =>
                    {
                        Intent intent1 = new Intent(this, typeof(PackageActivity));
                        StartActivity(intent1);
                    });
                    alertDialog.SetNegativeButton("NO THANKS", (senderAlert, args) =>
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                    return;
                }

                foreach (var data in selectedItem)
                {
                    imageInfo = new ImageInfo();
                    imageInfo.IsVideo = data.IsVideo;
                    imageInfo.Filename = data.FileName;
                    selectedMedia.Add(imageInfo);
                }

                //WeClipInfo wc_holder = new WeClipInfo();
                //wc_holder.MediaFile = selectedMedia;
                //wc_holder.EventID = EventID;
                //wc_holder.IsDefault = IsDefaultEvent;
                //var jasondata = JsonConvert.SerializeObject(wc_holder);
                //Intent intent = new Intent(this, typeof(MusicView));
                //intent.PutExtra("strEventGalleryView", jasondata);
                //StartActivity(intent);
                //this.Finish();

                WeClipInfo wc_holder = new WeClipInfo();
                wc_holder.MediaFile = selectedMedia;
                wc_holder.EventID = EventID;
                wc_holder.IsDefault = IsDefaultEvent;
                var jasondata = JsonConvert.SerializeObject(wc_holder);
                Intent intent = new Intent(this, typeof(ThemeActivity));
                intent.PutExtra("strEventGalleryView", jasondata);
                StartActivity(intent);
                this.Finish();

            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventGalleryView", "IvNext_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnBackPressed()
        {
            this.Finish();
        }
    }
}