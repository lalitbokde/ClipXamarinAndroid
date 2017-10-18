using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Content.PM;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using WeClip.Droid.Adapters;
using System.Collections.Generic;
using WeClip.Droid.Helper;
using Android.Content;
using Square.Picasso;
using Android.Runtime;
using WeClip.Droid.AsyncTask;
using WeClip.Core.Common;
using System;
using WeClip.Core.Model;

namespace WeClip.Droid
{
   
    public class MainActivity : AppComCustomeActivty
    {
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private ListView listDrawerMenu;
        private Button btnEditProfile;
        private ImageView profile_pic;
        private TextView tvUsername;
        public static Bundle myBundle = new Bundle();
        public static WeClipInfo weclipInfo = new WeClipInfo();
        public string From;
        public long SearchUserId, EventIdRequest;
        public string FriendNotification, FromESF, toNotification;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            try
            {
                SetContentView(Resource.Layout.Main);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                TextView toolbar_title = (TextView)FindViewById(Resource.Id.toolbar_title);
                SetSupportActionBar(toolbar);
                this.Title = "";

                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_white_24dp);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                From = Intent.GetStringExtra("FromEvent");
                EventIdRequest = Intent.GetLongExtra("EventRequest", 0);
                SearchUserId = Intent.GetLongExtra("FromPeople", 0);
                FriendNotification = Intent.GetStringExtra("FriendNotification");
                FromESF = Intent.GetStringExtra("FromESF");
                toNotification = Intent.GetStringExtra("ToNotification");

                btnEditProfile = (Button)FindViewById(Resource.Id.btnEditProfile);
                drawerLayout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
                navigationView = (NavigationView)FindViewById(Resource.Id.navigation_view);
                listDrawerMenu = (ListView)FindViewById(Resource.Id.listDrawerMenu);
                profile_pic = (ImageView)FindViewById(Resource.Id.profile_pic);
                tvUsername = (TextView)FindViewById(Resource.Id.tvUsername);
                profile_pic.Click += Profile_pic_Click;

                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                string strUserName = prefs.GetString("UserName", "");
                string strProfilePic = prefs.GetString("ProfilePic", "");
                tvUsername.Text = strUserName;

                if (string.IsNullOrEmpty(strProfilePic))
                {
                    profile_pic.SetImageResource(Resource.Drawable.contact_withoutphoto);
                }
                else
                {
                    Picasso.With(this).Load(strProfilePic)
                                 .Transform(new CircleTransformation()).Resize(240, 240)
                                   .CenterInside().Into(profile_pic);
                }

                List<DrawerItemModel> listDrawersItems = new List<DrawerItemModel>();
                listDrawersItems.Add(new DrawerItemModel("Home"));
                listDrawersItems.Add(new DrawerItemModel("Invite friends"));
                listDrawersItems.Add(new DrawerItemModel("Settings"));
                listDrawersItems.Add(new DrawerItemModel("Help"));
                //   listDrawersItems.Add(new DrawerItemModel("Change password"));
                listDrawersItems.Add(new DrawerItemModel("Logout"));
                listDrawerMenu.Adapter = new DrawerItemAdapter(this, listDrawersItems);
                listDrawerMenu.ItemClick += ListDrawerMenu_ItemClick;
                btnEditProfile.Click += BtnEditProfile_Click;

                if (EventIdRequest != 0)
                {
                    var eventRequest = new EventRequestFragment();
                    MainActivity.myBundle.PutLong("EventID", EventIdRequest);
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, eventRequest).Commit();
                }
                else if (!string.IsNullOrEmpty(FriendNotification))
                {
                    var notification = new NotificationFragment();
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, notification).Commit();
                }

                else if (!string.IsNullOrEmpty(toNotification))
                {
                    try
                    {
                        var notification = new NotificationFragment();
                        Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                        eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, notification).Commit();
                    }
                    catch (System.Exception ex)
                    {
                        throw;
                    }
                }

                else if (!string.IsNullOrEmpty(FromESF))
                {
                    var homeFragment = new EventFragment();
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, homeFragment).Commit();
                }

                else if (SearchUserId != 0)
                {
                    var profile = new ProfileFragment();
                    Bundle b = new Bundle();
                    long UserId = SearchUserId;
                    b.PutLong("UserId", UserId);
                    profile.Arguments = (b);
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, profile).Commit();
                }
                else if (string.IsNullOrEmpty(From))
                {
                    var homeFragment = new HomeFragment();
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, homeFragment, "home").Commit();
                }
                else
                {
                    var homeFragment = new EventFragment();
                    Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
                    eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, homeFragment, "event").AddToBackStack("event").Commit();
                }
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("MainActivity", "OnCreates", ex.Message + ex.StackTrace + ex.Cause + ex.InnerException).Execute();
            }

        }

        private void Profile_pic_Click(object sender, System.EventArgs e)
        {
            var profile = new ProfileFragment();
            Android.Support.V4.App.Fragment containerFragment = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);

            if (containerFragment.Class.Name.Equals(profile.Class.Name) && SearchUserId == 0)
            {
                drawerLayout.CloseDrawers();
                return;
            }
            Bundle b = new Bundle();
            long UserId = Convert.ToInt64(GlobalClass.UserID);
            b.PutLong("UserId", UserId);
            profile.Arguments = (b);
            Android.Support.V4.App.FragmentManager eventFragmentManager = SupportFragmentManager;
            eventFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, profile, "profile").AddToBackStack("profile").Commit();
            drawerLayout.CloseDrawers();
        }

        private void BtnEditProfile_Click(object sender, System.EventArgs e)
        {
            Android.Support.V4.App.Fragment editProfile = new EditProfile();
            Android.Support.V4.App.FragmentManager fragmentManager = SupportFragmentManager;
            fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, editProfile, "editProfile").AddToBackStack("EditProfile").Commit();
            drawerLayout.CloseDrawers();
        }

        private void ListDrawerMenu_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                SelectItem(e.Position);
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("MainActivity", "ListDrawerMenu_ItemClick", ex.StackTrace + ex.Message).Execute();
            }
        }

        private void SelectItem(int position)
        {
            Android.Support.V4.App.Fragment fragment = null;
            string tag = "";

            switch (position)
            {
                case 0:
                    fragment = new HomeFragment();
                    tag = "home";
                    break;
                case 1:
                    fragment = new InviteContacts();
                    tag = "invite";
                    break;
                case 2:
                    fragment = new SettingFragment();
                    tag = "setting";

                    break;
                //case 4:
                //    fragment = new ChangePasswordFragment();
                //    tag = "setting";

                //    break;
                case 4:
                    drawerLayout.CloseDrawers();
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alert.SetInverseBackgroundForced(true);
                    alert.SetTitle("Logout");
                    alert.SetMessage("Are you sure?");
                    alert.SetPositiveButton("YES", (senderAlert, args) =>
                    {
                        new LogOutFromWeClip(this).Execute();
                    });

                    alert.SetNegativeButton("No", (s, e) => { alert.Dispose(); });
                    Android.App.Dialog dialog = alert.Create();
                    dialog.Show();
                    break;
                default:
                    fragment = new InviteContacts();
                    break;
            }

            if (fragment != null)
            {
                Android.Support.V4.App.Fragment containerFragment = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);
                if (containerFragment.Class.Name.Equals(fragment.Class.Name))
                {
                    drawerLayout.CloseDrawers();
                    return;
                }

                Android.Support.V4.App.Fragment itemFragement = fragment;
                Android.Support.V4.App.FragmentManager fragmentManager = SupportFragmentManager;
                fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, itemFragement, tag).AddToBackStack("my_fragment").Commit();
                drawerLayout.CloseDrawers();
            }
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //e.MenuItem.SetChecked(true);
            drawerLayout.CloseDrawers();

            switch (e.MenuItem.ItemId)
            {
                default:
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    bool isopen = drawerLayout.IsDrawerOpen(GravityCompat.Start);
                    if (!isopen)
                        drawerLayout.OpenDrawer(GravityCompat.Start);
                    else
                        drawerLayout.CloseDrawers();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //var Fragment = SupportFragmentManager.FindFragmentByTag("editProfile");
            //Fragment.OnActivityResult(requestCode,(int)resultCode, data);
        }

        public int MyProperty { get; set; }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                var myFragment = SupportFragmentManager.FindFragmentByTag("home");
                if (myFragment != null && myFragment.IsVisible)
                {
                    this.Finish();
                }

                SupportFragmentManager.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private class LogOutFromWeClip : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private MainActivity mainActivity;
            private JsonResult jResult;

            public LogOutFromWeClip(MainActivity mainActivity)
            {
                this.mainActivity = mainActivity;
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jResult = RestSharpCall.Post<JsonResult>(null, "Account/Logout");
                return jResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                if (result != null)
                {
                    var settings = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    settings.Edit().Clear().Commit();
                    mainActivity.StartActivity(typeof(SplashActivity));
                    mainActivity.Finish();
                }
            }
        }
    }
}



