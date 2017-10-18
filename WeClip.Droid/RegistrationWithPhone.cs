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

namespace WeClip.Droid
{
    [Activity(Label = "RegistrationWithPhone", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, NoHistory = true)]
    public class RegistrationWithPhone : AppCompatActivity
    {
        private EditText txtPhone, txtPassword, txtConfirmPassword;
        private Button btnRegister;
        private TextView btnSignIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.RegistrationWithPhone);

                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                var prefEditor = prefs.Edit();
                prefEditor.PutBoolean("RegisterPhone", false);
                prefEditor.Commit();

                txtPhone = FindViewById<EditText>(Resource.Id.etRegisterPhoneNo);
                txtPassword = FindViewById<EditText>(Resource.Id.etRegisterPhonePassword);
                txtConfirmPassword = FindViewById<EditText>(Resource.Id.etConfirmPhonePassword);

                btnRegister = FindViewById<Button>(Resource.Id.btnRegistrationPhone);
                btnRegister.Click += btnRegister_click;

                btnSignIn = FindViewById<TextView>(Resource.Id.btnPhoneSignIn);
                btnSignIn.Click += BtnSignIn_Click;
                txtPhone.RequestFocus();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("Registration", "OnCreate", ex.Message + ex.StackTrace);
            }
        }

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
        }

        private void btnRegister_click(object sender, EventArgs e)
        {
            HideKeyBoard.hideSoftInput(this);

            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                AlertBox.Create("Error", "Please enter phone number.", this);
                txtPhone.RequestFocus();
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
            reg.PhoneNumber = txtPhone.Text;
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
                jsonResult = RestSharpCall.Post<JsonResult>(reg, "Account/RegisterNewWithPhone");
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
                        prefEditor.PutString("LoginUserName", reg.PhoneNumber);
                        prefEditor.PutBoolean("RegisterPhone", true);
                        prefEditor.Commit();
                        Intent intent = new Intent(activity, typeof(AddUserNameForPhone));
                        intent.PutExtra("FromRegistrationPhone", reg.PhoneNumber);
                        activity.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(activity, result.Message, ToastLength.Short).Show();
                    }
                }
            }
        }

    }
}