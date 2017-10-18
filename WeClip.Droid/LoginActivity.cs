using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using Android.App;
using Android.Content.PM;
using WeClip.Droid.Helper;
using WeClip.Droid.AsyncTask;
using Android.Content;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.GcmMain;
using Android.Util;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Runtime;
using System.Collections.Generic;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.Gms.Plus;
using Java.Lang;

namespace WeClip.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : AppComCustomeActivty, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IFacebookCallback
    {
        LinearLayout llayoutSignIn, llayoutForgotPassword;
        LinearLayout llayoutSignUpOptions;
        TextView btnSignUpWsocial;
        TextView btnSignInView;
        TextView btnForgotPassword;
        TextView btnsignInInForgotPassword;
        private EditText txtUserName;
        private EditText txtPassword;
        private EditText txtForgotUsername;

        private Button btnLogIn;
        private Button btnSignupWithEmail, btnSignupWithPhone, btnSignupWithGoogle, btnSignUpWithFacebook, btnSumitForgotPassword;
        public string email;
        private GoogleApiClient mGoogleApiClient;
        private ConnectionResult mConnectionResult;
        private bool mIntentInProgress;
        private bool mSignInClicked;
        private bool mInfoPopulated = true;
        private const string TAG = "GoogleApiClient";
        private ICallbackManager mcallBackManager;
        private string personName;
        private string personPhoto;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                FacebookSdk.SdkInitialize(this.ApplicationContext);
                SetContentView(Resource.Layout.Login);
                txtUserName = FindViewById<EditText>(Resource.Id.etEmailPhone);
                txtPassword = FindViewById<EditText>(Resource.Id.etPassword);
                txtForgotUsername = FindViewById<EditText>(Resource.Id.etForgotUsername);

                btnLogIn = FindViewById<Button>(Resource.Id.btnLogin);
                GoogleApiClient.Builder builder = new GoogleApiClient.Builder(this);
                builder.AddConnectionCallbacks(this);
                builder.AddOnConnectionFailedListener(this);
                builder.AddApi(PlusClass.API);
                builder.AddScope(new Scope(Scopes.PlusLogin));
                mGoogleApiClient = builder.Build();
                mGoogleApiClient.Connect();

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }

                ISharedPreferences prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);

                var FromUserScreen = Intent.GetStringExtra("FromUserName");

                if (!string.IsNullOrEmpty(FromUserScreen))
                {
                    var prefEditor = prefs.Edit();
                    prefEditor.PutBoolean("RegisterEmail", false);
                    prefEditor.PutBoolean("RegisterPhone", false);
                    prefEditor.Commit();
                }

                bool RegisterEmail = prefs.GetBoolean("RegisterEmail", false);
                bool RegisterPhone = prefs.GetBoolean("RegisterPhone", false);

                llayoutSignIn = FindViewById<LinearLayout>(Resource.Id.llayoutSignIn);
                llayoutSignUpOptions = FindViewById<LinearLayout>(Resource.Id.llayoutSignUpOptions);
                llayoutForgotPassword = FindViewById<LinearLayout>(Resource.Id.llayoutForgot);


                btnSignUpWsocial = FindViewById<TextView>(Resource.Id.btnSignUpWsocial);
                btnSignUpWsocial.Click += BtnSignUpWsocial_Click;

                btnSignInView = FindViewById<TextView>(Resource.Id.btnSignInView);
                btnSignInView.Click += BtnSignInView_Click;

                btnForgotPassword = FindViewById<TextView>(Resource.Id.btnForgotPassword);
                btnForgotPassword.Click += BtnForgotPassword_Click;

                btnsignInInForgotPassword = FindViewById<TextView>(Resource.Id.tvSigninInForgotPassword);
                btnsignInInForgotPassword.Click += BtnSigninForgotPassword_Click;


                btnSignupWithEmail = FindViewById<Button>(Resource.Id.btnSignUpWithEmail);
                btnSignupWithPhone = FindViewById<Button>(Resource.Id.btnSignUpWithPhone);
                btnSignupWithGoogle = FindViewById<Button>(Resource.Id.btnGPlus);
                btnSignUpWithFacebook = FindViewById<Button>(Resource.Id.btnFacebook);
                btnSumitForgotPassword = FindViewById<Button>(Resource.Id.btnSubmitForgotPassword);
                mcallBackManager = CallbackManagerFactory.Create();

                btnSignupWithPhone.Visibility = ViewStates.Gone;

                LoginManager.Instance.RegisterCallback(mcallBackManager, this);

                btnSignupWithEmail.Click += btnSignupWithEmail_Click;
                btnSignupWithPhone.Click += BtnSignupWithPhone_Click;
                btnSignupWithGoogle.Click += BtnSignupWithGoogle_Click;
                btnSignUpWithFacebook.Click += BtnSignUpWithFacebook_Click;
                btnSumitForgotPassword.Click += BtnSumitForgotPassword_Click;
                btnLogIn.Click += BtnLogIn_Click;

                if (RegisterEmail == true)
                {
                    SignUpView();
                }

            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("LoginActivity", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void BtnSumitForgotPassword_Click(object sender, EventArgs e)
        {
            string username = txtForgotUsername.Text;
            HideKeyBoard.hideSoftInput(this);
            if (username == "")
            {
                AlertBox.Create("Error", "Please enter username!", this);
                return;
            }
            new callForgotPassword(this, username).Execute();
        }

        private void BtnSigninForgotPassword_Click(object sender, EventArgs e)
        {
            llayoutSignIn.Visibility = ViewStates.Visible;
            llayoutForgotPassword.Visibility = ViewStates.Gone;
        }

        protected override void OnStart()
        {
            base.OnStart();
            logoutFromGoogle(mGoogleApiClient);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            Log.Debug(TAG + "OnActivityResult", resultCode.ToString());
            mcallBackManager.OnActivityResult(requestCode, (int)resultCode, data);

            if (requestCode == 0)
            {
                if (resultCode != Result.Ok)
                {
                    mSignInClicked = false;
                }

                mIntentInProgress = false;

                if (!mGoogleApiClient.IsConnecting)
                {
                    mGoogleApiClient.Connect();
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

        }

        private void BtnSignupWithPhone_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegistrationWithPhone));
        }

        private void btnSignupWithEmail_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegistrationWithEmail));
            this.Finish();
        }

        private void BtnForgotPassword_Click(object sender, EventArgs e)
        {
            llayoutSignIn.Visibility = ViewStates.Gone;
            llayoutForgotPassword.Visibility = ViewStates.Visible;
        }

        private void BtnSignUpWsocial_Click(object sender, EventArgs e)
        {
            llayoutSignIn.Visibility = ViewStates.Gone;
            llayoutSignUpOptions.Visibility = ViewStates.Visible;
        }

        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(this);
                string username = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (username == "")
                {
                    AlertBox.Create("Error", "Please enter username.", this);
                    txtUserName.RequestFocus();
                    return;
                }
                if (password == "")
                {
                    AlertBox.Create("Error", "Please enter password.", this);
                    txtPassword.RequestFocus();
                    return;
                }
           
                new LogInWeClip(username, password, this).Execute();
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("LoginActivity", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void BtnSignInView_Click(object sender, EventArgs e)
        {
            llayoutSignUpOptions.Visibility = ViewStates.Gone;
            llayoutSignIn.Visibility = ViewStates.Visible;
        }

        private void SignUpView()
        {
            llayoutSignIn.Visibility = ViewStates.Gone;
            llayoutSignUpOptions.Visibility = ViewStates.Visible;
        }

        private void BtnSignupWithGoogle_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, "mSignInClicked" + mSignInClicked);
            Log.Debug(TAG, "mInfoPopulated" + mInfoPopulated);

            if (!mGoogleApiClient.IsConnecting)
            {
                Log.Debug(TAG, "BtnSignupWithGoogle mpiClietNotConnecting");

                mSignInClicked = true;
                mInfoPopulated = false;
                ResolveSignInError();
            }
            else
            {
                Log.Debug(TAG, "BtnSignupWithGoogle mpiClietConnecting");
            }
        }

        private void ResolveSignInError()
        {
            if (mGoogleApiClient.IsConnected)
            {
                Log.Debug(TAG + "ResolveSignInError", "ApiClientConnected");
            }
            else
            {
                Log.Debug(TAG + "ResolveSignInError", "ApiClientNotConnected");
                mGoogleApiClient.Connect();
            }


            if (mConnectionResult.HasResolution)
            {
                Log.Debug(TAG, "mConnectionResult" + mConnectionResult.HasResolution);

                try
                {
                    mIntentInProgress = true;
                    Log.Debug(TAG, "mConnectionResult" + mConnectionResult.Resolution.IntentSender.ToString());
                    StartIntentSenderForResult(mConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }

                catch (Android.Content.IntentSender.SendIntentException e)
                {
                    //The intent was cancelled before it was sent. Return to the default
                    //state and attempt to connect to get an updated ConnectionResult

                    Log.Debug(TAG, "mConnectionResult" + e.Cause);

                    mIntentInProgress = false;
                    mGoogleApiClient.Connect();
                }
            }
            else
            {
                Log.Debug(TAG, "mConnectionResult" + mConnectionResult.HasResolution);
            }

        }

        public void OnConnected(Bundle connectionHint)
        {
            mSignInClicked = false;

            Log.Debug(TAG, "onConnectedCall" + "And IS infoPopulated :" + mInfoPopulated);

            if (mInfoPopulated)
            {
                logoutFromGoogle(mGoogleApiClient);
            }
            else
            {
                email = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);

                if (PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient) != null)
                {
                    var currentPerson = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
                    personName = currentPerson.DisplayName;
                    personPhoto = currentPerson.Image.Url;
                    if (!string.IsNullOrEmpty(personPhoto))
                    {
                        string original = personPhoto;
                        int pos = original.IndexOf("?sz=");
                        string newstring = original.Substring(0, pos);
                        personPhoto = newstring + "?sz=480";
                    }
                }

                if (string.IsNullOrEmpty(email))
                {
                    Log.Debug(TAG, email);
                }
                else
                {
                    Log.Debug(TAG, "email is null");
                }
                new LogInWithGoogle(new GoogleInfo
                {
                    email = email,
                    picture = personPhoto,
                    name = personName
                }, this, mGoogleApiClient).Execute();
                logoutFromGoogle(mGoogleApiClient);
            }
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIntentInProgress)
            {
                //Store the ConnectionResult so that we can use it later when the user clicks 'sign-in;
                mConnectionResult = result;

                if (mSignInClicked)
                {
                    //The user has already clicked 'sign-in' so we attempt to resolve all
                    //errors until the user is signed in, or the cancel
                    ResolveSignInError();
                }
            }
        }

        public void OnConnectionSuspended(int cause)
        {

        }

        private void BtnSignUpWithFacebook_Click(object sender, EventArgs e)
        {
            LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "email" });
        }

        public void OnCancel()
        {
        }

        public void OnError(FacebookException p0)
        {

        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var loginResult = result as Xamarin.Facebook.Login.LoginResult;

            Bundle parameters = new Bundle();
            parameters.PutString("fields", "email");

            GraphRequest request = GraphRequest.NewMeRequest(loginResult.AccessToken, new GraphJSONCallback(loginResult, this));
            request.Parameters = parameters;
            request.ExecuteAsync();
        }

        private class LogInWithGoogle : AsyncTask<Java.Lang.Void, Java.Lang.Void, Token>
        {
            private GoogleInfo googleInfo;
            ProgressDialog progressDialog;
            Activity context;
            Token authentication;
            GoogleApiClient gac;
            private const string signInWithGooglePassword = "WeClip@App_GooglePassword";

            public LogInWithGoogle(GoogleInfo googleInfo, Activity context, GoogleApiClient gac)
            {
                this.googleInfo = googleInfo;
                this.context = context;
                authentication = new Token();
                progressDialog = ProgressDialog.Show(context, "", "Please wait...");
                this.gac = gac;
            }
            protected override Token RunInBackground(params Java.Lang.Void[] @params)
            {
                authentication = RestSharpCall.Post<Token>(googleInfo, "Account/LoginWithGoogle");
                return authentication;
            }
            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                if (result != null && authentication != null && authentication.access_token != null && authentication.Success == true)
                {
                    GlobalClass.AccessToken = authentication.access_token;
                    GlobalClass.UserEmail = authentication.EmailID;
                    GlobalClass.UserID = authentication.UserID.ToString();
                    Log.Debug("AccessToken", GlobalClass.AccessToken);
                    Log.Debug("AccessTokenExpiresIn", authentication.expires_in.ToString());
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("Password", signInWithGooglePassword);
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
                        AlertBox.Create("Error", authentication.Message, context);
                    else
                        AlertBox.Create("Error", "Error occured", context);
                }
                progressDialog.Dismiss();
            }
        }

        private class LoginWithFb : AsyncTask<Java.Lang.Void, Java.Lang.Void, Token>
        {
            private faceBookProfileInfo faceBookProfileInfo;
            private Activity context;
            ProgressDialog progressDialog;
            Token authentication;
            private const string signInWithGooglePassword = "WeClip@App_GooglePassword";

            public LoginWithFb(faceBookProfileInfo faceBookProfileInfo, Activity context)
            {
                this.faceBookProfileInfo = faceBookProfileInfo;
                this.context = context;
                authentication = new Token();
                progressDialog = ProgressDialog.Show(context, "", "Please wait...");
            }

            protected override Token RunInBackground(params Java.Lang.Void[] @params)
            {
                authentication = RestSharpCall.Post<Token>(faceBookProfileInfo, "Account/LoginWithFacebook");
                return authentication;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                if (result != null && authentication != null && authentication.access_token != null && authentication.Success == true)
                {
                    GlobalClass.AccessToken = authentication.access_token;
                    GlobalClass.UserEmail = authentication.EmailID;
                    GlobalClass.UserID = authentication.UserID.ToString();
                    Log.Debug("AccessToken", GlobalClass.AccessToken);
                    Log.Debug("AccessTokenExpiresIn", authentication.expires_in.ToString());
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("Password", signInWithGooglePassword);
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
                        AlertBox.Create("Error", authentication.Message, context);
                    else
                        AlertBox.Create("Error", "Error occured", context);
                }
                progressDialog.Dismiss();
            }
        }

        private class GraphJSONCallback : Java.Lang.Object, GraphRequest.IGraphJSONObjectCallback
        {
            private Activity context;
            private LoginResult loginResult;

            public GraphJSONCallback(LoginResult loginResult, Activity context)
            {
                this.loginResult = loginResult;
                this.context = context;
            }

            public void OnCompleted(JSONObject json, GraphResponse response)
            {
                if (response.Error != null)
                {
                }
                else
                {
                    string fbEmail = json.GetString("email");

                    Profile profile = Profile.CurrentProfile;
                    string id = profile.Id;
                    string fbprofilePicpath = "";
                    string fbProfilename = "";


                    if (Profile.CurrentProfile != null)
                    {
                        fbprofilePicpath = Profile.CurrentProfile.GetProfilePictureUri(240, 240).ToString();
                        fbProfilename = Profile.CurrentProfile.FirstName + " " + Profile.CurrentProfile.LastName;
                    }


                    if (!string.IsNullOrEmpty(fbEmail))
                    {
                        new LoginWithFb(new faceBookProfileInfo { email = fbEmail, ProfileName = fbProfilename, ProfilePic = fbprofilePicpath }, context).Execute();
                    }
                    else
                    {
                        Toast.MakeText(context, "Fail to authenticate!", ToastLength.Short).Show();
                    }

                }
            }
        }

        public void logoutFromGoogle(GoogleApiClient mGoogleApiClient)
        {
            if (mGoogleApiClient != null && mGoogleApiClient.IsConnected)
            {
                mGoogleApiClient.ClearDefaultAccountAndReconnect().SetResultCallback(new resultCallbackMethod(mGoogleApiClient));
            }
        }

        private class resultCallbackMethod : Java.Lang.Object, IResultCallback
        {
            private GoogleApiClient mGoogleApiClient;

            public resultCallbackMethod(GoogleApiClient mGoogleApiClient)
            {
                this.mGoogleApiClient = mGoogleApiClient;
            }

            public void OnResult(Java.Lang.Object result)
            {
                mGoogleApiClient.Disconnect();
            }
        }

        private class callForgotPassword : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private Activity activity;
            private string username;
            private JsonResult jResult;
            private ProgressDialog p;

            public callForgotPassword(Activity activity, string username)
            {
                this.activity = activity;
                this.username = username;
                p = ProgressDialog.Show(activity, "", "Please wait...");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jResult = RestSharpCall.Post<JsonResult>(null, "Account/ForgotPassword?username=" + username);
                return jResult;
            }
            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    if (jResult.Success == true)
                    {
                        Toast.MakeText(activity, jResult.Message, ToastLength.Long).Show();
                        Intent intent = new Intent(activity, typeof(ResetPasswordActivity));
                        intent.PutExtra("ForgotpassId", jResult.ID);
                        intent.PutExtra("ForgotpassUserName", username);
                        activity.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(activity, jResult.Message, ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(activity, "Error occured!", ToastLength.Long).Show();
                }
            }

        }
    }
}
