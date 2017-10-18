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
    public class EventFragment : Fragment
    {
        private View view;
        long EventID;
        LinearLayout parentLayout;
        ViewPager viewPagerPWC;
        TabLayout tablayoutPWC;
        protected static int CAMERA_REQUEST = 1337;
        protected static int GALLERY_PICTURE = 1;
        private string uploadImagePath;
        Android.Net.Uri selectedImagePath;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EventID = MainActivity.myBundle.GetLong("EventID");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
                Activity.Title = "";
                toolbar_title.Text = "Event Details";
                view = inflater.Inflate(Resource.Layout.EventFragment, container, false);
                parentLayout = view.FindViewById<LinearLayout>(Resource.Id.parentLayout);
                viewPagerPWC = view.FindViewById<ViewPager>(Resource.Id.viewPagerPWC);
                tablayoutPWC = view.FindViewById<TabLayout>(Resource.Id.tablayoutPWC);
                return view;
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventFragment", "OnCreateView", ex.StackTrace + ex.Message).Execute();
                return view;
            }
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                this.HasOptionsMenu = true;
                new GetEventDetails(Activity, Activity.ApplicationContext, view, EventID, this).Execute();
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
                new CrashReportAsync("EventFragment", "OnStart", ex.StackTrace + ex.Message).Execute();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.SearchMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_EF_search:
                    Activity.StartActivity(typeof(SearchActivity));
                    return true;
            }
            return false;
        }

    }
}