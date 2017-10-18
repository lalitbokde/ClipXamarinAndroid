using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    [Activity(Label = "ResetPasswordActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ResetPasswordActivity : Activity
    {
        TextView btnsignIn;
        private EditText txtCode;
        private EditText txtPassword;
        private EditText txtconfirmPassword;
        private Button btnSubmit;
        private long Userid;
        private string username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReSetPasswordInfo);
            Userid = Intent.GetLongExtra("ForgotpassId", 0);
            username = Intent.GetStringExtra("ForgotpassUserName");
            txtconfirmPassword = FindViewById<EditText>(Resource.Id.etResetConformPassword);
            txtPassword = FindViewById<EditText>(Resource.Id.etResetPassword);
            txtCode = FindViewById<EditText>(Resource.Id.etCode);
            btnsignIn = FindViewById<TextView>(Resource.Id.btnSignInInRestPassword);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmitRestPassword);
            btnsignIn.Click += BtnsignIn_Click;
            btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, System.EventArgs e)
        {
            string code, password, confirmPassword;
            code = txtCode.Text;
            password = txtPassword.Text;
            confirmPassword = txtconfirmPassword.Text;

            if (code == "")
            {
                AlertBox.Create("Error", "Please enter code!", this);
                return;
            }
            if (password == "")
            {
                AlertBox.Create("Error", "Please enter password!", this);
                return;

            }
            if (confirmPassword == "")
            {
                AlertBox.Create("Error", "Please enter confirm password!", this);
                return;

            }
            if (password != confirmPassword)
            {
                AlertBox.Create("Error", "password and confirm password not matched!", this);
                return;
            }
            new restPassword(this, new Reset_AddNewPassword { code = code, Password = password, userID = Userid, ConfirmPassword = confirmPassword }, username).Execute();
        }

        private void BtnsignIn_Click(object sender, System.EventArgs e)
        {
            this.StartActivity(typeof(LoginActivity));
            this.Finish();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }

        private class restPassword : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private Activity activity;
            private Reset_AddNewPassword reset_AddNewPassword;
            JsonResult jResult;
            private string username;

            public restPassword(Activity activity, Reset_AddNewPassword reset_AddNewPassword, string username)
            {
                this.activity = activity;
                this.reset_AddNewPassword = reset_AddNewPassword;
                this.username = username;
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jResult = RestSharpCall.Post<JsonResult>(reset_AddNewPassword, "Account/ResetPassword");
                return jResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                if (result != null)
                {
                    if (jResult.Success == true)
                    {
                        Toast.MakeText(activity, jResult.Message, ToastLength.Short).Show();
                        Handler mHandler = new Handler();
                        mHandler.PostDelayed(() =>
                        {
                            new LogInWeClip(username, reset_AddNewPassword.Password, activity).Execute();
                        }, 2000);
                    }
                    else
                    {
                        Toast.MakeText(activity, jResult.Message, ToastLength.Long).Show();
                    }
                }
            }
        }
    }
}