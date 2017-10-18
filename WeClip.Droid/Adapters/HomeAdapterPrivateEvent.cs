using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Support.V7.Widget;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Android.Support.V4.App;

namespace WeClip.Droid.Adapters
{
    public class HomeAdapterPrivateEvent : RecyclerView.Adapter
    {
        private FragmentActivity context;
        private List<GetAllEventList> listPublicEvent;

        public HomeAdapterPrivateEvent(List<GetAllEventList> listPublicEvent, FragmentActivity context)
        {
            this.listPublicEvent = listPublicEvent;
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return listPublicEvent.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = listPublicEvent[position];
            var viewholder = holder as PrivateEventViewHolder;

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            viewholder.rvEventPhoto.SetLayoutManager(layoutManager);
            var madapter = new HomeItemPrivateEventPhotoAdapter(item.listFiles, item.EventID, context);
            viewholder.rvEventPhoto.SetAdapter(madapter);

            Picasso.With(context).Load(item.creatorpic).Placeholder(Resource.Drawable.contact_withoutphoto)
              .Transform(new CircleTransformation()).CenterCrop()
                .Resize(150, 150).Into(viewholder.ivUserPhoto);

            viewholder.tvEventName.Text = item.EventName;
            viewholder.tvEventTime.Text = item.EventDate.ToString();
            viewholder.tvEventDetails.Text = item.EventName;
            viewholder.tvUserName.Text = item.creatorname;
            viewholder.llEventName.SetOnClickListener(new llEventNameClickListner(viewholder.llEventName, context, item.EventID));
            viewholder.ivChat.SetOnClickListener(new ivChatOnClickListner(viewholder.ivMore, context, item.EventID));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomeItemPrivateEvent, parent, false);
            var pvh = new PrivateEventViewHolder(v);
            return pvh;
        }

        private class llEventNameClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity context;
            private LinearLayout ivMore;
            private long eventID;

            public llEventNameClickListner(LinearLayout ivMore, FragmentActivity context, long eventID)
            {
                this.ivMore = ivMore;
                this.context = context;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                var fragment = new EventFragment();
                Android.Support.V4.App.FragmentManager eventFragmentManager = context.SupportFragmentManager;
                MainActivity.myBundle.PutLong("EventID", eventID);
                eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment, "event").AddToBackStack("event").Commit();

                //Fragment fragment = new EventFragment();
                //var fragmentManager = context.SupportFragmentManager;
                //Android.Support.V4.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                //fragmentTransaction.Replace(Resource.Id.content_frame, fragment, "my_fragment");
                //MainActivity.myBundle.PutLong("EventID", eventID);
                //fragmentTransaction.Commit();
                ivMore.SetOnClickListener(null);
            }
        }

        private class PrivateEventViewHolder : RecyclerView.ViewHolder
        {
            public ImageView ivUserPhoto, ivChat, ivMore;
            public TextView tvUserName, tvEventTime, tvEventName, tvEventDetails;
            public RecyclerView rvEventPhoto;
            public LinearLayout llEventName;
            public PrivateEventViewHolder(View v) : base(v)
            {
                ivUserPhoto = v.FindViewById<ImageView>(Resource.Id.ivHIPrivateUserPhoto);
                ivChat = v.FindViewById<ImageView>(Resource.Id.ivHIPrivateEventChat);
                ivMore = v.FindViewById<ImageView>(Resource.Id.ivHIPrivateEventMore);
                rvEventPhoto = v.FindViewById<RecyclerView>(Resource.Id.rvHIPrivateEventPhoto);
                tvUserName = v.FindViewById<TextView>(Resource.Id.tvHIPrivateUserName);
                tvEventTime = v.FindViewById<TextView>(Resource.Id.tvHIPrivateEventTime);
                tvEventName = v.FindViewById<TextView>(Resource.Id.tvHIPrivateEventName);
                tvEventDetails = v.FindViewById<TextView>(Resource.Id.tvHIPrivateEventDetails);
                llEventName = v.FindViewById<LinearLayout>(Resource.Id.llPrivateEventName);
            }
        }

        private class ivChatOnClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity context;
            private long iD;
            private ImageView ivMore;

            public ivChatOnClickListner(ImageView ivMore, FragmentActivity context, long iD)
            {
                this.ivMore = ivMore;
                this.context = context;
                this.iD = iD;
            }

            public void OnClick(View v)
            {
                ivMore.SetOnClickListener(null);
                Intent pwcActivity = new Intent(context, typeof(CommentActivity));
                pwcActivity.PutExtra("CurrentTab", "comment");
                MainActivity.myBundle.PutLong("EventID", iD);
                context.StartActivity(pwcActivity);
                // ivMore.SetOnClickListener(null);
            }
        }

    }

}