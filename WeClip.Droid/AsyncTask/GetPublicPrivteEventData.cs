using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using WeClip.Core.Model;
using WeClip.Droid.Helper;
using WeClip.Droid.Adapters;
using Android.Support.V4.App;
using System.Linq;
using WeClip.Core.Common;
using Android.Widget;
using Android.Views;
using Android.Support.Design.Widget;
using static Android.Support.Design.Widget.TabLayout;
using System;
using Android.Support.V4.Widget;
using Java.Lang;

namespace WeClip.Droid.AsyncTask
{
    public class GetPublicPrivteEventData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<GetAllEventList>>, IOnTabSelectedListener
    {
        ProgressDialog p;
        RecyclerView rvPublicEvent, rvPrivateEvent;
        FragmentActivity context;
        List<GetAllEventList> listPublicEvent;
        private View rootView;
        private TabLayout tablayout;
        HomeAdapterPrivateEvent privateEventAdapter;
        List<GetAllEventList> privateEventList, publicEventList;

        LinearLayout emptyPrivateEvent;
        private SwipeRefreshLayout mSwipeRefreshLayoutpublic;
        private SwipeRefreshLayout mSwipeRefreshLayoutPrivate;

        public GetPublicPrivteEventData(View rootView, RecyclerView rvPublicEvent, RecyclerView rvPrivateEvent, FragmentActivity context, TabLayout tablayout, SwipeRefreshLayout mSwipeRefreshLayoutpublic, SwipeRefreshLayout mSwipeRefreshLayoutPrivate)
        {
            this.rootView = rootView;
            this.rvPublicEvent = rvPublicEvent;
            this.rvPrivateEvent = rvPrivateEvent;
            this.context = context;
            this.tablayout = tablayout;
            this.mSwipeRefreshLayoutpublic = mSwipeRefreshLayoutpublic;
            this.mSwipeRefreshLayoutPrivate = mSwipeRefreshLayoutPrivate;
            listPublicEvent = new List<GetAllEventList>();
            p = ProgressDialog.Show(context, "Please wait...", "Loading data...");
        }

        protected override List<GetAllEventList> RunInBackground(params Java.Lang.Void[] @params)
        {
            listPublicEvent = RestSharpCall.GetList<GetAllEventList>("EventRequest/GetMyEventList?id=" + GlobalClass.UserID);
            return listPublicEvent;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            p.Dismiss();
            tablayout.SetOnTabSelectedListener(this);
            emptyPrivateEvent = rootView.FindViewById<LinearLayout>(Resource.Id.emptyPrivateEvent);

            if (result != null && listPublicEvent.Count > 0)
            {
                try
                {
                    publicEventList = (from x in listPublicEvent where x.EventType == "P" select x).ToList();
                    privateEventList = (from x in listPublicEvent where x.EventType == "M" select x).ToList();

                    HomeAdapterPublicEvent publicEventAdapter = new HomeAdapterPublicEvent(publicEventList, context);
                    privateEventAdapter = new HomeAdapterPrivateEvent(privateEventList, context);
                    rvPublicEvent.SetAdapter(publicEventAdapter);
                    rvPrivateEvent.SetAdapter(privateEventAdapter);
                    mSwipeRefreshLayoutpublic.SetOnRefreshListener(new onRefereshPublicLayout(publicEventAdapter, context, mSwipeRefreshLayoutpublic, rvPublicEvent));
                    mSwipeRefreshLayoutPrivate.SetOnRefreshListener(new onRefereshPrivateLayout(privateEventAdapter, context, mSwipeRefreshLayoutPrivate, rvPrivateEvent));


                    if (privateEventList.Count == 0)
                    {
                        emptyPrivateEvent.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        emptyPrivateEvent.Visibility = ViewStates.Gone;
                    }
                }
                catch (System.Exception ex)
                {
                    new CrashReportAsync("GetPublicEventData", "OnPostExecute", ex.StackTrace + ex.Message).Execute();
                }
            }

            else
            {
                emptyPrivateEvent.Visibility = ViewStates.Visible;
            }

        }

        public void OnTabReselected(Tab tab)
        {
        }

        private class onRefereshPublicLayout : Java.Lang.Object, SwipeRefreshLayout.IOnRefreshListener
        {
            private FragmentActivity context;
            private SwipeRefreshLayout mSwipeRefreshLayout;
            private HomeAdapterPublicEvent publicEventAdapter;
            private RecyclerView rvPublicEvent;

            public onRefereshPublicLayout(HomeAdapterPublicEvent publicEventAdapter, FragmentActivity context, SwipeRefreshLayout mSwipeRefreshLayout, RecyclerView rvPublicEvent)
            {
                this.rvPublicEvent = rvPublicEvent;
                this.mSwipeRefreshLayout = mSwipeRefreshLayout;
                this.publicEventAdapter = publicEventAdapter;
                this.context = context;
            }

            public void OnRefresh()
            {
                new Handler().PostDelayed(() => Dosomething(publicEventAdapter, context), 2500);
                mSwipeRefreshLayout.Refreshing = (false);
            }

            private void Dosomething(HomeAdapterPublicEvent publicEventAdapter, FragmentActivity context)
            {
                new PullToRefreshGetData(context, publicEventAdapter, rvPublicEvent).Execute();
            }

            private class PullToRefreshGetData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<GetAllEventList>>
            {
                private FragmentActivity context;
                private HomeAdapterPublicEvent publicEventAdapter;
                private RecyclerView rvPublicEvent;
                List<GetAllEventList> listPublicEvent;

                public PullToRefreshGetData(FragmentActivity context, HomeAdapterPublicEvent publicEventAdapter, RecyclerView rvPublicEvent)
                {
                    this.context = context;
                    this.publicEventAdapter = publicEventAdapter;
                    this.rvPublicEvent = rvPublicEvent;
                }

                protected override List<GetAllEventList> RunInBackground(params Java.Lang.Void[] @params)
                {
                    listPublicEvent = RestSharpCall.GetList<GetAllEventList>("EventRequest/GetMyEventList?id=" + GlobalClass.UserID);
                    return listPublicEvent;
                }

                protected override void OnPostExecute(Java.Lang.Object result)
                {
                    base.OnPostExecute(result);
                    var publicEventList = (from x in listPublicEvent where x.EventType == "P" select x).ToList();
                    publicEventAdapter = new HomeAdapterPublicEvent(publicEventList, context);
                    rvPublicEvent.SetAdapter(publicEventAdapter);
                    rvPublicEvent.Invalidate();
                }
            }
        }

        public void OnTabSelected(Tab tab)
        {
            switch (tab.Position)
            {

                case 0:
                    mSwipeRefreshLayoutPrivate.Visibility = ViewStates.Visible;
                    mSwipeRefreshLayoutpublic.Visibility = ViewStates.Gone;
                    if (privateEventList != null)
                    {
                        if (privateEventList.Count == 0)
                        {
                            emptyPrivateEvent.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            emptyPrivateEvent.Visibility = ViewStates.Gone;
                        }
                    }

                    break;

                case 1:
                    mSwipeRefreshLayoutpublic.Visibility = ViewStates.Visible;
                    mSwipeRefreshLayoutPrivate.Visibility = ViewStates.Gone;
                    emptyPrivateEvent.Visibility = ViewStates.Gone;

                    if (publicEventList != null)
                    {
                        if (publicEventList.Count == 0)
                        {
                            emptyPrivateEvent.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            emptyPrivateEvent.Visibility = ViewStates.Gone;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void OnTabUnselected(Tab tab)
        {

        }

        private class onRefereshPrivateLayout : Java.Lang.Object, SwipeRefreshLayout.IOnRefreshListener
        {
            private FragmentActivity context;
            private SwipeRefreshLayout mSwipeRefreshLayout;
            private HomeAdapterPrivateEvent privateEventAdapter;
            private RecyclerView rvPrivateEvent;

            public onRefereshPrivateLayout(HomeAdapterPrivateEvent privateEventAdapter, FragmentActivity context, SwipeRefreshLayout mSwipeRefreshLayout, RecyclerView rvPrivateEvent)
            {
                this.rvPrivateEvent = rvPrivateEvent;
                this.mSwipeRefreshLayout = mSwipeRefreshLayout;
                this.privateEventAdapter = privateEventAdapter;
                this.context = context;
            }

            public void OnRefresh()
            {
                new Handler().PostDelayed(() => Dosomething(privateEventAdapter, context), 2500);
                mSwipeRefreshLayout.Refreshing = (false);
            }

            private void Dosomething(HomeAdapterPrivateEvent privateEventAdapter, FragmentActivity context)
            {
                new PullToRefreshGetPrivateData(context, privateEventAdapter, rvPrivateEvent).Execute();
            }

            private class PullToRefreshGetPrivateData : AsyncTask<Java.Lang.Void, Java.Lang.Void, List<GetAllEventList>>
            {
                private FragmentActivity context;
                private HomeAdapterPrivateEvent privateEventAdapter;
                private RecyclerView rvPrivateEvent;
                private List<GetAllEventList> listPublicEvent;

                public PullToRefreshGetPrivateData(FragmentActivity context, HomeAdapterPrivateEvent privateEventAdapter, RecyclerView rvPrivateEvent)
                {
                    this.context = context;
                    this.privateEventAdapter = privateEventAdapter;
                    this.rvPrivateEvent = rvPrivateEvent;
                }

                protected override List<GetAllEventList> RunInBackground(params Java.Lang.Void[] @params)
                {
                    listPublicEvent = RestSharpCall.GetList<GetAllEventList>("EventRequest/GetMyEventList?id=" + GlobalClass.UserID);
                    return listPublicEvent;
                }

                protected override void OnPostExecute(Java.Lang.Object result)
                {
                    base.OnPostExecute(result);
                    var privateEventList = (from x in listPublicEvent where x.EventType == "M" select x).ToList();
                    privateEventAdapter = new HomeAdapterPrivateEvent(privateEventList, context);
                    rvPrivateEvent.SetAdapter(privateEventAdapter);
                    rvPrivateEvent.Invalidate();
                }
            }
        }
    }
}