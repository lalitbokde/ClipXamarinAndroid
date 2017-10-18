using Android.App;
using Android.OS;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Core.Common;
using Android.Widget;
using WeClip.Droid.Adapters;

namespace WeClip.Droid.AsyncTask
{
    class postEventRequest : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity context;
        private long eventID;
        private EventRequest eventRequestModel;
        private FriendListModel objFLM;
        private JsonResult jResult;
        private ProgressDialog p;
        private Contact_FriendAdapter objContact_FriendAdapter;
        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(context, "Please wait...", "Sending request");
        }

        public postEventRequest(FriendListModel objFLM, Activity context, long eventID, Contact_FriendAdapter objContact_FriendAdapter)
        {
            this.objFLM = objFLM;
            this.context = context;
            this.eventID = eventID;
            this.objContact_FriendAdapter = objContact_FriendAdapter;

            if (objFLM.IsEmail == true)
            {
                this.eventRequestModel = new EventRequest
                {
                    EventID = eventID,
                    FriendID = objFLM.ID,
                    Email = objFLM.PhoneNumber,
                };
            }
            else
            {
                this.eventRequestModel = new EventRequest
                {
                    EventID = eventID,
                    FriendID = objFLM.ID,
                    PhoneNumber = objFLM.PhoneNumber,
                };
            }


        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            jResult = RestSharpCall.Post<JsonResult>(eventRequestModel, "EventRequest/InviteForEvent");
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
                    if (result.Message == "Invited")
                    {
                        if (objFLM.IsFriend == "1")
                        {
                            objFLM.isInvitedFriend = "1";
                        }
                        else
                        {
                            objFLM.InvitedContact = "1";
                        }
                    }
                    objContact_FriendAdapter.NotifyDataSetChanged();
                    Toast.MakeText(context, "Success", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(context, "Error", ToastLength.Short).Show();
                }
            }
        }
    }
}