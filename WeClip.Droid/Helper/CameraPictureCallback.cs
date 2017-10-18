using Android.Graphics;
using Android.Hardware;
using Android.Media;
using Java.IO;
using System;

namespace WeClip.Droid.Helper
{
    public class CameraPictureCallback : Java.Lang.Object, Android.Hardware.Camera.IPictureCallback
    {
        /// <summary>
        /// Path of the saved image. (Note: Use this property only after "Camera.TakePicture()" is called.)
        /// </summary>
        private string _filePath;
        private int _rotation;
       

        public event EventHandler PictureTakenCompleted;

        public CameraPictureCallback(string filePath, int rotation)
        {
            _filePath = filePath;
            _rotation = rotation;
           
        }

        public void OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
        {
            try
            {
               
                System.IO.FileStream fos = new System.IO.FileStream(_filePath, System.IO.FileMode.Create);
                fos.Write(data, 0, data.Length);
                              
                fos.Flush();
                fos.Close();

                ExifInterface exif = new ExifInterface(_filePath);
                if(exif != null)
                {
                    int exifroation = 1;
                   
                        switch (_rotation)
                        {
                            case 90:
                                exifroation = 6; break;
                            case 180:
                                exifroation = 3; break;
                            case 270:
                                exifroation = 8; break;
                            default:
                                exifroation = 1; break;
                        }
                    
                    exif.SetAttribute(ExifInterface.TagOrientation,Convert.ToString(exifroation));
                    exif.SaveAttributes();
                    int orientation = Convert.ToInt16(exif.GetAttributeInt(ExifInterface.TagOrientation, (int)0));
                    System.Diagnostics.Debug.WriteLine(orientation);
                }
                OnPictureTakenCompleted(EventArgs.Empty);
            }
            catch (FileNotFoundException ex)
            {
                string message = ex.Message;
                //Log.Debug(TAG, "File not found: " + ex.getMessage());
            }
            catch (IOException ex)
            {
                string message = ex.Message;
                //Log.Debug(Tag, "Error accessing file: " + ex.getMessage());
            }
        }

        // Invoke the PictureTakenCompleted event; called after picture saved successfully
        protected virtual void OnPictureTakenCompleted(EventArgs e)
        {
            PictureTakenCompleted?.Invoke(this, e);
        }
    }
}