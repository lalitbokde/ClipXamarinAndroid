using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Util;
using Android.Gms.Gcm.Iid;
using Android.Gms.Gcm;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using WeClip.Core.Common;

namespace WeClip.Droid.GcmMain
{
    [Service(Exported = false)]
    class GCMRegistrationService : IntentService
    {
        static object locker = new object();
        public string topic = "topic";

        public GCMRegistrationService() : base("RegistrationIntentService") { }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");
                lock (locker)
                {
                    InstanceID instanceID = InstanceID.GetInstance(this);
                    try
                    {
                        instanceID.DeleteInstanceID();
                    }
                    catch (Java.IO.IOException e)
                    {
                        e.PrintStackTrace();
                    }
                    instanceID = InstanceID.GetInstance(this);
                    var sender_id = "510976446814";
                    var token = InstanceID.GetInstance(this).GetToken(sender_id
                        , GoogleCloudMessaging.InstanceIdScope, null);

                    var sentToken = sharedPreferences.GetString("token", "");

                    if (token != sentToken)
                    {
                        Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
                        sharedPreferences.Edit().PutString("token", token).Apply();
                        if (token != null && token != "")
                        {
                            SendRegistrationToAppServer(token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("RegistrationIntentService", "Failed to get a registration token " + ex.Message + "");
                return;
            }
        }

        void SendRegistrationToAppServer(string token)
        {
            UserDeviceInfo model = new UserDeviceInfo();
            model.UserID = Core.Common.GlobalClass.UserID;
            model.DeviceID = token;

            var result = RestSharpCall.Put<JsonResult>(model, "User/SetDeviceID?id=" + model.UserID);
            if (result.Success)
            {
                Log.Debug("RegistrationIntentService", "Success");
                return;
            }
        }
    }

}