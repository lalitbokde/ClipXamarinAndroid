using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WeClip.Core.Model;
using WeClip.Droid.Adapters;
using WeClip.Droid.AsyncTask;
using WeClip.Droid.Helper;
using WeClip.Droid.Services;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace WeClip.Droid
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddImageAndVideoForEventActivity : AppCompatActivity, Helper.OrientationManager.OrientationListener
    {
        public long EventID;
        private bool IsDefault;

        CameraFacing _cameraFacing = CameraFacing.Back;
        FlashState _flashState = FlashState.NotAvailable;
        CaptureMode _captureMode = CaptureMode.Photo;
        private bool isRecording = false;   // Know if video is being recorded

        private FrameLayout camera_preview;
        private Android.Hardware.Camera _camera;
        CameraPreview _preview;
        MediaRecorder _mediaRecorder;
        string mediaFilePath = "";
        List<MediaFile> mediaFiles = new List<MediaFile>();
        OrientationManager orientationManager;
        int imageRotation = 0;

        private Button btnPhotos, btnCamera, btnVideos;
        private ImageButton btnCapture, btnSwitchCamera, btnFlash, btnCaptureMode;
        SurfaceOrientation displayRotation;
        private string uploadImagePath;
        private int typeOfFile;
        private RelativeLayout rvCamera, rvGalleryView;
        protected static int GALLERY_PICTURE = 1;
        protected static int GALLERY_VIDEO = 10;
        private GridView gvSelectedImage;
        Android.Net.Uri selectedImagePath;
        private List<string> selectedImage;
        private LinearLayout llCaptureView;
        private ImageButton ivGalleryPhotoVideoUpload, ivImageCaptureCameraUpload;
        private long MaxVideoSize;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.ImageCapture);
                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                MaxVideoSize = prefs.GetLong("MaxVideoSize", 0);
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
                IsDefault = Intent.GetBooleanExtra("strIsDefaultEvent", false);
                camera_preview = FindViewById<FrameLayout>(Resource.Id.camera_preview);
                btnPhotos = FindViewById<Button>(Resource.Id.btnPhotos);
                btnCamera = FindViewById<Button>(Resource.Id.btnCamera);
                btnVideos = FindViewById<Button>(Resource.Id.btnVideos);
                btnCapture = FindViewById<ImageButton>(Resource.Id.ivImageCapturebtn);
                ivGalleryPhotoVideoUpload = FindViewById<ImageButton>(Resource.Id.ivImageCaptureGalleryUpload);
                btnSwitchCamera = FindViewById<ImageButton>(Resource.Id.btnSwitchCamera);
                btnFlash = FindViewById<ImageButton>(Resource.Id.btnFlash);
                btnCaptureMode = FindViewById<ImageButton>(Resource.Id.btnCaptureMode);
                ivImageCaptureCameraUpload = FindViewById<ImageButton>(Resource.Id.ivImageCaptureCameraUpload);

                rvCamera = FindViewById<RelativeLayout>(Resource.Id.rvCameraView);
                rvGalleryView = FindViewById<RelativeLayout>(Resource.Id.rvGalleryView);
                gvSelectedImage = FindViewById<GridView>(Resource.Id.gvSelectedImage);
                llCaptureView = FindViewById<LinearLayout>(Resource.Id.llCaptureView);

                btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
                btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
                btnVideos.SetTextColor(Color.Rgb(217, 217, 217));

                btnPhotos.Click += BtnPhotos_Click;
                btnCamera.Click += BtnCamera_Click;
                btnVideos.Click += BtnVideos_Click;
                btnCapture.Click += BtnCapture_Click;

                btnSwitchCamera.Click += BtnSwitchCamera_Click;
                btnFlash.Click += BtnFlash_Click;
                btnCaptureMode.Click += BtnCaptureMode_Click;
                ivGalleryPhotoVideoUpload.Click += IvGalleryPhotoVideoUpload_Click;
                ivImageCaptureCameraUpload.Click += IvImageCaptureCameraUpload_Click;
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

        private void IvImageCaptureCameraUpload_Click(object sender, EventArgs e)
        {

        }

        private void IvGalleryPhotoVideoUpload_Click(object sender, EventArgs e)
        {
            if (selectedImage == null)
            {
                AlertBox.Create("Error", "Select file from gallery or capture image", this);
                return;
            }
            else
            {
                foreach (var filepath in selectedImage)
                {
                    Java.IO.File file = new Java.IO.File(filepath);
                    var length1 = file.Length();
                    double length = Convert.ToDouble(length1) / (1024 * 1024); // Size in MB
                    if (length > MaxVideoSize)
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
                }
                if (IsDefault != true)
                {
                    new UploadEventsImageAndVideoFile(this, selectedImage, typeOfFile, EventID).Execute();
                }
                else
                {
                    new UploadDefultEventsImageAndVideoFile(this, selectedImage, typeOfFile, EventID).Execute();
                }
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

        private void BtnCaptureMode_Click(object sender, EventArgs e)
        {
            int icon;

            switch (_captureMode)
            {
                case CaptureMode.Photo:
                    _captureMode = CaptureMode.Video;
                    icon = Resource.Drawable.ic_photo_camera_white_24dp;
                    break;
                case CaptureMode.Video:
                    _captureMode = CaptureMode.Photo;
                    icon = Resource.Drawable.ic_videocam_white_24dp;
                    break;
                default:
                    _captureMode = CaptureMode.Photo;
                    icon = Resource.Drawable.ic_videocam_white_24dp;
                    break;
            }

            // Perform icon swapping here
            btnCaptureMode.SetImageResource(icon);
        }

        // Open Photo Library
        private void BtnPhotos_Click(object sender, EventArgs e)
        {
            btnPhotos.SetTextColor(Color.Rgb(0, 208, 150));
            btnCamera.SetTextColor(Color.Rgb(217, 217, 217));
            btnVideos.SetTextColor(Color.Rgb(217, 217, 217));

            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
          intent.PutExtra(Intent.ExtraLocalOnly, true);
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), GALLERY_PICTURE);
        }

        // Open Video Library
        private void BtnVideos_Click(object sender, EventArgs e)
        {
            rvGalleryView.Visibility = ViewStates.Gone;
            rvCamera.Visibility = ViewStates.Visible;

            btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
            btnCamera.SetTextColor(Color.Rgb(217, 217, 217));
            btnVideos.SetTextColor(Color.Rgb(0, 208, 150));

            Intent intent = new Intent();
            intent.SetType("video/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.PutExtra(Intent.ExtraLocalOnly, true);
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select video"), GALLERY_VIDEO);

            //Intent pictureActionIntent = null;
            //pictureActionIntent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Video.Media.ExternalContentUri);
            //this.StartActivityForResult(pictureActionIntent, GALLERY_VIDEO);
        }

        // Open In App Camera
        private void BtnCamera_Click(object sender, EventArgs e)
        {
            if (_captureMode == CaptureMode.Photo)
            {
                ReleaseCamera();
            }
            else
            if (_captureMode == CaptureMode.Video) // Record Video (Start/Stop)
            {
                if (isRecording)
                {
                    // stop recording and release camera
                    _mediaRecorder.Stop();  // stop the recording
                    ReleaseMediaRecorder(); // release the MediaRecorder object
                    ReleaseCamera();
                }
            }

            ivImageCaptureCameraUpload.Visibility = ViewStates.Gone;
            rvGalleryView.Visibility = ViewStates.Gone;
            rvCamera.Visibility = ViewStates.Visible;
            llCaptureView.Visibility = ViewStates.Visible;
            InitializeCamera();
            btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
            btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
            btnVideos.SetTextColor(Color.Rgb(217, 217, 217));
        }

        // Initialize Camera
        private void InitializeCamera()
        {
            _camera = CameraHelper.GetCameraInstance((int)_cameraFacing);
            if (_camera != null)
            {

                displayRotation = this.WindowManager.DefaultDisplay.Rotation;

                _preview = new CameraPreview(this, _camera, (int)_cameraFacing, displayRotation);
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
                int rotation = CameraHelper.GetCameraDisplayOrientation(null, (int)_cameraFacing, displayRotation); ;
                mediaFilePath = CameraHelper.GetOutputMediaFile(CaptureMode.Photo).ToString();

                if (_cameraFacing == CameraFacing.Front)
                {
                   // rotation = CameraHelper.GetCameraDisplayOrientation(null, (int)_cameraFacing, displayRotation,true);
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
                    AddCapturedMediaFileToList(mediaFilePath, Core.Model.MediaType.Photo, EventID);

                    ResetCamera();

                    UploadFiles();
                };

                _camera.TakePicture(null, null, _picture);
            }
            else if (_captureMode == CaptureMode.Video) // Record Video (Start/Stop)
            {
                if (isRecording)
                {
                    // stop recording and release camera
                    _mediaRecorder.Stop();  // stop the recording

                    AddCapturedMediaFileToList(mediaFilePath, Core.Model.MediaType.Video, EventID);

                    ReleaseMediaRecorder(); // release the MediaRecorder object
                    _camera.Lock();         // take camera access back from MediaRecorder

                    // inform the user that recording has stopped
                    // --Change here the icon of capture button to stopped...
                    btnCapture.SetImageDrawable(null);
                    btnSwitchCamera.Visibility = ViewStates.Visible;
                    btnCaptureMode.Visibility = ViewStates.Visible;
                    isRecording = false;

                    ResetCamera();
                    UploadFiles();
                }
                else
                {
                    // initialize video camera
                    if (PrepareVideoRecorder())
                    {
                        // Camera is available and unlocked, MediaRecorder is prepared,
                        // now you can start recording
                        _mediaRecorder.Start();

                        // inform the user that recording has started
                        btnCapture.SetImageResource(Resource.Drawable.ic_stop_white_36dp);
                        btnSwitchCamera.Visibility = ViewStates.Invisible;
                        btnCaptureMode.Visibility = ViewStates.Invisible;
                        isRecording = true;
                    }
                    else
                    {
                        // prepare didn't work, release the camera
                        ReleaseMediaRecorder();
                        AlertBox.Create("Error", "Something went wrong!", this);
                    }
                }
            }
        }

        // Used to reset camera and preview after photo or video capture
        private void ResetCamera()
        {
            ReleaseCamera();
            InitializeCamera();
        }

        // Preparing MediaRecorder
        public bool PrepareVideoRecorder()
        {
            if (_camera == null)
                _camera = CameraHelper.GetCameraInstance((int)_cameraFacing);

            if (_preview == null)
                _preview = new CameraPreview(this, _camera, (int)_cameraFacing, displayRotation);

            _mediaRecorder = new MediaRecorder();

            // Step 1: Unlock and set camera to MediaRecorder
            _camera.Unlock();
            _mediaRecorder.SetCamera(_camera);

            // Step 2: Set sources
            _mediaRecorder.SetAudioSource(AudioSource.Camcorder);
            _mediaRecorder.SetVideoSource(VideoSource.Camera);

            // Step 3: Set a CamcorderProfile (requires API Level 8 or higher)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                _mediaRecorder.SetProfile(CamcorderProfile.Get(CamcorderQuality.High));
            }
            else
            {
                // For API Level 8 and lower
                _mediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);
                _mediaRecorder.SetAudioEncoder(AudioEncoder.Default);
                _mediaRecorder.SetVideoEncoder(VideoEncoder.Default);
            }

            // Step 4: Set output file
            mediaFilePath = CameraHelper.GetOutputMediaFile(CaptureMode.Video).ToString();
            _mediaRecorder.SetOutputFile(mediaFilePath);

            // Step 5: Set the preview output
            _mediaRecorder.SetPreviewDisplay(_preview.Holder.Surface);

            // Step 6: Prepare configured MediaRecorder
            try
            {
                _mediaRecorder.Prepare();
            }
            catch (IllegalStateException ex)
            {
                //Log.Debug(Tag, "IllegalStateException preparing MediaRecorder: " + ex.Message);
                ReleaseMediaRecorder();
                return false;
            }
            catch (IOException ex)
            {
                //Log.Debug(Tag, "IOException preparing MediaRecorder: " + ex.Message);
                ReleaseMediaRecorder();
                return false;
            }

            return true;
        }

        // Releasing MediaRecorder
        private void ReleaseMediaRecorder()
        {
            if (_mediaRecorder != null)
            {
                _mediaRecorder.Reset();   // clear recorder configuration
                _mediaRecorder.Release(); // release the recorder object
                _mediaRecorder = null;
                _camera.Lock();           // lock camera for later use
            }
            mediaFilePath = "";
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
            ReleaseMediaRecorder();
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
            ReleaseMediaRecorder();
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

                if (data.ClipData != null)
                {
                    typeOfFile = 0;
                    ClipData mClipData = data.ClipData;
                    selectedImage = new List<string>();
                    for (int i = 0; i < mClipData.ItemCount; i++)
                    {
                        ClipData.Item item = mClipData.GetItemAt(i);
                        Android.Net.Uri uri = item.Uri;
                        var onefilepath = MediaHelper.GetMediaPath(ApplicationContext, uri);
                        selectedImage.Add(onefilepath);
                    }
                    gvSelectedImage.Adapter = new SelectImageAdapter(selectedImage, this, 0);
                }
                else
                {
                    if (data.Data != null)
                    {
                        typeOfFile = 0;
                        selectedImagePath = data.Data;
                        uploadImagePath = MediaHelper.GetMediaPath(ApplicationContext, selectedImagePath);
                        selectedImage = new List<string>();
                        selectedImage.Add(uploadImagePath);
                        gvSelectedImage.Adapter = new SelectImageAdapter(selectedImage, this, 0);

                    }
                }
            }

            if (resultCode == Result.Canceled)
            {
                ivImageCaptureCameraUpload.Visibility = ViewStates.Gone;
                rvGalleryView.Visibility = ViewStates.Gone;
                rvCamera.Visibility = ViewStates.Visible;
                llCaptureView.Visibility = ViewStates.Visible;
                // InitializeCamera();
                btnPhotos.SetTextColor(Color.Rgb(217, 217, 217));
                btnCamera.SetTextColor(Color.Rgb(0, 208, 150));
                btnVideos.SetTextColor(Color.Rgb(217, 217, 217));
            }

            if ((requestCode == GALLERY_VIDEO) && (resultCode == Result.Ok) && (data != null))
            {
                rvGalleryView.Visibility = ViewStates.Visible;
                rvCamera.Visibility = ViewStates.Gone;
                llCaptureView.Visibility = ViewStates.Gone;
                ivImageCaptureCameraUpload.Visibility = ViewStates.Visible;

                typeOfFile = 1;
                if (data.Data != null)
                {
                    selectedImagePath = data.Data;
                    uploadImagePath = MediaHelper.GetMediaPath(ApplicationContext, selectedImagePath);


                    selectedImage = new List<string>();
                    selectedImage.Add(uploadImagePath);
                    gvSelectedImage.Adapter = new SelectImageAdapter(selectedImage, this, 1);
                }

                else
                    if (data.ClipData != null)
                {
                    ClipData mClipData = data.ClipData;
                    selectedImage = new List<string>();
                    for (int i = 0; i < mClipData.ItemCount; i++)
                    {
                        ClipData.Item item = mClipData.GetItemAt(i);
                        Android.Net.Uri uri = item.Uri;
                        var onefilepath = MediaHelper.GetMediaPath(ApplicationContext, uri);

                        selectedImage.Add(onefilepath);
                    }
                    gvSelectedImage.Adapter = new SelectImageAdapter(selectedImage, this, 1);
                }
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
            intent.PutExtra("MaxVideoSize", MaxVideoSize);

            StartService(intent);
            mediaFiles.Clear();

            Toast.MakeText(this, "Upload started", ToastLength.Short);
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
