using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using WeClip.Droid.AsyncTask;
using WeClip.Core.Common;

namespace WeClip.Droid
{
    [Activity(Label = "AddUserNameForPhone")]
    public class AddUserNameForPhone : AppCompatActivity
    {
        private Button btnLogIn;
        private EditText txtProfileName, txtOPTCode;
        private string userName;
        private TextView btnSignUp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddUserNameForPhone);
            txtProfileName = FindViewById<EditText>(Resource.Id.etUserNameAfterRegisterForPhone);
            txtOPTCode = FindViewById<EditText>(Resource.Id.etPhoneOTP);
            btnLogIn = FindViewById<Button>(Resource.Id.btnAddUserNameForPhone);
            btnSignUp = FindViewById<TextView>(Resource.Id.btnSignUpAddUserNameForPhone);

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
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
            this.Finish();
        }

        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(this);
                string profileName = txtProfileName.Text.Trim();
                string strOTPName = txtOPTCode.Text.Trim();

                if (strOTPName == "")
                {
                    AlertBox.Create("Error", "Please enter the code.", this);
                    txtProfileName.RequestFocus();
                    return;
                }

                if (profileName == "")
                {
                    AlertBox.Create("Error", "Please enter your name.", this);
                    txtProfileName.RequestFocus();
                    return;
                }

                var userinfo = new UserInfo
                {
                    LoginType = "P",
                    ProfileName = profileName,
                    RegistrationCode = strOTPName,
                    UserName = userName
                };

                if (profileName == "")
                {
                    AlertBox.Create("Error", "Please enter your name.", this);
                    txtProfileName.RequestFocus();
                    return;
                }
                new LogInBackGround(userinfo, this).Execute();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("AddUserNameForPhone", "BtnLogIn_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        private class LogInBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private Activity activity;
            private JsonResult jsonResult;
            private ProgressDialog p;
            private UserInfo userinfo;

            public LogInBackGround(UserInfo userinfo, Activity activity)
            {
                this.userinfo = userinfo;
                this.activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait...");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jsonResult = RestSharpCall.Post<JsonResult>(userinfo, "Account/AddUserInfo");
                return jsonResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();

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
    }
}