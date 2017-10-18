using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using Square.Picasso;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class DefaultEventGalleryAdapter : BaseAdapter<EventFiles>
    {
        private Activity context;
        private List<EventFiles> eventfile;

        public DefaultEventGalleryAdapter(Activity context, List<EventFiles> eventfile) : base()
        {
            this.context = context;
            this.eventfile = eventfile;

        }

        public override EventFiles this[int position]
        {
            get
            {
                return eventfile[position];
            }
        }

        public override int Count
        {
            get { return eventfile.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            EventPhotosViewHolder holder;
            var item = eventfile[position];
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Resource.Layout.EventGalleryItem, parent, false);
                holder = new EventPhotosViewHolder();
                holder.ivEventPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipItemEventPhoto);
                holder.ivIcRightImage = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipItem_icRight);
                holder.ivEventVideo = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipItem_video);
                convertView.Tag = holder;
            }
            else
            {
                holder = (EventPhotosViewHolder)convertView.Tag;
            }

            holder.ivEventPhoto.SetImageResource(0);
            Picasso.With(context).CancelRequest(holder.ivEventPhoto);

            if (item.ID != 0)
            {
                holder.ivIcRightImage.SetImageResource(Resource.Drawable.ic_check_white_36dp);

                Picasso.With(context).Load(item.Thumb).Placeholder(Resource.Drawable.default_event_back).Transform(new RoundedTransformation())
                     .Resize(100, 100).Into(holder.ivEventPhoto);

                if (!item.IsVideo)
                {
                    holder.ivEventVideo.Visibility = ViewStates.Gone;
                }
                else
                {
                    holder.ivEventVideo.Visibility = ViewStates.Visible;
                }

                if (item.isSelected())
                {
                    holder.ivIcRightImage.Visibility = ViewStates.Visible;
                }
                else
                {
                    holder.ivIcRightImage.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                holder.ivIcRightImage.SetImageResource(Resource.Drawable.add_photo);
                holder.ivEventPhoto.SetBackgroundResource(Resource.Drawable.imageView_Border);
                holder.ivEventVideo.Visibility = ViewStates.Gone;
            }

            return convertView;
        }

        protected class EventPhotosViewHolder : Java.Lang.Object
        {
            public ImageView ivEventPhoto { get; set; }
            public ImageView ivIcRightImage { get; set; }
            public ImageView ivEventVideo { get; set; }
        }

    }
}