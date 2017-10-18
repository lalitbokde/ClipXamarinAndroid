using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.AsyncTask;
using Android.Support.V4.View;

namespace WeClip.Droid
{
    public class WeClipsFragment : Fragment
    {
        private GridView gvWeClip;
        private View view;
        long eventID;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            eventID = MainActivity.myBundle.GetLong("EventID");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            view = inflater.Inflate(Resource.Layout.WeClipsFragment, container, false);
            gvWeClip = view.FindViewById<GridView>(Resource.Id.gvWeClip);
            ViewCompat.SetNestedScrollingEnabled(gvWeClip, true);
            return view;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                new WeClipFragmentLoadData(Activity, gvWeClip, view, eventID).Execute();
            }
            catch (Exception ex)
            {
                new CrashReportAsync("WeClipsFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }
    }
}