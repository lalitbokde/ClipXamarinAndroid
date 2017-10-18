using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Hardware;
using Android.Util;
using Android.Views;
using Java.IO;
using System.Collections.Generic;

namespace WeClip.Droid.Helper
{
    public class CameraHelper
    {
        Context _context;
        bool _available;

        public bool Available
        {
            get { return _available; }
        }

        public CameraHelper(Context context)
        {
            _context = context;
            _available = CheckCameraAvailability(_context);
        }

        public static bool CheckCameraAvailability(Context context)
        {
            if (context.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureCamera))
                return true;
            else
                return false;
        }

        public static Camera GetCameraInstance(int cameraId)
        {
            Camera c = null;

            try
            {
                c = Camera.Open(cameraId); // attempt to get a Camera instance

            }
            catch (Java.Lang.Exception ex)
            {
                // Camera is not available (in use or does not exist)
                string message = ex.Message;
            }
           return c; // returns null if camera is unavailable
        }

        // Create a file Uri for saving an image or video */
        public static Android.Net.Uri GetOutputMediaFileUri(CaptureMode captureMode)
        {
            return Android.Net.Uri.FromFile(GetOutputMediaFile(captureMode));
        }

        // Create a File for saving an image or video */
        public static File GetOutputMediaFile(CaptureMode captureMode)
        {
            // To be safe, you should check that the SDCard is mounted
            // using Environment.getExternalStorageState() before doing this.

            File mediaStorageDir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(
                                            Android.OS.Environment.DirectoryPictures), "WeClip");
            //// This location works best if you want the created images to be shared
            //// between applications and persist after your app has been uninstalled.

            // Create the storage directory if it does not exist
            if (!mediaStorageDir.Exists())
            {
                if (!mediaStorageDir.Mkdirs())
                {
                    Log.Debug("CameraFileSave", "Failed to create directory.");
                    return null;
                }
            }

            string timeStamp = new Java.Text.SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Java.Util.Date());
            File mediaFile;
            if (captureMode == CaptureMode.Photo)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator + "IMG_" + timeStamp + ".jpg");
            }
            else if (captureMode == CaptureMode.Video)
            {
                mediaFile = new File(mediaStorageDir.Path + File.Separator + "VID_" + timeStamp + ".mp4");
            }
            else
            {
                return null;
            }

            return mediaFile;
        }

        public static int GetCameraDisplayOrientation(Activity activity, int cameraId, SurfaceOrientation rotation,bool isfromCapture = false)
        {
            int degrees = 0;

            Android.Hardware.Camera.CameraInfo info = new Android.Hardware.Camera.CameraInfo();
            Android.Hardware.Camera.GetCameraInfo(cameraId, info);

            if (info.Facing == CameraFacing.Front)
            {

                switch (rotation)
                {
                    case SurfaceOrientation.Rotation0: degrees = 0; break;
                    case SurfaceOrientation.Rotation90: degrees = 90; break;
                    case SurfaceOrientation.Rotation180: degrees = 180; break;
                    case SurfaceOrientation.Rotation270: degrees = 270; break;
                }

            }
            else
            {

                switch (rotation)
                {
                    case SurfaceOrientation.Rotation0:
                        degrees = 90;
                        break;
                    case SurfaceOrientation.Rotation90:
                        degrees = 0;
                        break;
                    case SurfaceOrientation.Rotation180:
                        degrees = 270;
                        break;
                    case SurfaceOrientation.Rotation270:
                        degrees = 180;
                        break;
                }
            }

          
            int result;

            if (info.Facing == CameraFacing.Front)
            {
                if (isfromCapture == false)
                {
                    result = (360 + info.Orientation - degrees) % 360;
                    result = (360 - result) % 360;
                }
                else
                {
                    result = (info.Orientation + degrees + 270) % 360;
                }
            }
            else
            {
                result = (info.Orientation + degrees + 270) % 360;  // back-facing
            }

          //  result = (info.Orientation + degrees + 270) % 360;
            return result;
        }

        public static void SwitchFlashMode(Camera camera, ref FlashState _flashState)
        {
            Camera.Parameters _params = camera.GetParameters();

            IList<string> flashModes = _params.SupportedFlashModes;
            if (flashModes == null || flashModes.Count == 0)
            {
                _flashState = FlashState.NotAvailable;
                return;
            }

            switch (_flashState)
            {
                case FlashState.Auto:
                    if (flashModes.Contains(Camera.Parameters.FlashModeOn))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeOn;
                        _flashState = FlashState.On;
                    }
                    else if (flashModes.Contains(Camera.Parameters.FlashModeOff))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeOff;
                        _flashState = FlashState.Off;
                    }
                    else
                    {
                        _flashState = FlashState.NotAvailable;
                        return;
                    }
                    break;

                case FlashState.On:
                    if (flashModes.Contains(Camera.Parameters.FlashModeOff))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeOff;
                        _flashState = FlashState.Off;
                    }
                    else if (flashModes.Contains(Camera.Parameters.FlashModeAuto))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeAuto;
                        _flashState = FlashState.Auto;
                    }
                    else
                    {
                        _flashState = FlashState.NotAvailable;
                        return;
                    }
                    break;

                case FlashState.Off:
                    if (flashModes.Contains(Camera.Parameters.FlashModeAuto))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeAuto;
                        _flashState = FlashState.Auto;
                    }
                    else if (flashModes.Contains(Camera.Parameters.FlashModeOn))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeOn;
                        _flashState = FlashState.On;
                    }
                    else
                    {
                        _flashState = FlashState.NotAvailable;
                        return;
                    }
                    break;

                case FlashState.NotAvailable:
                    if (flashModes.Contains(Camera.Parameters.FlashModeAuto))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeAuto;
                        _flashState = FlashState.Auto;
                    }
                    else if (flashModes.Contains(Camera.Parameters.FlashModeOn))
                    {
                        _params.FlashMode = Camera.Parameters.FlashModeOn;
                        _flashState = FlashState.On;
                    }
                    else
                    {
                        _flashState = FlashState.NotAvailable;
                        return;
                    }
                    break;

                default:
                    return;
            }

            camera.SetParameters(_params);
        }

    }

    public enum FlashState : int
    {
        NotAvailable = -1,
        Off = 0,
        On = 1,
        Auto = 2
    }

    public enum CaptureMode : int
    {
        Photo = 1,
        Video = 2
    }


}
