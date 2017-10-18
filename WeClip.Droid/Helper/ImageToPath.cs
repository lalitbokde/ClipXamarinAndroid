using System;
using Java.Util;

namespace WeClip.Droid.Helper
{
    public class ImageToPath
    {

        public enum UploadFileType
        {
            image,
            video
        }

        public Android.Net.Uri GetOutputMediaFileUri(UploadFileType type)
        {
            return Android.Net.Uri.FromFile(GetOutputMediaFile(type));
        }

        public static Java.IO.File GetOutputMediaFile(UploadFileType type)
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

            if(type == UploadFileType.image)
            {
                mediaFile = new Java.IO.File(mFolder.AbsolutePath + Java.IO.File.Separator
                   + "IMG_" + timeStamp + ".jpg");
            }
            else
            {
                mediaFile = new Java.IO.File(mFolder.AbsolutePath + Java.IO.File.Separator
                   + "VID_" + timeStamp + ".mp4");
            }
            return mediaFile;
        }

    }
}