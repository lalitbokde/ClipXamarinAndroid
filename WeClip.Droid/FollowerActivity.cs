using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Core.Model;
using System.Collections.Generic;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;

namespace WeClip.Droid
{
    [Activity(Label = "FollowerActivity")]
    public class FollowerActivity : AppCompatActivity
    {
        private ListView lvFollower;
        private long UserID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Follower);
            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";
            UserID = Intent.GetLongExtra("UserID", 0);
            toolbar_title.Text = "Followers";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }
            lvFollower = FindViewById<ListView>(Resource.Id.lvFollower);
            var emptyFollower = FindViewById<TextView>(Resource.Id.emptyFollower);
            new GetFollowerData(lvFollower, this, UserID, emptyFollower).Execute();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        private class GetFollowerData : AsyncTask<Java.Lang.Object, Java.Lang.Object, List<SPGetUserFollowers_Result>>
        {
            private Activity activity;
            private ListView lvFollower;
            private long userID;
            private List<SPGetUserFollowers_Result> followerList;
            private ProgressDialog p;
            private TextView emptyFollower;

            public GetFollowerData(ListView lvFollower, Activity activity, long userID, TextView emptyFollower)
            {
                this.emptyFollower = emptyFollower;
                this.lvFollower = lvFollower;
                this.activity = activity;
                this.userID = userID;
                followerList = new List<SPGetUserFollowers_Result>();
                p = ProgressDialog.Show(activity, "Please wait", "loading data");
                emptyFollower.Visibility = ViewStates.Gone;
            }

            protected override List<SPGetUserFollowers_Result> RunInBackground(params Java.Lang.Object[] @params)
            {
                followerList = RestSharpCall.GetList<SPGetUserFollowers_Result>("Friend/GetFollower?userId=" + userID);
                return followerList;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    if (followerList.Count != 0)
                    {
                        var objAdapter = new FollowerAdapter(followerList, activity);
                        lvFollower.Adapter = objAdapter;
                    }
                    else
                    {
                        emptyFollower.Visibility = ViewStates.Visible;
                    }
                }
            }
        }
    }
}