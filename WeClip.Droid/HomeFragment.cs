using Android.OS;
using Android.Support.V7.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Views;
using WeClip.Droid.AsyncTask;
using Android.App;
using WeClip.Droid.Helper;
using Android.Widget;
using System;
using Android.Support.V4.App;
using WeClip.Core.Common;
using Android.Support.Design.Widget;
using System.Net;
using System.ComponentModel;
using Android.Support.V4.Widget;
using Android.Graphics;

namespace WeClip.Droid
{
    public class HomeFragment : Fragment
    {
        private RecyclerView rvPublicEvent, rvPrivateEvent;
        private View rootView;
        private ImageButton btnCreateEvent, btnViewProfile, btnEventSearch;
        TabLayout tablayout;
        private SwipeRefreshLayout mSwipeRefreshLayoutpublic, mSwipeRefreshLayoutPrivate;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Home";
        //    toolbar_title.Gravity = GravityFlags.Center;
            Typeface face = Typeface.CreateFromAsset(Activity.Assets, "Fonts/Avenir.ttc");
            toolbar_title.SetTypeface(face, TypefaceStyle.Bold);
            rootView = inflater.Inflate(Resource.Layout.HomeFragment, container, false);
            rvPublicEvent = rootView.FindViewById<RecyclerView>(Resource.Id.rvHomePublicEvent);
            rvPrivateEvent = rootView.FindViewById<RecyclerView>(Resource.Id.rvHomePrivateEvent);
            btnCreateEvent = rootView.FindViewById<ImageButton>(Resource.Id.ivHFCreateEvent);
            btnViewProfile = rootView.FindViewById<ImageButton>(Resource.Id.ivHFUserProfie);
            btnEventSearch = rootView.FindViewById<ImageButton>(Resource.Id.ivHFSearchEvent);
            mSwipeRefreshLayoutpublic = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout_Public);
            mSwipeRefreshLayoutpublic.SetColorSchemeResources(Resource.Color.primaryDark, Resource.Color.primaryLight, Resource.Color.primaryDark);

            mSwipeRefreshLayoutPrivate = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout_Private);
            mSwipeRefreshLayoutPrivate.SetColorSchemeResources(Resource.Color.primaryDark, Resource.Color.primaryLight, Resource.Color.primaryDark);

            tablayout = rootView.FindViewById<TabLayout>(Resource.Id.tablayoutHome);

            btnCreateEvent.Click += BtnCreateEvent_Click;
            btnViewProfile.Click += BtnViewProfile_Click;
            btnEventSearch.Click += BtnEventSearch_Click;

            tablayout.AddTab(tablayout.NewTab().SetText("Your Event"), 0, true);
            tablayout.AddTab(tablayout.NewTab().SetText("Public Event"), 1);

            RecyclerView.LayoutManager verticalLayoutManagerForPublic = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            RecyclerView.LayoutManager verticalLayoutManagerForPrivate = new LinearLayoutManager(Activity);

            rvPrivateEvent.SetLayoutManager(verticalLayoutManagerForPrivate);
            rvPrivateEvent.AddItemDecoration(new DividerItemDecoration(this.Activity));
            rvPrivateEvent.AddItemDecoration(new VerticalSpaceItemDecoration(10));
            rvPublicEvent.SetLayoutManager(verticalLayoutManagerForPublic);
            rvPublicEvent.AddItemDecoration(new DividerItemDecoration(this.Activity));
            rvPublicEvent.AddItemDecoration(new VerticalSpaceItemDecoration(10));
            return rootView;
        }

        private void BtnEventSearch_Click(object sender, EventArgs e)
        {
            var eventSearch = new EventSearchFragmentFromHome();
            Android.Support.V4.App.FragmentManager eventFragmentManager = Activity.SupportFragmentManager;
            eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, eventSearch, "eventSearch").AddToBackStack("eventSearch").Commit();
        }

        private void BtnViewProfile_Click(object sender, EventArgs e)
        {
            var profile = new ProfileFragment();
            Bundle b = new Bundle();
            long UserId = Convert.ToInt64(GlobalClass.UserID);
            b.PutLong("UserId", UserId);
            profile.Arguments = (b);
            Android.Support.V4.App.FragmentManager eventFragmentManager = Activity.SupportFragmentManager;
            eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, profile, "profile").AddToBackStack("profile").Commit();
        }

        private void BtnCreateEvent_Click(object sender, System.EventArgs e)
        {
            Activity.StartActivity(typeof(CreateEvent));
        }

        public override void OnStart()
        {
            base.OnStart();
            this.HasOptionsMenu = true;
            new GetPublicPrivteEventData(rootView, rvPublicEvent, rvPrivateEvent, Activity, tablayout, mSwipeRefreshLayoutpublic, mSwipeRefreshLayoutPrivate).Execute();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            AlertBox.Create("Done", "FileDownloaded", Activity);
        }

        private class LinearLayoutManagerMethod : LinearLayoutManager
        {
            Activity activity;

            public LinearLayoutManagerMethod(Activity activity) : base(activity, LinearLayoutManager.Vertical, false)
            {
                this.activity = activity;
            }

            public override bool CanScrollVertically()
            {
                return false;
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.NotificationMenu, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_notification:
                    Android.Support.V4.App.Fragment fragment = new NotificationFragment();
                    var fragmentManager = Activity.SupportFragmentManager;
                    Android.Support.V4.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                    fragmentTransaction.Replace(Resource.Id.content_frame, fragment);
                    fragmentTransaction.AddToBackStack("HomeFragment");
                    fragmentTransaction.Commit();
                    return true;

                case Resource.Id.action_search:
                    Activity.StartActivity(typeof(SearchActivity));
                    return true;
            }
            return false;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        private class toolbarClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private TextView toolbar_title;

            public toolbarClickListner(FragmentActivity activity, TextView toolbar_title)
            {
                this.activity = activity;
                this.toolbar_title = toolbar_title;
            }

            public void OnClick(View v)
            {
                activity.StartActivity(typeof(SearchActivity));
            }
        }
    }
}