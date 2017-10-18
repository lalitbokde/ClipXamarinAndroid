
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace WeClip.Droid.Helper
{
    public class TimePickerFragment : Android.Support.V4.App.DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        private Context context;
        private TextView Updatetext;
        int hour;
        int minute;
        DateTime date;

        public TimePickerFragment(Context context, TextView Updatetext, DateTime date)
        {
            this.context = context;
            this.Updatetext = Updatetext;
            this.date = date;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            hour = date.Hour;
            minute = date.Minute;
            return new TimePickerDialog(context, this, hour, minute, false);
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            string time = string.Format("{0}:{1}", hourOfDay, minute.ToString().PadLeft(2, '0'));
            Updatetext.Text = time;
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            string time = "";
            Updatetext.Text = time;
        }
    }
}