using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using WeClip.Droid.Adapters;
using WeClip.Droid.Helper;
using static Android.Views.View;

namespace WeClip.Droid.AsyncTask
{
    public class GetEventFile : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<EventFiles>>
    {
        GridView grv_view;
        Activity context;
        List<EventFiles> eventfile;
        private TextView noOfImage;
        private long eventID;
        ProgressDialog p;

        public GetEventFile(GridView grv_view, Activity context, TextView noOfImage, long eventID)
        {
            this.noOfImage = noOfImage;
            eventfile = new List<EventFiles>();
            this.context = context;
            this.grv_view = grv_view;
            this.eventID = eventID;
            p = ProgressDialog.Show(context, "", "Please wait");
        }

        protected override List<EventFiles> RunInBackground(params Java.Lang.Void[] @params)
        {
            eventfile = RestSharpCall.GetList<EventFiles>("Event/GetEventFiles?eventid=" + eventID);
            return eventfile;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (eventfile.Count > 0)
            {
                ListOfEventFiles.listOfEventFiles.Clear();
                try
                {
                    foreach (var x in eventfile)
                    {
                        ListOfEventFiles.listOfEventFiles.Add(x);
                    }

                    grv_view.Adapter = new EventGalleryAdapter(context, eventfile);
                    grv_view.OnItemClickListener = new EventPhotoClickListener(context, eventfile, noOfImage);
                }
                catch (System.Exception ex)
                {
                    new CrashReportAsync("GetEventFile", "OnPostExecute", ex.Message + ex.StackTrace).Execute();
                }
            }
        }

        private class EventPhotoClickListener : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private Activity context;
            private List<EventFiles> eventfile;
            private TextView etTitle;
            int NoofImage = 0;
            int NoofVideo = 0;

            public EventPhotoClickListener(Activity context, List<EventFiles> eventfile, TextView etTitle)
            {
                this.etTitle = etTitle;
                this.context = context;
                this.eventfile = eventfile;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                ImageView icRightBtn = view.FindViewById<ImageView>(Resource.Id.ivWeClipItem_icRight);
                EventFiles model = ListOfEventFiles.listOfEventFiles[position];
                if (model.isSelected())
                {
                    if (model.IsVideo)
                    {
                        NoofVideo--;
                    }
                    else
                    {
                        NoofImage--;
                    }

                    model.setSelected(false);
                    icRightBtn.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (model.IsVideo)
                    {
                        NoofVideo++;
                    }
                    else
                    {
                        NoofImage++;
                    }
                    model.setSelected(true);
                    icRightBtn.Visibility = ViewStates.Visible;
                }
                etTitle.Text = NoofVideo + " Videos |" + " " + NoofImage + " Images";
            }
        }

        public class ListOfEventFiles
        {
            public static List<EventFiles> listOfEventFiles = new List<EventFiles>();
        }

        //private void setListViewHeightBasedOnChildren(GridView listView)
        //{
        //    var listAdapter = listView.Adapter;
        //    if (listAdapter == null)
        //        return;

        //    int desiredWidth = MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
        //    int totalHeight = 0;
        //    View view = null;
        //    var item = listAdapter.Count / 3;
        //    for (int i = 0; i < item + 1; i++)
        //    {
        //        view = listAdapter.GetView(i, view, listView);
        //        if (i == 0)
        //            view.LayoutParameters = (new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.WrapContent));

        //        view.Measure(desiredWidth, (int)MeasureSpecMode.Unspecified);
        //        totalHeight = view.MeasuredHeight;
        //    }
        //    ViewGroup.LayoutParams params1 = listView.LayoutParameters;
        //    params1.Height = 175 * (listAdapter.Count / 3 + 1) + 100;
        //    listView.LayoutParameters = (params1);
        //}
    }
}