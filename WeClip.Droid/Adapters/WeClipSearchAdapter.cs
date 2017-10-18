using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Droid.Helper;
using Newtonsoft.Json;
using WeClip.Core.Common;

namespace WeClip.Droid.Adapters
{
    public class WeClipSearchAdapter : BaseAdapter<SPGetSearchWeclipList_Result>
    {
        private FragmentActivity activity;
        private List<SPGetSearchWeclipList_Result> weClipdata;

        public WeClipSearchAdapter(FragmentActivity activity, List<SPGetSearchWeclipList_Result> weClipdata)
        {
            this.activity = activity;
            this.weClipdata = weClipdata;
        }

        public override SPGetSearchWeclipList_Result this[int position]
        {
            get
            {
                return weClipdata[position];
            }
        }

        public override int Count
        {
            get
            {
                return weClipdata.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            var videoInfo = weClipdata[position];

            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.WeClipSearchFragmentItem, parent, false);
                holder = new ViewHolder();
                holder.ivWeClipPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivWSFItemPhoto);
                holder.ivWeClipPhoto.Click += delegate
                {

                    var weclipVideo = new WeClipVideo
                    {
                        VideoFileName = videoInfo.Filename,
                        VideoFileUrl = videoInfo.VideoFileUrl,
                        EventName = videoInfo.EventName,
                        WeClipTitle = videoInfo.WeClipTitle,
                        WeClipTag = videoInfo.WeClipTag
                    };
                    var videoPlayerActivity = new Android.Content.Intent(activity, typeof(VideoPlayerActivity));
                    videoPlayerActivity.PutExtra("VideoUrl", JsonConvert.SerializeObject(weclipVideo));
                    activity.StartActivity(videoPlayerActivity);
                };

                holder.ivWeClipOverImage = convertView.FindViewById<ImageView>(Resource.Id.ivWSFItemPhoto);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }

            Picasso.With(activity).Load(weClipdata[position].ImageFileUrl).Placeholder(Resource.Drawable.default_event_back)
            .Transform(new RoundedTransformation())
              .Resize(150, 150).Into(holder.ivWeClipPhoto);
            return convertView;
        }

    }

    public class ViewHolder : Java.Lang.Object
    {
        public ImageView ivWeClipPhoto { get; set; }
        public ImageView ivWeClipOverImage { get; set; }
    }
}