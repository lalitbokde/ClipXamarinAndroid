using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using SearchView = Android.Support.V7.Widget.SearchView;
using WeClip.Droid.AsyncTask;
using Android.Support.V4.App;
using WeClip.Core.Model;
using Android.Content;
using static Android.Views.View;
using Android.Runtime;

namespace WeClip.Droid
{
    [Activity(Label = "PeoplesFragment")]
    public class PeoplesFragment : Fragment, SearchView.IOnQueryTextListener
    {
        long EventID;
        private View view;
        private ListView lvAllUser;
        SearchView searchView;
        private string query = "";
        TextView emptyText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EventID = MainActivity.myBundle.GetLong("EventID");
            // Create your application here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            // toolbar_title.Text = "Search";
            view = inflater.Inflate(Resource.Layout.PeopleFragment, container, false);
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                this.HasOptionsMenu = true;
                lvAllUser = view.FindViewById<ListView>(Resource.Id.lvAllUser);
                lvAllUser.ChoiceMode = ChoiceMode.Single;

                emptyText = view.FindViewById<TextView>(Resource.Id.emptyPeople);

                var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
                query = prefs.GetString("key", null);

                if (query != null)
                {
                    new getAllUserData(Activity, lvAllUser, query, emptyText,searchView).Execute();
                }
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("InviteContacts", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {

            inflater.Inflate(Resource.Menu.menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.action_search);
            var searchItem = Android.Support.V4.View.MenuItemCompat.GetActionView(item);
            searchView = searchItem.JavaCast<SearchView>();
            searchView.QueryHint = "Search People";
            searchView.Focusable = (true);
            
            searchView.Iconified = (false);
            searchView.SetQuery(query, false);
            searchView.RequestFocusFromTouch();
            searchView.SetOnQueryTextListener(this);
            if (query != null)
            {
                searchView.ClearFocus();
            }
        }

        public bool OnQueryTextChange(string newText)
        {
            return false;
        }

        public bool OnQueryTextSubmit(string query)
        {
            new getAllUserData(Activity, lvAllUser, query, emptyText, searchView).Execute();
            searchView.ClearFocus();
            var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.Clear();
            prefEditor.PutString("key", query);
            prefEditor.Commit();
            return true;
        }

        private class getAllUserData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<SPGetSearchUserList_Result>>
        {
            private FragmentActivity activity;
            private ListView lvAllUser;
            private string query;
            private List<SPGetSearchUserList_Result> userdata;
            ProgressDialog p;
            private TextView emptyText;
            private SearchView searchView;

            public getAllUserData(FragmentActivity activity, ListView lvAllUser, string query, TextView emptyText, SearchView searchView) 
            {
                this.searchView = searchView;
                this.emptyText = emptyText;
                this.activity = activity;
                this.lvAllUser = lvAllUser;
                this.query = query;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();

                if(emptyText != null)
                {
                    emptyText.Visibility = ViewStates.Gone;
                }
                p = ProgressDialog.Show(activity, "Please wait", "");
            }

            protected override List<SPGetSearchUserList_Result> RunInBackground(params Java.Lang.Void[] @params)
            {
                userdata = Helper.RestSharpCall.GetList<SPGetSearchUserList_Result>("User/GetAllUser?query=" + query);
                return userdata;
            }
            protected override void OnPostExecute(Java.Lang.Object result)
            {
                try
                {
                    base.OnPostExecute(result);
                    if (searchView != null)
                    {
                        searchView.ClearFocus();
                    }
                    p.Dismiss();
                    if (result != null && userdata != null)
                    {
                        var adapter = new Adapters.PeopleDataAdapter(userdata, activity);
                        if (userdata.Count > 0)
                        {
                            lvAllUser.Adapter = adapter;
                            //    setListViewHeightBasedOnChildren(lvAllUser);
                            lvAllUser.OnItemClickListener = new lvItemClickListner(userdata, activity);
                        }
                        else
                        {
                            lvAllUser.Adapter = adapter;
                            emptyText.Visibility = ViewStates.Visible;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    new CrashReportAsync("PeoplesFragment", "OnPostExecute", ex.Message + ex.StackTrace).Execute();
                }
            }

            private void setListViewHeightBasedOnChildren(ListView listView)
            {
                var listAdapter = listView.Adapter;
                if (listAdapter == null)
                    return;

                int desiredWidth = MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
                int totalHeight = 0;
                View view = null;
                for (int i = 0; i < listAdapter.Count; i++)
                {
                    view = listAdapter.GetView(i, view, listView);
                    if (i == 0)
                        view.LayoutParameters = (new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.WrapContent));

                    view.Measure(desiredWidth, (int)MeasureSpecMode.Unspecified);
                    totalHeight += view.MeasuredHeight;
                }
                ViewGroup.LayoutParams params1 = listView.LayoutParameters;
                params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
                listView.LayoutParameters = (params1);
            }

            private class lvItemClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
            {
                private FragmentActivity activity;
                private List<SPGetSearchUserList_Result> userdata;

                public lvItemClickListner(List<SPGetSearchUserList_Result> userdata, FragmentActivity activity)
                {
                    this.userdata = userdata;
                    this.activity = activity;
                }

                public void OnItemClick(AdapterView parent, View view, int position, long id)
                {
                    var item = userdata[position];
                    Intent mainActivity = new Intent(activity, typeof(MainActivity));
                    mainActivity.PutExtra("FromPeople", item.UserId);
                    activity.StartActivity(mainActivity);
                }
            }
        }
    }
}