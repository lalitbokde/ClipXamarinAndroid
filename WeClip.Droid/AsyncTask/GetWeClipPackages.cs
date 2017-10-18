using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Graphics;

namespace WeClip.Droid.AsyncTask
{
    public class GetWeClipPackages : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<WeClipPackageInfo>>
    {
        private GridView gvPackage;
        private Activity activity;
        private List<WeClipPackageInfo> packageInfo;
        ProgressDialog p;

        public GetWeClipPackages(Activity activity, GridView gvPackage)
        {
            this.activity = activity;
            this.gvPackage = gvPackage;
            packageInfo = new List<WeClipPackageInfo>();
            p = ProgressDialog.Show(activity, "Please wait", "Loading data");
        }

        protected override List<WeClipPackageInfo> RunInBackground(params Java.Lang.Void[] @params)
        {
            packageInfo = RestSharpCall.GetList<WeClipPackageInfo>("Setting/GetWeClipPackages");
            return packageInfo;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (packageInfo != null)
            {
                gvPackage.Adapter = new WeClipPackageAdapter(packageInfo, activity);
                gvPackage.OnItemClickListener = new grvOnClickListner(activity, packageInfo, gvPackage);
            }
        }

        private class grvOnClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private Activity activity;
            private List<WeClipPackageInfo> packageInfo;
            private int backposition = -1;
            private GridView gvPackage;

            public grvOnClickListner(Activity activity, List<WeClipPackageInfo> packageInfo, GridView gvPackage)
            {
                this.gvPackage = gvPackage;
                this.activity = activity;
                this.packageInfo = packageInfo;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                var selectedItem = parent.GetItemAtPosition(position).ToString();
                var GridViewItems = (LinearLayout)view;
                GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                var BackSelectedItem = (LinearLayout)gvPackage.GetChildAt(backposition);
                if (backposition != -1)
                {
                    BackSelectedItem.Selected = (false);
                    BackSelectedItem.SetBackgroundColor(Color.Transparent);
                }

                if (backposition == position)
                {
                    GridViewItems.SetBackgroundColor(Color.ParseColor("#ff33b5e5"));
                }

                backposition = position;
            }
        }
    }
}