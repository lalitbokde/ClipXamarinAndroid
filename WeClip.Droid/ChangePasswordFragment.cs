using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.Helper;

namespace WeClip.Droid
{
    public class ChangePasswordFragment : Fragment
    {
        private View rootView;
        protected EditText etoldPassword, etnewPassword, etConfirmPassword;
        protected ImageButton btnChangePassword;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Change Password";
            rootView = inflater.Inflate(Resource.Layout.ChangePassword, container, false);
            btnChangePassword = rootView.FindViewById<ImageButton>(Resource.Id.btnCPSubmit);
            btnChangePassword.Click += BtnChangePassword_Click;
            return rootView;
        }

        public override void OnStart()
        {
            base.OnStart();
            etoldPassword = rootView.FindViewById<EditText>(Resource.Id.etCPOldPassword);
            etoldPassword.RequestFocus();
            etnewPassword = rootView.FindViewById<EditText>(Resource.Id.etCPNewPassword);
            etConfirmPassword = rootView.FindViewById<EditText>(Resource.Id.etCPConfirmPassword);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (etoldPassword.Text == "")
            {
                AlertBox.Create("Error", "Please enter old password", Activity);
                return;
            }

            if (etnewPassword.Text == "")
            {
                AlertBox.Create("Error", "Please enter new password", Activity);
                return;
            }

            if (etnewPassword.Text != etConfirmPassword.Text)
            {
                AlertBox.Create("Error", "Password and confirm password does not match", Activity);
                return;
            }
            new PostChangePassword(Activity, new ChangePasswordBindingModel { OldPassword = etoldPassword.Text, NewPassword = etnewPassword.Text, ConfirmPassword = etConfirmPassword.Text }).Execute();
        }

        private class PostChangePassword : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private Activity activity;
            private ChangePasswordBindingModel changePasswordBindingModel;
            private JsonResult jresult;
            private ProgressDialog p;


            public PostChangePassword(Activity activity, ChangePasswordBindingModel changePasswordBindingModel)
            {
                this.activity = activity;
                this.changePasswordBindingModel = changePasswordBindingModel;
                p = ProgressDialog.Show(activity, "", "Please wait...");

            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jresult = RestSharpCall.Post<JsonResult>(changePasswordBindingModel, "Account/ChangePassword");
                return jresult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result.Success == true)
                {
                    Toast.MakeText(activity, result.Message, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(activity, result.Message, ToastLength.Short).Show();
                }
            }
        }
    }
}