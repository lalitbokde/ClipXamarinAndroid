using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Android.Media;
using System;
using WeClip.Core.Model;

namespace WeClip.Droid.GcmMain
{
    [Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
    class GcmListenerServiceClass : GcmListenerService
    {
        //List<EventModel> ListUserEvents;
        //EventService eventService;
        string userName = "";

        public override void OnMessageReceived(string from, Bundle data)
        {
            //eventService = new EventService();
            var message = data.GetString("message");
            var EventID = data.GetString("Eventid");
            //   var EventRQID = data.GetString("EventRQID");
            userName = data.GetString("Username");
            var type = data.GetString("type");
            var notificationFor = data.GetString("notificationFor");

            var notificationmodel = new NotificationModel
            {
                //   ID = Convert.ToInt64(EventRQID),
                EventID = Convert.ToInt64(EventID),
                Type = type,
                SenderName = userName,
                NotificationFor = notificationFor,
                Message = message
            };
            GetNotificationForEventRequest(notificationmodel);
        }

        private void GetNotificationForEventRequest(NotificationModel notificationmodel)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("ToNotification", "ToNotification");
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.UpdateCurrent);
            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle(notificationmodel.NotificationFor)
                .SetContentText(notificationmodel.Message)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            notificationBuilder.SetSound(alarmSound);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private void GetNotificationForFriendRequest(NotificationModel notificationmodel)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("ToNotification", "ToNotification");
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.UpdateCurrent);
            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle(notificationmodel.NotificationFor)
                .SetContentText("Friend Request From " + notificationmodel.SenderName)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            notificationBuilder.SetSound(alarmSound);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private void GetNotificationForWeClip(long EventID, string message)
        {
            //Intent intent = new Intent(this, typeof(WeClipActivity));

            //EventModel userEvent = new EventModel
            //{
            //    ID = EventID,
            //    UserID = Convert.ToInt64(GlobalClass.UserID)
            //};
            //intent.PutExtra("EventDetails", JsonConvert.SerializeObject(userEvent));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.UpdateCurrent);
            //var notificationBuilder = new Notification.Builder(this)
            //    .SetSmallIcon(Resource.Drawable.Icon)
            //    .SetContentTitle("We clip Message")
            //    .SetContentText("Your WeClip is ready")
            //    .SetAutoCancel(true)
            //    .SetContentIntent(pendingIntent);
            //var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            //notificationManager.Notify(0, notificationBuilder.Build());
        }

        void SendNotification(string message)
        {
            //var intent = new Intent(this, typeof(MainDrawerActivity));
            //intent.PutExtra("SendNotification", "SendNotification");
            //intent.AddFlags(ActivityFlags.ClearTop);
            //var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.UpdateCurrent);
            //var notificationBuilder = new Notification.Builder(this)
            //    .SetSmallIcon(Resource.Drawable.Icon)
            //    .SetContentTitle("We clip Message")
            //    .SetContentText("Send You Event Request")
            //    .SetAutoCancel(true)
            //    .SetContentIntent(pendingIntent);
            //var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            //notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}