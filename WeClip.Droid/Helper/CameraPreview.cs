using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Java.IO;
using System;
using System.Collections.Generic;
using Android.Hardware;
using static Android.Hardware.Camera;

namespace WeClip.Droid.Helper
{
    public class CameraPreview : SurfaceView, ISurfaceHolderCallback
    {
        private Context _context;
        private ISurfaceHolder _holder;
        private Android.Hardware.Camera _camera;
        int _cameraId;
        private IList<Android.Hardware.Camera.Size> _supportedPreviewSizes;
        private IList<Android.Hardware.Camera.Size> _supportedPicSizes;

        private Android.Hardware.Camera.Size _previewSize;
        private Android.Hardware.Camera.Size _picSize;

        public SurfaceOrientation displayRotation;


        public CameraPreview(Context context, Android.Hardware.Camera camera, int cameraId, SurfaceOrientation displayRotation) : base(context)
        {
            _context = context;
            _camera = camera;
            _cameraId = cameraId;
            _supportedPreviewSizes = _camera.GetParameters().SupportedPreviewSizes;
            _supportedPicSizes = _camera.GetParameters().SupportedPictureSizes;
            _holder = Holder;
            _holder.AddCallback(this);

            // deprecated setting, but required on Android versions prior to 3.0
            //  if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Cupcake)
            {
                _holder.SetType(SurfaceType.PushBuffers);
            }

            this.displayRotation = displayRotation;
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            // The Surface has been created, now tell the camera where to draw the preview.
            try
            {
                // _camera.SetPreviewDisplay(holder);

                Parameters parameters = _camera.GetParameters();

                foreach (var previewSize in _camera.GetParameters().SupportedPreviewSizes)
                {
                    // if the size is suitable for you, use it and exit the loop.
                    parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
                    break;
                }

                _camera.SetParameters(parameters);
                _camera.SetPreviewDisplay(_holder);
                _camera.StartPreview();
            }
            catch (IOException e)
            {
                Log.Debug(Tag.ToString(), "Error setting camera preview: " + e.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            // empty. Take care of releasing the Camera preview in your activity.
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            if (_holder.Surface == null)
            {
                return;
            }
            try
            {
                _camera.StopPreview();

               
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            if (_previewSize != null)
            {
                try
                {
                    Android.Hardware.Camera.Parameters parameters = _camera.GetParameters();
                    parameters.SetPreviewSize(_previewSize.Width, _previewSize.Height);
                    parameters.SetPictureSize(_picSize.Width, _picSize.Height);
                    _camera.SetParameters(parameters);
                    _camera.SetPreviewDisplay(_holder);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            Android.App.Activity activity = _context as Android.App.Activity;
            if (activity != null)
            {
                int orientation = CameraHelper.GetCameraDisplayOrientation(activity, _cameraId, displayRotation);
                _camera.SetDisplayOrientation(orientation);
            }

            try
            {
                Parameters parameters = _camera.GetParameters();

                foreach (var previewSize in  _camera.GetParameters().SupportedPreviewSizes)
                {
                    // if the size is suitable for you, use it and exit the loop.
                    parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
                    break;
                }

                _camera.SetParameters(parameters);
                _camera.SetPreviewDisplay(_holder);
                _camera.StartPreview();
            }
            catch (Exception ex)
            {
                Log.Debug(Tag.ToString(), "Error starting camera preview: " + ex.Message);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = _context.Resources.DisplayMetrics.WidthPixels;
            var height = _context.Resources.DisplayMetrics.HeightPixels;

            if (_supportedPreviewSizes != null)
            {
                _previewSize = GetOptimalPreviewSize(_supportedPreviewSizes, width, height);
                _picSize = GetOptimalPicSize(_supportedPicSizes, width, height);
            }

            float ratio;
            if (_previewSize.Height >= _previewSize.Width)
                ratio = (float)_previewSize.Height / (float)_previewSize.Width;
            else
                ratio = (float)_previewSize.Width / (float)_previewSize.Height;
            float camHeight = (int)(width * ratio);
            float newCamHeight;
            float newHeightRatio;

            if (camHeight < height)
            {
                newHeightRatio = (float)height / (float)_previewSize.Height;
                newCamHeight = (newHeightRatio * camHeight);
                SetMeasuredDimension((int)(width * newHeightRatio), (int)newCamHeight);
            }
            else
            {
                newCamHeight = camHeight;
                SetMeasuredDimension(width, (int)newCamHeight);
            }
        }

        private Android.Hardware.Camera.Size GetOptimalPicSize(IList<Android.Hardware.Camera.Size> _supportedPicSizes, int width, int height)
        {
            double ASPECT_TOLERANCE = 0.1;
            double targetRatio = (double)height / width;

            if (_supportedPicSizes == null)
                return null;

            Android.Hardware.Camera.Size optimalSize = null;
            double minDiff = Double.MaxValue;

            int targetHeight = height;

            foreach (Android.Hardware.Camera.Size size in _supportedPicSizes)
            {
                double ratio = (double)size.Height / size.Width;
                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                    continue;

                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = Double.MaxValue;
                foreach (Android.Hardware.Camera.Size size in _supportedPicSizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;
        }

        private Android.Hardware.Camera.Size GetOptimalPreviewSize(IList<Android.Hardware.Camera.Size> sizes, int w, int h)
        {
            double ASPECT_TOLERANCE = 0.1;
            double targetRatio = (double)h / w;

            if (sizes == null)
                return null;

            Android.Hardware.Camera.Size optimalSize = null;
            double minDiff = Double.MaxValue;

            int targetHeight = h;

            foreach (Android.Hardware.Camera.Size size in sizes)
            {
                double ratio = (double)size.Height / size.Width;
                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                    continue;

                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = Double.MaxValue;
                foreach (Android.Hardware.Camera.Size size in sizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;
        }
    }
}