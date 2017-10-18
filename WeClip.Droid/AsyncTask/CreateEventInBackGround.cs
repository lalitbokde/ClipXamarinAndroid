using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Android.Content;

namespace WeClip.Droid.AsyncTask
{
    public class CreateEventInBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity activity;
        private EventModel model;
        private JsonResult jResult;
        private ProgressDialog progressDialog;

        public CreateEventInBackGround(EventModel model, Activity activity, ProgressDialog progressDialog)
        {
            this.progressDialog = progressDialog;
            this.model = model;
            this.activity = activity;
            jResult = new JsonResult();
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            if (model.EventID == 0)
            {
                jResult = RestSharpCall.Post<JsonResult>(model, "Event/Create");
            }
            else
            {
                var m = new Event
                {
                    ID = model.EventID,
                    EventName = model.EventName,
                    EventDescription = model.EventDescription,
                    EventTag = model.EventTag,
                    EventStartTime = model.EventStartTime,
                    EventPic = model.EventPic,
                    EventCategory = model.EventCategory,
                    EventDate = model.EventDate,
                    EventLocation = model.EventLocation,
                    UserID = model.UserID,
                    Address = model.Address,
                    EventSubCategoryID = 0
                };
                jResult = RestSharpCall.Post<JsonResult>(m, "Event/Update?id=" + model.EventID);
            }
            return jResult;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            progressDialog.Hide();
            if (result != null && jResult != null)
            {
                if (jResult.Success == true)
                {
                    if (model.EventID == 0)
                    {
                        Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(activity);
                        builder.SetTitle("Success");
                        builder.SetMessage("Event has been created.");

                        builder.SetPositiveButton("OK", (senderAlert, args) =>
                        {
                            activity.Finish();
                            Intent intent = new Intent(activity, typeof(MainActivity));
                            activity.StartActivity(intent);
                        });
                        Dialog dialog = builder.Create();
                        dialog.Show();
                    }
                    else
                    {
                        Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(activity);
                        builder.SetTitle("Success");
                        builder.SetMessage("Event has been updated");

                        builder.SetPositiveButton("OK", (senderAlert, args) =>
                        {
                            activity.Finish();
                        });
                        Dialog dialog = builder.Create();
                        dialog.Show();
                    }
                }
            }
            else
            {
                Toast.MakeText(activity, "Error.", ToastLength.Long).Show();
            }
        }
    }
}