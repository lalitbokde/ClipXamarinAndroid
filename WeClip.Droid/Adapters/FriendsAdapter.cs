using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Square.Picasso;

namespace WeClip.Droid.Adapters
{
    class FriendsAdapter : BaseAdapter<FriendListModel>
    {
        private List<FriendListModel> friendList;
        private Activity activity;

        public FriendsAdapter(List<FriendListModel> friendList, Activity activity)
        {
            this.friendList = friendList;
            this.activity = activity;
        }

        public override FriendListModel this[int position]
        {
            get
            {
                return friendList[position];
            }
        }

        public override int Count
        {
            get
            {
                return friendList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            FriendsHolder viewHolder;
            if (convertView == null)
            {
                viewHolder = new FriendsHolder();
                LayoutInflater inflater = activity.LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.FriendsItem, parent, false);
                viewHolder.tvFriendName = convertView.FindViewById<TextView>(Resource.Id.tvFriendsName);
                viewHolder.ivFriendsPic = convertView.FindViewById<ImageView>(Resource.Id.ivFriendsImage);
                viewHolder.btnSendInvite = convertView.FindViewById<Button>(Resource.Id.btnFriendInvite);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as FriendsHolder;
            }

            viewHolder.tvFriendName.Text = friendList[position].FriendName;
            var str = friendList[position].Picture;

            if (friendList[position].Picture == null)
            {
                viewHolder.ivFriendsPic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                Picasso.With(activity).Load(friendList[position].Picture).Placeholder(Resource.Drawable.ic_settings_black_24dp)
                 .Transform(new CircleTransformation())
                   .Resize(150, 150).Into(viewHolder.ivFriendsPic);
            }

       
            return convertView;
        }

        public class FriendsHolder : Java.Lang.Object
        {
            public ImageView ivFriendsPic { get; set; }
            public TextView tvFriendName { get; set; }
            public Button btnSendInvite { get; set; }
        }
    }
}