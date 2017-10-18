using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Hardware;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using WeClip.Droid.AsyncTask;
using WeClip.Droid.Services;
using Square.Picasso;
using WeClip.Core.Common;
using Newtonsoft.Json;

namespace WeClip.Droid
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UploadEventProfilePic : AppCompatActivity, Helper.OrientationManager.OrientationListener
    {
        public long EventID, EventCreatorID;
        private bool IsDefault;

        CameraFacing _cameraFacing = CameraFacing.Back;
        FlashState _flashState = FlashState.NotAvailable;
        CaptureMode _captureMode = CaptureMode.Photo;
        private FrameLayout camera_preview;
        private Android.Hardware.Camera _camera;
        CameraPreview _preview;
        string mediaFilePath = "";
        List<MediaFile> mediaFiles = new List<MediaFile>();
        private Button btnPhotos, btnCamera;
        private ImageButton btnCapture, btnSwitchCamera, btnFlash;
        private string uploadImagePath;
        private RelativeLayout rvCamera, rvGalleryView;
        protected static int GALLERY_PICTURE = 1;
        private ImageView ivSelectedImage;
        Android.Net.Uri selectedImagePath;
        private LinearLayout llCaptureView;
        private ImageButton ivImageCaptureCameraUpload, ivGalleryPhotoVideoUpload;
        OrientationManager orientationManager;
        int imageRotation = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AddDefaultEventPic);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "";
                toolbar_title.Text = "Take a picture";
                orientationManager = new OrientationManager(this, SensorDelay.Normal, this);
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                EventID = Intent.GetLongExtra("strCapture", 0);
                EventCreatorID = Intent.GetLongExtra("strCreatorId", 0);
                IsDefault = Intent.GetBooleanExtra("strIsDefaultEvent", false);
                camera_preview = FindViewById<FrameLayout>(Resource.Id.camera_previewE);
                btnPhotos = FindViewById<Button>(Resource.Id.btnPhotosE);
                btnCamera = FindViewById<Button>(Resource.Id.btnCameraE);
                btnCapture = FindViewById<ImageButton>(Resource.Id.ivImageCapturebtnE);
                ivGalleryPhotoVideoUpload = FindViewById<ImageButton>(Resource.Id.ivImageCaptureGalleryUploadE);
                btnSwitchCamera = FindViewById<ImageButton>(Resource.Id.btnSwitchCameraE);
                btnFlash = FindViewById<ImageButton>(Resource.Id.btnFlashE);
                ivImageCaptureCameraUpload = FindViewById<ImageButton>(Resource.Id.ivImageCaptureCameraUploadE);
                rvCamera = FindViewById<RelativeLayout>(Resource.Id.rvCameraViewE);
                rvGalleryView = FindViewById<RelativeLayout>(Resource.Id.rvGalleryViewE);
                ivSelectedImage = FindViewById<ImageView>(Resource.Id.ivSelectedImageE);
                llCaptureView = FindViewById<LinearLayout>(Resource.Id.llCaptureViewE);
                btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
                btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
                btnPhotos.Click += BtnPhotos_Click;
                btnCamera.Click += BtnCamera_Click;
                btnCapture.Click += BtnCapture_Click;
                btnSwitchCamera.Click += BtnSwitchCamera_Click;
                btnFlash.Click += BtnFlash_Click;
                ivGalleryPhotoVideoUpload.Click += IvGalleryPhotoVideoUpload_Click;
                if (EventID == 0)
                {
                    AlertBox.Create("Error", "Error occurred!", this);
                    return;
                }

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("ImageCapture", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        public void OnOrientationChange(OrientationManager.ScreenOrientation screenOrientation)
        {
            switch (screenOrientation)
            {
                case OrientationManager.ScreenOrientation.PORTRAIT:
                    imageRotation = 90;
                    break;

                case OrientationManager.ScreenOrientation.REVERSED_PORTRAIT:
                    imageRotation = 270;
                    break;

                case OrientationManager.ScreenOrientation.LANDSCAPE:
                    imageRotation = 0;
                    break;

                case OrientationManager.ScreenOrientation.REVERSED_LANDSCAPE:
                    imageRotation = 180;
                    break;
            }
        }

        private void IvGalleryPhotoVideoUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uploadImagePath))
            {
                AlertBox.Create("Error", "Select file from gallery or capture image", this);
                return;
            }
            else
            {
                new UploadEventProfileFileInBackGround(this, EventID, uploadImagePath, EventCreatorID).Execute();
            }
        }

        private void BtnSwitchCamera_Click(object sender, EventArgs e)
        {
            switch (_cameraFacing)
            {
                case CameraFacing.Back:
                    _cameraFacing = CameraFacing.Front;
                    break;
                case CameraFacing.Front:
                    _cameraFacing = CameraFacing.Back;
                    break;
                default:
                    return;
            }

            ReleaseCamera();
            InitializeCamera();
        }

        private void BtnFlash_Click(object sender, EventArgs e)
        {
            SetFlash();
        }

        // Open Photo Library
        private void BtnPhotos_Click(object sender, EventArgs e)
        {
            btnPhotos.SetTextColor(Color.Rgb(0, 208, 150));
            btnCamera.SetTextColor(Color.Rgb(217, 217, 217));
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.PutExtra(Intent.ExtraLocalOnly, true);
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), GALLERY_PICTURE);
        }

        // Open Video Library

        // Open In App Camera
        private void BtnCamera_Click(object sender, EventArgs e)
        {

            if (_captureMode == CaptureMode.Photo)
            {
                ReleaseCamera();
            }

            ivImageCaptureCameraUpload.Visibility = ViewStates.Gone;
            rvGalleryView.Visibility = ViewStates.Gone;
            rvCamera.Visibility = ViewStates.Visible;
            llCaptureView.Visibility = ViewStates.Visible;
            InitializeCamera();
            btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
            btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
        }

        // Initialize Camera
        private void InitializeCamera()
        {
            _camera = CameraHelper.GetCameraInstance((int)_cameraFacing);
            if (_camera != null)
            {
                //CameraHelper.SetCameraDisplayOrientation(this, 0, _camera);
                SurfaceOrientation displayRotation = this.WindowManager.DefaultDisplay.Rotation;

                // Create our Preview view and set it as the content of our activity.
                _preview = new CameraPreview(this, _camera, (int)_cameraFacing, displayRotation);

                //IList<Android.Hardware.Camera.Size> sizes = _camera.GetParameters().SupportedPreviewSizes;
                //_camera.GetParameters().SetPreviewSize(sizes[0].Width, sizes[0].Height);

                camera_preview.AddView(_preview);

                SetFlash();
            }
            else
            {
                AlertBox.Create("", "Could not access camera!", this);
            }
        }

        // Set Flash
        private void SetFlash()
        {
            CameraHelper.SwitchFlashMode(_camera, ref _flashState);

            int resourceID;

            switch (_flashState)
            {
                case FlashState.Auto:
                    resourceID = Resource.Drawable.ic_flash_auto_white_24dp;
                    break;
                case FlashState.On:
                    resourceID = Resource.Drawable.ic_flash_on_white_24dp;
                    break;
                case FlashState.Off:
                    resourceID = Resource.Drawable.ic_flash_off_white_24dp;
                    break;
                case FlashState.NotAvailable:
                    btnFlash.Visibility = ViewStates.Gone;
                    return;
                default:
                    btnFlash.Visibility = ViewStates.Gone;
                    return;
            }

            btnFlash.SetImageResource(resourceID);
        }

        // Capture Photo or Video
        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (_captureMode == CaptureMode.Photo)      // Capture Photo
            {
                int rotation = imageRotation;
                mediaFilePath = CameraHelper.GetOutputMediaFile(CaptureMode.Photo).ToString();
                if (_cameraFacing == CameraFacing.Front)
                {
                    switch (rotation)
                    {
                        case 90:
                            //PORTRAIT
                            rotation = 270;
                            break;
                        case 270:
                            //REVERSED_PORTRAIT
                            rotation = 90;
                            break;

                        case 0:
                            //landscap
                            rotation = 0;
                            break;

                        case 180:
                            //REVERSED_landscap
                            rotation = 180;
                            break;
                    }
                }

                CameraPictureCallback _picture = new CameraPictureCallback(mediaFilePath, rotation);
                _picture.PictureTakenCompleted += delegate
                {
                    if (!string.IsNullOrEmpty(mediaFilePath))
                    {
                        var uploadfile = new UploadEventProfileFileInBackGround(this, EventID, mediaFilePath, EventCreatorID);
                        uploadfile.Execute();
                    }
                };
                _camera.TakePicture(null, null, _picture);
            }
        }

        // Used to reset camera and preview after photo or video capture
        private void ResetCamera()
        {
            ReleaseCamera();
            InitializeCamera();
        }

        // Releasing Camera
        private void ReleaseCamera()
        {
            if (_camera != null)
            {
                _camera.StopPreview();  // stop the preview

                _camera.Release();      // release the camera for other applications
                _camera = null;

                if (_preview != null)
                {
                    camera_preview.RemoveView(_preview);
                    _preview.Dispose();
                    _preview = null;
                }
            }
            mediaFilePath = "";
        }

        // Releasing MediaRecorder and Camera while application goes in background
        protected override void OnPause()
        {
            base.OnPause();
            ReleaseCamera();
            if (orientationManager != null)
            {
                orientationManager.Disable();
            }
        }

        // Releasing MediaRecorder and Camera while activity is been destroyed
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ReleaseCamera();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == GALLERY_PICTURE) && (resultCode == Result.Ok) && (data != null))
            {
                rvGalleryView.Visibility = ViewStates.Visible;
                rvCamera.Visibility = ViewStates.Gone;
                llCaptureView.Visibility = ViewStates.Gone;
                ivImageCaptureCameraUpload.Visibility = ViewStates.Visible;

                if (data.Data != null)
                {
                    selectedImagePath = data.Data;
                    uploadImagePath = MediaHelper.GetMediaPath(ApplicationContext, selectedImagePath);
                    Picasso.With(this).Load(new Java.IO.File(uploadImagePath)).Placeholder(Resource.Drawable.default_event_back)
               .Resize(150, 150).Into(ivSelectedImage);

                }
            }

            if (resultCode == Result.Canceled)
            {
                ivImageCaptureCameraUpload.Visibility = ViewStates.Gone;
                rvGalleryView.Visibility = ViewStates.Gone;
                rvCamera.Visibility = ViewStates.Visible;
                llCaptureView.Visibility = ViewStates.Visible;
                InitializeCamera();
                btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
                btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        // Adds the captured Photo or Video file path to "mediaFiles" list
        private void AddCapturedMediaFileToList(string filePath, Core.Model.MediaType mediaType, long eventID)
        {
            MediaFile mediaFile = new MediaFile();
            mediaFile.FilePath = filePath;
            mediaFile.MediaType = mediaType;
            mediaFile.EventID = eventID;

            mediaFiles.Add(mediaFile);
        }

        // Upload all files to server.
        private void UploadFiles()
        {
            Intent intent = new Intent(this, typeof(WeClipUploadService));
            intent.PutExtra("UploadList", JsonConvert.SerializeObject(mediaFiles));

            StartService(intent);
            mediaFiles.Clear();

            Toast.MakeText(this, "Upload started", ToastLength.Short);


        }

        private class UploadEventProfileFileInBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private UploadEventProfilePic context;
            private long eventID;
            private string uploadImagePath;
            private string compressImage;
            JsonResult jsonResult;
            private ProgressDialog p;
            private long eventCreatorID;

            public UploadEventProfileFileInBackGround(UploadEventProfilePic context, long eventID, string uploadImagePath, long eventCreatorID)
            {
                this.eventCreatorID = eventCreatorID;
                this.context = context;
                this.eventID = eventID;
                this.uploadImagePath = uploadImagePath;
                jsonResult = new JsonResult();
                p = ProgressDialog.Show(context, "", "Please wait...");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                Java.IO.File file = new Java.IO.File(uploadImagePath);
                long length = file.Length() / 1024; // Size in KB

                if (length > 500)
                {
                    compressImage = CompressFile.compressImage(uploadImagePath);
                }
                else
                {
                    compressImage = uploadImagePath;
                }

                jsonResult = RestSharpCall.PostFile<JsonResult>(compressImage, "Event/SetEventPicture?eventid=" + eventID + "&eventCreatorID=" + eventCreatorID);
                CompressFile.deleteTempFile();
                return jsonResult;
            }
            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();

                if (result != null)
                {
                    if (result.Success == true)
                    {
                        Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(context);
                        builder.SetTitle("Success");
                        builder.SetMessage("Event Profile Photo Updated.");

                        builder.SetPositiveButton("OK", (senderAlert, args) =>
                        {
                            context.Finish();
                        });
                        Dialog dialog = builder.Create();
                        dialog.Show();
                    }
                    else
                    {
                        Toast.MakeText(context, "Error.", ToastLength.Long).Show();
                    }
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (CameraHelper.CheckCameraAvailability(this))
            {
                InitializeCamera();
            }
            else
            {
                AlertBox.Create("", "Your device does not seem to have camera.", this);
            }

            if (orientationManager != null)
            {
                orientationManager.Enable();
            }
        }
    }

}