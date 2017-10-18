using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using Android.Graphics;
using System;
using Android.OS;
using Java.Lang;
using Android.App;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    class PeopleDataAdapter : BaseAdapter<SPGetSearchUserList_Result>
    {
        private FragmentActivity activity;
        private List<SPGetSearchUserList_Result> userdata;

        public PeopleDataAdapter(List<SPGetSearchUserList_Result> userdata, FragmentActivity activity)
        {
            this.userdata = userdata;
            this.activity = activity;
        }

        public override SPGetSearchUserList_Result this[int position]
        {
            get
            {
                return userdata[position];
            }
        }

        public override int Count
        {
            get
            {
                return userdata.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHolder;
            if (convertView == null)
            {
                viewHolder = new ViewHolder();
                LayoutInflater inflater = activity.LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.PeopleFragmentItem, parent, false);
                viewHolder.tvContactName = convertView.FindViewById<TextView>(Resource.Id.tvPFIuserName);
                viewHolder.ivContactPic = convertView.FindViewById<ImageView>(Resource.Id.ivPFIUserPhoto);
                viewHolder.btnSendInvite = convertView.FindViewById<Button>(Resource.Id.btnPFIinvited);
                viewHolder.rvlItem = convertView.FindViewById<RelativeLayout>(Resource.Id.rlPFItem);

                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as ViewHolder;
            }

            viewHolder.tvContactName.Text = userdata[position].UserName;

            if (string.IsNullOrEmpty(userdata[position].UserPhoto))
            {
                viewHolder.ivContactPic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                Square.Picasso.Picasso.With(activity).Load(userdata[position].UserPhoto).Placeholder(Resource.Drawable.contact_withoutphoto)
             .Resize(150, 150).Transform(new Helper.CircleTransformation()).Into(viewHolder.ivContactPic);
            }

            if (userdata[position].UserId == Convert.ToInt64(GlobalClass.UserID))
            {
                viewHolder.btnSendInvite.Visibility = ViewStates.Gone;
            }

            if (userdata[position].isFriendRequestPending == true)
            {
                viewHolder.btnSendInvite.Text = "Request sent";
                viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
            }
            else if (userdata[position].IsFriend)
            {
                viewHolder.btnSendInvite.Text = "Following";
                viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
            }
            else
            {
                viewHolder.btnSendInvite.Text = "Follow";
                viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#04c285"));
            }

            viewHolder.btnSendInvite.SetOnClickListener(new btnInviteClick(activity, userdata[position], viewHolder.btnSendInvite, this));

            return convertView;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView ivContactPic { get; set; }
            public TextView tvContactName { get; set; }
            public Button btnSendInvite { get; set; }
            public RelativeLayout rvlItem { get; set; }
        }

        private class btnInviteClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private Button btnSendInvite;
            private PeopleDataAdapter peopleDataAdapter;
            private SPGetSearchUserList_Result userData;

            public btnInviteClick(FragmentActivity activity, SPGetSearchUserList_Result userData, Button btnSendInvite, PeopleDataAdapter peopleDataAdapter)
            {
                this.peopleDataAdapter = peopleDataAdapter;
                this.activity = activity;
                this.userData = userData;
                this.btnSendInvite = btnSendInvite;
            }

            public void OnClick(View v)
            {
                if (!userData.IsFriend && !userData.isFriendRequestPending)
                {
                    new postInviteContact(activity, userData, btnSendInvite, peopleDataAdapter).Execute();
                }
                else
                {
                    if (userData.isFriendRequestPending == true)
                    {
                        showDiloage("Cancel Request", btnSendInvite, false, userData);
                    }
                    if (userData.IsFriend == true)
                    {
                        showDiloage("Unfollow", btnSendInvite, true, userData);
                    }
                }
            }

            private void showDiloage(string positiveBtnText, Button btnFollowing, bool isFriend, SPGetSearchUserList_Result userData)
            {
                Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(activity);
                alertDialog.SetTitle("Are you sure ?");
                alertDialog.SetPositiveButton(positiveBtnText, (senderAlert, args) =>
                {
                    new postCancelFriendRequest(activity, isFriend, btnFollowing, userData).Execute();
                });
                alertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }

            private class postCancelFriendRequest : AsyncTask<Java.Lang.Object, Java.Lang.Object, JsonResult>
            {
                private ProgressDialog p;
                private JsonResult jResult;
                private FragmentActivity activity;
                private bool isFriend;
                private Button btnFollowing;
                private SPGetSearchUserList_Result userData;

                public postCancelFriendRequest(FragmentActivity activity, bool isFriend, Button btnFollowing, SPGetSearchUserList_Result userData)
                {
                    this.activity = activity;
                    this.isFriend = isFriend;
                    this.btnFollowing = btnFollowing;
                    this.userData = userData;
                    jResult = new JsonResult();
                }

                protected override void OnPreExecute()
                {
                    base.OnPreExecute();
                    p = ProgressDialog.Show(activity, "", "Please wait");
                }

                protected override JsonResult RunInBackground(params Java.Lang.Object[] @params)
                {
                    jResult = RestSharpCall.Post<JsonResult>(null, "Friend/CancelFriendRequest?id=" + userData.UserId + "&isFriend=" + isFriend);
                    return jResult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    base.OnPostExecute(result);
                    p.Dismiss();
                    if (result != null)
                    {
                        if (result.Success == true)
                        {
                            btnFollowing.Text = "Follow";
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#04c285"));

                            if (isFriend == true)
                            {
                                userData.IsFriend = false;
                            }
                            else
                            {
                                userData.isFriendRequestPending = false;
                            }
                        }
                        else
                        {
                            Toast.MakeText(activity, "Error", ToastLength.Short).Show();
                        }
                    }
                }
            }


            private class postInviteContact : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
            {
                private FragmentActivity activity;
                private Button btnSendInvite;
                private SPGetSearchUserList_Result userData;
                private JsonResult jResult;
                private FriendRequest friendRequest;
                ProgressDialog p;
                private PeopleDataAdapter peopleDataAdapter;

                public postInviteContact(FragmentActivity activity, SPGetSearchUserList_Result userData, Button btnSendInvite, PeopleDataAdapter peopleDataAdapter)
                {
                    this.activity = activity;
                    this.userData = userData;
                    this.btnSendInvite = btnSendInvite;
                    this.peopleDataAdapter = peopleDataAdapter;
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
                            userData.isFriendRequestPending = true;
                        }
                        peopleDataAdapter.NotifyDataSetChanged();
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