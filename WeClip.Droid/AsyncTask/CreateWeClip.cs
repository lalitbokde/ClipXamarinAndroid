using Android.App;
using Android.OS;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using Builder = Android.Support.V7.App.AlertDialog.Builder;

namespace WeClip.Droid.AsyncTask
{
    public class CreateWeClip : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private PostWeClip postWeClip;
        private WeClipInfo wcHolder;
        private JsonResult jResult;
        private ProgressDialog p;
        public CreateWeClip(PostWeClip postWeClip, WeClipInfo wcHolder)
        {
            this.postWeClip = postWeClip;
            this.wcHolder = wcHolder;
            p = ProgressDialog.Show(postWeClip, "Please wait..", "Uploading data");
            jResult = new JsonResult();
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            jResult = RestSharpCall.Post<JsonResult>(wcHolder, "Event/CreateWeClipVideo?eventid=" + wcHolder.EventID);
            return jResult;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (result != null)
            {
                if (jResult != null)
                {
                    if (jResult.Success)
                    {
                        Builder builder = new Builder(this.postWeClip);
                        builder.SetTitle("Success");
                        builder.SetMessage("We Clip is creating, Ones done we will notify you.");
                        builder.SetPositiveButton("OK", (senderAlert, args) =>
                        {
                            postWeClip.Finish();
                        });
                        Dialog dialog = builder.Create();
                        dialog.Show();
                    }

                    if (jResult.Success == false)
                    {
                        AlertBox.Create("Message", jResult.Message, postWeClip);
                    }
                }
            }
        }
    }
}
