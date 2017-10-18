using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Support.V7.Widget;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using System;
using Android.Content;

namespace WeClip.Droid.Adapters
{
    class ProfileEventAdapter : BaseAdapter<GetAllEventList>
    {
        private FragmentActivity context;
        private List<GetAllEventList> listOfEvent;

        public ProfileEventAdapter(List<GetAllEventList> listOfEvent, FragmentActivity context)
        {
            this.listOfEvent = listOfEvent;
            this.context = context;
        }

        public override GetAllEventList this[int position]
        {
            get
            {
                return listOfEvent[position];
            }
        }

        public override int Count
        {
            get
            {
                return listOfEvent.Count;
            }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            EventViewHolder viewHolder;

            if (convertView == null)
            {
                viewHolder = new EventViewHolder();
                convertView = context.LayoutInflater.Inflate(Resource.Layout.ProfileFragmentItem, parent, false);
                viewHolder.ivUserPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivPFIUserPhoto);
                viewHolder.ivChat = convertView.FindViewById<ImageView>(Resource.Id.ivPFIEventChat);
                viewHolder.ivMore = convertView.FindViewById<ImageView>(Resource.Id.ivPFIEventMore);
                viewHolder.rvEventPhoto = convertView.FindViewById<RecyclerView>(Resource.Id.rvPFIEventPhoto);
                viewHolder.tvUserName = convertView.FindViewById<TextView>(Resource.Id.tvPFIUserName);
                viewHolder.tvEventTime = convertView.FindViewById<TextView>(Resource.Id.tvPFIEventTime);
                viewHolder.tvEventName = convertView.FindViewById<TextView>(Resource.Id.tvPFIEventName);
                viewHolder.llEventName = convertView.FindViewById<LinearLayout>(Resource.Id.llPFEventName);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as EventViewHolder;
            }
            viewHolder.ivMore.Visibility = ViewStates.Invisible;
            var item = listOfEvent[position];

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            viewHolder.rvEventPhoto.SetLayoutManager(layoutManager);
            var madapter = new HomeItemPrivateEventPhotoAdapter(item.listFiles,item.EventID, context);
            viewHolder.rvEventPhoto.SetAdapter(madapter);

            Picasso.With(context).Load(item.creatorpic).Placeholder(Resource.Drawable.contact_withoutphoto)
              .Transform(new CircleTransformation()).CenterCrop()
                .Resize(150, 150).Into(viewHolder.ivUserPhoto);

            viewHolder.tvEventName.Text = item.EventName;
            viewHolder.tvEventTime.Text = item.EventDate.ToString();
            viewHolder.tvUserName.Text = item.creatorname;

            viewHolder.llEventName.SetOnClickListener(new llEventNameClickListner(viewHolder.llEventName, context, item.EventID));
            viewHolder.ivChat.SetOnClickListener(new ivChatOnClickListner(viewHolder.ivMore, context, item.EventID));

            return convertView;

        }


        private class EventViewHolder : Java.Lang.Object
        {
            public ImageView ivUserPhoto { get; set; }
            public ImageView ivChat { get; set; }
            public ImageView ivMore { get; set; }
            public TextView tvUserName { get; set; }
            public TextView tvEventTime { get; set; }
            public TextView tvEventName { get; set; }
            public RecyclerView rvEventPhoto { get; set; }
            public LinearLayout llEventName { get; set; }
        }

        private class ivChatOnClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity context;
            private long eventID;
            private ImageView ivMore;

            public ivChatOnClickListner(ImageView ivMore, FragmentActivity context, long eventID)
            {
                this.ivMore = ivMore;
                this.context = context;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                ivMore.SetOnClickListener(null);
                Intent pwcActivity = new Intent(context, typeof(CommentActivity));
                pwcActivity.PutExtra("CurrentTab", "comment");
                MainActivity.myBundle.PutLong("EventID", eventID);
                context.StartActivity(pwcActivity);
                // ivMore.SetOnClickListener(null);
            }
        }

        private class llEventNameClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity context;
            private long eventID;
            private LinearLayout llEventName;

            public llEventNameClickListner(LinearLayout llEventName, FragmentActivity context, long eventID)
            {
                this.llEventName = llEventName;
                this.context = context;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Android.Support.V4.App.Fragment editProfile = new EventFragment();
                Android.Support.V4.App.FragmentManager fragmentManager = context.SupportFragmentManager;
                MainActivity.myBundle.PutLong("EventID", eventID);
                fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, editProfile).AddToBackStack("eventFromProfile").Commit();
                llEventName.SetOnClickListener(null);
            }
        }
    }
}