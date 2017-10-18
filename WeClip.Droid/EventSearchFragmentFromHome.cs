using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Droid.AsyncTask;
using Android.Support.V4.App;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Content;
using Android.Text;
using Java.Lang;

namespace WeClip.Droid
{
    [Activity(Label = "EventSearchFragmentFromHome")]
    public class EventSearchFragmentFromHome : Fragment
    {
        private ListView lvEvent;
        private View view;
        private ImageView btnBack;
        private EditText etTextEventSearch;
        private TextView tvNodata;

        public override void OnCreate(Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Select Event to Add Media Screen";
            toolbar_title.SetTextSize(Android.Util.ComplexUnitType.Dip, 16);
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.EventsSearchFragment, container, false);
            lvEvent = view.FindViewById<ListView>(Resource.Id.lvEventSearch);
            tvNodata = view.FindViewById<TextView>(Resource.Id.tvESFNodata);
            etTextEventSearch = view.FindViewById<EditText>(Resource.Id.etEventSearch);
            btnBack = view.FindViewById<ImageView>(Resource.Id.ivESFback);
            btnBack.Visibility = ViewStates.Visible;
            tvNodata.Visibility = ViewStates.Gone;
            btnBack.Click += BtnBack_Click;
            return view;
        }

        private void BtnBack_Click(object sender, System.EventArgs e)
        {
            Activity.OnBackPressed();
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                new getAllEventDetails(Activity, lvEvent, etTextEventSearch, tvNodata).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("EventSearchFragmentFromHome", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        private class getAllEventDetails : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventSearchData>>
        {
            List<EventSearchData> eventData;
            ProgressDialog p;
            private FragmentActivity activity;
            private ListView lvEvent;
            private EditText etTextEventSearch;
            private EventSearchAdapter objAdapter;
            private TextView tvNodata;

            public getAllEventDetails(FragmentActivity activity, ListView lvEvent, EditText etTextEventSearch, TextView tvNodata)
            {
                this.tvNodata = tvNodata;
                this.activity = activity;
                this.lvEvent = lvEvent;
                this.etTextEventSearch = etTextEventSearch;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait");
            }

            protected override List<EventSearchData> RunInBackground(params Java.Lang.Void[] @params)
            {
                eventData = RestSharpCall.GetList<EventSearchData>("Event/GetAllEventSearchDetails?query=empty_string");
                return eventData;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null && eventData != null)
                {
                    tvNodata.Visibility = ViewStates.Gone;
                    objAdapter = new EventSearchAdapter(activity, eventData);
                    lvEvent.Adapter = objAdapter;
                    etTextEventSearch.AddTextChangedListener(new textwatcher(this));
                    lvEvent.TextFilterEnabled = true;
                    lvEvent.OnItemClickListener = new lvItemClickListner(activity, eventData);
                    if (eventData.Count == 0)
                    {
                        tvNodata.Visibility = ViewStates.Visible;
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
                    Intent intent = new Intent(activity, typeof(AddImageAndVideoForEventActivity));
                    intent.PutExtra("strCapture", eventData[position].EventId);
                    intent.PutExtra("strIsDefaultEvent", false);
                    activity.StartActivity(intent);
                }
            }

            private class textwatcher : Java.Lang.Object, ITextWatcher
            {
                private getAllEventDetails eventSearchData;

                public textwatcher(getAllEventDetails eventSearchData)
                {
                    this.eventSearchData = eventSearchData;
                }

                public void AfterTextChanged(IEditable s)
                {

                }

                public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
                {
                }

                public void OnTextChanged(ICharSequence s, int start, int before, int count)
                {
                    string text = eventSearchData.etTextEventSearch.Text.ToString()
                               .ToLower();
                    eventSearchData.objAdapter.filter(text);
                }
            }
        }
    }
}