using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using WeClip.Droid.Helper;
using WeClip.Core.Model;
using WeClip.Core.Common;
using System.Globalization;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content.PM;
using Android.Views;
using Android.Support.V4.Content;
using WeClip.Droid.AsyncTask;
using Exception = Java.Lang.Exception;

namespace WeClip.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateEvent : AppCompatActivity
    {
        protected ImageButton imgCreateEvent, ivCreateEventBack;
        private EditText eventTitle, eventLocation, EventInfo;
        private TextView eventDate, eventTime;
        private EventModel model;
        private CultureInfo cultureInfo = CultureInfo.InvariantCulture;
        private RadioGroup rgPrivacy;
        private string category;
        private string updateData, strFromAddEvent;
        private RadioButton rbPublic, rbPrivate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.CreateEvent);
                Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                TextView toolbar_title = (TextView)this.FindViewById(Resource.Id.toolbar_title);
                SupportActionBar.Title = "";
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(new Android.Graphics.Color(Android.Support.V4.Content.ContextCompat.GetColor(this, Resource.Color.primaryDark)));
                }
                eventTitle = FindViewById<EditText>(Resource.Id.etEventTitle);
                eventDate = FindViewById<TextView>(Resource.Id.etEventDate);
                eventTime = FindViewById<TextView>(Resource.Id.etEventTime);
                eventLocation = FindViewById<EditText>(Resource.Id.etEventLocation);
                EventInfo = FindViewById<EditText>(Resource.Id.etEventInfo);
                //   coHost = FindViewById<EditText>(Resource.Id.etEventHost);
                rgPrivacy = FindViewById<RadioGroup>(Resource.Id.rgPrivacy);
                rbPublic = FindViewById<RadioButton>(Resource.Id.rbPublic);
                rbPrivate = FindViewById<RadioButton>(Resource.Id.rbPrivate);
                imgCreateEvent = FindViewById<ImageButton>(Resource.Id.imgCreateEvent);
                ivCreateEventBack = FindViewById<ImageButton>(Resource.Id.ivCreateEventBack);
                imgCreateEvent.Click += ImgCreateEvent_Click;
                ivCreateEventBack.Click += IvCreateEventBack_Click;
                rgPrivacy.ClearCheck();
                rgPrivacy.SetOnCheckedChangeListener(new rbgCheckedChangeListner(this, rbPublic, rbPrivate));

                eventDate.Click += EventDate_Click;
                eventTime.Click += EventTime_Click;

                updateData = this.Intent.GetStringExtra("strEditEvent") ?? null;
                strFromAddEvent = this.Intent.GetStringExtra("strAddEventPic") ?? null;

                if (strFromAddEvent != null)
                {
                    model = JsonConvert.DeserializeObject<EventModel>(strFromAddEvent);
                    FillEventDetails(model);
                }
                else
                {
                    if (updateData != null)
                    {
                        model = JsonConvert.DeserializeObject<EventModel>(updateData);
                        FillEventDetails(model);
                    }
                    else
                    {
                        model = new EventModel();
                        rbPublic.Checked = true;
                    }
                }

                if (model.EventID != 0)
                {
                    toolbar_title.Text = "Edit Event";
                }
                else
                {
                    toolbar_title.Text = "Create Event";
                }
            }
            catch (Exception ex)
            {
                new CrashReportAsync("CreateEvent", "OnCreate", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void IvCreateEventBack_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        private void FillEventDetails(EventModel em)
        {
            try
            {
                eventTitle.Text = em.EventName;

                if (!string.IsNullOrEmpty(em.EventDate.ToString()))
                {
                    eventDate.Text = String.Format("{0:dd MMM yyyy}", em.EventDate);
                }
                if (!string.IsNullOrEmpty(em.EventStartTime.ToString()))
                {
                    var startdate = em.EventStartTime;
                    var hour = startdate.Value.Hour;
                    var minute = startdate.Value.Minute;
                    string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
                    eventTime.Text = time;
                }
                eventLocation.Text = em.EventLocation;
                EventInfo.Text = em.EventDescription;
                if (em.EventCategory == "Public")
                {
                    rbPublic.Checked = true;
                }

                if (em.EventCategory == "Personal")
                {
                    rbPrivate.Checked = true;
                }
            }
            catch (Exception ex)
            {
                new CrashReportAsync("CreateEvent", "FillEventDetails", ex.Message + ex.StackTrace).Execute();
            }
        }

        private void EventTime_Click(object sender, EventArgs e)
        {
            HideKeyBoard.hideSoftInput(this);

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.EventStartTime.ToString()))
                {
                    Android.Support.V4.App.DialogFragment newFragment = new TimePickerFragment(this, eventTime, model.EventStartTime.Value);
                    newFragment.Show(SupportFragmentManager, "TimePicker");
                }
                else
                {
                    Android.Support.V4.App.DialogFragment newFragment = new TimePickerFragment(this, eventTime, DateTime.Now);
                    newFragment.Show(SupportFragmentManager, "TimePicker");
                }
            }
        }

        private void EventDate_Click(object sender, EventArgs e)
        {
            HideKeyBoard.hideSoftInput(this);
            DatePickerFragment dpFragment = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                eventDate.Text = string.Format("{0:dd MMM yyyy}", time.Date);
            });
            dpFragment.Show(SupportFragmentManager, DatePickerFragment.TAG);
        }

        private void ImgCreateEvent_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(this);
                if (eventTitle.Text.Trim() == "")
                {
                    AlertBox.Create("Error", "Please enter event title.", this);
                    return;
                }

                if (eventDate.Text.Trim() == "")
                {
                    AlertBox.Create("Error", "Please enter event date.", this);
                    return;
                }

                model.UserID = Convert.ToInt64(GlobalClass.UserID);
                model.EventName = eventTitle.Text;

                if (eventDate.Text != "")
                {
                    model.EventDate = DateTime.ParseExact(eventDate.Text, "dd MMM yyyy", cultureInfo);
                }

                if (eventTime.Text != "")
                {
                    string StartTimeInput = eventTime.Text;
                    var timeFromInput = DateTime.ParseExact(StartTimeInput, "H:m", cultureInfo);
                    model.EventStartTime = timeFromInput;
                }

                else
                {
                    model.EventStartTime = null;
                }

                model.EventLocation = eventLocation.Text;
                model.EventDescription = EventInfo.Text == null ? "" : EventInfo.Text;

                int selectedId = rgPrivacy.CheckedRadioButtonId;
                var rbselected = FindViewById<RadioButton>(selectedId);

                if (rbselected.Text == "Public")
                {
                    category = "Public";
                }
                else
                {
                    category = "Personal";
                }
                model.EventCategory = category;
                Intent intent = new Intent(this, typeof(EventCreateAddEventProfilePic));
                intent.PutExtra("strCreateEvent", JsonConvert.SerializeObject(model));
                this.StartActivity(intent);
                this.Finish();
            }
            catch (Exception ex)
            {
                new CrashReportAsync("CreateEvent", "ImgCreateEvent_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }
    }

    internal class rbgCheckedChangeListner : Java.Lang.Object, RadioGroup.IOnCheckedChangeListener
    {
        private CreateEvent createEvent;
        private RadioButton rbPrivate;
        private RadioButton rbPublic;

        public rbgCheckedChangeListner(CreateEvent createEvent, RadioButton rbPublic, RadioButton rbPrivate)
        {
            this.rbPublic = rbPublic;
            this.rbPrivate = rbPrivate;
            this.createEvent = createEvent;
        }

        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            var Checkedcolor = ContextCompat.GetColor(createEvent, Resource.Color.radioButtonCheckedText);
            var UnCheckedcolor = ContextCompat.GetColor(createEvent, Resource.Color.radioButtonUnCheckedText);

            if (checkedId == Resource.Id.rbPublic)
            {
                rbPublic.SetTextColor(new Android.Graphics.Color(Checkedcolor));
                rbPrivate.SetTextColor(new Android.Graphics.Color(UnCheckedcolor));
            }
            else
                if (checkedId == Resource.Id.rbPrivate)
            {
                rbPublic.SetTextColor(new Android.Graphics.Color(UnCheckedcolor));
                rbPrivate.SetTextColor(new Android.Graphics.Color(Checkedcolor));
            }
        }
    }
}