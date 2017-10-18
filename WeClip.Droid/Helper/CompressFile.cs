using System;
using Android.Graphics;
using Android.Media;
using Android.Util;
using Java.Util;
using Java.IO;
using WeClip.Droid.AsyncTask;
using Android.OS;
using WeClip.Droid.Services;
using System.Threading.Tasks;

namespace WeClip.Droid.Helper
{
    public static class CompressFile
    {
        public static string compressImage(string imageUri)
        {
            string strMyImagePath = null;
            Bitmap scaledBitmap = null;

            var val =  MediaFileExtensions.FixOrientationAsync(imageUri);

            BitmapFactory.Options options = new BitmapFactory.Options();


            //  by setting this field as true, the actual bitmap pixels are not loaded in the memory. Just the bounds are loaded. If
            //  you try the use the bitmap here, you will get null.
            options.InJustDecodeBounds = true;
            Bitmap bmp = BitmapFactory.DecodeFile(imageUri, options);

            int actualHeight = options.OutHeight;
            int actualWidth = options.OutWidth;
            float imgRatio = (float)actualWidth / (float)actualHeight;

            //  max Height and width values of the compressed image is taken as 1920x1080
            float maxWidth = 1920.00f;
            float maxHeight = 1080.00f;
            if (actualHeight > actualWidth)
            {
                maxWidth = 1080.00f;
                maxHeight = 1920.00f;
            }
            float maxRatio = maxWidth / maxHeight;

            //  width and height values are set maintaining the aspect ratio of the image
            if (actualHeight > maxHeight || actualWidth > maxWidth)
            {
                if (imgRatio < maxRatio)
                {
                    imgRatio = maxHeight / actualHeight;
                    actualWidth = (int)(imgRatio * actualWidth);
                    actualHeight = (int)maxHeight;
                }
                else if (imgRatio > maxRatio)
                {
                    imgRatio = maxWidth / actualWidth;
                    actualHeight = (int)(imgRatio * actualHeight);
                    actualWidth = (int)maxWidth;
                }
                else
                {
                    actualHeight = (int)maxHeight;
                    actualWidth = (int)maxWidth;
                }
            }

            //  setting inSampleSize value allows to load a scaled down version of the original image
            options.InSampleSize = calculateInSampleSize(options, actualWidth, actualHeight);

            //  inJustDecodeBounds set to false to load the actual bitmap
            options.InJustDecodeBounds = false;

            //  this options allow android to claim the bitmap memory if it runs low on memory
            options.InPurgeable = true;
            options.InInputShareable = true;
            options.InTempStorage = new byte[16 * 1024];

            try
            {
                //  load the bitmap from its path
                bmp = BitmapFactory.DecodeFile(imageUri, options);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }

            try
            {
                scaledBitmap = Bitmap.CreateBitmap(actualWidth, actualHeight, Bitmap.Config.Argb8888);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }

            float ratioX = actualWidth / (float)options.OutWidth;
            float ratioY = actualHeight / (float)options.OutHeight;
            float middleX = actualWidth / 2.0f;
            float middleY = actualHeight / 2.0f;

            Matrix scaleMatrix = new Matrix();
            scaleMatrix.SetScale(ratioX, ratioY, middleX, middleY);

            Canvas canvas = new Canvas(scaledBitmap);
            canvas.Matrix = (scaleMatrix);
            canvas.DrawBitmap(bmp, middleX - bmp.Width / 2, middleY - bmp.Height / 2, new Paint(PaintFlags.FilterBitmap));

            //  check the rotation of the image and display it properly
            Android.Media.ExifInterface exif;
            try
            {
                exif = new ExifInterface(imageUri);

                int orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 0);
                Matrix matrix = new Matrix();
                if (orientation == (int)Orientation.Rotate90)   //  6
                {
                    matrix.PostRotate(90);
                }
                else if (orientation == (int)Orientation.Rotate180)  // 3
                {
                    matrix.PostRotate(180);
                }
                else if (orientation == (int)Orientation.Rotate270)  // 8
                {
                    matrix.PostRotate(270);
                }
                scaledBitmap = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, matrix, true);
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }

            strMyImagePath = getOutputMediaFile().AbsolutePath;

            System.IO.FileStream fos = null;
            try
            {
                fos = new System.IO.FileStream(strMyImagePath, System.IO.FileMode.Create);
                scaledBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fos);
                fos.Flush();
                fos.Close();
            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }

            return strMyImagePath;
        }

        public static int calculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                int heightRatio = height / reqHeight;
                int widthRatio = width / reqWidth;
                inSampleSize = heightRatio < widthRatio ? heightRatio : widthRatio;
            }
            float totalPixels = width * height;
            float totalReqPixelsCap = reqWidth * reqHeight * 2;
            while (totalPixels / (inSampleSize * inSampleSize) > totalReqPixelsCap)
            {
                inSampleSize++;
            }
            return inSampleSize;
        }

        private static Java.IO.File getOutputMediaFile()
        {
            string extr = Android.OS.Environment.ExternalStorageDirectory.ToString();
            Java.IO.File mFolder = new Java.IO.File(extr + "/TMMFOLDER");
            if (!mFolder.Exists())
            {
                mFolder.Mkdir();
            }
            String timeStamp = new Java.Text.SimpleDateFormat("yyyyMMdd_HHmmss",
                    Locale.Default).Format(new Date());
            Java.IO.File mediaFile;
            mediaFile = new Java.IO.File(mFolder.AbsolutePath + Java.IO.File.Separator
                    + "IMG_" + timeStamp + ".jpg");
            return mediaFile;
        }

        public static Android.Net.Uri getOutputMediaFileUri()
        {
            return Android.Net.Uri.FromFile(getOutputMediaFile());
        }

        public static void deleteTempFile()
        {
            try
            {
                string path = Android.OS.Environment.ExternalStorageDirectory.ToString() + "/TMMFOLDER";
                Java.IO.File directory = new File(path);
                File[] files = directory.ListFiles();
                if (files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        Java.IO.File file = new Java.IO.File(files[i].AbsolutePath);
                        bool deleted = file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                new CrashReportAsync("CompressFile", "deleteTempFile", ex.Message + ex.StackTrace).Execute();
            }
        }
    }
}