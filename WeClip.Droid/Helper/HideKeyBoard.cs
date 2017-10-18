using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace WeClip.Droid.Helper
{
    public static class HideKeyBoard
    {
        public static void hideSoftInput(Activity activity)
        {
            View view = activity.CurrentFocus;
            if (view != null)
            {
                InputMethodManager imm = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
            }
        }

    }
}