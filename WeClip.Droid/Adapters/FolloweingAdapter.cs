using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Droid.Helper;
using WeClip.Core.Common;
using Android.Graphics;

namespace WeClip.Droid.Adapters
{
    public class FolloweingAdapter : BaseAdapter
    {
        private Activity activity;
        private List<SPGetUserFollowings_Result> FolloweingList;
        private FolloweingViewHolder viewHolder;

        public FolloweingAdapter(List<SPGetUserFollowings_Result> FolloweingList, Activity activity) : base()
        {
            this.FolloweingList = FolloweingList;
            this.activity = activity;
        }

        public override int Count
        {
            get
            {
                return FolloweingList.Count;
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
                viewHolder = new FolloweingViewHolder();
                LayoutInflater inflater = activity.LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.FollowingItem, parent, false);
                viewHolder.tvFolloweingName = convertView.FindViewById<TextView>(Resource.Id.tvFollowingName);
                viewHolder.ivFolloweingPic = convertView.FindViewById<ImageView>(Resource.Id.ivFollowingPhoto);
                viewHolder.btnSendInvite = convertView.FindViewById<Button>(Resource.Id.btnFollowingInvite);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as FolloweingViewHolder;
            }

            viewHolder.tvFolloweingName.Text = FolloweingList[position].UserName;

            if (string.IsNullOrEmpty(FolloweingList[position].ProfilePic))
            {
                viewHolder.ivFolloweingPic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                Picasso.With(activity).Load(FolloweingList[position].ProfilePic).Placeholder(Resource.Drawable.contact_withoutphoto)
           .Resize(80, 80).Transform(new CircleTransformation()).Into(viewHolder.ivFolloweingPic);
            }

            if (FolloweingList[position].ParentUserID == Convert.ToInt64(GlobalClass.UserID))
            {
                viewHolder.btnSendInvite.Visibility = ViewStates.Gone;
            }

            if (FolloweingList[position].isFriendRequestPending == true)
            {
                viewHolder.btnSendInvite.Text = "Request sent";
                viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));

            }
            else
            {
                if (FolloweingList[position].IsFriend == false)
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

            viewHolder.btnSendInvite.SetOnClickListener(new btnInviteClick(activity, FolloweingList[position], viewHolder.btnSendInvite, this));
            return convertView;
        }

        public class FolloweingViewHolder : Java.Lang.Object
        {
            public ImageView ivFolloweingPic { get; set; }
            public TextView tvFolloweingName { get; set; }
            public Button btnSendInvite { get; set; }
        }

        private class btnInviteClick : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private Button btnSendInvite;
            private FolloweingAdapter dataAdapter;
            private SPGetUserFollowings_Result FolloweingData;

            public btnInviteClick(Activity activity, SPGetUserFollowings_Result FolloweingData, Button btnSendInvite, FolloweingAdapter dataAdapter)
            {
                this.dataAdapter = dataAdapter;
                this.activity = activity;
                this.FolloweingData = FolloweingData;
                this.btnSendInvite = btnSendInvite;
            }

            public void OnClick(View v)
            {
                if (!FolloweingData.IsFriend && !FolloweingData.isFriendRequestPending)
                {
                    new postInviteContact(activity, FolloweingData, btnSendInvite, dataAdapter).Execute();
                }
            }

            private class postInviteContact : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
            {
                private Activity activity;
                private Button btnSendInvite;
                private SPGetUserFollowings_Result userData;
                private JsonResult jResult;
                private FriendRequest friendRequest;
                ProgressDialog p;
                private FolloweingAdapter FolloweingDataAdapter;

                public postInviteContact(Activity activity, SPGetUserFollowings_Result userData, Button btnSendInvite, FolloweingAdapter FolloweingDataAdapter)
                {
                    this.activity = activity;
                    this.userData = userData;
                    this.btnSendInvite = btnSendInvite;
                    this.FolloweingDataAdapter = FolloweingDataAdapter;
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
                        SendToId = userData.ParentUserID
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
                            userData.isFriendRequestPending = true;
                        }
                        FolloweingDataAdapter.NotifyDataSetChanged();
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