using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using static Android.Support.Design.Widget.TabLayout;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    public class ProfileFragment : Fragment, IOnTabSelectedListener
    {
        private View view;
        TabLayout tablayout;
        RelativeLayout layoutStories;
        RelativeLayout layoutEvents;
        long UserID;
        TextView toolbar_title;
        ImageView ivBack;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";

            view = inflater.Inflate(Resource.Layout.ProfileFragment, container, false);
            UserID = this.Arguments.GetLong("UserId");
            tablayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            layoutStories = view.FindViewById<RelativeLayout>(Resource.Id.layoutStories);
            layoutEvents = view.FindViewById<RelativeLayout>(Resource.Id.layoutEvents);
            ivBack = view.FindViewById<ImageView>(Resource.Id.ivPFback);

            tablayout.AddTab(tablayout.NewTab().SetText("Story"), 0, true);
            tablayout.AddTab(tablayout.NewTab().SetText("Events"), 1);
            tablayout.SetOnTabSelectedListener(this);

            layoutStories.Visibility = ViewStates.Visible;
            layoutEvents.Visibility = ViewStates.Gone;
            ivBack.Click += IvBack_Click;

            return view;
        }

        private void IvBack_Click(object sender, System.EventArgs e)
        {
            Activity.OnBackPressed();
        }

        public void OnTabSelected(Tab tab)
        {
            switch (tab.Position)
            {
                case 0:
                    layoutStories.Visibility = ViewStates.Visible;
                    layoutEvents.Visibility = ViewStates.Gone;
                    break;

                case 1:
                    layoutStories.Visibility = ViewStates.Gone;
                    layoutEvents.Visibility = ViewStates.Visible;
                    break;

                default:
                    break;
            }
        }

        public void OnTabReselected(Tab tab)
        { }

        public void OnTabUnselected(Tab tab)
        { }

        public override void OnStart()
        {
            base.OnStart();
            new GetProfileDetails(Activity, view, UserID, toolbar_title).Execute();
        }
    }
}