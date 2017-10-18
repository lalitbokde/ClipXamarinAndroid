package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetPublicPrivteEventData
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer,
		android.support.design.widget.TabLayout.OnTabSelectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"n_onTabReselected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabReselected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"n_onTabSelected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabSelected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"n_onTabUnselected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabUnselected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetPublicPrivteEventData.class, __md_methods);
	}


	public GetPublicPrivteEventData () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetPublicPrivteEventData (android.view.View p0, android.support.v7.widget.RecyclerView p1, android.support.v7.widget.RecyclerView p2, android.support.v4.app.FragmentActivity p3, android.support.design.widget.TabLayout p4, android.support.v4.widget.SwipeRefreshLayout p5, android.support.v4.widget.SwipeRefreshLayout p6) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetPublicPrivteEventData.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetPublicPrivteEventData, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Support.V7.Widget.RecyclerView, Xamarin.Android.Support.v7.RecyclerView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V7.Widget.RecyclerView, Xamarin.Android.Support.v7.RecyclerView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.Design.Widget.TabLayout, Xamarin.Android.Support.Design, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.Widget.SwipeRefreshLayout, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V4.Widget.SwipeRefreshLayout, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1, p2, p3, p4, p5, p6 });
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


	public void onTabReselected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabReselected (p0);
	}

	private native void n_onTabReselected (android.support.design.widget.TabLayout.Tab p0);


	public void onTabSelected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabSelected (p0);
	}

	private native void n_onTabSelected (android.support.design.widget.TabLayout.Tab p0);


	public void onTabUnselected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabUnselected (p0);
	}

	private native void n_onTabUnselected (android.support.design.widget.TabLayout.Tab p0);

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
