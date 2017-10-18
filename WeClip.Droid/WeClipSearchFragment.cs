using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using SearchView = Android.Support.V7.Widget.SearchView;
using WeClip.Droid.AsyncTask;
using Android.Runtime;
using Android.Support.V4.App;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using System.Collections.Generic;
using WeClip.Droid.Adapters;
using Android.App;

namespace WeClip.Droid
{
    public class WeClipSearchFragment : Fragment, SearchView.IOnQueryTextListener
    {
        private GridView gvWeClip;
        private View view;
        SearchView searchView;
        private string query = "";
        TextView emptyText;
        IMenuItem item;

        public override void OnCreate(Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            //  toolbar_title.Text = "Search";
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.WeClipSearchFragment, container, false);
            gvWeClip = view.FindViewById<GridView>(Resource.Id.gvWeClipSearch);
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                emptyText = view.FindViewById<TextView>(Resource.Id.emptyWeClip);
                this.HasOptionsMenu = true;
                var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
                query = prefs.GetString("key", null);

                if(searchView != null)
                {

                }

                if(item != null)
                {

                }

                if (query != null)
                {
                    new WeClipSearchFragmentLoadData(Activity, gvWeClip, query, emptyText,searchView).Execute();
                }

            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("WeClipSearchFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu, menu);
            item = menu.FindItem(Resource.Id.action_search);
            var searchItem = Android.Support.V4.View.MenuItemCompat.GetActionView(item);
            searchView = searchItem.JavaCast<SearchView>();
            searchView.QueryHint = "Search WeClip";
            searchView.Focusable = (false);
            searchView.SetQuery(query, false);
            searchView.Iconified = (false);
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
            new WeClipSearchFragmentLoadData(Activity, gvWeClip, query, emptyText,searchView).Execute();
            var prefs = Application.Context.GetSharedPreferences("SearchData", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.Clear();
            prefEditor.PutString("key", query);
            prefEditor.Commit();
            return true;
        }

        private class WeClipSearchFragmentLoadData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<SPGetSearchWeclipList_Result>>
        {
            private FragmentActivity activity;
            private GridView gvWeClip;
            private string query;
            private ProgressDialog p;

            List<SPGetSearchWeclipList_Result> weClipdata;
            private TextView emptyText;
            private SearchView searchView;

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait");

                if (emptyText != null)
                {
                    emptyText.Visibility = ViewStates.Gone;
                }
            }

            public WeClipSearchFragmentLoadData(FragmentActivity activity, GridView gvWeClip, string query, TextView emptyText, SearchView searchView)
            {
                this.searchView = searchView;
                this.emptyText = emptyText;
                this.activity = activity;
                this.gvWeClip = gvWeClip;
                this.query = query;
            }

            protected override List<SPGetSearchWeclipList_Result> RunInBackground(params Java.Lang.Void[] @params)
            {
                weClipdata = RestSharpCall.GetList<SPGetSearchWeclipList_Result>("Event/GetAllWeClip?query=" + query);
                return weClipdata;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);

                if(searchView != null)
                {
                    searchView.ClearFocus();
                }
                p.Dismiss();
                if (result != null && weClipdata != null)
                {
                    var adapter = new WeClipSearchAdapter(activity, weClipdata);

                    if (weClipdata.Count > 0)
                    {
                        gvWeClip.Adapter = adapter;
                    }
                    else
                    {
                        gvWeClip.Adapter = adapter;
                        emptyText.Visibility = ViewStates.Visible;
                    }
                }
            }
        }
    }
}
