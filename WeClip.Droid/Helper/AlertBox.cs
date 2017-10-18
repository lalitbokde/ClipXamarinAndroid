
namespace WeClip.Droid.Helper
{
    public static class AlertBox
    {
        public static void Create(string title, string message, Android.Content.Context context)
        {
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(context);
            alert.SetInverseBackgroundForced(true);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton("CLOSE", (senderAlert, args) =>
            {
                alert.Dispose();
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}