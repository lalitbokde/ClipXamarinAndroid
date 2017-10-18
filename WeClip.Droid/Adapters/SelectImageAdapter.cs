using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Media;
using Square.Picasso;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class SelectImageAdapter : BaseAdapter<string>
    {
        private Activity activity;
        private List<string> selectedImage;
        private int type;

        public SelectImageAdapter(List<string> selectedImage, Activity activity, int type)
        {
            this.selectedImage = selectedImage;
            this.activity = activity;
            this.type = type;
        }

        public override string this[int position]
        {
            get { return selectedImage[position]; }
        }

        public override int Count
        {
            get
            {
                return selectedImage.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            var item = selectedImage[position];
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.SelectImageItem, parent, false);
                holder = new ViewHolder();
                holder.ivEventPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivSelectedItemEventPhoto);
                holder.ivEventVideo = convertView.FindViewById<ImageView>(Resource.Id.ivSelectItem_video);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            Bitmap thumb = null;
            holder.ivEventPhoto.SetImageResource(0);

            if (type == 1)
            {
                holder.ivEventVideo.Visibility = ViewStates.Visible;
                thumb = ThumbnailUtils.CreateVideoThumbnail(item, Android.Provider.ThumbnailKind.MiniKind);
                if (thumb != null)
                {
                    holder.ivEventPhoto.SetImageBitmap(thumb);
                }
            }
            else
            {
                Picasso.With(activity).Load(new Java.IO.File(item)).Placeholder(Resource.Drawable.default_event_back)
             .Transform(new RoundedTransformation())
               .Resize(150, 150).Into(holder.ivEventPhoto);
                holder.ivEventVideo.Visibility = ViewStates.Gone;
            }
            // Dispose of the Java side bitmap.
            return convertView;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView ivEventPhoto { get; set; }
            public ImageView ivEventVideo { get; set; }
        }
    }
}