using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using WeClip.Core.Common;
using WeClip.Droid.Adapters;
using WeClip.Droid.Helper;
using Android.Widget;

namespace WeClip.Droid.AsyncTask
{
    public class PostDeleteEventFiles : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity context;
        private ActionMode mode;
        private List<EventFiles> tempFile;
        JsonResult jResult;
        ProgressDialog p;
        private PhotosFragmentAdapter photosFragmentAdapter;
        private List<EventFiles> eventfile;

        public PostDeleteEventFiles(Activity context, ActionMode mode, List<EventFiles> tempFile, List<EventFiles> eventfile, PhotosFragmentAdapter photosFragmentAdapter)
        {
            this.context = context;
            this.mode = mode;
            this.tempFile = tempFile;
            this.eventfile = eventfile;
            this.photosFragmentAdapter = photosFragmentAdapter;
            p = ProgressDialog.Show(context, "", "Please wait");
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            jResult = RestSharpCall.Post<JsonResult>(tempFile, "Event/DeleteEventPicture");
            return jResult;
        }

        protected override void OnPostExecute(JsonResult result)
        {
            base.OnPostExecute(result);
            mode.Finish();
            if (mode != null)
            {
                mode.Dispose();
            }

            p.Dismiss();

            if (result.Success == true)
            {
                foreach (var x in tempFile)
                {
                    eventfile.Remove(x);
                }
                photosFragmentAdapter.NotifyDataSetChanged();
                Toast.MakeText(context, tempFile.Count + " file deleted", ToastLength.Short).Show();
            }
            else
            {
                AlertBox.Create("Error", result.Message, context);
            }
        }
    }
}