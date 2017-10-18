using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Controls;
using Square.Picasso;
using WeClip.Droid.Helper;
using WeClip.Core.Common;

namespace WeClip.Droid.Adapters
{
    public class CommentsAdapter : BaseAdapter<EventFeedModel>
    {
        private Activity activity;
        private List<EventFeedModel> eventfeed;

        public CommentsAdapter(List<EventFeedModel> eventfeed, Activity activity)
        {
            this.eventfeed = eventfeed;
            this.activity = activity;
        }

        public override EventFeedModel this[int position]
        {
            get
            {
                return eventfeed[position];
            }
        }

        public override int Count
        {
            get
            {
                return eventfeed.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            CommentsViewHolder viewHolder;

            if (convertView == null)
            {
                viewHolder = new CommentsViewHolder();
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.CommentsFragmentItem, parent, false);
                viewHolder.tvDay = convertView.FindViewById<TextView>(Resource.Id.tvDay);
                viewHolder.imgProfilePic = convertView.FindViewById<ImageView>(Resource.Id.ivCFItemUserPhoto);
                viewHolder.tvUsername = convertView.FindViewById<TextView>(Resource.Id.tvCFItemUserName);
                viewHolder.tvUserLabel = convertView.FindViewById<TextView>(Resource.Id.tvCFItemUserLabel);
                viewHolder.tvEventName = convertView.FindViewById<TextView>(Resource.Id.tvCFItemEventName);
                viewHolder.tvComment = convertView.FindViewById<ExpandableTextView>(Resource.Id.tvCFItemComment);
                viewHolder.tvCommentTime = convertView.FindViewById<TextView>(Resource.Id.tvCFItemTime);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as CommentsViewHolder;
            }

            viewHolder.tvUsername.Text = eventfeed[position].UserName;
            viewHolder.tvUserLabel.Text = "Hosts";
            viewHolder.tvEventName.Text = eventfeed[position].EventCreaterName;
            viewHolder.tvComment.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;

            viewHolder.tvComment.Text = eventfeed[position].Message;
            viewHolder.tvCommentTime.Text = eventfeed[position].FeedDate.Value.ToString();

            Picasso.With(activity).Load(eventfeed[position].UserProfilePicUrl).CenterCrop().Placeholder(Resource.Drawable.contact_withoutphoto)
          .Resize(100, 100).Transform(new CircleTransformation()).Into(viewHolder.imgProfilePic);

            return convertView;
        }

        private class CommentsViewHolder : Java.Lang.Object
        {
            public TextView tvDay { get; set; }
            public ImageView imgProfilePic { get; set; }
            public TextView tvUsername { get; set; }
            public TextView tvUserLabel { get; set; }
            public TextView tvEventName { get; set; }
            public ExpandableTextView tvComment { get; set; }
            public TextView tvCommentTime { get; set; }
        }
    }

}