using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using WeClip.Core.Model;
using Android.Provider;
using Android.Database;
using WeClip.Droid.Helper;
using Java.Lang;
using WeClip.Droid.Adapters;
using Android.Text;

namespace WeClip.Droid.AsyncTask
{
    public class GetContactFriendData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<FriendListModel>>
    {
        ProgressDialog p;
        ListView lvContact;
        Activity context;
        List<FriendListModel> _friendList;
        static List<Contact> _contactList;
        List<Contact> _contactListPhoto;
        List<Contact> _contactListEmail;
        List<Contact> _contactListPhone;
        EditText txtSearch;
        FriendListModel friendListModel;
        Contact_FriendAdapter objAdapter;
        private long eventID;

        public GetContactFriendData(ListView lvContact, Activity context, EditText txtSearch, long eventID)
        {
            this.lvContact = lvContact;
            this.context = context;
            this.txtSearch = txtSearch;
            _contactList = new List<Contact>();
            _contactListPhoto = new List<Contact>();
            _contactListEmail = new List<Contact>();
            _contactListPhone = new List<Contact>();
            this.eventID = eventID;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = new ProgressDialog(context);
            p.SetTitle("Please wait...");
            p.SetMessage("Loading contact");
            p.SetCancelable(false);
            p.Show();
        }

        protected override List<FriendListModel> RunInBackground(params Java.Lang.Void[] @params)
        {
            FillContacts();
            FillContactsEmail();
            FillContactsPhoto();
            _friendList = RestSharpCall.GetList<FriendListModel>("Friend/GetEventRequestList?eventID=" + eventID);
            return _friendList;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();

            for (int i = 0; i < _contactList.Count; i++)
            {
                var objE = (from x in _friendList where x.Email == _contactList[i].PhoneNo && x.SignUpType == "E" select x).FirstOrDefault();
                if (objE != null && objE.InvitedContact == "1")
                {
                    objE.FriendName = _contactList[i].DisplayName;
                    objE.PhoneNumber = _contactList[i].PhoneNo;
                    objE.PhoneContactId = _contactList[i].PhoneContactId;
                    objE.Picture = _contactList[i].PhotoId;
                    objE.IsEmail = true;
                    objE.IsContact = true;
                    continue;
                }
                else if (objE != null && objE.IsFriend == "1")
                {
                    objE.FriendName = objE.FriendName;
                    objE.PhoneNumber = _contactList[i].PhoneNo;
                    objE.IsEmail = true;

                    if (objE.Picture == "")
                    {
                        objE.PhoneContactId = _contactList[i].PhoneContactId;
                        objE.Picture = _contactList[i].PhotoId;
                    }

                    continue;
                }

                objE = (from x in _friendList where x.PhoneNumber == _contactList[i].PhoneNo && x.SignUpType == "P" select x).FirstOrDefault();
                if (objE != null && objE.InvitedContact == "1")
                {
                    objE.FriendName = _contactList[i].DisplayName;
                    objE.PhoneNumber = _contactList[i].PhoneNo;
                    objE.PhoneContactId = _contactList[i].PhoneContactId;
                    objE.Picture = _contactList[i].PhotoId;
                    objE.IsContact = true;
                    objE.IsEmail = false;
                    continue;
                }
                else if (objE != null && objE.IsFriend == "1")
                {
                    objE.FriendName = objE.FriendName;
                    objE.PhoneNumber = _contactList[i].PhoneNo;
                    objE.IsEmail = false;
                    objE.IsContact = true;
                    if (objE.Picture == "")
                    {
                        objE.PhoneContactId = _contactList[i].PhoneContactId;
                        objE.Picture = _contactList[i].PhotoId;
                    }

                    continue;
                }

                friendListModel = new FriendListModel();
                friendListModel.FriendName = _contactList[i].DisplayName;
                friendListModel.PhoneNumber = _contactList[i].PhoneNo;
                friendListModel.Picture = _contactList[i].PhotoId;
                friendListModel.IsContact = true;
                friendListModel.IsEmail = _contactList[i].isEmailAddress;
                friendListModel.ID = 0;
                friendListModel.PhoneContactId = _contactList[i].PhoneContactId;
                _friendList.Add(friendListModel);
            }

            var invitedFriends = (from x in _friendList where x.PhoneNumber == "" && x.IsFriend == "1" select x).ToList();
            invitedFriends.Select(c => { c.PhoneNumber = c.Email; c.IsEmail = true; return c; }).ToList();

            //If Other user invited to that event and that user not in over contact list then we have remove that contact
            _friendList = (from x in _friendList
                           where x.FriendName != null && x.FriendName != ""
                           orderby x.isInvitedFriend descending, x.InvitedContact descending,
                           x.IsFriend descending, x.IsContact descending, x.FriendName, x.Picture, x.PhoneContactId
                           select x).ToList();

            objAdapter = new Contact_FriendAdapter(_friendList, context, eventID);
            lvContact.Adapter = objAdapter;
            txtSearch.AddTextChangedListener(new textwatcher(this));
            lvContact.TextFilterEnabled = true;
            lvContact.ChoiceMode = ChoiceMode.None;
        }

        void FillContacts()
        {
            try
            {
                string[] projection =
                    {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
               ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber,
               ContactsContract.CommonDataKinds.Phone.Number  };

                ContentResolver cr = context.ContentResolver;
                ICursor cursor = cr.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, projection, null, null, null);
                _contactListPhone = new List<Contact>();

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        Contact c = new Contact();
                        string c_name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                        c.Id = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                        c.PhoneContactId = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                        c.DisplayName = c_name;
                        string hasPhone = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                        if (hasPhone == "1")
                        {
                            c.PhoneNo = cursor.GetString(cursor.GetColumnIndex(projection[3]));
                        }
                        _contactListPhone.Add(c);

                    } while (cursor.MoveToNext());
                }
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("GetContactFriendData", "FillContacts", ex.Message + ex.StackTrace).Execute();
            }
        }

        void FillContactsEmail()
        {
            try
            {
                var uri = ContactsContract.CommonDataKinds.Email.ContentUri;//ContactsContract.Contacts.ContentUri;

                string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                     ContactsContract.CommonDataKinds.Email.Address
            };
                ICursor cursor = context.ContentResolver.Query(uri, projection, null, null, null);

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        Contact c = new Contact();
                        c.DisplayName = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                        c.PhoneNo = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                        c.Id = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                        c.PhoneContactId = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                        c.isEmailAddress = true;
                        _contactListPhone.Add(c);
                    } while (cursor.MoveToNext());
                }

            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("GetContactFriendData", "FillContactsEmail", ex.Message + ex.StackTrace).Execute();
            }
        }

        void FillContactsPhoto()
        {
            try
            {
                var uri = ContactsContract.Contacts.ContentUri;

                string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                ContactsContract.Contacts.InterfaceConsts.PhotoId
            };

                ICursor cursor = context.ContentResolver.Query(uri, projection, null, null, null);
                _contactListPhoto = new List<Contact>();

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        Contact c = new Contact();
                        c.Id = cursor.GetLong(
                        cursor.GetColumnIndex(projection[0]));
                        c.PhoneContactId = cursor.GetLong(cursor.GetColumnIndex(projection[0]));

                        c.PhotoId = cursor.GetString(
                        cursor.GetColumnIndex(projection[2]));
                        c.DisplayName = cursor.GetString(
                       cursor.GetColumnIndex(projection[1]));
                        _contactListPhoto.Add(c);
                    } while (cursor.MoveToNext());
                }

                var query = (from c1 in _contactListPhone
                             join c2 in _contactListPhoto on c1.DisplayName equals c2.DisplayName
                             select new { c1, c2 }).ToList();

                query = query.GroupBy(c => c.c1.PhoneNo).Select(g => g.First()).ToList();


                int count = query.Count();
                _contactList = new List<Contact>();

                foreach (var c in query)
                {
                    var q = _contactList.Where(a => a.DisplayName == c.c1.DisplayName && a.PhoneNo == c.c1.PhoneNo);
                    if (!q.Any())
                    {

                        var temp = c.c1.PhoneNo.Replace(" ", string.Empty);
                        temp = temp.Replace("(", string.Empty);
                        temp = temp.Replace(")", string.Empty);
                        temp = temp.Replace("-", string.Empty);

                        _contactList.Add(new Contact
                        {
                            DisplayName = c.c1.DisplayName,
                            Id = c.c2.Id,
                            PhoneContactId = c.c2.PhoneContactId,
                            PhoneNo = temp,
                            PhotoId = c.c2.PhotoId,
                            isEmailAddress = c.c1.isEmailAddress
                        }
                        );
                    }
                }
                _contactList = _contactList.OrderBy(a => a.DisplayName).ToList();
            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("GetContactFriendData", "FillContactsPhoto", ex.Message + ex.StackTrace).Execute();
            }
        }

        private class textwatcher : Java.Lang.Object, ITextWatcher
        {
            private GetContactFriendData getContactFriendData;

            public textwatcher(GetContactFriendData getContactFriendData)
            {
                this.getContactFriendData = getContactFriendData;
            }

            public void AfterTextChanged(IEditable s)
            {
            }

            public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
            {
            }

            public void OnTextChanged(ICharSequence s, int start, int before, int count)
            {
                string text = getContactFriendData.txtSearch.Text.ToString()
                           .ToLower();
                getContactFriendData.objAdapter.filter(text);
            }
        }
    }
}