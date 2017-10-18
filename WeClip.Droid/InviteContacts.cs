using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.AsyncTask;
using Android.Content;

namespace WeClip.Droid
{
    public class InviteContacts : Fragment
    {
        private View rootView;
        private ListView lvContact;
        private EditText txtSearch;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Invite your contacts";
            rootView = inflater.Inflate(Resource.Layout.InviteContactView, container, false);
            return rootView;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                this.HasOptionsMenu = true;
                lvContact = rootView.FindViewById<ListView>(Resource.Id.lvContact);
                txtSearch = rootView.FindViewById<EditText>(Resource.Id.etContactSearch);
                var setadapter = new GetContacts(lvContact, Activity, txtSearch);
                setadapter.Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("InviteContacts", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.InviteContactShareMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_shareWeClipApp:
                    Intent intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Android.Content.Intent.ExtraText, "Download weclip from http://www.weclip.com");
                    this.StartActivity(Intent.CreateChooser(intent, "Share To"));
                    return true;
            }
            return false;
        }
    }
}
