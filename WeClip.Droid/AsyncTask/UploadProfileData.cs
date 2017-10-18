using Android.App;
using Android.Content;
using Android.OS;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Android.Widget;

namespace WeClip.Droid.AsyncTask
{
    class UploadProfileData : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private UserProfile objUP;
        private ProgressDialog progressDialog;
        JsonResult jsonResult;
        Activity activity;
        private TextView drawerUsername;

        public UploadProfileData(ProgressDialog progressDialog, UserProfile objUP, Activity activity, TextView drawerUsername)
        {
            this.drawerUsername = drawerUsername;
            this.progressDialog = progressDialog;
            this.objUP = objUP;
            this.activity = activity;
            jsonResult = new JsonResult();
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {
            jsonResult = RestSharpCall.Post<JsonResult>(objUP, "User/SetProfile");
            return jsonResult;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            if (result != null && jsonResult != null)
            {
                if (jsonResult.Success == true)
                {
                    AlertBox.Create("Success", "Data updated", activity);
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("UserName", objUP.UserName);
                    prefEditor.PutString("Bio", objUP.Bio);
                    prefEditor.Commit();
                    drawerUsername.Text = objUP.UserName;
                    progressDialog.Hide();
                }
            }
            else
            {
                AlertBox.Create("Error", "Faile to updated", activity);
                progressDialog.Hide();
            }
        }
    }
}
