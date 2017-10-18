using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Services
{
    [Service]
    [IntentFilter(new String[] { "com.WeClip" })]
    public class WeClipUploadService : Service
    {
        Thread thread;
        List<List<MediaFile>> RequestList;
        private long MaxVideoSize;
        Intent _intent;
        WebClient wClient;
        Notification notification;
        NotificationManager notificationManager;
        Notification.Builder notificationBuilder;
        int notificationID;

        long totalBytes = 0;
        long uploadedBytes = 0;
        int percentComplete = 0;

        System.Timers.Timer timer;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            _intent = intent;

            try
            {
                List<MediaFile> mediaFiles = JsonConvert.DeserializeObject<List<MediaFile>>(intent.GetStringExtra("UploadList"));
                MaxVideoSize = intent.GetLongExtra("MaxVideoSize", 0);

                if (mediaFiles.Count > 0)
                {
                    if (RequestList == null)
                    {
                        RequestList = new List<List<MediaFile>>();
                    }
                    RequestList.Add(mediaFiles);
                }

                ManageQueue();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                StopSelf();
            }

            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        private void ManageQueue()
        {
            if (RequestList.Count == 0)
            {
                StopSelf();
                return;
            }
            else
            {
                if (thread == null)
                {
                    InitializeThread();
                }
            }
        }

        private void InitializeThread()
        {
            thread = new Thread(async () =>
            {
                List<MediaFile> mediaFiles = RequestList[0];

                await UploadFiles(mediaFiles);

                RequestList.Remove(mediaFiles);

                thread = null;

                ManageQueue();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private async Task UploadFiles(List<MediaFile> mediaFiles)
        {
            foreach (var file in mediaFiles)
            {
                await UploadSingleFile(file);
            }
        }

        private async Task UploadSingleFile(MediaFile file)
        {
            Uri uri;

            if (file.MediaType == Core.Model.MediaType.Photo)
            {
                uri = new Uri(GlobalClass.BaseUrl + "Event/UploadEventFiles?eventid=" + file.EventID);
            }
            else
            {
                uri = new Uri(GlobalClass.BaseUrl + "Event/UploadVideo?eventid=" + file.EventID);
            }

            string uploadFilePath = file.FilePath;
            Java.IO.File mediaFile = new Java.IO.File(uploadFilePath);
            totalBytes = mediaFile.Length();

            if (totalBytes == 0) return;

            double totalMB = Convert.ToDouble(totalBytes) / (1024 * 1024); // Size in MB

            if (totalMB > MaxVideoSize)
            {
                notificationBuilder = new Notification.Builder(ApplicationContext);
                notificationManager = (NotificationManager)GetSystemService(NotificationService);

                notificationBuilder.SetOngoing(false)
                              .SetSmallIcon(Resource.Drawable.icon);

                notificationBuilder.SetContentTitle("Upload failed");
                Intent notificationIntent = new Intent(ApplicationContext, typeof(UploadErrorActivity));
                PendingIntent contentIntent = PendingIntent.GetActivity(ApplicationContext,
                                0, notificationIntent, PendingIntentFlags.UpdateCurrent);
                notificationBuilder.SetContentIntent(contentIntent);
                notificationBuilder.SetContentText("Video size is too large");
                notificationBuilder.SetAutoCancel(true);

                notification = notificationBuilder.Build();
                notificationID = new Random().Next(10000, 99999);
                notificationManager.Notify(notificationID, notification);
                notificationID = 0;
                notificationBuilder.Dispose();
                notification.Dispose();
                notificationManager.Dispose();

                return;
            }
            else

               //  && (totalBytes / 1024) > 500
               if (file.MediaType == Core.Model.MediaType.Photo)
            {
                uploadFilePath = CompressFile.compressImage(uploadFilePath);
                mediaFile = new Java.IO.File(uploadFilePath);
                totalBytes = mediaFile.Length();
            }

            uploadedBytes = 0;
            percentComplete = 0;
            var totalfileSize = Math.Round(totalMB, 2);

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationBuilder = new Notification.Builder(ApplicationContext);
            notificationBuilder.SetOngoing(true)
                               .SetSmallIcon(Resource.Drawable.ic_file_upload_white_18dp)
                               .SetContentTitle("Uploading")
                               .SetContentText
 (Math.Round((Convert.ToDecimal(uploadedBytes) / (1024.0m * 1024.0m)), 2) + " MB of " + totalfileSize + " MB")
                               .SetProgress(100, percentComplete, false);

            notification = notificationBuilder.Build();
            notificationID = new Random().Next(10000, 99999);
            notificationManager.Notify(notificationID, notification);
            try
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += Timer_Elapsed;
                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Start();
            }
            catch (Exception ex)
            {
                throw;
            }

            wClient = new WebClient();
            wClient.Headers.Add("authorization", "bearer " + GlobalClass.AccessToken);
            wClient.UploadFileAsync(uri, uploadFilePath);
            wClient.UploadProgressChanged += new UploadProgressChangedEventHandler(ProgressChanged);
            wClient.UploadFileCompleted += WClient_UploadFileCompleted;
        }


        private void ProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            uploadedBytes = e.BytesSent;
            percentComplete = e.ProgressPercentage;
            Log.Debug("ProgressChanged", "uploadedBytes = " + uploadedBytes + "percentComplete = " + percentComplete);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NotifyUpdateStatus();
        }

        private void NotifyUpdateStatus()
        {
            var contentText = Math.Round((Convert.ToDecimal(uploadedBytes) / (1024.0m * 1024.0m)), 2) + " MB of " + Math.Round((Convert.ToDecimal(totalBytes) / (1024.0m * 1024.0m)), 2) + " MB";
            //    Log.Debug("NotifyUpdateStatus", "uploadedBytes = " + uploadedBytes + "percentComplete = " + percentComplete);
            notificationBuilder.SetProgress(100, percentComplete, false)
                               .SetContentText(contentText);
            notificationManager.Notify(notificationID, notificationBuilder.Build());
        }


        private void WClient_UploadFileCompleted(object sender, System.Net.UploadFileCompletedEventArgs e)
        {
            timer.Stop();
            timer.Dispose();

            notificationManager.Cancel(notificationID);

            notificationID = new Random().Next(10000, 99999);
            notificationBuilder = new Notification.Builder(ApplicationContext);
            notificationBuilder.SetOngoing(false)
                               .SetSmallIcon(Resource.Drawable.icon);

            if (e.Cancelled == false && e.Error == null)
            {
                JsonResult result = JsonConvert.DeserializeObject<JsonResult>(System.Text.Encoding.UTF8.GetString(e.Result));

                if (result != null && result.Success == true)
                    notificationBuilder.SetContentTitle("Upload complete");
                else
                    if (result.Success == false)
                {
                    notificationBuilder.SetContentTitle("Upload failed");
                    Intent notificationIntent = new Intent(ApplicationContext, typeof(UploadErrorActivity));
                    PendingIntent contentIntent = PendingIntent.GetActivity(ApplicationContext,
                                    0, notificationIntent, PendingIntentFlags.UpdateCurrent);
                    notificationBuilder.SetContentIntent(contentIntent);
                    notificationBuilder.SetContentText(result.Message);
                    notificationBuilder.SetAutoCancel(true);
                }
            }
            else
            {
                notificationBuilder.SetContentTitle("Upload failed");
            }

            notification = notificationBuilder.Build();
            notificationManager.Notify(notificationID, notification);

            notificationID = 0;

            notificationBuilder.Dispose();
            notification.Dispose();
            notificationManager.Dispose();

            uploadedBytes = 0;
            totalBytes = 0;
            percentComplete = 0;
            wClient.Dispose();
        }
    }
}