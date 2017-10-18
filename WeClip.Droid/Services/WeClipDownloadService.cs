using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System.Threading;
using WeClip.Core.Model;
using System.Net;
using Newtonsoft.Json;
using System.ComponentModel;

namespace WeClip.Droid.Services
{

    [Service]
    [IntentFilter(new String[] { "com.WeClip" })]
    public class WeClipDownloadService : Service
    {
        Thread thread;
        List<List<VideoDownloadInfo>> RequestList;

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
                List<VideoDownloadInfo> mediaFiles = JsonConvert.DeserializeObject<List<VideoDownloadInfo>>(intent.GetStringExtra("UploadList"));

                if (mediaFiles.Count > 0)
                {
                    if (RequestList == null)
                    {
                        RequestList = new List<List<VideoDownloadInfo>>();
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
                List<VideoDownloadInfo> mediaFiles = RequestList[0];

                await UploadFiles(mediaFiles);

                RequestList.Remove(mediaFiles);

                thread = null;

                ManageQueue();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private async System.Threading.Tasks.Task UploadFiles(List<VideoDownloadInfo> mediaFiles)
        {
            foreach (var file in mediaFiles)
            {
                if (!getfileFromPath(file.VideoFileName))
                {
                    await UploadSingleFile(file);
                }
            }
        }

        private async System.Threading.Tasks.Task UploadSingleFile(VideoDownloadInfo file)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(file.VideoUrl);
            req.Method = "HEAD";
            HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
            totalBytes = resp.ContentLength;

            uploadedBytes = 0;
            percentComplete = 0;

            Decimal fileSizeInMB = Convert.ToDecimal(totalBytes) / (1024.0m * 1024.0m);
            var totalfileSize = Math.Round(fileSizeInMB, 2);

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationBuilder = new Notification.Builder(ApplicationContext);
            notificationBuilder.SetOngoing(true)
                               .SetSmallIcon(Resource.Drawable.ic_file_download_white_18dp)
                               .SetContentTitle("Downloading")
                               .SetContentText(

               Math.Round((Convert.ToDecimal(uploadedBytes) / (1024.0m * 1024.0m)), 2) + " MB of " + totalfileSize + " MB")
                               .SetProgress(100, percentComplete, false);

            notification = notificationBuilder.Build();
            notificationID = new System.Random().Next(10000, 99999);
            notificationManager.Notify(notificationID, notification);

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Start();

            wClient = new WebClient();
            wClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            wClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
           
            await wClient.DownloadFileTaskAsync(new Uri(file.VideoUrl), GetOutputVideoPath(file.VideoFileName));           // Defines the URL and destination directory for the downloaded file
        }


        private bool getfileFromPath(string videoFileName)
        {
            string extr = Android.OS.Environment.ExternalStorageDirectory.ToString();
            Java.IO.File file = new Java.IO.File(extr + "/TMMFOLDER", videoFileName);

            if (file.Exists())
            {
                return true;
            }
            else
                return false;
        }

        private string GetOutputVideoPath(string videoFileName)
        {
            string extr = Android.OS.Environment.ExternalStorageDirectory.ToString();
            Java.IO.File mFolder = new Java.IO.File(extr + "/TMMFOLDER");
            if (!mFolder.Exists())
            {
                mFolder.Mkdir();
            }
            Java.IO.File mediaFile;
            mediaFile = new Java.IO.File(mFolder.AbsolutePath + Java.IO.File.Separator + videoFileName);
            return mediaFile.AbsolutePath;
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            uploadedBytes = e.BytesReceived;
            percentComplete = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            timer.Stop();
            timer.Dispose();
            notificationManager.Cancel(notificationID);

            notificationID = new System.Random().Next(10000, 99999);
            notificationBuilder = new Notification.Builder(ApplicationContext);
            notificationBuilder.SetOngoing(false)
                               .SetSmallIcon(Resource.Drawable.icon);

            if (e.Cancelled == false && e.Error == null)
            {
                notificationBuilder.SetContentTitle("Download complete");
            }
            else
                notificationBuilder.SetContentTitle("Upload failed");

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


        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NotifyUpdateStatus();
        }

        private void NotifyUpdateStatus()
        {

            var contentText = Math.Round((Convert.ToDecimal(uploadedBytes) / (1024.0m * 1024.0m)), 2) + " MB of " + Math.Round((Convert.ToDecimal(totalBytes) / (1024.0m * 1024.0m)), 2) + " MB";

            notificationBuilder.SetProgress(100, percentComplete, false)
                               .SetContentText(contentText);

            notification = notificationBuilder.Build();
            notificationManager.Notify(notificationID, notification);
        }

    }
}