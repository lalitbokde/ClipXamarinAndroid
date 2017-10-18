using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Adapters;
using WeClip.Droid.AsyncTask;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace WeClip.Droid
{
    [Activity(Label = "InviteFriends")]
    public class InviteFriends : AppCompatActivity
    {
        private ListView lvFriend;
        private List<FriendListModel> friendList;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FriendsView);
            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";
            toolbar_title.Text = "Friend List";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }
            lvFriend = FindViewById<ListView>(Resource.Id.lvFriends);
            await getFriendsDetails();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish(); // close this activity and return to preview activity (if there is any)
            }
            return base.OnOptionsItemSelected(item);
        }
        public async Task<List<FriendListModel>> getFriendsDetails()
        {
            friendList = new List<FriendListModel>();
            try
            {
                friendList = await GetDetails.GetAll<FriendListModel>("Friend/GetFriendList");
                lvFriend.Adapter = new FriendsAdapter(friendList, this);
                return friendList;
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("InviteFriends", "getFriendsDetails", ex.StackTrace + ex.Message).Execute();
                return friendList;
            }
        }
    }
}