using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using WeClip.Droid.Adapters;
using Android.Widget;
using Android.Support.V4.App;

namespace WeClip.Droid
{
    [Activity(Label = "SearchActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SearchActivity : AppCompatActivity
    {
        Toolbar toolbar;
        TextView toolbar_title;
        TabLayout tablayoutPWE;
        ViewPager viewPagerPWE;
        string currentTab = "";
        private FragmentPagerAdapter madapter;
        int tabIndex = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchActivity);

            var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.Clear();
            prefEditor.Commit();

            currentTab = Intent.GetStringExtra("CurrentTab") ?? null;

            toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            toolbar_title = (TextView)FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.Title = "";
            // toolbar_title.Text = "Search";
            viewPagerPWE = FindViewById<ViewPager>(Resource.Id.viewPagerSearch);
            tablayoutPWE = FindViewById<TabLayout>(Resource.Id.tablayoutPWE);

            var fragments = new Fragment[]
            {
                new PeoplesFragment(),
                new WeClipSearchFragment(),
                new EventSearchFragment()
            };

            viewPagerPWE.OffscreenPageLimit = 3;

            var titles = CharSequence.ArrayFromStringArray(new[]
            {
                "People",
                "WeClips",
                "Events"
            });
            madapter = new SearchTabsAdapter(this.SupportFragmentManager, fragments, titles);
            viewPagerPWE.Adapter = madapter;
            tablayoutPWE.SetupWithViewPager(viewPagerPWE);


            switch (currentTab)
            {
                case "People":
                    tabIndex = 0;

                    break;
                case "WeClips":
                    tabIndex = 1;

                    break;
                case "Events":
                    tabIndex = 2;
                    break;

                default:
                    tabIndex = 0;
                    break;
            }

            viewPagerPWE.SetCurrentItem(tabIndex, true);
            viewPagerPWE.AddOnPageChangeListener(new pageChangeListner(this, viewPagerPWE, madapter));

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
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

        private class pageChangeListner : Java.Lang.Object, ViewPager.IOnPageChangeListener
        {
            private FragmentPagerAdapter madapter;
            private SearchActivity searchActivity;
            private ViewPager viewPagerPWE;

            public pageChangeListner(SearchActivity searchActivity, ViewPager viewPagerPWE)
            {
                this.viewPagerPWE = viewPagerPWE;
                this.searchActivity = searchActivity;

            }

            public pageChangeListner(SearchActivity searchActivity, ViewPager viewPagerPWE, FragmentPagerAdapter madapter) : this(searchActivity, viewPagerPWE)
            {
                this.madapter = madapter;
            }

            public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
            {
            }

            public void OnPageScrollStateChanged(int state)
            {
            }

            public void OnPageSelected(int position)
            {
                if (position == 0)
                {
                    PeoplesFragment fragment = (PeoplesFragment)madapter.GetItem(position);
                    fragment.OnStart();
                }
                if (position == 1)
                {
                    WeClipSearchFragment fragment = (WeClipSearchFragment)madapter.GetItem(position);
                    fragment.OnStart();
                }
                if (position == 2)
                {
                    EventSearchFragment fragment = (EventSearchFragment)madapter.GetItem(position);
                    fragment.OnStart();
                }
            }
        }
    }
}
