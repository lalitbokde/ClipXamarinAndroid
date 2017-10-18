using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Media;
using WeClip.Core.Model;
using System.IO;

namespace WeClip.Droid.Services
{
    public static class MediaFileExtensions 
    {
        /// <summary>
        ///  Rotate an image if required.
        /// </summary>
        /// <param name="file">The file image</param>
        /// <returns>True if rotation occured, else fal</returns>
        public static bool FixOrientationAsync(string file)
        {
            if (file == null)
                return false;
            try
            {

                var filePath = file;
                var orientation = GetRotation(filePath);
                if (orientation == null)
                    return false;

                if (orientation != null && !orientation.HasValue)
                    return false;

                Bitmap bmp = RotateImage(filePath, orientation.Value);
                var quality = 90;

                using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                     bmp.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);

                bmp.Recycle();

                return true;
            }
            catch (Exception ex)
            {
               // ex.Report(); //Extension method for Xamarin.Insights
                return false;
            }
        }

       public static int? GetRotation(string filePath)
        {
            try
            {
                ExifInterface ei = new ExifInterface(filePath);
                int orientation = Convert.ToInt16(ei.GetAttributeInt(ExifInterface.TagOrientation, (int)0));
                switch (orientation)
                {
                    case 6:
                        return 90;
                    case 3:
                        return 180;
                    case 8:
                        return 270;
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
              //  ex.report();
                return null;
            }
        }

        private static Bitmap RotateImage(string filePath, int rotation)
        {
            Bitmap originalImage = BitmapFactory.DecodeFile(filePath);

            Matrix matrix = new Matrix();
            matrix.PostRotate(rotation);
            var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            originalImage.Recycle();
            return rotatedImage;
        }
    }
}