using System;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Uri = Android.Net.Uri;
namespace WeClip.Droid.Helper
{
    public class MediaHelper
    {

        public static string GetMediaPath(Context context, Uri uri)
        {
            bool afterKitKat = Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat;

            if (afterKitKat && DocumentsContract.IsDocumentUri(context, uri))                       //   DocumentProvider
            {
                string docId = DocumentsContract.GetDocumentId(uri);

                if (IsExternalStorageDocument(uri))         //  ExternalStorageProvider
                {
                    string[] split = docId.Split(':');
                    string type = split[0];

                    if (string.Equals(type, "primary", StringComparison.OrdinalIgnoreCase))
                    {
                        return Android.OS.Environment.ExternalStorageDirectory + "/" + split[1];
                    }

                    // TODO handle non-primary volumes

                }
                else if (IsDownloadsDocument(uri))          //  DownloadsProvider
                {
                    long id;
                    if (!long.TryParse(docId, out id)) { return null; }

                    Uri contentUri = ContentUris.WithAppendedId(Uri.Parse("content://downloads/public_downloads"), id);

                    return GetDataColumn(context, contentUri, null, null);
                }
                else if (IsMediaDocument(uri))              //  MediaProvider
                {
                    string[] split = docId.Split(':');
                    string type = split[0];

                    Uri contentUri = null;

                    if (string.Equals(type, "image", StringComparison.OrdinalIgnoreCase))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    }
                    else if (string.Equals(type, "video", StringComparison.OrdinalIgnoreCase))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    }
                    else if (string.Equals(type, "audio", StringComparison.OrdinalIgnoreCase))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    string selection = "_id=?";
                    string[] selectionArgs = new string[] { split[1] };

                    return GetDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            else if (string.Equals(uri.Scheme, "content", StringComparison.OrdinalIgnoreCase))      //  MediaStore (and general)
            {
                return GetDataColumn(context, uri, null, null);
            }
            else if (string.Equals(uri.Scheme, "file", StringComparison.OrdinalIgnoreCase))         //  File
            {
                return uri.Path;
            }

            return null;
        }

        private static string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
        {
            Android.Database.ICursor cursor = null;
            string column = MediaStore.Images.Media.InterfaceConsts.Data;       // "_data"
            string[] projection = { column };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(column_index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }

            return null;
        }

        private static bool IsExternalStorageDocument(Uri uri)
        {
            return string.Equals(uri.Authority, "com.android.externalstorage.documents");
        }

        private static bool IsDownloadsDocument(Uri uri)
        {
            return string.Equals(uri.Authority, "com.android.providers.downloads.documents");
        }

        private static bool IsMediaDocument(Uri uri)
        {
            return string.Equals(uri.Authority, "com.android.providers.media.documents");
        }

        internal static string GetMediaPath(object applicationContext, Uri selectedImagePath)
        {
            throw new NotImplementedException();
        }
    }
}