using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class HomeItemPublicEventPhotoAdapter : RecyclerView.Adapter
    {
        private FragmentActivity context;
        private long eventID;
        private List<string> listFiles;

        public HomeItemPublicEventPhotoAdapter(List<string> listFiles, long eventID, FragmentActivity context)
        {
            this.listFiles = listFiles;
            this.eventID = eventID;
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return listFiles.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = listFiles[position];
            var viewholder = holder as PrivateEventPhotosViewHolder;
            Picasso.With(context).Load(item).Placeholder(Resource.Drawable.default_event_back)
             .Transform(new RoundedTransformation()).Resize(200, 200).CenterCrop()
              .Into(viewholder.ivEventPhoto);

            viewholder.ivEventPhoto.SetOnClickListener(new eventPhotoClickListner(viewholder.ivEventPhoto, context, eventID));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomeItemPublicEventPhotoItem, parent, false);
            var pvh = new PrivateEventPhotosViewHolder(v);
            return pvh;
        }

        private class eventPhotoClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity context;
            private ImageView ivEventPhoto;
            private long eventID;

            public eventPhotoClickListner(ImageView ivEventPhoto, FragmentActivity context, long eventID)
            {
                this.ivEventPhoto = ivEventPhoto;
                this.context = context;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                var fragment = new EventFragment();
                Android.Support.V4.App.FragmentManager eventFragmentManager = context.SupportFragmentManager;
                MainActivity.myBundle.PutLong("EventID", eventID);
                eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment, "event").AddToBackStack("event").Commit();
                ivEventPhoto.SetOnClickListener(null);
            }
        }

        private class PrivateEventPhotosViewHolder : RecyclerView.ViewHolder
        {
            public ImageView ivEventPhoto;

            public PrivateEventPhotosViewHolder(View v) : base(v)
            {
                ivEventPhoto = v.FindViewById<ImageView>(Resource.Id.ivHomeItemPublicEventPhoto);
            }
        }
    }
}