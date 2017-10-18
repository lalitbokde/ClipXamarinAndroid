package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetPublicPrivteEventData_onRefereshPrivateLayout
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.v4.widget.SwipeRefreshLayout.OnRefreshListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRefresh:()V:GetOnRefreshHandler:Android.Support.V4.Widget.SwipeRefreshLayout/IOnRefreshListenerInvoker, Xamarin.Android.Support.v4\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPrivateLayout, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetPublicPrivteEventData_onRefereshPrivateLayout.class, __md_methods);
	}


	public GetPublicPrivteEventData_onRefereshPrivateLayout () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData_onRefereshPrivateLayout.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPrivateLayout, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetPublicPrivteEventData_onRefereshPrivateLayout (md5bff77e4c75941941410bfd921e0274b7.HomeAdapterPrivateEvent p0, android.support.v4.app.FragmentActivity p1, android.support.v4.widget.SwipeRefreshLayout p2, android.support.v7.widget.RecyclerView p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData_onRefereshPrivateLayout.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPrivateLayout, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.Adapters.HomeAdapterPrivateEvent, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.Widget.SwipeRefreshLayout, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V7.Widget.RecyclerView, Xamarin.Android.Support.v7.RecyclerView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onRefresh ()
	{
		n_onRefresh ();
	}

	private native void n_onRefresh ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
