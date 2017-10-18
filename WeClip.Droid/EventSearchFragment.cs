using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using SearchView = Android.Support.V7.Widget.SearchView;
using WeClip.Droid.AsyncTask;
using Android.Support.V4.App;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Content;

namespace WeClip.Droid
{
    public class EventSearchFragment : Fragment, SearchView.IOnQueryTextListener
    {
        private ListView lvEvent;
        private View view;
        SearchView searchView;
        private EditText etTextEventSearch;
        private string query = "";
        private TextView emptyText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.EventsSearchFragment, container, false);
            lvEvent = view.FindViewById<ListView>(Resource.Id.lvEventSearch);
            etTextEventSearch = view.FindViewById<EditText>(Resource.Id.etEventSearch);
            etTextEventSearch.Visibility = ViewStates.Gone;
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                this.HasOptionsMenu = true;
                emptyText = view.FindViewById<TextView>(Resource.Id.emptyEvent);
                var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
                query = prefs.GetString("key", null);

                if (query != null)
                {
                    new getAllEvent(Activity, lvEvent, query, emptyText, searchView).Execute();
                }
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventSearchFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu, menu);
            IMenuItem item = menu.FindItem(Resource.Id.action_search);
            var searchItem = Android.Support.V4.View.MenuItemCompat.GetActionView(item);
            searchView = searchItem.JavaCast<SearchView>();
            searchView.QueryHint = "Select Event to Add Media Screen";
            searchView.Focusable = (false);
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
            new getAllEvent(Activity, lvEvent, query, emptyText, searchView).Execute();

            var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.Clear();
            prefEditor.PutString("key", query);
            prefEditor.Commit();
            return true;
        }

        private class getAllEvent : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventSearchData>>
        {
            private FragmentActivity activity;
            private ListView lvEventSearch;
            private string query;
            List<EventSearchData> eventData;
            ProgressDialog p;
            private TextView emptyText;
            private SearchView searchView;



            public getAllEvent(FragmentActivity activity, ListView lvEventSearch, string query, TextView emptyText, SearchView searchView)
            {
                this.searchView = searchView;
                this.emptyText = emptyText;
                this.activity = activity;
                this.lvEventSearch = lvEventSearch;
                this.query = query;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();


                emptyText.Visibility = ViewStates.Gone;
                p = ProgressDialog.Show(activity, "", "Please wait");
            }

            protected override List<EventSearchData> RunInBackground(params Java.Lang.Void[] @params)
            {
                eventData = RestSharpCall.GetList<EventSearchData>("Event/GetAllEvent?query=" + query);
                return eventData;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                if (searchView != null)
                {
                    searchView.ClearFocus();
                }
                p.Dismiss();
                if (result != null && eventData != null)
                {
                    var adapter = new EventSearchAdapter(activity, eventData);

                    if (eventData.Count > 0)
                    {
                        lvEventSearch.Adapter = adapter;
                        lvEventSearch.OnItemClickListener = new lvItemClickListner(activity, eventData);
                    }
                    else
                    {
                        lvEventSearch.Adapter = adapter;
                        emptyText.Visibility = ViewStates.Visible;
                    }
                }
            }

            private class lvItemClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
            {
                private FragmentActivity activity;
                private List<EventSearchData> eventData;

                public lvItemClickListner(FragmentActivity activity, List<EventSearchData> eventData)
                {
                    this.activity = activity;
                    this.eventData = eventData;
                }

                public void OnItemClick(AdapterView parent, View view, int position, long id)
                {
                    Intent intent = new Intent(activity, typeof(MainActivity));
                    intent.PutExtra("FromESF", "FromESF");
                    MainActivity.myBundle.PutLong("EventID", eventData[position].EventId);
                    activity.StartActivity(intent);
                }
            }
        }
    }
}