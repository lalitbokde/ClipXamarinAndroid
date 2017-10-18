using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using WeClip.Droid.Adapters;
using WeClip.Droid.AsyncTask;
using Fragment = Android.Support.V4.App.Fragment;

namespace WeClip.Droid
{
    public class EventRequestFragment : Fragment
    {
        private View view;
        long EventID;
        LinearLayout parentLayout;
        ViewPager viewPagerPWC;
        TabLayout tablayoutPWC;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
                Activity.Title = "";
                toolbar_title.Text = "Event Details";
                EventID = MainActivity.myBundle.GetLong("EventID");
                view = inflater.Inflate(Resource.Layout.EventRequestlayout, container, false);
                parentLayout = view.FindViewById<LinearLayout>(Resource.Id.llERparentLayout);
                viewPagerPWC = view.FindViewById<ViewPager>(Resource.Id.viewPagerERPWC);
                tablayoutPWC = view.FindViewById<TabLayout>(Resource.Id.tablayoutERPWC);
                return view;
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventRequestFragment", "OnCreateView", ex.StackTrace + ex.Message).Execute();
                return view;
            }
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                new GetEventRequestDetails(Activity, view, EventID).Execute();
                var fragments = new Fragment[]
              {
                    new PhotosFragment(),
                    new WeClipsFragment(),
                    new CommentsFragment()
              };

                viewPagerPWC.OffscreenPageLimit = 3;

                var titles = CharSequence.ArrayFromStringArray(new[]
                {
                    "Photos",
                    "WeClips",
                    "Comments"
                });
                viewPagerPWC.Focusable = false;
                tablayoutPWC.Focusable = false;
                viewPagerPWC.Adapter = new EventPWCTabsAdapter(ChildFragmentManager, fragments, titles);
                tablayoutPWC.SetupWithViewPager(viewPagerPWC);

            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventRequestFragment", "OnStart", ex.StackTrace + ex.Message).Execute();
            }
        }

    }
}