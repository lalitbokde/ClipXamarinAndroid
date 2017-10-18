using Android.OS;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using Android.Views;
using Android.Widget;
using WeClip.Droid.Controls;
using Square.Picasso;
using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Newtonsoft.Json;
using System;
using Android.Util;
using Android.Provider;

namespace WeClip.Droid.AsyncTask
{

    public class GetEventDetails : AsyncTask<Java.Lang.Void, Java.Lang.Void, EventDetails>
    {
        EventDetails eventDetails;
        View view;
        ProgressDialog p;
        private FragmentActivity activity;
        private long eventID;
        private Context applicationContext;
        protected static int CAMERA_REQUEST = 1337;
        protected static int GALLERY_PICTURE = 1;
        private EventFragment evFragment;

        public GetEventDetails(FragmentActivity activity, Context applicationContext, View view, long eventID, EventFragment evFragment)
        {
            this.activity = activity;
            this.applicationContext = applicationContext;
            this.view = view;
            this.eventID = eventID;
            this.evFragment = evFragment;
            p = ProgressDialog.Show(activity, "Please wait", "Loading data");
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
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
                var rlEventpic = view.FindViewById<RelativeLayout>(Resource.Id.rlEventPic);
                var rlEventpicUpload = view.FindViewById<RelativeLayout>(Resource.Id.rvEventFragmentUploadPhoto);
                var btnUploadEventpic = view.FindViewById<Button>(Resource.Id.btnUploadEventFragmentPic);
                var btnEFUMore = view.FindViewById<ImageView>(Resource.Id.btnEFUMore);

                if (string.IsNullOrEmpty(result.EventPic))
                {
                    if (result.IsCohostOrOwnEvent == true)
                    {
                        rlEventpic.Visibility = ViewStates.Gone;
                        rlEventpicUpload.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        rlEventpic.Visibility = ViewStates.Gone;
                        rlEventpicUpload.Visibility = ViewStates.Gone;
                    }
                }
                else
                {
                    rlEventpic.Visibility = ViewStates.Visible;
                    rlEventpicUpload.Visibility = ViewStates.Gone;
                }

                var btnEdit = view.FindViewById<Button>(Resource.Id.btnEdit);
                var btnInvite = view.FindViewById<Button>(Resource.Id.btnInvite);
                var btnShare = view.FindViewById<Button>(Resource.Id.btnShare);
                var btnCreateWeClip = view.FindViewById<ImageView>(Resource.Id.ivEFCreateWeClip);
                var btnChat = view.FindViewById<ImageView>(Resource.Id.ivEFChat);
                var btnUploadPhotos = view.FindViewById<ImageView>(Resource.Id.ivEFUploadPhotos);
                var btneventMore = view.FindViewById<ImageView>(Resource.Id.btnEFMore);

                btneventMore.SetOnClickListener(new btnMoreClickListner(btneventMore, activity, eventID, evFragment, eventDetails.EventPic, result.CreatorId));
                btnEFUMore.SetOnClickListener(new btnMoreClickListner(btnEFUMore, activity, eventID, evFragment, eventDetails.EventPic, result.CreatorId));
                btnCreateWeClip.SetOnClickListener(new btnCreateWeClipClick(btnCreateWeClip, activity, eventID));

                btnUploadPhotos.SetOnClickListener(new btnUploadEventFileClick(btnUploadPhotos, activity, eventID));
                btnUploadEventpic.SetOnClickListener(new btnUploadEventProfilepic(btnUploadPhotos, activity, eventID, result.CreatorId));

                btnInvite.SetOnClickListener(new btnInviteContact(btnInvite, activity, eventID));
                btnShare.SetOnClickListener(new btnShareClick(btnShare, activity, result));

                btnChat.SetOnClickListener(new btnchatClick(btnChat, activity, eventID));

                var starttime = eventDetails.EventStartTime;
                var time = starttime != null ? starttime.Value.ToString("hh:mm tt") : "";

                if (eventDetails.EventDate != null)
                {
                    var day = eventDetails.EventDate.Value.Day;
                    var month = eventDetails.EventDate.Value.ToString("MMM");
                    view.FindViewById<TextView>(Resource.Id.tvEventDay).Text = day.ToString();
                    view.FindViewById<TextView>(Resource.Id.tvEventMonth).Text = month;
                }

                var linerESI = view.FindViewById<LinearLayout>(Resource.Id.llEventFragmentEIS);
                view.FindViewById<TextView>(Resource.Id.tvEventName).Text = eventDetails.EventName;
                view.FindViewById<TextView>(Resource.Id.tvEventTags).Text = eventDetails.EventTag;
                view.FindViewById<TextView>(Resource.Id.tvGoing).Text = eventDetails.Going;
                view.FindViewById<TextView>(Resource.Id.tvMaybe).Text = eventDetails.Maybe;
                view.FindViewById<TextView>(Resource.Id.tvInvited).Text = eventDetails.Invited;
                view.FindViewById<ExpandableTextView>(Resource.Id.tvEventDescription).Text = eventDetails.EventDescription;
                view.FindViewById<TextView>(Resource.Id.tvEventType).Text = eventDetails.EventCategory;
                view.FindViewById<TextView>(Resource.Id.tvEventHostName).Text = eventDetails.HostName;
                view.FindViewById<TextView>(Resource.Id.tvEventAddress).Text = eventDetails.EventLocation;
                view.FindViewById<TextView>(Resource.Id.tvEventTime).Text = time;
                var totalcount = view.FindViewById<TextView>(Resource.Id.tvFavouriteCount);
                var btnFavourite = view.FindViewById<ImageView>(Resource.Id.btnFavourite);
                btnFavourite.SetOnClickListener(null);

                var EventPic = view.FindViewById<ImageView>(Resource.Id.ivEventFragmentEventPic);
                btnEdit.SetOnClickListener(new btnCickistner(activity, btnEdit, eventDetails));

                if (string.IsNullOrEmpty(eventDetails.EventPicUrl))
                {
                    Picasso.With(activity).Load(Resource.Drawable.default_event_back).Fit().CenterCrop().Into(EventPic);
                }
                else
                {
                    Picasso.With(activity).Load(eventDetails.EventPicUrl).Fit().CenterCrop().Into(EventPic);
                }

                if (!result.IsCohostOrOwnEvent)
                {
                    linerESI.Visibility = ViewStates.Gone;
                    btneventMore.Visibility = ViewStates.Gone;
                }

                if (result.isEventLike == true)
                {
                    btnFavourite.Tag = Resource.Drawable.ic_favorite_red_24dp;
                    btnFavourite.SetImageResource(Resource.Drawable.ic_favorite_red_24dp);
                }
                else
                {
                    btnFavourite.Tag = Resource.Drawable.ic_favorite_white_24dp;
                    btnFavourite.SetImageResource(Resource.Drawable.ic_favorite_white_24dp);
                }
                totalcount.Text = eventDetails.TotalLikes;
                btnFavourite.SetOnClickListener(new btnFavouriteClick(activity, result.Id, result.isEventLike, totalcount, btnFavourite));
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            activity.OnBackPressed();
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
                    EventPic = eventDetails.EventPic,
                    EventTag = eventDetails.EventTag,
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

        private class btnchatClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btnChat;
            private long eventID;

            public btnchatClick(ImageView btnChat, FragmentActivity activity, long eventID)
            {
                this.btnChat = btnChat;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Intent pwcActivity = new Intent(activity, typeof(CommentActivity));
                pwcActivity.PutExtra("CurrentEvent", eventID);
                activity.StartActivity(pwcActivity);
                btnChat.SetOnClickListener(null);
            }
        }

        private class btnUploadEventFileClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btnUploadPhotos;
            private long eventID;

            public btnUploadEventFileClick(ImageView btnUploadPhotos, FragmentActivity activity, long eventID)
            {
                this.btnUploadPhotos = btnUploadPhotos;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(AddImageAndVideoForEventActivity));
                intent.PutExtra("strCapture", eventID);
                intent.PutExtra("strIsDefaultEvent", false);
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
                }
                new UpdateLikeCount(activity, id, totalcount, btnFavourite, value).Execute();
            }
        }

        private class btnMoreClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btneventMore;
            private EventFragment evFragment;
            private long eventID;
            private string EventPic;
            private long creatorId;

            public btnMoreClickListner(ImageView btneventMore, FragmentActivity activity, long eventID, EventFragment evFragment, string EventPic, long creatorId)
            {
                this.creatorId = creatorId;
                this.EventPic = EventPic;
                this.btneventMore = btneventMore;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                PopupMenu popup = new PopupMenu(activity, btneventMore);
                popup.MenuInflater.Inflate(Resource.Menu.poupup_menu, popup.Menu);
                popup.SetOnMenuItemClickListener(new PopupMenuItemClickListener(activity, eventID, creatorId));

                IMenu popupMenu = popup.Menu;
                if (string.IsNullOrEmpty(EventPic))
                    popupMenu.FindItem(Resource.Id.popup_Add_EventPhoto).SetVisible(false);
                else
                    popupMenu.FindItem(Resource.Id.popup_Add_EventPhoto).SetVisible(true);
                popup.Show();
            }

            private class PopupMenuItemClickListener : Java.Lang.Object, PopupMenu.IOnMenuItemClickListener
            {
                private FragmentActivity activity;
                private long eventID;
                private long creatorId;

                public PopupMenuItemClickListener(FragmentActivity activity, long eventID, long creatorId)
                {
                    this.creatorId = creatorId;
                    this.activity = activity;
                    this.eventID = eventID;
                }

                public bool OnMenuItemClick(IMenuItem item)
                {
                    if (item.ItemId == Resource.Id.popup_Add_CoHost)
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

                    if (item.ItemId == Resource.Id.popup_Add_EventPhoto)
                    {
                        Intent intent = new Intent(activity, typeof(UploadEventProfilePic));
                        intent.PutExtra("strCapture", eventID);
                        intent.PutExtra("strCreatorId", creatorId);
                        activity.StartActivity(intent);
                        return true;
                    }
                    return false;
                }
            }

            private class dialogeItemClickListner : Java.Lang.Object, IDialogInterfaceOnClickListener
            {
                private FragmentActivity activity;
                private EventFragment evFragment;


                public dialogeItemClickListner(FragmentActivity activity, EventFragment evFragment)
                {
                    this.activity = activity;
                    this.evFragment = evFragment;
                }

                public void OnClick(IDialogInterface dialog, int which)
                {
                    try
                    {
                        if (which == 0)
                        {
                            Intent intent = new Intent(MediaStore.ActionImageCapture);
                            BitmapHelper._file = ImageToPath.GetOutputMediaFile(ImageToPath.UploadFileType.image);
                            BitmapHelper.captureImageUri = Android.Net.Uri.FromFile(BitmapHelper._file);
                            intent.PutExtra(MediaStore.ExtraOutput, BitmapHelper.captureImageUri);
                            evFragment.StartActivityForResult(intent, CAMERA_REQUEST);
                        }
                        else
                     if (which == 1)
                        {
                            Intent pictureActionIntent = null;
                            pictureActionIntent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
                            evFragment.StartActivityForResult(pictureActionIntent, GALLERY_PICTURE);
                        }
                    }
                    catch (Java.Lang.Exception ex)
                    {
                        new CrashReportAsync("dialogeItemClickListner", "OnClick", ex.Message + ex.StackTrace).Execute();
                    }
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

        private class btnShareClick : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private Button btnShare;
            private EventDetails result;

            public btnShareClick(Button btnShare, FragmentActivity activity, EventDetails result)
            {
                this.btnShare = btnShare;
                this.activity = activity;
                this.result = result;
            }

            public void OnClick(View v)
            {
                try
                {
                    Intent intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, result.EventName + "\n" + result.EventDescription + "\n" + result.EventDate + "\n" + "Download weclip from http://www.weclip.com");
                    activity.StartActivity(Intent.CreateChooser(intent, "Share To"));

                    //   ShareDialog shareDialog = new ShareDialog(activity);
                    //   //  shareDialog.RegisterCallback(callbackManager, this);

                    //   ShareLinkContent.Builder shareLinkContentBuilder = new ShareLinkContent.Builder().
                    //         SetContentTitle(result.EventName).
                    //   SetContentDescription(result.EventDescription);
                    //shareLinkContentBuilder.SetContentUrl(Android.Net.Uri.Parse(GlobalClass.strImagePath + result.EventPic));
                    //   ShareLinkContent shareLinkContent = shareLinkContentBuilder.Build();
                    //   shareDialog.Show(shareLinkContent);
                }
                catch (System.Exception ex)
                {
                    Log.Debug("Erorr", ex.Message);
                }
            }
        }

        private class btnUploadEventProfilepic : Java.Lang.Object, View.IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView btnUploadPhotos;
            private long creatorId;
            private long eventID;

            public btnUploadEventProfilepic(ImageView btnUploadPhotos, FragmentActivity activity, long eventID, long creatorId)
            {
                this.creatorId = creatorId;
                this.btnUploadPhotos = btnUploadPhotos;
                this.activity = activity;
                this.eventID = eventID;
            }

            public void OnClick(View v)
            {
                btnUploadPhotos.SetOnClickListener(null);
                Intent intent = new Intent(activity, typeof(UploadEventProfilePic));
                intent.PutExtra("strCapture", eventID);
                intent.PutExtra("strCreatorId", creatorId);

                activity.StartActivity(intent);
            }
        }
    }
}