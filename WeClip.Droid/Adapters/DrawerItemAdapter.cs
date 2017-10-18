using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using WeClip.Droid.Helper;

namespace WeClip.Droid.Adapters
{
    public class DrawerItemAdapter : BaseAdapter<DrawerItemModel>
    {
        List<DrawerItemModel> items;
        Activity context;

        public DrawerItemAdapter(Activity context, List<DrawerItemModel> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override DrawerItemModel this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.drawer_menu_item, null);

            TextView drawerItemText = view.FindViewById<TextView>(Resource.Id.drawerItemText);

            DrawerItemModel item = items[position];
            drawerItemText.Text = item.name;

            return view;
        }

    }
}