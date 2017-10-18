using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid.Adapters
{
    class CohostAdapter : BaseAdapter
    {
        private FragmentActivity activity;
        private List<CoHost> coHostList;
        private List<CoHost> arraylist;

        public override int Count
        {
            get
            {
                return coHostList.Count;
            }
        }

        public CohostAdapter(FragmentActivity activity, List<CoHost> coHostList)
        {
            this.activity = activity;
            this.coHostList = coHostList;
            this.arraylist = new List<CoHost>();
            foreach (var obj in coHostList)
            {
                this.arraylist.Add(obj);
            }
        }

        public void filter(string charText)
        {
            try
            {
                charText = charText.ToLower();
                coHostList.Clear();
                if (charText.Length == 0)
                {
                    foreach (var x in arraylist)
                    {
                        coHostList.Add(x);
                    }
                }
                else
                {
                    foreach (CoHost wp in arraylist)
                    {
                        if (wp.CoHostName.ToLower()
                                .Contains(charText))
                        {
                            coHostList.Add(wp);
                        }
                    }
                }
                this.NotifyDataSetChanged();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("CohostAdapter", "filter", ex.StackTrace + ex.Message);
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            if (convertView == null)
            {
                holder = new ViewHolder();
                LayoutInflater inflater = LayoutInflater.From(activity);
                convertView = inflater.Inflate(Resource.Layout.AddCoHostItem, parent, false);
                holder.ivCoHostPic = convertView.FindViewById<ImageView>(Resource.Id.ivCHFICoHostItemImage);
                holder.tvCoHostName = convertView.FindViewById<TextView>(Resource.Id.tvCHFICoHostName);
                holder.chkCoHost = convertView.FindViewById<CheckBox>(Resource.Id.chkCHFICoHost);
                convertView.Tag = (holder);
                convertView.SetTag(Resource.Id.ivCHFICoHostItemImage, holder.ivCoHostPic);
                convertView.SetTag(Resource.Id.tvCHFICoHostName, holder.tvCoHostName);
                convertView.SetTag(Resource.Id.chkCHFICoHost, holder.chkCoHost);

                if (coHostList[position].ID != 0)
                {
                    holder.chkCoHost.SetOnCheckedChangeListener(new CheckChangeListner(coHostList, convertView, this));
                }
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }
            holder.chkCoHost.Tag = position;
            holder.chkCoHost.Visibility = ViewStates.Visible;

            holder.tvCoHostName.Text = coHostList[position].CoHostName;

            if (string.IsNullOrEmpty(coHostList[position].CoHostProfilePic))
            {
                Picasso.With(activity).Load(Resource.Drawable.contact_withoutphoto).Placeholder(Resource.Drawable.contact_withoutphoto)
                          .Resize(80, 80).Transform(new CircleTransformation()).Into(holder.ivCoHostPic);
            }
            else
            {
                Picasso.With(activity).Load(coHostList[position].CoHostProfilePic).Placeholder(Resource.Drawable.contact_withoutphoto)
               .Resize(80, 80).Transform(new CircleTransformation()).Into(holder.ivCoHostPic);
            }

            if (coHostList[position].ID == 0)
            {
                Picasso.With(activity).Load(Resource.Drawable.Share24).Resize(30, 30).CenterInside().Into(holder.ivCoHostPic);
                holder.chkCoHost.Visibility = ViewStates.Gone;
            }
            holder.chkCoHost.Checked = (coHostList[position].isSelected());
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView ivCoHostPic { get; set; }
            public TextView tvCoHostName { get; set; }
            public CheckBox chkCoHost { get; set; }
        }

        private class CheckChangeListner : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private CohostAdapter cohostAdapter;
            private List<CoHost> coHostList;
            private View convertView;

            public CheckChangeListner(List<CoHost> coHostList, View convertView, CohostAdapter cohostAdapter)
            {
                this.coHostList = coHostList;
                this.convertView = convertView;
                this.cohostAdapter = cohostAdapter;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                int getPosition = (int)buttonView.Tag;
                coHostList[getPosition].setSelected(
                        buttonView.Checked);
            }
        }
    }
}