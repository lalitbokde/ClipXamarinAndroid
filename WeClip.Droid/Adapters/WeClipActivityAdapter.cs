using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using Square.Picasso;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class WeClipActivityAdapter : BaseAdapter<WeClipVideo>
    {
        private Activity activity;
        private List<WeClipVideo> weclipFile;

        public WeClipActivityAdapter(Activity activity, List<WeClipVideo> weclipFile)
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
            WeClipActivityViewHolder holder;
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.WeClipViewItem, parent, false);
                holder = new WeClipActivityViewHolder();
                holder.ivWeClipPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipViewWeClipPhoto);
                holder.ivWeClipOverImage = convertView.FindViewById<ImageView>(Resource.Id.ivWeClipViewWeClipVideo);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as WeClipActivityViewHolder;
            }

            var da = weclipFile[position].Thumb;
            Picasso.With(activity).CancelRequest(holder.ivWeClipPhoto);
            Picasso.With(activity).Load(weclipFile[position].Thumb).Placeholder(Resource.Drawable.default_event_back)
            .Transform(new RoundedTransformation())
              .Resize(150, 150).Into(holder.ivWeClipPhoto);
            return convertView;
        }
    }

    public class WeClipActivityViewHolder : Java.Lang.Object
    {
        public ImageView ivWeClipPhoto { get; set; }
        public ImageView ivWeClipOverImage { get; set; }
    }
}