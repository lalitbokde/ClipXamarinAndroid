using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;

namespace WeClip.Droid.Adapters
{
    class WeClipPackageAdapter : BaseAdapter<WeClipPackageInfo>
    {
        private Activity activity;
        private List<WeClipPackageInfo> packageInfo;

        public WeClipPackageAdapter(List<WeClipPackageInfo> packageInfo, Activity activity)
        {
            this.packageInfo = packageInfo;
            this.activity = activity;
        }

        public override WeClipPackageInfo this[int position]
        {
            get
            {
                return packageInfo[position];
            }
        }

        public override int Count
        {
            get
            {
                return packageInfo.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            PackageViewAdapter holder;
            var item = packageInfo[position];
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.PremiumActivityItem, parent, false);
                holder = new PackageViewAdapter();
                holder.tvPackageName = convertView.FindViewById<TextView>(Resource.Id.tvPackageName);
                holder.tvPackagePrice = convertView.FindViewById<TextView>(Resource.Id.tvPackagePrice);
                holder.btnPackagebuy = convertView.FindViewById<Button>(Resource.Id.btnPackageBuy);
                holder.ivPackageBack = convertView.FindViewById<ImageView>(Resource.Id.ivPackageItemPic);

                holder.btnPackagebuy.Click += delegate
                {

                };
                convertView.Tag = holder;

            }
            else
            {
                holder = convertView.Tag as PackageViewAdapter;
            }

            holder.tvPackageName.Text = item.Name;
            holder.tvPackagePrice.Text = item.Price;
            Picasso.With(activity).Load(Resource.Drawable.Package_Back).Resize(60, 60).Into(holder.ivPackageBack);

            return convertView;
        }
    }
}

public class PackageViewAdapter : Java.Lang.Object
{
    public Button btnPackagebuy { get; set; }
    public TextView tvPackagePrice { get; set; }
    public TextView tvPackageName { get; set; }
    public ImageView ivPackageBack { get; set; }
}

