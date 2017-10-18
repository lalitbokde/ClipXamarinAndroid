using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Provider;
using Android.Runtime;
using Android.Database;
using WeClip.Droid.AsyncTask;
using Android.Views;
using Square.Picasso;

namespace WeClip.Droid
{
    [Activity(Label = "AddEventPic", WindowSoftInputMode = SoftInput.StateAlwaysHidden, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddEventPic : AppCompatActivity
    {
        protected ImageButton imgCreateEvent, imgBack;
        protected ImageView imgEventPic;
        protected EditText eventTag;
        private string eventData;
        Android.Net.Uri selectedImagePath;
        protected static int CAMERA_REQUEST = 1337;
        protected static int GALLERY_PICTURE = 1;
        private ImageToPath imageToPath;
        private string uploadImagePath;
        private EventModel eventModel;
        private Button btnUploadEventDefultPic;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AddEventPic);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SupportActionBar.Title = "";
                toolbar_title.Text = "Post picture";

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                eventData = this.Intent.GetStringExtra("strCreateEvent") ?? null;
                if (eventData != null)
                {
                    imageToPath = new ImageToPath();
                    imgEventPic = FindViewById<ImageView>(Resource.Id.ivEventPic);
                    imgBack = FindViewById<ImageButton>(Resource.Id.ivAddEventPicBack);
                    imgCreateEvent = FindViewById<ImageButton>(Resource.Id.ivCreateEvent);
                    eventTag = FindViewById<EditText>(Resource.Id.etEventDescription);
                    btnUploadEventDefultPic = FindViewById<Button>(Resource.Id.btnUploadEventDefultPic);
                    imgEventPic.Click += ImgEventPic_Click;
                    btnUploadEventDefultPic.Click += ImgEventPic_Click;

                    imgCreateEvent.Click += ImgCreateEvent_Click;
                    imgBack.Click += ImgBack_Click;
                    eventModel = JsonConvert.DeserializeObject<EventModel>(eventData);

                    if (eventModel.EventID != 0)
                    {
                        fillEventPicData();
                    }
                }
                else
                {
                    this.Finish();
                }
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("AddEventPic", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void ImgBack_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(this);
                this.Finish();
                Intent intent = new Intent(this, typeof(CreateEvent));
                intent.PutExtra("strAddEventPic", eventData);
                this.StartActivity(intent);
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("AddEventPic", "ImgBack_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void fillEventPicData()
        {
            eventTag.Text = eventModel.EventTag;
            eventTag.SetSelection(eventTag.Text.Length);

            if (!string.IsNullOrEmpty(eventModel.EventPicUrl))
            {
                btnUploadEventDefultPic.Visibility = ViewStates.Gone;
                imgEventPic.Visibility = ViewStates.Visible;
                Picasso.With(this).Load(eventModel.EventPicUrl)
                  .Resize(150, 150).Into(imgEventPic);
            }
        }

        private void ImgCreateEvent_Click(object sender, EventArgs e)
        {
            try
            {
                eventModel.EventTag = eventTag.Text;
                HideKeyBoard.hideSoftInput(this);

                if (!string.IsNullOrEmpty(uploadImagePath))
                {
                    var uploadfile = new UploadFileBackGround(this, eventModel, uploadImagePath);
                    uploadfile.Execute();
                }
                else
                {
                    ProgressDialog p = ProgressDialog.Show(this, "Uploading Data", "Please wait...");
                    var addUpdateEvent = new CreateEventInBackGround(eventModel, this, p);
                    addUpdateEvent.Execute();
                }
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("AddEventPic", "ImgCreateEvent_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            if (resultCode == Result.Canceled)
            {
                btnUploadEventDefultPic.Visibility = ViewStates.Visible;
                imgEventPic.Visibility = ViewStates.Gone;
            }

            if ((requestCode == CAMERA_REQUEST) && (resultCode == Result.Ok))
            {
                btnUploadEventDefultPic.Visibility = ViewStates.Gone;
                imgEventPic.Visibility = ViewStates.Visible;


                try
                {
                    int height = imgEventPic.Height;
                    int width = imgEventPic.Width;
                    BitmapHelper.bitmap = BitmapHelper._file.Path.LoadAndResizeBitmap(width, height);

                    uploadImagePath = BitmapHelper._file.AbsolutePath;

                    if (BitmapHelper.bitmap != null)
                    {
                        imgEventPic.SetImageBitmap(BitmapHelper.bitmap);
                        BitmapHelper.bitmap = null;
                    }
                    // Dispose of the Java side bitmap.
                    GC.Collect();
                }
                catch (Java.Lang.Exception ex)
                {
                    new CrashReportAsync("AddEventPic", "OnActivityResult", ex.Message + ex.StackTrace).Execute();
                }
            }
            if ((requestCode == GALLERY_PICTURE) && (resultCode == Result.Ok) && (data != null))
            {
                btnUploadEventDefultPic.Visibility = ViewStates.Gone;
                imgEventPic.Visibility = ViewStates.Visible;

                if (data.Data != null)
                {
                    selectedImagePath = data.Data;
                    uploadImagePath = MediaHelper.GetMediaPath(ApplicationContext, selectedImagePath);
                    Picasso.With(this).Load(selectedImagePath)
            .Resize(150, 150).Into(imgEventPic);

                }
            }
        }

        private void ImgEventPic_Click(object sender, EventArgs e)
        {
            string[] items = { "Take photo", "Choose from gallery", "Cancel" };
            Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
            alertDialog.SetTitle("Upload event picture");
            alertDialog.SetItems(items, new dialogeItemClickListner(this));
            alertDialog.Show();
        }

        public string getRealPathFromURI(Android.Net.Uri contentUri)
        {
            try
            {
                string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
                ICursor cursor = this.ContentResolver.Query(contentUri, proj, null, null, null);
                int column_index = cursor
                        .GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                return cursor.GetString(column_index);
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("AddEventPic", "getRealPathFromURI", ex.Message + ex.StackTrace).Execute();
            }
            return null;
        }

        private class dialogeItemClickListner : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            private AddEventPic context;

            public dialogeItemClickListner(AddEventPic context)
            {
                this.context = context;
            }

            public void OnClick(IDialogInterface dialog, int which)
            {
                try
                {
                    if (which == 0)
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        BitmapHelper._file = ImageToPath.GetOutputMediaFile(ImageToPath.UploadFileType.image);
                        BitmapHelper.captureImageUri = Android.Net.Uri.FromFile(BitmapHelper._file);
                        intent.PutExtra(MediaStore.ExtraOutput, BitmapHelper.captureImageUri);
                        context.StartActivityForResult(intent, CAMERA_REQUEST);
                    }
                    else
                 if (which == 1)
                    {
                        Intent pictureActionIntent = null;
                        pictureActionIntent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
                        context.StartActivityForResult(pictureActionIntent, GALLERY_PICTURE);
                    }
                }
                catch (Java.Lang.Exception ex)
                {
                    new CrashReportAsync("dialogeItemClickListner", "OnClick", ex.Message + ex.StackTrace).Execute();
                }
            }
        }
    }
}
