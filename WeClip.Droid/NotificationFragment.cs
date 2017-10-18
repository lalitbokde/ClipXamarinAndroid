using Fragment = Android.Support.V4.App.Fragment;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    public class NotificationFragment : Fragment
    {
        private View view;
        private ListView lvNotification;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Activity.Title = "";
                TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
                toolbar_title.Text = "Notifications";
                view = inflater.Inflate(Resource.Layout.Notification, container, false);
                return view;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                lvNotification = view.FindViewById<ListView>(Resource.Id.lvNotification);
                new GetAllNotification(lvNotification, Activity).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("NotificationFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }
    }
}