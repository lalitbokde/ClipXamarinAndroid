using System;
using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Common;
using Android.Util;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Views;
using WeClip.Droid.Helper;
using Android.Content;
using Android.Support.V4.App;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid
{
    public class SettingFragment : Fragment
    {
        private RadioGroup rgPushNotification, rgPrivacy;
        private RadioButton rbEnable, rbDisabled;
        private RadioButton rbPublic, rbPrivate;
        private ImageButton btnSave;
        private RadioButton radioButton, radioButtonPrivacy;
        private string TAG = "SettingFragment";
        private View rootView;
        bool isPublic;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);
            Activity.Title = "";
            toolbar_title.Text = "Settings";
            rootView = inflater.Inflate(Resource.Layout.Setting, container, false);
            return rootView;
        }

        public override void OnStart()
        {
            try
            {
                base.OnStart();
                Helper.HideKeyBoard.hideSoftInput(this.Activity);
                rgPushNotification = rootView.FindViewById<RadioGroup>(Resource.Id.rgNotification);
                rbEnable = rootView.FindViewById<RadioButton>(Resource.Id.rbEnable);
                rbDisabled = rootView.FindViewById<RadioButton>(Resource.Id.rbDisabled);
                rgPushNotification.ClearCheck();

                rgPrivacy = rootView.FindViewById<RadioGroup>(Resource.Id.rgPrivacy);
                rbPublic = rootView.FindViewById<RadioButton>(Resource.Id.rbPublic);
                rbPrivate = rootView.FindViewById<RadioButton>(Resource.Id.rbPrivate);
                rgPrivacy.ClearCheck();

                var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                string strIsNotificationEnable = prefs.GetString("IsNotificationEnable", "");
                isPublic = prefs.GetBoolean("IsPublic", false);

                bool isNotification;

                bool.TryParse(strIsNotificationEnable, out isNotification);


                if (isNotification == true)
                    rbEnable.Checked = true;
                else
                    rbDisabled.Checked = true;

                if (isPublic == true)
                    rbPublic.Checked = true;
                else
                    rbPrivate.Checked = true;

                btnSave = rootView.FindViewById<ImageButton>(Resource.Id.btnSaveSetting);
                btnSave.Click += BtnSave_Click;
                int selectedId = rgPushNotification.CheckedRadioButtonId;
                radioButton = rootView.FindViewById<RadioButton>(selectedId);

                int selectedId2 = rgPrivacy.CheckedRadioButtonId;
                radioButtonPrivacy = rootView.FindViewById<RadioButton>(selectedId2);
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("SettingFragment", "OnStart", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int selectedId = rgPushNotification.CheckedRadioButtonId;
            radioButton = rootView.FindViewById<RadioButton>(selectedId);

            int selectedId2 = rgPrivacy.CheckedRadioButtonId;
            radioButtonPrivacy = rootView.FindViewById<RadioButton>(selectedId2);


            bool IsEnable;
            if (radioButton.Text == "Enabled")
            {
                IsEnable = true;
            }
            else
            {
                IsEnable = false;
            }

            bool IsPublic;
            if (radioButtonPrivacy.Text == "Public")
            {
                IsPublic = true;
            }
            else
            {
                IsPublic = false;
            }

            try
            {
                new PostNotification(IsEnable, IsPublic, Activity).Execute();
            }
            catch (System.Exception ex)
            {
                Log.Debug(TAG, ex.StackTrace);
            }
        }

        public class PostNotification : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private bool isEnable;
            private FragmentActivity activity;
            JsonResult jResult;
            ProgressDialog progress;
            private bool isPublic;

            public PostNotification(bool isEnable, bool isPublic, FragmentActivity activity)
            {
                this.isEnable = isEnable;
                this.isPublic = isPublic;
                this.activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                progress = ProgressDialog.Show(activity, "Please wait", "Setting updating...");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jResult = RestSharpCall.Post<JsonResult>(null, "User/NotificationSetting?IsPublic=" + isPublic + "&IsEnable=" + isEnable);
                return jResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                progress.Hide();
                if (result != null)
                {
                    if (result.Success)
                    {
                        var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                        var prefEditor = prefs.Edit();
                        prefEditor.PutString("IsNotificationEnable", isEnable.ToString());
                        prefEditor.PutBoolean("IsPublic", isPublic);

                        prefEditor.Commit();

                        Toast.MakeText(activity, "Success", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(activity, "Fail", ToastLength.Long).Show();
                    }

                }
            }
        }
    }
}