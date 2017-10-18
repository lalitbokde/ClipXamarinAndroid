using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using WeClip.Droid.AsyncTask;
using Fragment = Android.Support.V4.App.Fragment;

namespace WeClip.Droid
{
    public class PhotosFragment : Fragment
    {
        private View view;
        private GridView gvEventPhoto;
        long EventID;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EventID = MainActivity.myBundle.GetLong("EventID");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            view = inflater.Inflate(Resource.Layout.PhotosFragment, container, false);
            gvEventPhoto = view.FindViewById<GridView>(Resource.Id.gvPhotosFragment);
            gvEventPhoto.ChoiceMode = ChoiceMode.MultipleModal;
            ViewCompat.SetNestedScrollingEnabled(gvEventPhoto, true);
            new PhotosFragmentLoadData(Activity, gvEventPhoto, view, EventID).Execute();
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("PhotosFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
        }
    }
}