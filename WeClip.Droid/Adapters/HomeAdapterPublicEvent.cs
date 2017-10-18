using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Support.V7.Widget;
using Square.Picasso;
using WeClip.Droid.Helper;
using Android.Support.V4.App;
using Android.Content;

namespace WeClip.Droid.Adapters
{

    public class HomeAdapterPublicEvent : RecyclerView.Adapter
    {
        private FragmentActivity context;
        private List<GetAllEventList> listPublicEvent;

        public HomeAdapterPublicEvent(List<GetAllEventList> listPublicEvent, FragmentActivity context)
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
            var viewholder = holder as PublicEventViewHolder;

            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false);
            viewholder.rvEventPhoto.SetLayoutManager(layoutManager);
            var madapter = new HomeItemPublicEventPhotoAdapter(item.listFiles, item.EventID, context);
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
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomeItemPublicEvent, parent, false);
            var pvh = new PublicEventViewHolder(v);
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
                ivMore.SetOnClickListener(null);
            }
        }

        private class PublicEventViewHolder : RecyclerView.ViewHolder
        {
            public ImageView ivUserPhoto, ivChat, ivMore;
            public TextView tvUserName, tvEventTime, tvEventName, tvEventDetails;
            public RecyclerView rvEventPhoto;
            public LinearLayout llEventName;
            public PublicEventViewHolder(View v) : base(v)
            {
                ivUserPhoto = v.FindViewById<ImageView>(Resource.Id.ivHIPublicUserPhoto);
                ivChat = v.FindViewById<ImageView>(Resource.Id.ivHIPublicEventChat);
                ivMore = v.FindViewById<ImageView>(Resource.Id.ivHIPublicEventMore);
                rvEventPhoto = v.FindViewById<RecyclerView>(Resource.Id.rvHIPublicEventPhoto);
                tvUserName = v.FindViewById<TextView>(Resource.Id.tvHIPublicUserName);
                tvEventTime = v.FindViewById<TextView>(Resource.Id.tvHIPublicEventTime);
                tvEventName = v.FindViewById<TextView>(Resource.Id.tvHIPublicEventName);
                tvEventDetails = v.FindViewById<TextView>(Resource.Id.tvHIPublicEventDetails);
                llEventName = v.FindViewById<LinearLayout>(Resource.Id.llPublicEventName);
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
            }
        }

    }

}