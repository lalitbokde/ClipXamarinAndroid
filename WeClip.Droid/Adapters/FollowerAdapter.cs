using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Droid.Helper;
using Android.Graphics;
using WeClip.Core.Common;
using System;
using Android.OS;

namespace WeClip.Droid.Adapters
{
    public class FollowerAdapter : BaseAdapter
    {
        private Activity activity;
        private List<SPGetUserFollowers_Result> followerList;
        private FollowerViewHolder viewHolder;

        public FollowerAdapter(List<SPGetUserFollowers_Result> followerList, Activity activity) : base()
        {
            this.followerList = followerList;
            this.activity = activity;
        }

        public override int Count
        {
            get
            {
                return followerList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                viewHolder = new FollowerViewHolder();
                LayoutInflater inflater = activity.LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.FollowerItem, parent, false);
                viewHolder.tvFollowerName = convertView.FindViewById<TextView>(Resource.Id.tvFollowerName);
                viewHolder.ivFollowerPic = convertView.FindViewById<ImageView>(Resource.Id.ivFollowerPhoto);
                viewHolder.btnSendInvite = convertView.FindViewById<Button>(Resource.Id.btnFollowerInvite);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as FollowerViewHolder;
            }

            viewHolder.tvFollowerName.Text = followerList[position].UserName;

            if (string.IsNullOrEmpty(followerList[position].ProfilePic))
            {
                viewHolder.ivFollowerPic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                Picasso.With(activity).Load(followerList[position].ProfilePic).Placeholder(Resource.Drawable.contact_withoutphoto)
           .Resize(80, 80).Transform(new CircleTransformation()).Into(viewHolder.ivFollowerPic);
            }

            if (followerList[position].UserId == Convert.ToInt64(GlobalClass.UserID))
            {
                viewHolder.btnSendInvite.Visibility = ViewStates.Gone;
            }

            if (followerList[position].IsFriendRequestPending == true)
            {
                viewHolder.btnSendInvite.Text = "Request sent";
                viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));

            }
            else
            {
                if (followerList[position].IsFriend == false)
                {
                    viewHolder.btnSendInvite.Text = "Follow";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#04c285"));
                }
                else
                {
                    viewHolder.btnSendInvite.Text = "Following";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
                }
            }

            viewHolder.btnSendInvite.SetOnClickListener(new btnInviteClick(activity, followerList[position], viewHolder.btnSendInvite, this));
            return convertView;
        }

        public class FollowerViewHolder : Java.Lang.Object
        {
            public ImageView ivFollowerPic { get; set; }
            public TextView tvFollowerName { get; set; }
            public Button btnSendInvite { get; set; }
        }

        private class btnInviteClick : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private Button btnSendInvite;
            private FollowerAdapter dataAdapter;
            private SPGetUserFollowers_Result followerData;

            public btnInviteClick(Activity activity, SPGetUserFollowers_Result followerData, Button btnSendInvite, FollowerAdapter dataAdapter)
            {
                this.dataAdapter = dataAdapter;
                this.activity = activity;
                this.followerData = followerData;
                this.btnSendInvite = btnSendInvite;
            }

            public void OnClick(View v)
            {
                if (!followerData.IsFriend && !followerData.IsFriendRequestPending)
                {
                    new postInviteContact(activity, followerData, btnSendInvite, dataAdapter).Execute();
                }
            }

            private class postInviteContact : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
            {
                private Activity activity;
                private Button btnSendInvite;
                private SPGetUserFollowers_Result userData;
                private JsonResult jResult;
                private FriendRequest friendRequest;
                ProgressDialog p;
                private FollowerAdapter followerDataAdapter;

                public postInviteContact(Activity activity, SPGetUserFollowers_Result userData, Button btnSendInvite, FollowerAdapter followerDataAdapter)
                {
                    this.activity = activity;
                    this.userData = userData;
                    this.btnSendInvite = btnSendInvite;
                    this.followerDataAdapter = followerDataAdapter;
                }

                protected override void OnPreExecute()
                {
                    base.OnPreExecute();
                    p = ProgressDialog.Show(activity, "", "Please wait");
                    friendRequest = new FriendRequest
                    {
                        SendToEmail = string.IsNullOrEmpty(userData.EmailId) ? "" : userData.EmailId,
                        SendToMobile = string.IsNullOrEmpty(userData.MobileNo) ? "" : userData.MobileNo,
                        Username = GlobalClass.UserName,
                        SendToId = userData.UserId
                    };
                }

                protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
                {
                    jResult = RestSharpCall.Post<JsonResult>(friendRequest, "Friend/Create");
                    return jResult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    base.OnPostExecute(result);
                    p.Dismiss();
                    if (result != null)
                    {
                        if (result.Message == "Following")
                        {
                            userData.IsFriend = true;
                        }
                        if (result.Message == "Friend Request sent")
                        {
                            userData.IsFriendRequestPending = true;
                        }
                        followerDataAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        Toast.MakeText(activity, "Error", ToastLength.Short).Show();
                    }
                }
            }
        }

    }
}