using System;
using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Core.Common;
using WeClip.Core.Service;
using Android.Content;
using Android.Support.V7.App;
using WeClip.Droid.AsyncTask;
using Android.Views;
using Android.Support.V4.Content;
using Android.Util;

namespace WeClip.Droid
{
    [Activity(Label = "Registration", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RegistrationWithEmail : AppCompatActivity
    {
        private EditText txtUsername, txtEmail, txtPhone, txtPassword, txtConfirmPassword;
        private Button btnRegister;
        private TextView btnSignIn;
        AccountService accountService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.RegistrationWithEmail);
                Log.Debug("RegistrationWithEmail", "OnCreate");


                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutBoolean("RegisterEmail", false);
                prefEditor.PutString("Password", null);
                prefEditor.PutString("LoginUserName", null);

                prefEditor.Commit();

                accountService = new AccountService();
                txtUsername = FindViewById<EditText>(Resource.Id.etRegisterUserName);
                txtEmail = FindViewById<EditText>(Resource.Id.etRegisterEmailId);
                txtPhone = FindViewById<EditText>(Resource.Id.etRegisterPhoneNo);
                txtPassword = FindViewById<EditText>(Resource.Id.etRegisterPassword);
                txtConfirmPassword = FindViewById<EditText>(Resource.Id.etConfirmPassword);

                btnRegister = FindViewById<Button>(Resource.Id.btnRegistration);

                btnRegister.Click += btnRegister_click;

                btnSignIn = FindViewById<TextView>(Resource.Id.btnSignIn);
                btnSignIn.Click += BtnSignIn_Click;

                txtUsername.RequestFocus();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("Registration", "OnCreate", ex.Message + ex.StackTrace);
            }
        }


        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            this.Finish();
        }

        private void btnRegister_click(object sender, EventArgs e)
        {
            HideKeyBoard.hideSoftInput(this);

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                AlertBox.Create("Error", "Please enter email id.", this);
                txtEmail.RequestFocus();
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                AlertBox.Create("Error", "Please enter password.", this);
                txtPassword.RequestFocus();
                return;
            }

            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                AlertBox.Create("Error", "Please enter confirm password.", this);
                txtConfirmPassword.RequestFocus();
                return;
            }
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                AlertBox.Create("Error", "Password and confirm password does not match.", this);
                txtPassword.RequestFocus();
                return;
            }

            RegistrationModel reg = new RegistrationModel();
            reg.Password = txtPassword.Text;
            reg.DeviceID = "";
            reg.ConfirmPassword = txtConfirmPassword.Text;
            reg.Email = txtEmail.Text;
            new RegistrationInBackGround(reg, this).Execute();

        }

        private class RegistrationInBackGround : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private RegistrationModel reg;
            private Activity activity;
            private JsonResult jsonResult;
            private ProgressDialog p;

            public RegistrationInBackGround(RegistrationModel reg, Activity activity)
            {
                this.reg = reg;
                this.activity = activity;
                p = ProgressDialog.Show(activity, "", "Please wait...");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jsonResult = RestSharpCall.Post<JsonResult>(reg, "Account/RegisterNew");
                return jsonResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();

                if (result != null)
                {
                    if (result.Success == true)
                    {
                        var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                        var prefEditor = prefs.Edit();
                        prefEditor.PutString("Password", reg.Password);
                        prefEditor.PutString("EmailID", reg.Email);
                        prefEditor.PutString("LoginUserName", reg.Email);
                        prefEditor.PutBoolean("RegisterEmail", true);
                        prefEditor.Commit();
                        Intent intent = new Intent(activity, typeof(AddUserName));
                        activity.StartActivity(intent);

                    }
                    else
                    {
                        Toast.MakeText(activity, result.Message, ToastLength.Short).Show();
                    }
                }
            }
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            Log.Debug("RegistrationWithEmail", "OnRestart");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("RegistrationWithEmail", "OnResume");

        }

        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug("RegistrationWithEmail", "OnStop");

        }
        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug("RegistrationWithEmail", "OnStart");

        }


    }
}