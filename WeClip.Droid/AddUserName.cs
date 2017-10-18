using System;
using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Droid.Helper;
using Android.Content;
using Android.Support.V7.App;
using WeClip.Droid.AsyncTask;
using Android.Content.PM;
using Android.Views;
using Android.Support.V4.Content;
using WeClip.Core.Common;
using WeClip.Core.Model;

namespace WeClip.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddUserName : AppCompatActivity
    {
        private Button btnLogIn, btnResendMail;
        private EditText txtUserName;
        private string userName;
        private TextView btnSignUp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddUserName);
            txtUserName = FindViewById<EditText>(Resource.Id.etUserNameAfterRegister);
            btnLogIn = FindViewById<Button>(Resource.Id.btnAddUserName);
            btnSignUp = FindViewById<TextView>(Resource.Id.btnSignUpAddUserName);
            btnResendMail = FindViewById<Button>(Resource.Id.btnResendMail);
            var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
            userName = prefs.GetString("LoginUserName", null);

            if (string.IsNullOrEmpty(userName))
            {
                this.Finish();
                return;
            }

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primaryDark)));
            }

            btnLogIn.Click += BtnLogIn_Click;
            btnSignUp.Click += BtnSignUp_Click;
            btnResendMail.Click += BtnResendMail_Click;
        }

        private void BtnResendMail_Click(object sender, EventArgs e)
        {
            new ResendMailInBack(this, userName).Execute();
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Application.Context, typeof(LoginActivity));
            intent.PutExtra("FromUserName", "FromUserName");
            StartActivity(intent);
            this.Finish();
        }

        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(this);
                string profileName = txtUserName.Text.Trim();

                var userinfo = new UserInfo
                {
                    LoginType = "E",
                    ProfileName = profileName,
                    UserName = userName
                };

                if (profileName == "")
                {
                    AlertBox.Create("Error", "Please enter your name.", this);
                    txtUserName.RequestFocus();
                    return;
                }
                new LogInBackGround(userinfo, this).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("AddUserName", "BtnLogIn_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        private class LogInBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private Activity activity;
            private JsonResult jsonResult;
            private UserInfo userinfo;


            public LogInBackGround(UserInfo userinfo, Activity activity)
            {
                this.userinfo = userinfo;
                this.activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jsonResult = RestSharpCall.Post<JsonResult>(userinfo, "Account/AddUserInfo");
                return jsonResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);

                if (jsonResult != null)
                {
                    if (jsonResult.Success == true)
                    {
                        var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                        string strUserName = prefs.GetString("LoginUserName", "");
                        string strPassword = prefs.GetString("Password", "");
                        new LogInWeClip(strUserName, strPassword, activity).Execute();
                    }
                    else
                    {
                        Toast.MakeText(activity, result.Message, ToastLength.Long).Show();
                    }
                }
            }
        }

        private class ResendMailInBack : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private AddUserName activity;
            private string userName;
            private JsonResult jsonResult;
            private ProgressDialog p;

            public ResendMailInBack(AddUserName activity, string userName)
            {
                this.activity = activity;
                this.userName = userName;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jsonResult = RestSharpCall.Post<JsonResult>(null, "Account/ReSendMail?username=" + userName);
                return jsonResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    AlertBox.Create("", result.Message, activity);
                }
            }
        }
    }
}