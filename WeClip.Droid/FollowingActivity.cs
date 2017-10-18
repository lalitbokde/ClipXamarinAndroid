using System.Collections.Generic;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;

namespace WeClip.Droid
{
    [Activity(Label = "FollowingActivity")]

    public class FollowingActivity : AppCompatActivity
    {
        private ListView lvFolloweing;
        private long UserID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Following);
            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";
            UserID = Intent.GetLongExtra("UserID", 0);
            toolbar_title.Text = "Following";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }
            lvFolloweing = FindViewById<ListView>(Resource.Id.lvFolloweing);
            var emptyFolloweing = FindViewById<TextView>(Resource.Id.emptyFolloweing);
            new GetFolloweingData(lvFolloweing, this, UserID, emptyFolloweing).Execute();
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

        private class GetFolloweingData : AsyncTask<Java.Lang.Object, Java.Lang.Object, List<SPGetUserFollowings_Result>>
        {
            private Activity activity;
            private ListView lvFolloweing;
            private long userID;
            private List<SPGetUserFollowings_Result> FolloweingList;
            private ProgressDialog p;
            private TextView emptyFolloweing;


            public GetFolloweingData(ListView lvFolloweing, Activity activity, long userID, TextView emptyFolloweing)
            {
                this.emptyFolloweing = emptyFolloweing;
                this.lvFolloweing = lvFolloweing;
                this.activity = activity;
                this.userID = userID;
                FolloweingList = new List<SPGetUserFollowings_Result>();
                p = ProgressDialog.Show(activity, "Please wait", "loading data");
                emptyFolloweing.Visibility = ViewStates.Gone;
            }

            protected override List<SPGetUserFollowings_Result> RunInBackground(params Java.Lang.Object[] @params)
            {
                FolloweingList = RestSharpCall.GetList<SPGetUserFollowings_Result>("Friend/GetFolloweing?userId=" + userID);
                return FolloweingList;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    if (FolloweingList.Count != 0)
                    {
                        var objAdapter = new FolloweingAdapter(FolloweingList, activity);
                        lvFolloweing.Adapter = objAdapter;
                    }
                    else
                    {
                        emptyFolloweing.Visibility = ViewStates.Visible;
                    }
                }
            }
        }
    }
}

