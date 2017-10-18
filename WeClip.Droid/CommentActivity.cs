using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Droid.AsyncTask;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using WeClip.Core.Common;
using Android.Views;

namespace WeClip.Droid
{
    [Activity(Label = "CommentActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CommentActivity : AppCompatActivity
    {
        Toolbar toolbar;
        private EditText txtComment;
        TextView toolbar_title;
        private Button btnSendComment;
        private long EventID;
        private ListView lvComment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CommentActivity);
            toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            toolbar_title = (TextView)FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";
            toolbar_title.Text = "Comments";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            EventID = Intent.GetLongExtra("CurrentEvent", 0);
            lvComment = FindViewById<ListView>(Resource.Id.lvCommentActivity);
            btnSendComment = FindViewById<Button>(Resource.Id.btnSendComment);
            txtComment = FindViewById<EditText>(Resource.Id.txtComment);
        }

        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                base.OnStart();
                new GetEventFeed(lvComment, this, EventID, btnSendComment, txtComment).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("CommentsFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        private class GetEventFeed : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventFeedModel>>
        {
            private Button btnSendComment;
            private CommentActivity activity;
            private long eventID;
            private ListView lvComment;
            private EditText txtComment;
            private List<EventFeedModel> eventfeed;

            public GetEventFeed(ListView lvComment, CommentActivity activity, long eventID, Button btnSendComment, EditText txtComment)
            {
                this.lvComment = lvComment;
                this.activity = activity;
                this.eventID = eventID;
                this.btnSendComment = btnSendComment;
                this.txtComment = txtComment;
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

                if (result != null)
                {
                    var madapter = new CommentsAdapter(eventfeed, activity);
                    lvComment.Adapter = madapter;
                    btnSendComment.Click += delegate
                    {
                        string comment = txtComment.Text.Trim();
                        if (comment == "")
                        {
                            AlertBox.Create("Error", "Please enter Comment", activity);
                            return;
                        };
                        EventFeedModel model = new EventFeedModel();
                        int userID = 0;
                        int.TryParse(GlobalClass.UserID.ToString(), out userID);
                        model.UserID = userID;
                        model.EventID = eventID;
                        model.Message = comment;
                        new PostFeedData(txtComment, model, lvComment, activity, eventID, madapter).Execute();
                    };
                }
                else
                {
                    AlertBox.Create("Error", "Error", activity);
                }
            }

            private class PostFeedData : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
            {
                private Activity activity;
                private long eventID;
                private ListView lvComment;
                private CommentsAdapter madapter;
                private EventFeedModel model;
                private EditText txtComment;
                private JsonResult jresult;

                public PostFeedData(EditText txtComment, EventFeedModel model, ListView lvComment, Activity activity, long eventID, CommentsAdapter madapter)
                {
                    this.txtComment = txtComment;
                    this.model = model;
                    this.lvComment = lvComment;
                    this.activity = activity;
                    this.eventID = eventID;
                    this.madapter = madapter;
                    jresult = new JsonResult();
                }

                protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
                {
                    jresult = RestSharpCall.Post<JsonResult>(model, "Event/SaveEventFeed");
                    return jresult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    base.OnPostExecute(result);
                    txtComment.Text = "";
                    if (result.Success == true)
                    {
                        var eventfeed = RestSharpCall.GetList<EventFeedModel>("Event/GetAllEventFeedDetails?eventId=" + eventID);
                        var madapter = new CommentsAdapter(eventfeed, activity);
                        lvComment.Post(() =>
                        {
                            lvComment.SetSelection(madapter.Count - 1);
                        });

                        lvComment.Adapter = madapter;
                        lvComment.Invalidate();
                    }
                }
            }
        }
    }
}