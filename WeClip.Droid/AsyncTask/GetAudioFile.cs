using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.App;
using Android.Graphics;

namespace WeClip.Droid.AsyncTask
{
    public class GetAudioFile : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<Audio>>
    {
        private GridView gvMusic;
        private MusicView activity;
        List<Audio> audioList;
        private WeClipInfo wcHolder;
        ProgressDialog p;

        public GetAudioFile(MusicView activity, GridView gvMusic, WeClipInfo wcHolder)
        {
            this.activity = activity;
            this.gvMusic = gvMusic;
            this.wcHolder = wcHolder;
            audioList = new List<Audio>();
            p = ProgressDialog.Show(activity, "Please wait", "Loading data");
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
        }

        protected override List<Audio> RunInBackground(params Java.Lang.Void[] @params)
        {
            audioList = RestSharpCall.GetList<Audio>("Event/GetAudios");
            return audioList;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (audioList != null)
            {
                gvMusic.Adapter = new MusicViewAdapter(audioList, activity);
                gvMusic.OnItemClickListener = new grvOnClickListner(activity, wcHolder, audioList, gvMusic);
            }
        }

        private class grvOnClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private MusicView activity;
            private List<Audio> audioList;
            private GridView gvMusic;
            private WeClipInfo wcHolder;
            private int backposition = -1;

            public grvOnClickListner(MusicView activity, WeClipInfo wcHolder, List<Audio> audioList, GridView gvMusic)
            {
                this.gvMusic = gvMusic;
                this.activity = activity;
                this.audioList = audioList;
                this.wcHolder = wcHolder;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                wcHolder.AudioId = audioList[position].ID;

                var selectedItem = parent.GetItemAtPosition(position).ToString();
                var GridViewItems = (LinearLayout)view;
                GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                var BackSelectedItem = (LinearLayout)gvMusic.GetChildAt(backposition);
                if (backposition != -1)
                {

                    BackSelectedItem.Selected = (false);
                    BackSelectedItem.SetBackgroundColor(Color.Transparent);
                }

                if (backposition == position)
                {
                    GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                }

                backposition = position;

            }
        }
    }
}