using Android.App;
using Android.Content;
using Android.Gms.Gcm.Iid;

namespace WeClip.Droid.GcmMain
{
    [Service(Exported = false), IntentFilter(new[] { "com.google.android.gms.iid.InstanceID" })]
    class InstanceIdListenerService : InstanceIDListenerService
    {
        public override void OnTokenRefresh()
        {
            var intent = new Intent(this, typeof(GCMRegistrationService));
            StartService(intent);
        }
    }
}
