using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.AsyncTask;
using Android.Support.V4.View;

namespace WeClip.Droid
{
    public class CommentsFragment : Fragment
    {
        private View view;
        private ListView lvComment;
        long eventID;
        private TextView emptyComment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            eventID = MainActivity.myBundle.GetLong("EventID");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            view = inflater.Inflate(Resource.Layout.CommentsFragment, container, false);
            lvComment = view.FindViewById<ListView>(Resource.Id.lvComments);
            ViewCompat.SetNestedScrollingEnabled(lvComment, true);
            emptyComment = view.FindViewById<TextView>(Resource.Id.emptyComment);
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                new GetAllEventFeed(lvComment, Activity, eventID, emptyComment).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("CommentsFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }
    }
}