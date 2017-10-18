using Android.OS;
using Android.App;
using Android.Support.V4.App;
using WeClip.Droid.Helper;
using Android.Widget;
using Android.Graphics;
using WeClip.Core.Model;

namespace WeClip.Droid.AsyncTask
{
    class postEventResponse : AsyncTask<Java.Lang.Void, Java.Lang.Void, EventResponse>
    {
        private long eventID;
        private string response;
        private ProgressDialog p;
        private FragmentActivity activity;
        private EventResponse jResult;
        private Button btnDecline;
        private Button btnMayBe;
        private Button btnGoing;
        private TextView tvGoing;
        private TextView tvMayBe;

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "", "Please wait");
        }

        public postEventResponse(long eventID, string response, FragmentActivity activity, Button btnDecline, Button btnMayBe, Button btnGoing, TextView tvGoing, TextView tvMayBe)
        {
            this.eventID = eventID;
            this.response = response;
            this.activity = activity;
            this.btnDecline = btnDecline;
            this.btnMayBe = btnMayBe;
            this.btnGoing = btnGoing;
            this.tvGoing = tvGoing;
            this.tvMayBe = tvMayBe;
        }

        protected override EventResponse RunInBackground(params Java.Lang.Void[] @params)
        {
            jResult = RestSharpCall.Post<EventResponse>(null, "EventRequest/UpdateResponse?id=" + eventID + "&responsetext=" + response);
            return jResult;
        }

        protected override void OnPostExecute(EventResponse result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (jResult != null)
            {
                if (response == "Decline")
                {
                    btnDecline.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnDecline.SetTextColor(Color.ParseColor("#ffffff"));

                    btnGoing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnGoing.SetTextColor(Color.ParseColor("#00D096"));

                    btnMayBe.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnMayBe.SetTextColor(Color.ParseColor("#00D096"));

                }
                if (response == "Maybe")
                {
                    btnMayBe.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnMayBe.SetTextColor(Color.ParseColor("#ffffff"));

                    btnGoing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnGoing.SetTextColor(Color.ParseColor("#00D096"));

                    btnDecline.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnDecline.SetTextColor(Color.ParseColor("#00D096"));

                }
                if (response == "Going")
                {
                    btnGoing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnGoing.SetTextColor(Color.ParseColor("#ffffff"));

                    btnDecline.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnDecline.SetTextColor(Color.ParseColor("#00D096"));

                    btnMayBe.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    btnMayBe.SetTextColor(Color.ParseColor("#00D096"));

                }

                tvGoing.Text = jResult.Going;
                tvMayBe.Text = jResult.Maybe;
            }
        }
    }
}