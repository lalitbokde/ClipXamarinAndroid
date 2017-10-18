using Android.App;
using Android.Content;
using Android.OS;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Core.Common;
using Android.Util;
using WeClip.Droid.GcmMain;
using System;

namespace WeClip.Droid.AsyncTask
{
    public class LogInWeClip : AsyncTask<Java.Lang.Void, Java.Lang.Void, Token>
    {
        string userName = "";
        string password = "";
        Token authentication;
        //  ProgressDialog progressDialog;
        Activity context;

        public LogInWeClip(string userName, string password, Activity context)
        {
            this.userName = userName;
            this.password = password;
            this.context = context;
            authentication = new Token();
            //    progressDialog = ProgressDialog.Show(context, "", "Please wait...");

        }

        protected override Token RunInBackground(params Java.Lang.Void[] @params)
        {
            authentication = RestSharpCall.Login<Token>(userName, password);
            return authentication;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            //    progressDialog.Dismiss();
            if (result != null && authentication != null && authentication.access_token != null && authentication.Success == true)
            {
                GlobalClass.AccessToken = authentication.access_token;
                GlobalClass.UserEmail = authentication.EmailID;
                GlobalClass.UserID = authentication.UserID.ToString();
                Log.Debug("AccessToken", GlobalClass.AccessToken);
                Log.Debug("AccessTokenExpiresIn", authentication.expires_in.ToString());
                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutString("Password", password);
                prefEditor.PutString("LoginUserName", authentication.LoginUserName);
                prefEditor.PutString("EmailID", authentication.EmailID);
                prefEditor.PutString("Access_Token", authentication.access_token);
                prefEditor.PutString("UserID", authentication.UserID.ToString());
                prefEditor.PutString("ProfilePic", authentication.ProfilePic.ToString());
                prefEditor.PutString("UserName", authentication.UserName.ToString());
                prefEditor.PutString("IsNotificationEnable", authentication.IsNotificationEnable == null ? "false" : authentication.IsNotificationEnable.ToString());
                prefEditor.PutBoolean("IsPublic", Convert.ToBoolean(authentication.IsPublic));
                prefEditor.PutString("PhoneNumber", authentication.PhoneNumber);
                prefEditor.PutString("Bio", authentication.Bio);
                prefEditor.PutInt("MaxImageForWeClip", authentication.MaxImageForWeClip);
                prefEditor.PutLong("MaxVideoDuration", authentication.MaxVideoDurationInMinute);
                prefEditor.PutInt("MaxVideoForWeclip", authentication.MaxVideoForWeclip);
                prefEditor.PutLong("MaxVideoSize", authentication.MaxVideoSize);

                prefEditor.PutBoolean("RegisterEmail", false);
                prefEditor.PutBoolean("RegisterPhone", false);

                if (authentication.DOB != null)
                {
                    prefEditor.PutString("DOB", string.Format("{0: dd MMM yyyy}", authentication.DOB));
                }
                else
                {
                    prefEditor.PutString("DOB", "N/A");
                }
                prefEditor.Commit();

                var intent = new Intent(context, typeof(GCMRegistrationService));
                context.StartService(intent);

                context.StartActivity(typeof(MainActivity));
                context.Finish();
            }
            else
            {
                if (result != null && authentication != null && authentication.Success == false)

                    if (authentication.status == "verify_account")
                    {
                        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(context);
                        alert.SetInverseBackgroundForced(true);
                        alert.SetTitle("Verify account");
                        alert.SetMessage(authentication.Message);
                        alert.SetPositiveButton("YES", (senderAlert, args) =>
                        {
                            var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                            var prefEditor = prefs.Edit();
                            prefEditor.PutString("LoginUserName", userName);
                            prefEditor.Commit();
                            context.StartActivity(new Intent(Application.Context, typeof(AddUserName)));
                            context.Finish();
                        });

                        alert.SetNegativeButton("NO", (senderAlert, args) =>
                        {
                            alert.Dispose();
                        });
                        Android.App.Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        AlertBox.Create("Error", authentication.Message, context);
                        var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                        var prefEditor = prefs.Edit();
                        prefEditor.Clear();
                        prefEditor.Commit();
                    }
                else
                {
                    AlertBox.Create("Error", "Error occured", context);
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.Clear();
                    prefEditor.Commit();
                }

            }
        }
    }
}