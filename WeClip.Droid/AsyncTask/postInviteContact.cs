using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Content;
using Android.Net;

namespace WeClip.Droid.AsyncTask
{
    public class postInviteContact : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Button btnSendInvite;
        private Contact contact;
        private Activity context;
        private JsonResult jResult;
        private FriendRequest friendRequest;
        ProgressDialog p;
        private InviteContactAdapter inviteContactAdapter;

        public postInviteContact(Activity context, Contact contact, Button btnSendInvite, InviteContactAdapter inviteContactAdapter)
        {
            this.context = context;
            this.contact = contact;
            this.btnSendInvite = btnSendInvite;
            this.inviteContactAdapter = inviteContactAdapter;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(context, "", "Please wait");

            if (contact.isEmailAddress == true)
            {
                friendRequest = new FriendRequest
                {
                    SendToEmail = string.IsNullOrEmpty(contact.PhoneNo) ? "" : contact.PhoneNo,
                    Username = GlobalClass.UserName,
                    SendToId = contact.Id
                };
            }
            else
            {
                friendRequest = new FriendRequest
                {
                    SendToMobile = string.IsNullOrEmpty(contact.PhoneNo) ? "" : contact.PhoneNo,
                    Username = GlobalClass.UserName,
                    SendToId = contact.Id
                };
            }
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
                if (result.Success == true)
                {
                    if (result.Message == "Following")
                    {
                        contact.isFriend = true;
                        inviteContactAdapter.NotifyDataSetChanged();
                    }
                    else if (result.Message == "Friend Request sent")
                    {
                        contact.isFriendRequestPending = true;
                        inviteContactAdapter.NotifyDataSetChanged();
                    }
                    else if (result.Message == "Invited")
                    {
                        contact.isInvited = true;
                        inviteContactAdapter.NotifyDataSetChanged();
                    }

                    if (friendRequest.SendToMobile != null)
                    {
                        Intent intent = new Intent(Intent.ActionView);
                        intent.PutExtra("address", friendRequest.SendToMobile);
                        intent.PutExtra("sms_body", "Download weclip from http://www.weclip.com");
                        intent.SetData(Uri.Parse("smsto:" + friendRequest.SendToMobile));
                        context.StartActivity(intent);
                    }
                    //   btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    //   btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                {
                    Toast.MakeText(context, "Error", ToastLength.Short).Show();
                }
            }
        }
    }
}