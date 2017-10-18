using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using Android.Provider;
using WeClip.Droid.Helper;
using Square.Picasso;
using Android.Graphics;
using WeClip.Droid.AsyncTask;
using Android.OS;
using WeClip.Core.Common;

namespace WeClip.Droid.Adapters
{
    public class InviteContactAdapter : BaseAdapter<Contact>
    {
        private Activity context;
        private List<Contact> contactlist;
        InviteContactHolder viewHolder;

        public InviteContactAdapter(Activity context, List<Contact> contactlist)
        {
            this.context = context;
            this.contactlist = contactlist;
        }

        public override int Count
        {
            get
            {
                return contactlist.Count;
            }
        }

        public override Contact this[int position]
        {
            get
            {
                return contactlist[position];
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
            if (convertView == null)
            {
                viewHolder = new InviteContactHolder();
                LayoutInflater inflater = context.LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.InviteContactViewItem, parent, false);
                viewHolder.tvContactName = convertView.FindViewById<TextView>(Resource.Id.tvContactName);
                viewHolder.tvContactPhone = convertView.FindViewById<TextView>(Resource.Id.tvContactPhoneNo);
                viewHolder.ivContactPic = convertView.FindViewById<ImageView>(Resource.Id.ivContactImage);
                viewHolder.btnSendInvite = convertView.FindViewById<Button>(Resource.Id.btnContactInvite);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as InviteContactHolder;
            }

            viewHolder.tvContactName.Text = contactlist[position].DisplayName;
            viewHolder.tvContactPhone.Text = contactlist[position].PhoneNo;

            if (contactlist[position].PhotoId == null)
            {
                viewHolder.ivContactPic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                var contactUri = ContentUris.WithAppendedId(
                    ContactsContract.Contacts.ContentUri, contactlist[position].PhoneContactId);
                var contactPhotoUri = Android.Net.Uri.WithAppendedPath(contactUri, ContactsContract.Contacts.Photo.ContentDirectory);
                Picasso.With(context).Load(contactPhotoUri).Placeholder(Resource.Drawable.contact_withoutphoto)
           .Resize(80, 80).Transform(new CircleTransformation()).Into(viewHolder.ivContactPic);
            }

            viewHolder.btnSendInvite.SetTextSize(Android.Util.ComplexUnitType.Sp, 16);

            if (contactlist[position].isWeClipUser == true)
            {
                if (contactlist[position].isFriend == true)
                {
                    viewHolder.btnSendInvite.Text = "Following";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                if (contactlist[position].isFriendRequestPending == true)
                {
                    viewHolder.btnSendInvite.Text = "Request sent";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    viewHolder.btnSendInvite.SetTextSize(Android.Util.ComplexUnitType.Sp, 12);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                {
                    viewHolder.btnSendInvite.Text = "Follow";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#04c285"));
                }
            }

            else
            {
                if (contactlist[position].isInvited == true)
                {
                    viewHolder.btnSendInvite.Text = "Invited";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_fill_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#ffffff"));
                }
                else
                {
                    viewHolder.btnSendInvite.Text = "Invite";
                    viewHolder.btnSendInvite.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                    viewHolder.btnSendInvite.SetTextColor(Color.ParseColor("#04c285"));
                }
            }

            if (contactlist[position].separator == true)
            {
                convertView.SetBackgroundResource(Resource.Drawable.ListViewBackGround);
            }
            else
            {
                convertView.SetBackgroundColor(Color.Transparent);
            }
            viewHolder.btnSendInvite.SetOnClickListener(new btnInviteClick(context, contactlist[position], this, viewHolder.btnSendInvite));
            return convertView;
        }

        public class InviteContactHolder : Java.Lang.Object
        {
            public ImageView ivContactPic { get; set; }
            public TextView tvContactName { get; set; }
            public TextView tvContactPhone { get; set; }
            public Button btnSendInvite { get; set; }
        }

        private class btnInviteClick : Java.Lang.Object, View.IOnClickListener
        {
            private Button btnSendInvite;
            private Contact contact;
            private Activity context;
            private InviteContactAdapter inviteContactAdapter;

            public btnInviteClick(Activity context, Contact contact, InviteContactAdapter inviteContactAdapter, Button btnSendInvite)
            {
                this.context = context;
                this.contact = contact;
                this.inviteContactAdapter = inviteContactAdapter;
                this.btnSendInvite = btnSendInvite;
            }

            public void OnClick(View v)
            {
                if (!contact.isFriend && !contact.isFriendRequestPending && !contact.isInvited)
                {
                    new postInviteContact(context, contact, btnSendInvite, inviteContactAdapter).Execute();
                }
                else
                {
                    if (contact.isWeClipUser == true)
                    {
                        if (contact.isFriendRequestPending == true)
                        {
                            showDiloage("Cancel Request", btnSendInvite, contact);
                        }
                        else
                        {
                            showDiloage("Unfollow", btnSendInvite, contact);
                        }
                    }
                    else
                    {
                        showDiloage("Cancel invitation", btnSendInvite, contact);
                    }
                }
            }

            private void showDiloage(string positiveBtnText, Button btnFollowing, Contact userData)
            {
                Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(context);
                alertDialog.SetTitle("Are you sure ?");
                alertDialog.SetPositiveButton(positiveBtnText, (senderAlert, args) =>
                {
                    new postCancelFriendRequestOrInvitation(context, btnFollowing, userData).Execute();
                });
                alertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }

            private class postCancelFriendRequestOrInvitation : AsyncTask<Java.Lang.Object, Java.Lang.Object, JsonResult>
            {
                private Button btnFollowing;
                private Activity context;
                private Contact userData;
                private JsonResult jResult;

                public postCancelFriendRequestOrInvitation(Activity context, Button btnFollowing, Contact userData)
                {
                    this.context = context;
                    this.btnFollowing = btnFollowing;
                    this.userData = userData;
                }

                protected override JsonResult RunInBackground(params Java.Lang.Object[] @params)
                {
                    jResult = RestSharpCall.Post<JsonResult>(userData, "Friend/CancelInvitedContact");
                    return jResult;
                }

                protected override void OnPostExecute(JsonResult result)
                {
                    base.OnPostExecute(result);

                    if (result.Success == true)
                    {
                        if (userData.isWeClipUser == true)
                        {
                            btnFollowing.Text = "Follow";
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#04c285"));

                            if (userData.isFriendRequestPending == true)
                            {
                                userData.isFriendRequestPending = false;
                            }

                            if (userData.isFriend == true)
                            {
                                userData.isFriend = false;
                            }
                        }
                        else
                        {
                            btnFollowing.Text = "Invite";
                            btnFollowing.SetBackgroundResource(Resource.Drawable.btn_round_green_border_padding);
                            btnFollowing.SetTextColor(Color.ParseColor("#04c285"));

                            if (userData.isInvited == true)
                            {
                                userData.isInvited = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
