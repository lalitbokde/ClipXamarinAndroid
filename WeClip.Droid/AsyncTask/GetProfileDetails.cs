using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using Square.Picasso;
using WeClip.Droid.Adapters;
using static Android.Views.View;
using Android.Graphics;
using Android.Content;
using WeClip.Core.Common;
using System;
using Newtonsoft.Json;
using Android.Graphics.Drawables;
using Java.Lang;

namespace WeClip.Droid.AsyncTask
{
    class GetProfileDetails : AsyncTask<Java.Lang.Void, Java.Lang.Void, UserProfile>
    {
        private FragmentActivity activity;
        private long userID;
        private View view;
        private UserProfile userProfile;
        private ProgressDialog p;
        TextView toolbar_title;


        public GetProfileDetails(FragmentActivity activity, View view, long userID, TextView toolbar_title)
        {
            this.activity = activity;
            this.view = view;
            this.userID = userID;
            this.toolbar_title = toolbar_title;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "", "Please wait");
        }

        protected override UserProfile RunInBackground(params Java.Lang.Void[] @params)
        {
            userProfile = RestSharpCall.Get<UserProfile>("User/GetUserProfileWithWeclipStory?id=" + userID);
            return userProfile;
        }

        protected override void OnPostExecute(UserProfile result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (result != null)
            {
                var profilepic = view.FindViewById<ImageView>(Resource.Id.imgProfilePic);
                var followerCount = view.FindViewById<TextView>(Resource.Id.txtFollowerCount);
                var followingCount = view.FindViewById<TextView>(Resource.Id.txtFollowingCount);
                var eventCount = view.FindViewById<TextView>(Resource.Id.txttotalEventCount);
                var fullname = view.FindViewById<TextView>(Resource.Id.tvFullname);
                var fullbio = view.FindViewById<TextView>(Resource.Id.tvBio);
                var email = view.FindViewById<TextView>(Resource.Id.tvEmail);
                var tvPFEventTitle = view.FindViewById<TextView>(Resource.Id.tvPFEventTitle);
                var tvPFEventTags = view.FindViewById<TextView>(Resource.Id.tvPFEventTags);
                var ivEventFragmentEventPic = view.FindViewById<ImageView>(Resource.Id.ivEventFragmentEventPic);
                var weclipstory = view.FindViewById<RelativeLayout>(Resource.Id.rvWeClipStories);
                var emptyweclipstory = view.FindViewById<TextView>(Resource.Id.emptyweClipStories);

                var followerview = view.FindViewById<LinearLayout>(Resource.Id.llFollower);
                var followingview = view.FindViewById<LinearLayout>(Resource.Id.llFolloweing);

                if (result.WeclipImagePath != null)
                {
                    Picasso.With(activity).Load(result.WeclipImagePath).Into(ivEventFragmentEventPic);
                    ivEventFragmentEventPic.SetOnClickListener(new onClickListner(activity, new WeClipVideo
                    {
                        VideoFileUrl = userProfile.WeclipVideoPath,
                        Thumb = userProfile.WeclipImagePath,
                        EventName = userProfile.DefultEventName,
                        EventDate = userProfile.EventDate,
                        WeClipTag = userProfile.WeClipTag,
                        WeClipTitle = userProfile.WeClipTitle,
                        VideoFileName = userProfile.WeclipVideoName
                    }
                        ));
                }


                if (string.IsNullOrEmpty(result.WeclipImagePath))
                {
                    weclipstory.Visibility = ViewStates.Gone;
                    emptyweclipstory.Visibility = ViewStates.Visible;

                }
                else
                {
                    weclipstory.Visibility = ViewStates.Visible;
                    emptyweclipstory.Visibility = ViewStates.Gone;
                }

                if (result.WeClipTitle != null)
                    tvPFEventTitle.Text = result.WeClipTitle;

                if (result.WeClipTag != null)
                    tvPFEventTags.Text = result.WeClipTag;

                var editWeClip = view.FindViewById<ImageView>(Resource.Id.ivPFEditWeClip);

                var btnFollowing = view.FindViewById<Button>(Resource.Id.btnPFFollow);



                var btnInvite = view.FindViewById<Button>(Resource.Id.btnPFSendInvite);
                var llInviteFollow = view.FindViewById<LinearLayout>(Resource.Id.llPFInviteFollow);
                var llEditProfile = view.FindViewById<LinearLayout>(Resource.Id.llPFEdit);
                var lvEvent = view.FindViewById<ListView>(Resource.Id.lvPFEvent);
                var svProfile = view.FindViewById<ScrollView>(Resource.Id.svProfileFragment);

                var createEvent = view.FindViewById<ImageView>(Resource.Id.ivPFCreateEvent);
                createEvent.SetOnClickListener(new btncreateEventClick(createEvent, activity));

                var btnEditProfile = view.FindViewById<Button>(Resource.Id.btnPFEditProfile);
                btnEditProfile.Click += BtnEditProfile_Click;


                editWeClip.SetOnClickListener(new btnCreateWeClipClick(editWeClip, activity, result.DefultEventId));

                followerview.SetOnClickListener(new followerClick(followerview, activity, result.UserID));
                followingview.SetOnClickListener(new followeingClick(followingview, activity, result.UserID));


                toolbar_title.Text = result.UserName;
                toolbar_title.Gravity = GravityFlags.Center;

                if (string.IsNullOrEmpty(result.ProfilePic))
                {
                    Picasso.With(activity).Load(Resource.Drawable.contact_withoutphoto).Placeholder(Resource.Drawable.contact_withoutphoto)
         .Resize(150, 150).Transform(new CircleTransformation()).Into(profilepic);
                }
                else
                {
                    Picasso.With(activity).Load(result.ProfilePic).Placeholder(Resource.Drawable.contact_withoutphoto)
         .Resize(150, 150).Transform(new CircleTransformation()).Into(profilepic);
                }

                followingCount.Text = result.Following;
                followerCount.Text = result.Follwers;
                eventCount.Text = result.TotalEvents.ToString();
                fullname.Text = result.UserName;
                fullbio.Text = result.Bio;
                email.Text = result.EmailId;

                if (result.isOwnProfile == "1")
                {
                    llInviteFollow.Visibility = ViewStates.Gone;
                    llEditProfile.Visibility = ViewStates.Visible;
                    editWeClip.Visibility = ViewStates.Visible;
                }
                else
                {
                    llInviteFollow.Visibility = ViewStates.Visible;
                    llEditProfile.Visibility = ViewStates.Gone;
                    editWeClip.Visibility = ViewStates.Gone;

                    if (result.RequestStatus == null)
                    {
                        btnFollowing.Text = "Follow";
                        btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                        btnFollowing.SetTextColor(Color.ParseColor("#04c285"));
                    }
                    else
                    {
                        btnFollowing.Text = result.RequestStatus;
                        btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                        btnFollowing.SetTextColor(Color.ParseColor("#ffffff"));
                    }
                    btnFollowing.SetOnClickListener(new btnFollowingClickListner(activity, userProfile, btnFollowing, followerCount));
                }

                svProfile.FullScroll(FocusSearchDirection.Up);
                svProfile.SmoothScrollTo(0, 0);
                lvEvent.Focusable = false;
                new GetAllEventDetails(view, activity, userID, lvEvent).Execute();
            }
        }


        private void EditProfile_Click(object sender, System.EventArgs e)
        {
            Android.Support.V4.App.Fragment editProfile = new EditProfile();
            Android.Support.V4.App.FragmentManager fragmentManager = activity.SupportFragmentManager;
            fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, editProfile, "editProfile").AddToBackStack("EditProfile").Commit();
        }

        private void BtnEditProfile_Click(object sender, System.EventArgs e)
        {
            Android.Support.V4.App.Fragment editProfile = new EditProfile();
            Android.Support.V4.App.FragmentManager fragmentManager = activity.SupportFragmentManager;
            fragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, editProfile, "editProfile").AddToBackStack("EditProfile").Commit();
        }

        private class GetAllEventDetails : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<GetAllEventList>>
        {
            private FragmentActivity activity;
            private long userID;
            private View view;
            List<GetAllEventList> eventdata;
            private ListView lvEvent;

            public GetAllEventDetails(View view, FragmentActivity activity, long userID, ListView lvEvent)
            {
                this.view = view;
                this.activity = activity;
                this.userID = userID;
                this.lvEvent = lvEvent;
            }

            protected override List<GetAllEventList> RunInBackground(params Java.Lang.Void[] @params)
            {
                eventdata = RestSharpCall.GetList<GetAllEventList>("EventRequest/GetMyEventList?id=" + userID);
                return eventdata;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                var madapter = new ProfileEventAdapter((from x in eventdata where x.EventType == "M" select x).ToList(), activity);
                lvEvent.Adapter = (madapter);
                setListViewHeightBasedOnChildren(lvEvent);
            }

            private void setListViewHeightBasedOnChildren(ListView listView)
            {
                var listAdapter = listView.Adapter;
                if (listAdapter == null)
                    return;

                int desiredWidth = MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
                int totalHeight = 0;
                View view = null;
                for (int i = 0; i < listAdapter.Count; i++)
                {
                    view = listAdapter.GetView(i, view, listView);
                    if (i == 0)
                        view.LayoutParameters = (new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.WrapContent));

                    view.Measure(desiredWidth, (int)MeasureSpecMode.Unspecified);
                    totalHeight += view.MeasuredHeight;
                }
                ViewGroup.LayoutParams params1 = listView.LayoutParameters;
                params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
                listView.LayoutParameters = (params1);
            }
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
                intent.PutExtra("strDefultEvent", true);
                activity.StartActivity(intent);
                //   activity.Finish();
                btnCreateWeClip.SetOnClickListener(null);
            }
        }

        private class postFriendRequest : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private FragmentActivity activity;
            private UserProfile userProfile;
            private JsonResult jResult;
            private ProgressDialog p;
            private Button btnFollowing;
            private TextView followerCount;


            public postFriendRequest(FragmentActivity activity, UserProfile userProfile, Button btnFollowing, TextView followerCount)
            {
                this.followerCount = followerCount;
                this.btnFollowing = btnFollowing;
                this.activity = activity;
                this.userProfile = userProfile;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                var friendRequest = new FriendRequest
                {
                    SendToEmail = userProfile.EmailId,
                    SendToMobile = userProfile.PhoneNo,
                    SendToId = userProfile.UserID,
                };

                jResult = RestSharpCall.Post<JsonResult>(friendRequest, "Friend/Create");
                return jResult;
            }

            protected override void OnPostExecute(JsonResult result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    if (result.Success == true)
                    {
                        if (result.Message == "Following")
                        {
                            followerCount.Text = result.ImageName;
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#ffffff"));
                            btnFollowing.Text = "Following";
                            userProfile.RequestStatus = "Following";
                        }
                        if (result.Message == "Friend Request sent")
                        {
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#ffffff"));
                            btnFollowing.Text = "Request sent";
                            userProfile.RequestStatus = "Request sent";
                        }
                    }
                    else
                    {
                        Toast.MakeText(activity, "Error", ToastLength.Short).Show();
                    }
                }
            }
        }

        private class btnFollowingClickListner : Java.Lang.Object, IOnClickListener
        {
            private FragmentActivity activity;
            private Button btnFollowing;
            private TextView followerCount;
            private UserProfile userProfile;



            public btnFollowingClickListner(FragmentActivity activity, UserProfile userProfile, Button btnFollowing, TextView followerCount)
            {
                this.followerCount = followerCount;
                this.activity = activity;
                this.userProfile = userProfile;
                this.btnFollowing = btnFollowing;
            }

            public void OnClick(View v)
            {

                if (userProfile.RequestStatus == null)
                {
                    new postFriendRequest(activity, userProfile, btnFollowing, followerCount).Execute();
                }
                else
                {
                    if (userProfile.RequestStatus == "Request sent")
                    {
                        showDiloage("Cancel Request", btnFollowing, false, userProfile, followerCount);
                    }
                    if (userProfile.RequestStatus == "Following")
                    {
                        showDiloage("Unfollow", btnFollowing, true, userProfile, followerCount);
                    }
                }
            }

            private void showDiloage(string positiveBtnText, Button btnFollowing, bool isFriend, UserProfile userProfile, TextView followerCount)
            {
                Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(activity);
                alertDialog.SetTitle("Are you sure ?");
                alertDialog.SetPositiveButton(positiveBtnText, (senderAlert, args) =>
                {

                    new postCancelFriendRequest(activity, isFriend, btnFollowing, userProfile, followerCount).Execute();
                });
                alertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }


            private class postCancelFriendRequest : AsyncTask<Java.Lang.Object, Java.Lang.Object, JsonResult>
            {
                private ProgressDialog p;
                private JsonResult jResult;
                private FragmentActivity activity;
                private bool isFriend;
                private Button btnFollowing;
                private UserProfile userProfile;
                private TextView followerCount;


                public postCancelFriendRequest(FragmentActivity activity, bool isFriend, Button btnFollowing, UserProfile userProfile, TextView followerCount)
                {
                    this.followerCount = followerCount;
                    this.activity = activity;
                    this.isFriend = isFriend;
                    this.btnFollowing = btnFollowing;
                    this.userProfile = userProfile;
                    jResult = new JsonResult();
                }

                protected override void OnPreExecute()
                {
                    base.OnPreExecute();
                    p = ProgressDialog.Show(activity, "", "Please wait");
                }

                protected override JsonResult RunInBackground(params Java.Lang.Object[] @params)
                {
                    jResult = RestSharpCall.Post<JsonResult>(null, "Friend/CancelFriendRequest?id=" + userProfile.UserID + "&isFriend=" + isFriend);
                    return jResult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    base.OnPostExecute(result);
                    p.Dismiss();
                    if (result != null)
                    {
                        if (result.Success == true)
                        {
                            followerCount.Text = result.Message;
                            userProfile.RequestStatus = null;
                            btnFollowing.Text = "Follow";
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#04c285"));
                        }
                        else
                        {
                            Toast.MakeText(activity, "Error", ToastLength.Short).Show();
                        }
                    }
                }


            }
        }

        private class onClickListner : Java.Lang.Object, IOnClickListener
        {
            private FragmentActivity activity;
            private WeClipVideo weclipVideoInfo;

            public onClickListner(FragmentActivity activity, WeClipVideo weclipVideoInfo)
            {
                this.activity = activity;
                this.weclipVideoInfo = weclipVideoInfo;
            }

            public void OnClick(View v)
            {
                var videoPlayerActivity = new Android.Content.Intent(activity, typeof(VideoPlayerActivity));
                videoPlayerActivity.PutExtra("VideoUrl", JsonConvert.SerializeObject(weclipVideoInfo));
                activity.StartActivity(videoPlayerActivity);
            }
        }

        private class btncreateEventClick : Java.Lang.Object, IOnClickListener
        {
            private FragmentActivity activity;
            private ImageView createEvent;

            public btncreateEventClick(ImageView createEvent, FragmentActivity activity)
            {
                this.createEvent = createEvent;
                this.activity = activity;
            }

            public void OnClick(View v)
            {
                activity.StartActivity(typeof(CreateEvent));
                createEvent.SetOnClickListener(null);
            }
        }

        private class followerClick : Java.Lang.Object, IOnClickListener
        {
            private FragmentActivity activity;
            private LinearLayout followerCount;
            private long userID;

            public followerClick(LinearLayout followerCount, FragmentActivity activity, long userID)
            {
                this.followerCount = followerCount;
                this.activity = activity;
                this.userID = userID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(FollowerActivity));
                intent.PutExtra("UserID", userID);
                activity.StartActivity(intent);
                followerCount.SetOnClickListener(null);
            }
        }

        private class followeingClick : Java.Lang.Object, IOnClickListener
        {
            private FragmentActivity activity;
            private LinearLayout followerCount;
            private long userID;

            public followeingClick(LinearLayout followerCount, FragmentActivity activity, long userID)
            {
                this.followerCount = followerCount;
                this.activity = activity;
                this.userID = userID;
            }

            public void OnClick(View v)
            {
                Intent intent = new Intent(activity, typeof(FollowingActivity));
                intent.PutExtra("UserID", userID);
                activity.StartActivity(intent);
                followerCount.SetOnClickListener(null);
            }
        }

    }
}