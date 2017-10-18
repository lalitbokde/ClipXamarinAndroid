using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Android.OS;
using WeClip.Droid.AsyncTask;
using System.Linq;

namespace WeClip.Droid.Adapters
{
    public class FrienRequestAdapter : BaseAdapter<GetRequestedFriend_Result>
    {
        private Activity activity;
        private List<GetRequestedFriend_Result> friendReq;

        public FrienRequestAdapter(List<GetRequestedFriend_Result> friendReq, Activity activity)
        {
            this.friendReq = friendReq;
            this.activity = activity;
        }

        public override GetRequestedFriend_Result this[int position]
        {
            get
            {
                return friendReq[position];
            }
        }

        public override int Count
        {
            get
            {
                return friendReq.Count;
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
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.FriendRequestItem, parent, false);
                viewHolder.imgProfilePic = convertView.FindViewById<ImageView>(Resource.Id.ivFRPhotos);
                viewHolder.tvUsername = convertView.FindViewById<TextView>(Resource.Id.tvFRName);
                viewHolder.imgAccept = convertView.FindViewById<ImageView>(Resource.Id.ivFRAccept);
                viewHolder.imgReject = convertView.FindViewById<ImageView>(Resource.Id.ivFRReject);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as ViewHolder;
            }

            viewHolder.tvUsername.Text = friendReq[position].SenderUserName;
            Picasso.With(activity).Load(friendReq[position].SenderProfilePic).CenterCrop().Placeholder(Resource.Drawable.contact_withoutphoto)
          .Resize(100, 100).Transform(new CircleTransformation()).Into(viewHolder.imgProfilePic);

            viewHolder.imgAccept.SetOnClickListener(new btnAcceptRejectClickListner(activity, "Accept", this, friendReq, friendReq[position]));
            viewHolder.imgReject.SetOnClickListener(new btnAcceptRejectClickListner(activity, "Reject", this, friendReq, friendReq[position]));

            return convertView;
        }

        private class btnAcceptRejectClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private FrienRequestAdapter frienRequestAdapter;
            ProgressDialog dialog;
            List<GetRequestedFriend_Result> friendReqList;
            string ResponseStatus;
            private GetRequestedFriend_Result getRequestedFriend_Result;

            public btnAcceptRejectClickListner(Activity activity, string ResponseStatus, FrienRequestAdapter frienRequestAdapter, List<GetRequestedFriend_Result> friendReqList, GetRequestedFriend_Result getRequestedFriend_Result)
            {
                this.getRequestedFriend_Result = getRequestedFriend_Result;
                this.activity = activity;
                this.frienRequestAdapter = frienRequestAdapter;
                this.friendReqList = friendReqList;
                this.ResponseStatus = ResponseStatus;
            }

            public void OnClick(View v)
            {
                dialog = ProgressDialog.Show(activity, "Please wait", "Processing request...", true);
                new PutFriendRequest(activity, ResponseStatus, frienRequestAdapter, dialog, friendReqList, getRequestedFriend_Result).Execute();
            }

            private class PutFriendRequest : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
            {
                private Activity activity;
                private FrienRequestAdapter frienRequestAdapter;
                JsonResult jResult;
                ProgressDialog dialog;
                List<GetRequestedFriend_Result> friendReq;
                private string ResponseStatus;
                private GetRequestedFriend_Result getRequestedFriend_Result;

                public PutFriendRequest(Activity activity, string ResponseStatus, FrienRequestAdapter frienRequestAdapter, ProgressDialog dialog, List<GetRequestedFriend_Result> friendReq, GetRequestedFriend_Result getRequestedFriend_Result)
                {
                    this.getRequestedFriend_Result = getRequestedFriend_Result;
                    this.activity = activity;
                    this.frienRequestAdapter = frienRequestAdapter;
                    this.dialog = dialog;
                    this.friendReq = friendReq;
                    this.ResponseStatus = ResponseStatus;
                }

                protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
                {
                    jResult = RestSharpCall.Post<JsonResult>(null, "Friend/RequestConfirm?id=" + getRequestedFriend_Result.ID + "&response=" + ResponseStatus);
                    return jResult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    dialog.Dismiss();
                    base.OnPostExecute(result);
                    if (result != null)
                    {
                        var del = (from x in friendReq where x.ID == getRequestedFriend_Result.ID select x).FirstOrDefault();
                        if (del != null)
                            friendReq.Remove(del);
                        frienRequestAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        AlertBox.Create("Error", "Error", activity);
                    }
                }
            }
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imgProfilePic { get; set; }
            public TextView tvUsername { get; set; }
            public ImageView imgAccept { get; set; }
            public ImageView imgReject { get; set; }
        }
    }

}