using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Provider;
using Android.Database;

namespace WeClip.Droid.AsyncTask
{
    public class GetContacts : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<SP_UserListWithFriends_Result>>
    {
        ProgressDialog p;
        ListView lvContact;
        Activity context;
        List<SP_UserListWithFriends_Result> friendList;
        static List<Contact> _contactList;
        List<Contact> _contactListPhoto;
        List<Contact> _contactListPhone;
        EditText txtSearch;

        public GetContacts(ListView lvContact, Activity context, EditText txtSearch)
        {
            this.lvContact = lvContact;
            this.context = context;
            this.txtSearch = txtSearch;
            friendList = new List<SP_UserListWithFriends_Result>();
            _contactList = new List<Contact>();
            _contactListPhoto = new List<Contact>();
            _contactListPhone = new List<Contact>();
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

        protected override List<SP_UserListWithFriends_Result> RunInBackground(params Java.Lang.Void[] @params)
        {
            FillContacts();
            FillContactsEmail();
            FillContactsPhoto();
            friendList = RestSharpCall.GetList<SP_UserListWithFriends_Result>("Friend/GetUserWithFriends");
            _contactList = _contactList.Select(c => { c.Id = 0; return c; }).ToList();
            return friendList;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            p.Dismiss();

            var weClipUser = (from con in _contactList
                              where friendList.Any(x1 => x1.ID != 0 &&
                          ((x1.Email == (con.PhoneNo == null ? "" : con.PhoneNo) && x1.SignUpType == "E") || (x1.PhoneNumber == (con.PhoneNo == null ? "" : con.PhoneNo) && x1.SignUpType == "P")))
                              select con).ToList();

            weClipUser.Select(c => { c.isWeClipUser = true; return c; }).ToList();

            var invited = (from con in _contactList
                           where friendList.Any(x1 => x1.InvitedContact == "1" &&
                           ((x1.Email == (con.PhoneNo == null ? "" : con.PhoneNo) && x1.SignUpType == "E") || (x1.PhoneNumber == (con.PhoneNo == null ? "" : con.PhoneNo) && x1.SignUpType == "P")))
                           select         con).ToList();

            invited.Select(c => { c.isInvited = true; return c; }).ToList();

            var friend = (from con in _contactList
                          where friendList.Any(x => x.IsFriend == "1" && ((x.Email == (con.PhoneNo == null ? "" : con.PhoneNo) && x.SignUpType == "E") || (x.PhoneNumber == (con.PhoneNo == null ? "" : con.PhoneNo) && x.SignUpType == "P")))
                          select con)
                     .ToList();

            friend.Select(c => { c.isFriend = true; return c; }).ToList();

            friend = (from contactno in _contactList
                      where friendList.Any(x => x.isFriendRequestPending == "1" && ((x.Email == contactno.PhoneNo && x.SignUpType == "E") || (x.PhoneNumber == contactno.PhoneNo && x.SignUpType == "P")))
                      select contactno)
                   .ToList();

            friend.Select(c => { c.isFriendRequestPending = true; return c; }).ToList();

            _contactList = _contactList.OrderBy(x => x.isWeClipUser != true).ToList();

            var objFirstContactItem = (from x in _contactList where x.isWeClipUser != true select x).FirstOrDefault();

            var objFriendItem = (from x in _contactList where x.isWeClipUser == true select x).FirstOrDefault();

            if (objFriendItem != null)
            {
                if (objFirstContactItem != null)
                {
                    objFirstContactItem.separator = true;
                }
            }

            var contactAdapter = new InviteContactAdapter(context, _contactList);
            lvContact.Adapter = contactAdapter;

            txtSearch.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                if (txtSearch.Text.Equals(""))
                {
                    contactAdapter = new InviteContactAdapter(context, _contactList); //If you remove all filtered text, return to base list
                    lvContact.Adapter = contactAdapter;
                }
                else
                {
                    var _contactList1 = _contactList.
                      Where(a => a.DisplayName.Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()) || a.PhoneNo.Trim().Contains(txtSearch.Text.Trim())).OrderBy(a => a.DisplayName).ToList();

                    _contactList1 = _contactList1.OrderBy(x => x.isWeClipUser != true).ToList();

                    foreach (var x in _contactList1)
                    {
                        x.separator = false;
                    }

                    var objFirstContactItem1 = (from x in _contactList where x.isWeClipUser != true select x).FirstOrDefault();
                    if (objFirstContactItem1 != null)
                    {
                        objFirstContactItem1.separator = true;
                    }

                    contactAdapter = new InviteContactAdapter(context, _contactList1); //Otherwise use your filtered list
                    lvContact.Adapter = contactAdapter;
                }
            };
            base.OnPostExecute(result);
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
                new CrashReportAsync("GetContacts", "FillContacts", ex.Message + ex.StackTrace).Execute();
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
                new CrashReportAsync("GetContacts", "FillContactsEmail", ex.Message + ex.StackTrace).Execute();
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
                new CrashReportAsync("GetContacts", "FillContactsPhoto", ex.Message + ex.StackTrace).Execute();
            }
        }

    }
}