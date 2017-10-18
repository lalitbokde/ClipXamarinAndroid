using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Support.V4.App;
using WeClip.Droid.Helper;
using WeClip.Droid.Controls;
using Newtonsoft.Json;
using Android.Graphics;

namespace WeClip.Droid.AsyncTask
{
    class GetEventRequestDetails : AsyncTask<Java.Lang.Void, Java.Lang.Void, EventDetails>
    {
        EventDetails eventDetails;
        View view;
        ProgressDialog p;
        private Button btnGoing, btnMayBe, btnDecline;
        private FragmentActivity activity;
        private long eventID;
        private TextView tvGoing, tvMayBe;

        bool IsGoing, IsMayBe, IsDecline = false;

        public GetEventRequestDetails(FragmentActivity activity, View view, long eventID)
        {
            this.activity = activity;
            this.view = view;
            this.eventID = eventID;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "Please wait", "Loading data");
        }

        protected override EventDetails RunInBackground(params Java.Lang.Void[] @params)
        {
            eventDetails = RestSharpCall.Get<EventDetails>("Event/GetEventDetails?eventid=" + eventID);
            return eventDetails;
        }

        protected override void OnPostExecute(EventDetails result)
        {
            base.OnPostExecute(result);
            p.Dismiss();

            if (result != null)
            {

                var btnCreateWeClip = view.FindViewById<ImageView>(Resource.Id.ivERCreateWeClip);
                var btnBack = view.FindViewById<ImageView>(Resource.Id.ivERback);
                var btnUploadPhotos = view.FindViewById<ImageView>(Resource.Id.ivERUploadPhotos);
                var btnInvite = view.FindViewById<Button>(Resource.Id.btnERInvite);
                btnGoing = view.FindViewById<Button>(Resource.Id.btnERGoing);
                btnMayBe = view.FindViewById<Button>(Resource.Id.btnERMaybe);
                btnDecline = view.FindViewById<Button>(Resource.Id.btnERDecline);

                if (eventDetails.EventResponse == "Decline")
                {
                    btnDecline.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnDecline.SetTextColor(Color.ParseColor("#ffffff"));
                    IsDecline = true;
                }
                if (eventDetails.EventResponse == "Maybe")
                {
                    btnMayBe.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnMayBe.SetTextColor(Color.ParseColor("#ffffff"));
                    IsMayBe = true;
                }
                if (eventDetails.EventResponse == "Going")
                {
                    btnGoing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    btnGoing.SetTextColor(Color.ParseColor("#ffffff"));
                    IsGoing = true;
                }

                btnGoing.Click += BtnGoing_Click;
                btnMayBe.Click += BtnMayBe_Click;
                btnDecline.Click += BtnDecline_Click;

                btnCreateWeClip.SetOnClickListener(new btnCreateWeClipClick(btnCreateWeClip, activity, eventID));
                btnUploadPhotos.SetOnClickListener(new btnUploadPhotoClick(btnUploadPhotos, activity, eventID));
                btnInvite.SetOnClickListener(new btnInviteContact(btnInvite, activity, eventID));
                btnBack.Click += BtnBack_Click;

                var starttime = eventDetails.EventStartTime;
                var time = starttime != null ? starttime.Value.ToString("hh:mm tt") : "n/a";
                var day = eventDetails.EventDate.Value.Day;
                var month = eventDetails.EventDate.Value.ToString("MMM");

                view.FindViewById<TextView>(Resource.Id.tvEREventName).Text = eventDetails.EventName;
                tvGoing = view.FindViewById<TextView>(Resource.Id.tvERGoing);
                tvGoing.Text = eventDetails.Going;
                tvMayBe = view.FindViewById<TextView>(Resource.Id.tvERMaybe);
                tvMayBe.Text = eventDetails.Maybe;
                var invited = view.FindViewById<TextView>(Resource.Id.tvERInvited).Text = eventDetails.Invited;
                view.FindViewById<ExpandableTextView>(Resource.Id.tvEREventDescription).Text = eventDetails.EventDescription;
                view.FindViewById<TextView>(Resource.Id.tvEREventType).Text = eventDetails.EventCategory;
                view.FindViewById<TextView>(Resource.Id.tvEREventHostName).Text = eventDetails.HostName;
                view.FindViewById<TextView>(Resource.Id.tvEREventDay).Text = day.ToString();
                view.FindViewById<TextView>(Resource.Id.tvEREventMonth).Text = month;
                view.FindViewById<TextView>(Resource.Id.tvEREventAddress).Text = eventDetails.EventLocation;
                view.FindViewById<TextView>(Resource.Id.tvEREventTime).Text = time;
                var editbutton = view.FindViewById<Button>(Resource.Id.btnEREdit);
                editbutton.SetOnClickListener(new btnCickistner(activity, editbutton, eventDetails));
            }
        }

        private void BtnDecline_Click(object sender, EventArgs e)
        {
            if (IsDecline != true)
            {
                new postEventResponse(eventID, "Decline", activity, btnDecline, btnMayBe, btnGoing, tvGoing, tvMayBe).Execute();
            }
            IsMayBe = false;
            IsDecline = true;
            IsGoing = false;
        }

        private void BtnMayBe_Click(object sender, EventArgs e)
        {
            if (IsMayBe != true)
            {
                new postEventResponse(eventID, "Maybe", activity, btnDecline, btnMayBe, btnGoing, tvGoing, tvMayBe).Execute();
            }
            IsMayBe = true;
            IsDecline = false;
            IsGoing = false;
        }

        private void BtnGoing_Click(object sender, EventArgs e)
        {
            if (IsGoing != true)
            {
                new postEventResponse(eventID, "Going", activity, btnDecline, btnMayBe, btnGoing, tvGoing, tvMayBe).Execute();
            }
            IsMayBe = false;
            IsDecline = false;
            IsGoing = true;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            activity.StartActivity(typeof(MainActivity));
            activity.Finish();
        }

        private class btnCickistner : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;
            private Button editbutton;
            private EventDetails eventDetails;

            public btnCickistner(Activity activity, Button editbutton, EventDetails eventDetails)
            {
                this.activity = activity;
                this.editbutton = editbutton;
                this.eventDetails = eventDetails;
            }

            public void OnClick(View v)
            {
                var data = new EventModel
                {
                    EventID = eventDetails.Id,
                    EventName = eventDetails.EventName,
                    EventDate = eventDetails.EventDate,
                    EventStartTime = eventDetails.EventStartTime,
                    EventPicUrl = eventDetails.EventPicUrl,
                    EventCategory = eventDetails.EventCategory,
                    EventDescription = eventDetails.EventDescription,
                    EventLocation = eventDetails.EventLocation
                };

                Intent intent = new Intent(activity, typeof(CreateEvent));
                intent.PutExtra("strEditEvent", JsonConvert.SerializeObject(data));
                activity.StartActivity(intent);
            }
        }

        private void BtnAddComment_Click(object sender, System.EventArgs e)
        {
            activity.StartActivity(typeof(MainActivity));
        }

        private class btnCreateWeClipClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btnCreateWeClip;
            private long eventID;

            public btnCreateWeClipClick(ImageView btnCreateWeClip, FragmentActivity activity, long eventID)
            {
                this.btnCreateWeClip = btnCreateWeClip;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(EventGalleryView));
                intent.PutExtra("strEventGallery", eventID);
                activity.StartActivity(intent);
                btnCreateWeClip.SetOnClickListener(null);
            }
        }

        private class btnUploadPhotoClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btnUploadPhotos;
            private long eventID;

            public btnUploadPhotoClick(ImageView btnUploadPhotos, FragmentActivity activity, long eventID)
            {
                this.btnUploadPhotos = btnUploadPhotos;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(AddImageAndVideoForEventActivity));
                intent.PutExtra("strCapture", eventID);
                activity.StartActivity(intent);
                btnUploadPhotos.SetOnClickListener(null);
            }
        }

        private class btnFavouriteClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private long id;
            private bool? isEventLike;
            private TextView totalcount;
            private ImageView btnFavourite;

            public btnFavouriteClick(FragmentActivity activity, long id, bool? isEventLike, TextView totalcount, ImageView btnFavourite)
            {
                this.activity = activity;
                this.id = id;
                this.isEventLike = isEventLike;
                this.totalcount = totalcount;
                this.btnFavourite = btnFavourite;
            }

            public void OnClick(View v)
            {
                int resource = (int)btnFavourite.Tag;
                bool value;
                if (resource == Resource.Drawable.ic_favorite_white_24dp)
                {
                    value = true;
                }
                else
                {
                    value = false;
                    //   btnFavourite.Tag = Resource.Drawable.ic_favorite_white_24dp;
                }
                new UpdateLikeCount(activity, id, totalcount, btnFavourite, value).Execute();
            }
        }

        private class btnMoreClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btneventMore;
            private long eventID;

            public btnMoreClickListner(ImageView btneventMore, FragmentActivity activity, long eventID)
            {
                this.btneventMore = btneventMore;
                this.activity = activity;
                this.eventID = eventID;
            }


            public void OnClick(View v)
            {
                PopupMenu popup = new PopupMenu(activity, btneventMore);
                popup.MenuInflater.Inflate(Resource.Menu.poupup_menu, popup.Menu);
                popup.SetOnMenuItemClickListener(new PopupMenuItemClickListener(activity, eventID));
                popup.Show();
            }

            private class PopupMenuItemClickListener : Java.Lang.Object, PopupMenu.IOnMenuItemClickListener
            {
                private FragmentActivity activity;
                private long eventID;

                public PopupMenuItemClickListener(FragmentActivity activity, long eventID)
                {
                    this.activity = activity;
                    this.eventID = eventID;
                }

                public bool OnMenuItemClick(IMenuItem item)
                {
                    Android.Support.V4.App.Fragment fragment = new AddCoHostFragment();
                    var fragmentManager = activity.SupportFragmentManager;
                    Android.Support.V4.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                    fragmentTransaction.Replace(Resource.Id.content_frame, fragment);
                    MainActivity.myBundle.PutLong("EventID", eventID);
                    fragmentTransaction.AddToBackStack("AddCoHostFragment");
                    fragmentTransaction.Commit();
                    return true;
                }
            }
        }

        private class btnInviteContact : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private Button btnInvite;
            private long eventID;

            public btnInviteContact(Button btnInvite, FragmentActivity activity, long eventID)
            {
                this.btnInvite = btnInvite;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(ContactFriend));
                intent.PutExtra("EventID", eventID);
                activity.StartActivity(intent);
                btnInvite.SetOnClickListener(null);
            }
        }
    }
}