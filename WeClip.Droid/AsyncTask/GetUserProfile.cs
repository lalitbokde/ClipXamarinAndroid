using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    public class GetUserProfile : AsyncTask<Java.Lang.Void, Java.Lang.Void, UserProfile>
    {
        UserProfile profile;
        private Activity context;

        public GetUserProfile(Activity context)
        {
            this.context = context;
        }

        protected override UserProfile RunInBackground(params Java.Lang.Void[] @params)
        {
            profile = RestSharpCall.Get<UserProfile>("User/GetProfile");
            return profile;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            if (result != null && profile != null)
            {
                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                GlobalClass.UserID = profile.UserID.ToString();
                prefEditor.PutString("UserID", profile.UserID.ToString());
                prefEditor.PutString("ProfilePic", profile.ProfilePic.ToString());
                prefEditor.PutString("UserName", profile.UserName.ToString());
                prefEditor.PutString("IsNotificationEnable", profile.IsNotificationEnable == null ? "false" : profile.IsNotificationEnable.ToString());
                if (profile.DOB != null)
                {
                    prefEditor.PutString("DOB", string.Format("{0: dd MMM yyyy}", profile.DOB));
                }
                else
                    prefEditor.PutString("DOB", "N/A");
                prefEditor.Commit();
                context.StartActivity(typeof(MainActivity));
                context.Finish();
            }
            else
            {
                Toast.MakeText(context, "Error", ToastLength.Long);
            }
        }
    }
}