using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using WeClip.Core.Model;
using Android.Views;
using Android.Content;
using WeClip.Core.Common;
using Newtonsoft.Json;

namespace WeClip.Droid.AsyncTask
{
    class GetAllNotification : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<NotificationModel>>
    {
        private FragmentActivity activity;
        private ListView lvNotification;
        private List<NotificationModel> notification;
        private ProgressDialog p;

        public GetAllNotification(ListView lvNotification, FragmentActivity activity)
        {
            this.lvNotification = lvNotification;
            this.activity = activity;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "Please wait", "Loading Data");
        }

        protected override List<NotificationModel> RunInBackground(params Java.Lang.Void[] @params)
        {
            notification = RestSharpCall.GetList<NotificationModel>("Notification/GetNotification");
            return notification;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            p.Dismiss();
            base.OnPostExecute(result);
            if (result != null && notification.Count > 0)
            {
                lvNotification.Adapter = new NotificationAdapter(notification, activity);
                lvNotification.OnItemClickListener = (new lvnotificationClick(activity, notification, lvNotification));
            }
        }

        private class lvnotificationClick : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private FragmentActivity activity;
            private ListView lvNotification;
            private List<NotificationModel> notification;
            private long mLastClickTime = 0;

            public lvnotificationClick(FragmentActivity activity, List<NotificationModel> notification, ListView lvNotification)
            {
                this.activity = activity;
                this.notification = notification;
                this.lvNotification = lvNotification;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                if (SystemClock.ElapsedRealtime() - mLastClickTime < 1000)
                {
                    return;
                }
                mLastClickTime = SystemClock.ElapsedRealtime();

                if (notification[position].Type == "FR")
                {
                    Intent intent = new Intent(activity, typeof(FriendRequestActivity));
                    activity.StartActivity(intent);
                }
                else
            if (notification[position].Type == "ER")
                {
                    var fragment = new EventRequestFragment();
                    Android.Support.V4.App.FragmentManager FragmentManager = activity.SupportFragmentManager;
                    MainActivity.myBundle.PutLong("EventID", notification[position].EventID);
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment, "eventRequest").AddToBackStack("eventRequest").Commit();
                }
                else
                if (notification[position].Type == "FA")
                {
                    var item = notification[position];
                    Intent mainActivity = new Intent(activity, typeof(MainActivity));
                    mainActivity.PutExtra("FromPeople", item.SenderID);
                    activity.StartActivity(mainActivity);
                }
                else
                if (notification[position].Type == "WE")
                {
                    new getWeClipInfo(activity, notification[position].WeClipID).Execute();
                }
            }

            public class getWeClipInfo : AsyncTask<Java.Lang.Object, Java.Lang.Object, WeClipVideo>
            {
                private FragmentActivity activity;
                private long? weClipID;
                private WeClipVideo weclipVideo;
                private ProgressDialog p;

                public getWeClipInfo(FragmentActivity activity, long? weClipID)
                {
                    this.activity = activity;
                    this.weClipID = weClipID;
                    p = ProgressDialog.Show(activity, "", "Please wait...");
                }

                protected override WeClipVideo RunInBackground(params Java.Lang.Object[] @params)
                {
                    weclipVideo = RestSharpCall.Get<WeClipVideo>("Event/GetWeClipInfo?weClipId=" + weClipID);
                    return weclipVideo;
                }

                protected override void OnPostExecute(WeClipVideo result)
                {
                    base.OnPostExecute(result);
                    p.Dismiss();
                    var videoPlayerActivity = new Android.Content.Intent(activity, typeof(VideoPlayerActivity));
                    videoPlayerActivity.PutExtra("VideoUrl", JsonConvert.SerializeObject(weclipVideo));
                    activity.StartActivity(videoPlayerActivity);
                }
            }
        }
    }
}