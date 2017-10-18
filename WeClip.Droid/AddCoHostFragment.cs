using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    public class AddCoHostFragment : Fragment
    {
        private View rootView;
        private long EventID;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EventID = MainActivity.myBundle.GetLong("EventID");
            var id = EventID;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Add Co-Hosts";
            rootView = inflater.Inflate(Resource.Layout.AddCoHost, container, false);
            return rootView;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                new GetCoHost(rootView, Activity, EventID).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("AddCoHostFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }
    }
}