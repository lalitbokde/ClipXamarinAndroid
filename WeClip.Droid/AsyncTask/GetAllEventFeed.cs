using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;

namespace WeClip.Droid.AsyncTask
{
    class GetAllEventFeed : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventFeedModel>>
    {
        private FragmentActivity activity;
        private ListView lvComment;
        private long eventID;
        private List<EventFeedModel> eventfeed;
        private TextView emptyComment;

        public GetAllEventFeed(ListView lvComment, FragmentActivity activity, long eventID, TextView emptyComment)
        {
            this.emptyComment = emptyComment;
            this.lvComment = lvComment;
            this.activity = activity;
            this.eventID = eventID;
            eventfeed = new List<EventFeedModel>();
        }

        protected override List<EventFeedModel> RunInBackground(params Java.Lang.Void[] @params)
        {
            eventfeed = RestSharpCall.GetList<EventFeedModel>("Event/GetAllEventFeedDetails?eventId=" + eventID);
            return eventfeed;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            if (result != null && eventfeed.Count > 0)
            {
                emptyComment.Visibility = Android.Views.ViewStates.Gone;
                var madapter = new CommentsAdapter(eventfeed, activity);
                lvComment.Adapter = madapter;
            }
            else
            {
                emptyComment.Visibility = Android.Views.ViewStates.Visible;
            }
        }

    }
}
