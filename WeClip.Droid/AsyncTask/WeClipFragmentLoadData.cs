using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;

namespace WeClip.Droid.AsyncTask
{
    class WeClipFragmentLoadData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<WeClipVideo>>
    {
        private FragmentActivity activity;
        private GridView gvWeClip;
        private View view;
        long eventId;
        private TextView EmptyText;
        List<WeClipVideo> weclipFile;

        public WeClipFragmentLoadData(FragmentActivity activity, GridView gvWeClip, View view, long eventId)
        {
            this.activity = activity;
            this.gvWeClip = gvWeClip;
            this.view = view;
            this.eventId = eventId;
            weclipFile = new List<WeClipVideo>();
        }

        protected override List<WeClipVideo> RunInBackground(params Java.Lang.Void[] @params)
        {
            weclipFile = RestSharpCall.GetList<WeClipVideo>("Event/GetWeClipVideo?eventid=" + eventId);
            return weclipFile;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            EmptyText = view.FindViewById<TextView>(Resource.Id.emptyWeClip);
            if (result != null && weclipFile.Count > 0)
            {
                var adapter = new WeClipFragmentAdapter(activity, weclipFile);
                gvWeClip.Adapter = adapter;
            }
            else
            {
                EmptyText.Visibility = ViewStates.Visible;
            }
        }
    }
}