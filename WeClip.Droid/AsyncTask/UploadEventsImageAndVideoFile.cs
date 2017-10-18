using System.Collections.Generic;
using Android.App;
using Android.OS;
using WeClip.Core.Common;
using RestSharp;
using Newtonsoft.Json;
using Builder = Android.Support.V7.App.AlertDialog.Builder;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    public class UploadEventsImageAndVideoFile : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity activity;
        private List<string> uploadImagePath;
        ProgressDialog progressDialog;
        private JsonResult jsonResult;
        private int typeOfFile;
        private long EventID;
        string[] Compressfile;

        public UploadEventsImageAndVideoFile(Activity activity, List<string> uploadImagePath, int typeOfFile, long EventID)
        {
            this.activity = activity;
            this.uploadImagePath = uploadImagePath;
            this.typeOfFile = typeOfFile;
            this.EventID = EventID;
            jsonResult = new JsonResult();
            progressDialog = ProgressDialog.Show(activity, "Uploading Data", "Please wait...");
            Compressfile = new string[uploadImagePath.Count];
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            RestClient client;
            if (typeOfFile == 0)
            {
                client = new RestClient(GlobalClass.BaseUrl + "Event/UploadEventFiles?eventid=" + EventID);
            }
            else
            {
                client = new RestClient(GlobalClass.BaseUrl + "Event/UploadVideo?eventid=" + EventID);
            }
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "bearer " + GlobalClass.AccessToken);
            for (int i = 0; i < uploadImagePath.Count; i++)
            {
                if (typeOfFile == 0)
                {
                    Compressfile[i] = CompressFile.compressImage(uploadImagePath[i]);
                    request.AddFile("file", Compressfile[i], null);
                }
                else
                {
                    request.AddFile("file", uploadImagePath[i], null);
                }
            }
            var response = client.Execute<JsonResult>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                jsonResult = JsonConvert.DeserializeObject<JsonResult>(response.Content);
                return jsonResult;
            }
            else
            {
                return jsonResult;
            }
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            progressDialog.Hide();

            if (result != null && jsonResult != null)
            {
                if (jsonResult.Success == true)
                {
                    Builder builder = new Builder(activity);
                    builder.SetTitle("Success");
                    builder.SetMessage("File has been uploaded.");
                    builder.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/" + "TMMFOLDER");
                        if (dir.IsDirectory)
                        {
                            string[] children = dir.List();
                            for (int ff = 0; ff < children.Length; ff++)
                            {
                                new Java.IO.File(dir, children[ff]).Delete();
                            }
                        }
                        activity.Finish();
                    });
                    Dialog dialog = builder.Create();
                    dialog.Show();
                }
            }
            else
            {
                AlertBox.Create("Error", "Error occurred", activity);
            }
        }
    }
}