using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Content;
using Android.Provider;
using Square.Picasso;
using WeClip.Droid.Helper;
using WeClip.Droid.AsyncTask;
using Android.Graphics;

namespace WeClip.Droid.Adapters
{
    public class Contact_FriendAdapter : BaseAdapter
    {
        public Activity context;
        private List<FriendListModel> contactList;
        private List<FriendListModel> arraylist;
        private long eventID;

        public Contact_FriendAdapter(List<FriendListModel> contactList, Activity context, long eventID) : base()
        {
            this.contactList = contactList;
            this.context = context;
            this.eventID = eventID;
            this.arraylist = new List<FriendListModel>();
            foreach (var obj in contactList)
            {
                this.arraylist.Add(obj);
            }
        }

        public override int Count
        {
            get
            {
                return contactList.Count;
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
            ViewHolderClass holder;
            if (convertView == null)
            {
                holder = new ViewHolderClass();
                int layout = Resource.Layout.Contact_FriendItem;
                LayoutInflater inflater = LayoutInflater.From(context);
                convertView = inflater.Inflate(layout, parent, false);
                holder.name = convertView.FindViewById<TextView>(Resource.Id.tvContactFriendName);
                holder.number = convertView.FindViewById<TextView>(Resource.Id.tvContactFriendNumber);
                holder.photo = convertView.FindViewById<ImageView>(Resource.Id.ivContactFriendImage);
                holder.btn = convertView.FindViewById<Button>(Resource.Id.btnContactFriendInvite);
                convertView.Tag = (holder);
                convertView.SetTag(Resource.Id.tvContactFriendName, holder.name);
                convertView.SetTag(Resource.Id.tvContactFriendNumber, holder.number);
                convertView.SetTag(Resource.Id.ivContactFriendImage, holder.photo);
                convertView.SetTag(Resource.Id.btnContactFriendInvite, holder.btn);
            }
            else
            {
                holder = (ViewHolderClass)convertView.Tag;
            }
            holder.btn.Tag = position;
            holder.btn.SetOnClickListener(new btnInviteClick(contactList[position], context, eventID, this));
            holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
            holder.btn.SetTextColor(Color.ParseColor("#04c285"));

            if (contactList[position].IsContact == true)
            {
                if (contactList[position].Picture == null || contactList[position].Picture == "")
                    holder.photo.SetImageResource(Resource.Drawable.contact_withoutphoto);
                else
                {
                    var contactUri = ContentUris.WithAppendedId(
                               ContactsContract.Contacts.ContentUri, contactList[position].PhoneContactId);

                    var contactPhotoUri = Android.Net.Uri.WithAppendedPath(contactUri, ContactsContract.Contacts.Photo.ContentDirectory);

                    Picasso.With(context).Load(contactPhotoUri).Placeholder(Resource.Drawable.contact_withoutphoto)
               .Resize(150, 150).Transform(new CircleTransformation()).Into(holder.photo);
                }

                if (contactList[position].InvitedContact == "1")
                {
                    holder.btn.Text = "Invited";
                    holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    holder.btn.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                {
                    holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    holder.btn.SetTextColor(Color.ParseColor("#04c285"));
                    holder.btn.Text = "Invite";
                }
            }
            else
            {
                if (contactList[position].Picture == null || contactList[position].Picture == "")
                    holder.photo.SetImageResource(Resource.Drawable.contact_withoutphoto);
                else
                {
                    Picasso.With(context).Load(contactList[position].Picture).Placeholder(Resource.Drawable.contact_withoutphoto)
               .Resize(150, 150).Transform(new CircleTransformation()).Into(holder.photo);
                }
                if (contactList[position].isInvitedFriend == "1")
                {
                    holder.btn.Text = "Invited";
                    holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    holder.btn.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                {
                    holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    holder.btn.SetTextColor(Color.ParseColor("#04c285"));
                    holder.btn.Text = "Invite";
                }
            }
            if (contactList[position].ResponseText != null && contactList[position].ResponseText != "" && contactList[position].isInvitedFriend != "")
            {
                holder.btn.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                holder.btn.SetTextColor(Color.ParseColor("#ffffff"));
                holder.btn.Text = contactList[position].ResponseText;
            }

            holder.name.Text = contactList[position].FriendName;
            holder.number.Text = contactList[position].PhoneNumber;

            return convertView;
        }

        public void filter(string charText)
        {
            charText = charText.ToLower();
            contactList.Clear();
            if (charText.Length == 0)
            {
                foreach (var x in arraylist)
                {
                    contactList.Add(x);
                }
            }
            else
            {
                foreach (FriendListModel wp in arraylist)
                {

                    if (wp.FriendName.ToLower()
                            .Contains(charText))
                    {
                        contactList.Add(wp);
                    }
                }
            }
            NotifyDataSetChanged();
        }

        public class ViewHolderClass : Java.Lang.Object
        {
            public TextView name, number;
            public ImageView photo;
            public Button btn;
        }

        private class btnInviteClick : Java.Lang.Object, View.IOnClickListener
        {
            private Activity context;
            private long eventID;
            private FriendListModel friendListModel;
            private Contact_FriendAdapter objContact_FriendAdapter;

            public btnInviteClick(FriendListModel friendListModel, Activity context, long eventID, Contact_FriendAdapter objContact_FriendAdapter)
            {
                this.friendListModel = friendListModel;
                this.context = context;
                this.eventID = eventID;
                this.objContact_FriendAdapter = objContact_FriendAdapter;
            }

            public void OnClick(View v)
            {

                if (friendListModel.isInvitedFriend != "1" && friendListModel.InvitedContact != "1")
                {
                    new postEventRequest(friendListModel, context, eventID, objContact_FriendAdapter).Execute();
                }
            }
        }
    }
}
