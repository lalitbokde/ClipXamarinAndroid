using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace WeClip.Droid.AsyncTask
{
    public class PhotosFragmentLoadData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventFiles>>
    {
        private GridView gvEventPhotos;
        private Activity activity;
        private List<EventFiles> eventFile;
        View view;
        long eventID;
        private TextView EmptyText;

        public PhotosFragmentLoadData(Activity activity, GridView gvEventPhotos, View view, long eventID)
        {
            this.activity = activity;
            this.gvEventPhotos = gvEventPhotos;
            eventFile = new List<EventFiles>();
            this.view = view;
            this.eventID = eventID;
        }

        protected override List<EventFiles> RunInBackground(params Java.Lang.Void[] @params)
        {
            eventFile = RestSharpCall.GetList<EventFiles>("Event/GetEventFiles?eventid=" + eventID);
            return eventFile;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            EmptyText = view.FindViewById<TextView>(Resource.Id.emptyPhoto);
            if (result != null && eventFile.Count > 0)
            {
                EmptyText.Visibility = ViewStates.Gone;
                var adapter = new PhotosFragmentAdapter(activity, eventFile);
                gvEventPhotos.Adapter = adapter;
                gvEventPhotos.OnItemClickListener = (new grvOnItemClickListner(activity, eventFile, eventFile.Count));
                gvEventPhotos.SetMultiChoiceModeListener(adapter);
            }

            else
            {
                EmptyText.Visibility = ViewStates.Visible;
            }
        }

        private class grvOnItemClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private Activity activity;
            private int count;
            private List<EventFiles> evFileInfo;

            public grvOnItemClickListner(Activity activity, List<EventFiles> evFileInfo, int count)
            {
                this.activity = activity;
                this.evFileInfo = evFileInfo;
                this.count = count;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                Intent intent = new Intent(activity, typeof(PWSliderActivity));
                intent.PutExtra("FromPhotoFragment", JsonConvert.SerializeObject(evFileInfo));
                intent.PutExtra("FragmentPosition", position);
                activity.StartActivity(intent);
            }
        }

        private class multichoiceModeClickListner : Java.Lang.Object, AbsListView.IMultiChoiceModeListener
        {
            private Activity activity;
            private List<EventFiles> eventFile;
            private IMenuItem menuItem_;
            private ActionMode mode;
            private int NoOfFile;
            private List<EventFiles> tempFile;

            public multichoiceModeClickListner(Activity activity, List<EventFiles> eventFile)
            {
                this.activity = activity;
                this.eventFile = eventFile;
                tempFile = new List<EventFiles>();
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                AlertBox.Create("Delete", "Are you sure", activity);
                return true;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                mode.MenuInflater.Inflate(Resource.Menu.DeleteMenu, menu);
                this.mode = mode;
                menuItem_ = menu.FindItem(Resource.Id.action_delete);
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
            }

            public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
            {
                var evFile = eventFile[position];

                if (@checked == true)
                {
                    tempFile.Add(evFile);
                    NoOfFile++;
                }
                else
                {
                    tempFile.Remove(evFile);
                    NoOfFile--;
                }

                int selectCount = tempFile.Count;

                if (selectCount == 0)
                {
                    mode.Finish();
                    return;
                }
                mode.Title = NoOfFile + " item selected";
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return true;
            }


        }
    }
}