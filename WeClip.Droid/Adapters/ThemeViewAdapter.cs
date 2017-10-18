using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Core.Common;
using Newtonsoft.Json;

namespace WeClip.Droid.Adapters
{
    class ThemeViewAdapter : BaseAdapter<Theme>
    {
        private Activity activity;
        private List<Theme> themelist;

        public ThemeViewAdapter(List<Theme> themelist, Activity activity)
        {
            this.themelist = themelist;
            this.activity = activity;
        }

        public override Theme this[int position]
        {
            get
            {
                return themelist[position];
            }
        }

        public override int Count
        {
            get
            {
                return themelist.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ThemeViewHolder holder;
            var item = themelist[position];
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.ThemeViewItem, parent, false);
                holder = new ThemeViewHolder();
                holder.Themename = convertView.FindViewById<TextView>(Resource.Id.tvThemeViewName);
                holder.Themepicture = convertView.FindViewById<ImageView>(Resource.Id.ivThemeViewItemPic);
                holder.PlayButton = convertView.FindViewById<ImageView>(Resource.Id.ivThemeViewPlay);
                holder.PlayButton.Click += delegate
                {
                    var weclipVideo = new WeClipVideo
                    {
                        VideoFileName = item.Filename,
                        VideoFileUrl = item.FileUrl,
                        EventName = item.Name
                    };
                    var videoPlayerActivity = new Android.Content.Intent(activity, typeof(VideoPlayerActivity));
                    videoPlayerActivity.PutExtra("VideoUrlFromTheme", JsonConvert.SerializeObject(weclipVideo));
                    activity.StartActivity(videoPlayerActivity);

                };
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ThemeViewHolder;
            }

            holder.Themename.Text = item.Name;
            holder.PlayButton.SetImageResource(Resource.Drawable.play_little);
            Picasso.With(activity).Load(item.Thumb).Placeholder(Resource.Drawable.default_event_back)
           .Resize(80, 80).Into(holder.Themepicture);
            return convertView;
        }
    }
}

public class ThemeViewHolder : Java.Lang.Object
{
    public ImageView Themepicture { get; set; }
    public ImageView PlayButton { get; set; }
    public TextView Themename { get; set; }
}
