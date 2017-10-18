using System.Collections.Generic;
using Android.OS;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using Java.Lang;
using Android.Widget;
using Android.App;
using WeClip.Droid.Adapters;

namespace WeClip.Droid.AsyncTask
{
    public class GetFriendRequest : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<GetRequestedFriend_Result>>
    {
        ProgressDialog p;
        private List<GetRequestedFriend_Result> friendRequest;
        private ListView lvFriendRequest;
        private Activity activity;

        public GetFriendRequest(ListView lvFriendRequest, Activity activity)
        {
            this.lvFriendRequest = lvFriendRequest;
            this.activity = activity;
            p = ProgressDialog.Show(this.activity, "Please wait...", "Loading data...");
        }

        protected override List<GetRequestedFriend_Result> RunInBackground(params Java.Lang.Void[] @params)
        {
            friendRequest = RestSharpCall.GetList<GetRequestedFriend_Result>("Friend/GetAll");
            return friendRequest;
        }

        protected override void OnPostExecute(Object result)
        {
            p.Dismiss();
            base.OnPostExecute(result);
            if (result != null && friendRequest != null)
            {
                lvFriendRequest.Adapter = new FrienRequestAdapter(friendRequest, activity);
            }
        }
    }
}