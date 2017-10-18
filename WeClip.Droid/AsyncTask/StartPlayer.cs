
using Android.App;
using Android.OS;
using Android.Media;
using Java.Lang;
using Android.Util;

namespace WeClip.Droid.AsyncTask
{
    public class StartPlayer : AsyncTask<string, Java.Lang.Void, bool>
    {
        private ProgressDialog progress;
        MediaPlayer mediaPlayer;
        bool prepared;
        Activity audioPlayerActivity;

        public StartPlayer(Activity audioPlayerActivity, MediaPlayer mediaPlayer)
        {
            this.audioPlayerActivity = audioPlayerActivity;
            this.mediaPlayer = mediaPlayer;
            progress = new ProgressDialog(audioPlayerActivity);
        }

        protected override bool RunInBackground(params string[] @params)
        {
            try
            {
                mediaPlayer.SetDataSource(@params[0]);
                mediaPlayer.SetOnCompletionListener(new OnCompletionListener(audioPlayerActivity));
                mediaPlayer.Prepare();
                prepared = true;
            }
            catch (IllegalArgumentException e)
            {
                // TODO Auto-generated catch block
                Log.Debug("IllegarArgument", e.Message);
                prepared = false;
                e.PrintStackTrace();
            }
            catch (SecurityException e)
            {
                // TODO Auto-generated catch block
                prepared = false;
                e.PrintStackTrace();
            }
            catch (IllegalStateException e)
            {
                // TODO Auto-generated catch block
                prepared = false;
                e.PrintStackTrace();
            }
            catch (Java.IO.IOException e)
            {
                // TODO Auto-generated catch block
                prepared = false;
                e.PrintStackTrace();
            }
            return prepared;
        }

        protected override void OnPostExecute(bool result)
        {
            base.OnPostExecute(result);
            if (progress.IsShowing)
            {
                progress.Cancel();
            }
            Log.Debug("Prepared", "//" + result);
            mediaPlayer.Start();

            //   btn.SetImageResource(Resource.Drawable.pause);
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            this.progress.SetMessage("Please wait");
            this.progress.Show();
        }


        protected override void OnProgressUpdate(params Java.Lang.Object[] native_values)
        {
            base.OnProgressUpdate(native_values);
        }

        public class OnCompletionListener : Java.Lang.Object, MediaPlayer.IOnCompletionListener
        {
            private Activity audioPlayerActivity;

            public OnCompletionListener(Activity audioPlayerActivity)
            {
                this.audioPlayerActivity = audioPlayerActivity;

            }

            public void OnCompletion(MediaPlayer mp)
            {
                mp.Stop();
                mp.Reset();
            }
        }
    }
}