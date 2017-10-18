package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPublicLayout+PullToRefreshGetData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData.class, __md_methods);
	}


	public GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPublicLayout+PullToRefreshGetData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData (android.support.v4.app.FragmentActivity p0, md5bff77e4c75941941410bfd921e0274b7.HomeAdapterPublicEvent p1, android.support.v7.widget.RecyclerView p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData_onRefereshPublicLayout_PullToRefreshGetData.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData+onRefereshPublicLayout+PullToRefreshGetData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:WeClip.Droid.Adapters.HomeAdapterPublicEvent, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V7.Widget.RecyclerView, Xamarin.Android.Support.v7.RecyclerView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public java.lang.Object doInBackground (java.lang.Object[] p0)
	{
		return n_doInBackground (p0);
	}

	private native java.lang.Object n_doInBackground (java.lang.Object[] p0);


	public void onPostExecute (java.lang.Object p0)
	{
		n_onPostExecute (p0);
	}

	private native void n_onPostExecute (java.lang.Object p0);

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
