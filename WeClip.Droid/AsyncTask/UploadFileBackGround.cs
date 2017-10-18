using Android.App;
using Android.OS;
using WeClip.Core.Common;
using WeClip.Core.Model;
using RestSharp;
using Newtonsoft.Json;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    public class UploadFileBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity activity;
        JsonResult jsonResult;
        private string uploadImagePath;
        private string compressImage;
        private ProgressDialog progressDialog;
        EventModel model;

        public UploadFileBackGround(Activity activity, EventModel model, string uploadImagePath)
        {
            this.activity = activity;
            this.uploadImagePath = uploadImagePath;
            this.model = model;
            progressDialog = ProgressDialog.Show(activity, "Uploading Data", "Please wait...");
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            jsonResult = new JsonResult();
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            var client = new RestClient(GlobalClass.BaseUrl + "Event/UploadPicture");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "bearer " + GlobalClass.AccessToken);

            Java.IO.File file = new Java.IO.File(uploadImagePath);
            long length = file.Length() / 1024; // Size in KB

            if (length > 500)
            {
                compressImage = CompressFile.compressImage(uploadImagePath);
            }
            else
            {
                compressImage = uploadImagePath;
            }

            request.AddFile("file", compressImage, null);
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
            if (result != null && jsonResult != null)
            {
                if (jsonResult.Success == true)
                {
                    model.EventPic = jsonResult.ImageName;
                    var createEventInBackGround = new CreateEventInBackGround(model, activity, progressDialog);
                    createEventInBackGround.Execute();
                }
            }
            else
            {
                progressDialog.Hide();
            }
        }
    }
}