using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using Square.Picasso;
using WeClip.Droid.Helper;
using Newtonsoft.Json;

namespace WeClip.Droid.Adapters
{
    class WeClipFragmentAdapter : BaseAdapter<WeClipVideo>
    {
        private FragmentActivity activity;
        private List<WeClipVideo> weclipFile;

        public WeClipFragmentAdapter(FragmentActivity activity, List<WeClipVideo> weclipFile)
        {
            this.activity = activity;
            this.weclipFile = weclipFile;
        }

        public override WeClipVideo this[int position]
        {
            get
            {
                return weclipFile[position];
            }
        }

        public override int Count
        {
            get
            { return weclipFile.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            WeClipFragmentViewHolder holder;
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.WeClipFragmentItem, parent, false);
                holder = new WeClipFragmentViewHolder();
                holder.ivWeClipPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipFragItemWeClipPhoto);
                holder.ivWeClipOverImage = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipFragItemvideo);
                holder.ivWeClipPhoto.Click += delegate
                {
                    var videoPlayerActivity = new Android.Content.Intent(activity, typeof(VideoPlayerActivity));
                    videoPlayerActivity.PutExtra("VideoUrl", JsonConvert.SerializeObject(weclipFile[position]));
                    activity.StartActivity(videoPlayerActivity);
                };
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as WeClipFragmentViewHolder;
            }

            var da = weclipFile[position].Thumb;

            Picasso.With(activity).Load(weclipFile[position].Thumb).Placeholder(Resource.Drawable.default_event_back)
            .Transform(new RoundedTransformation())
              .Resize(100, 100).Into(holder.ivWeClipPhoto);
            return convertView;
        }
    }

    public class WeClipFragmentViewHolder : Java.Lang.Object
    {
        public ImageView ivWeClipPhoto { get; set; }
        public ImageView ivWeClipOverImage { get; set; }
    }
}