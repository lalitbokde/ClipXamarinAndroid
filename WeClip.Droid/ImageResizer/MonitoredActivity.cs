using Android.Support.V7.App;
using System;

namespace WeClip.Droid.ImageResizer
{
    public class MonitoredActivity : AppCompatActivity
    {
        #region IMonitoredActivity implementation

        public event EventHandler Destroying;
        public event EventHandler Stopping;
        public event EventHandler Starting;

        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Destroying?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnStop()
        {
            base.OnStop();

            Stopping?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnStart()
        {
            base.OnStart();

            Starting?.Invoke(this, EventArgs.Empty);
        }
    }
}