using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using WeClip.Core.Model;

namespace WeClip.Droid.AsyncTask
{
    public class UpdateLikeCount : AsyncTask<Java.Lang.Void, Java.Lang.Void, LikeEventData>
    {
        private FragmentActivity activity;
        private ImageView btnFavourite;
        private long id;
        private TextView totalcount;
        private LikeEventData jResult;
        private bool value;
        ProgressDialog p;
        public UpdateLikeCount(FragmentActivity activity, long id, TextView totalcount, ImageView btnFavourite, bool value)
        {
            this.activity = activity;
            this.id = id;
            this.totalcount = totalcount;
            this.btnFavourite = btnFavourite;
            this.value = value;

        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "", "please wait");
        }

        protected override LikeEventData RunInBackground(params Java.Lang.Void[] @params)
        {
            jResult = RestSharpCall.Post<LikeEventData>(null, "Event/SetLikeEvent?eventID=" + id + "&isLike=" + value);
            return jResult;
        }

        protected override void OnPostExecute(LikeEventData result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            if (result != null)
            {
                totalcount.Text = result.TotalLike;
                if (result.IsLike == true)
                {
                    btnFavourite.SetImageResource(Resource.Drawable.ic_favorite_red_24dp);
                    btnFavourite.Tag = Resource.Drawable.ic_favorite_red_24dp;
                }
                else
                {
                    btnFavourite.SetImageResource(Resource.Drawable.ic_favorite_white_24dp);
                    btnFavourite.Tag = Resource.Drawable.ic_favorite_white_24dp;
                }

            }
        }
    }
}