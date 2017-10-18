using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using Square.Picasso;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class NotificationAdapter : BaseAdapter<NotificationModel>
    {
        private FragmentActivity activity;
        private List<NotificationModel> notification;

        public NotificationAdapter(List<NotificationModel> notification, FragmentActivity activity)
        {
            this.notification = notification;
            this.activity = activity;
        }

        public override NotificationModel this[int position]
        {
            get
            {
                return notification[position];
            }
        }

        public override int Count
        {
            get
            {
                return notification.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHolder;

            if (convertView == null)
            {
                viewHolder = new ViewHolder();
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.NotificationItem, parent, false);
                viewHolder.tvNotification = convertView.FindViewById<TextView>(Resource.Id.tvNINotification);
                viewHolder.ivUserPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivNIUserPhoto);
                viewHolder.tvTime = convertView.FindViewById<TextView>(Resource.Id.tvNIDate);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as ViewHolder;
            }
            viewHolder.tvNotification.Text = notification[position].Message;
            //if (notification[position].EventID != 0)
            //{

            if (notification[position].CreatedOn != null)
            {
                viewHolder.tvTime.Text = notification[position].CreatedOn.Value.ToString("MM/dd/yyyy HH:mm");
            }
            //}
            //else
            //{
            //    viewHolder.tvHost.Visibility = ViewStates.Gone;
            //}
            Picasso.With(activity).Load(notification[position].SenderImage).Placeholder(Resource.Drawable.contact_withoutphoto)
                .Transform(new CircleTransformation())
                  .Resize(60, 60).Into(viewHolder.ivUserPhoto);
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView ivUserPhoto { get; set; }
            public TextView tvNotification { get; set; }
            public TextView tvTime { get; set; }
            public TextView tvHost { get; set; }
        }
    }
}