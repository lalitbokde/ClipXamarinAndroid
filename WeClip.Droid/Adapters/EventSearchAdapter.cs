using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Droid.Helper;
using WeClip.Core.Common;

namespace WeClip.Droid.Adapters
{
    public class EventSearchAdapter : BaseAdapter<EventSearchData>
    {
        private FragmentActivity activity;
        private List<EventSearchData> eventData;
        private List<EventSearchData> arraylist;


        public EventSearchAdapter(FragmentActivity activity, List<EventSearchData> eventData)
        {
            this.activity = activity;
            this.eventData = eventData;
            arraylist = new List<EventSearchData>();

            foreach (var obj in eventData)
            {
                this.arraylist.Add(obj);
            }
        }

        public override EventSearchData this[int position]
        {
            get
            {
                return eventData[position];
            }
        }

        public override int Count
        {
            get
            {
                return eventData.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.EventSearchFragmentItem, parent, false);
                holder = new ViewHolder();
                holder.ivEventPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivESFIEventPhoto);
                holder.tvEventName = convertView.FindViewById<TextView>(Resource.Id.tvESFIEventName);
                holder.tvHostBy = convertView.FindViewById<TextView>(Resource.Id.tvESFIEventHost);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }

            if (string.IsNullOrEmpty(eventData[position].EventPhoto))
            {
                Picasso.With(activity).Load(Resource.Drawable.defult_event_pic).Placeholder(Resource.Drawable.default_event_back)
       .Transform(new CircleTransformation())
         .Resize(150, 150).Into(holder.ivEventPhoto);
            }
            else
            {
                Picasso.With(activity).Load(eventData[position].EventPhoto).Placeholder(Resource.Drawable.default_event_back)
         .Transform(new CircleTransformation())
           .Resize(150, 150).Into(holder.ivEventPhoto);
            }

            holder.tvEventName.Text = eventData[position].EventName;
            holder.tvHostBy.Text = eventData[position].CreatorName;
            return convertView;
        }

        protected class ViewHolder : Java.Lang.Object
        {
            public ImageView ivEventPhoto { get; set; }
            public TextView tvEventName { get; set; }
            public TextView tvHostBy { get; set; }

        }

        public void filter(string charText)
        {
            charText = charText.ToLower();
            eventData.Clear();
            if (charText.Length == 0)
            {
                foreach (var x in arraylist)
                {
                    eventData.Add(x);
                }
            }
            else
            {
                foreach (EventSearchData wp in arraylist)
                {
                    if (wp.EventName.ToLower()
                            .Contains(charText))
                    {
                        eventData.Add(wp);
                    }
                }
            }
            NotifyDataSetChanged();
        }
    }


}