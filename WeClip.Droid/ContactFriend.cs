using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "ContactFriend", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait) ]
    public class ContactFriend : AppCompatActivity
    {
        private ListView lvContact;
        private long EventID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Contact_Friend);
            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";
            EventID = Intent.GetLongExtra("EventID", 0);
            toolbar_title.Text = "Friend List";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }
            lvContact = FindViewById<ListView>(Resource.Id.lvContactFriend);
            var txtSearch = FindViewById<EditText>(Resource.Id.etContactFriendSearch);
            new GetContactFriendData(lvContact, this, txtSearch, EventID).Execute();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}