using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Adapters;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    class GetCoHost : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<CoHost>>
    {
        private FragmentActivity activity;
        private List<CoHost> coHostList;
        private long eventID;
        private CohostAdapter adapter;
        private View rootView;
        private EditText txtSearch;
        private TextView tvNoCoHost;
        protected ProgressDialog p;
      //  List<Contact> _contactListEmail;
     //   List<Contact> _contactListPhone;

        public GetCoHost(View rootView, FragmentActivity activity, long eventID)
        {
            this.rootView = rootView;
            this.activity = activity;
            this.eventID = eventID;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            p = ProgressDialog.Show(activity, "", "Please wait");
        }

        protected override List<CoHost> RunInBackground(params Java.Lang.Void[] @params)
        {
            coHostList = RestSharpCall.GetList<CoHost>("Friend/GetCoHostList?eventId=" + eventID);
            return coHostList;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            CoHostListItem.getCoHostList.Clear();

            if (result != null)
            {
                var lvCoHost = rootView.FindViewById<ListView>(Resource.Id.lvCohost);
                txtSearch = rootView.FindViewById<EditText>(Resource.Id.etCoHostSearch);
                tvNoCoHost = rootView.FindViewById<TextView>(Resource.Id.tvNoCoHost);

                var btnAddAdmin = rootView.FindViewById<ImageView>(Resource.Id.btnSaveCoHost);
                var btnBack = rootView.FindViewById<ImageView>(Resource.Id.ivAddCoHostBack);

                coHostList.Add(
                    new CoHost
                    {
                        CoHostName = "Invite Contacts to WeClip",
                        ID = 0
                    });

                if (coHostList != null && coHostList.Count > 0)
                {
                    foreach (var x in coHostList)
                    {
                        if (x.IsAdmin)
                        {
                            x.setSelected(true);
                        }
                        CoHostListItem.getCoHostList.Add(x);
                    }
                }
                else
                {
                    tvNoCoHost.Visibility = ViewStates.Visible;
                }

                adapter = new CohostAdapter(activity, coHostList);
                lvCoHost.Adapter = adapter;
                lvCoHost.OnItemClickListener = (new lvOnItemClickListner(coHostList, activity, lvCoHost));
                btnAddAdmin.Click += BtnAddAdmin_Click;
                btnBack.Click += BtnBack_Click;
                txtSearch.AddTextChangedListener(new textwatcher(this));
                lvCoHost.TextFilterEnabled = true;
                lvCoHost.ChoiceMode = ChoiceMode.None;
            }
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            activity.OnBackPressed();
        }

        private void BtnAddAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                List<EventAdmin> eventAdminList = new List<EventAdmin>();
                foreach (CoHost model in CoHostListItem.getCoHostList)
                {
                    if (model.isSelected())
                    {
                        var adminmodel = new EventAdmin
                        {
                            EventID = eventID,
                            FriendID = model.CoHostUserID,
                            UserID = Convert.ToInt64(GlobalClass.UserID)
                        };
                        eventAdminList.Add(adminmodel);
                    }
                }

                if (eventAdminList.Count > 0)
                {
                    new PostAdminData(eventAdminList, activity).Execute();
                }
                else
                {
                    AlertBox.Create("Error", "Select friend.", activity);
                }

            }
            catch (System.Exception ex)
            {
                new CrashReportAsync("", "", ex.StackTrace + ex.Message).Execute();
            }
        }

        private class lvOnItemClickListner : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private FragmentActivity activity;
            private List<CoHost> coHost;
            private ListView lvCoHost;

            public lvOnItemClickListner(List<CoHost> coHost, FragmentActivity activity, ListView lvCoHost)
            {
                this.activity = activity;
                this.lvCoHost = lvCoHost;
                this.coHost = coHost;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                CheckBox chk = (CheckBox)view.FindViewById(Resource.Id.chkCHFICoHost);
                CoHost item = CoHostListItem.getCoHostList[position];
                if (item.isSelected())
                {
                    item.setSelected(false);
                    chk.Checked = (false);
                }
                else
                {
                    item.setSelected(true);
                    chk.Checked = (true);
                }

                if (item.ID == 0)
                {
                    var fragment = new InviteContacts();
                    Android.Support.V4.App.FragmentManager FragmentManager = activity.SupportFragmentManager;
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment, "co-host").AddToBackStack("co-host").Commit();
                }
            }
        }

        public class CoHostListItem
        {
            public static List<CoHost> getCoHostList = new List<CoHost>();
        }

        private class textwatcher : Java.Lang.Object, ITextWatcher
        {
            readonly GetCoHost getCoHost;
            public textwatcher(GetCoHost getCoHost)
            {
                this.getCoHost = getCoHost;
            }

            public void AfterTextChanged(IEditable s)
            {
            }

            public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
            {
            }

            public void OnTextChanged(ICharSequence s, int start, int before, int count)
            {
                string text = getCoHost.txtSearch.Text.ToString().ToLower();
                getCoHost.adapter.filter(text);
            }
        }

        private class PostAdminData : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
        {
            private FragmentActivity activity;
            private List<EventAdmin> eventAdminList;
            private JsonResult jResult;
            private ProgressDialog p;

            public PostAdminData(List<EventAdmin> eventAdminList, FragmentActivity activity)
            {
                this.eventAdminList = eventAdminList;
                this.activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                p = ProgressDialog.Show(activity, "", "Please wait");
            }

            protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
            {
                jResult = RestSharpCall.Post<JsonResult>(eventAdminList, "Event/MakeAdmin");
                return jResult;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                p.Dismiss();
                if (result != null)
                {
                    if (jResult.Success == true)
                    {
                        AlertBox.Create("Success", "Send Successfully", activity);
                    }
                    else
                    {
                        AlertBox.Create("Error", "Error occured", activity);
                    }
                }
                else
                {
                    AlertBox.Create("Error", "Error occured", activity);
                }
            }

        }
    }
}