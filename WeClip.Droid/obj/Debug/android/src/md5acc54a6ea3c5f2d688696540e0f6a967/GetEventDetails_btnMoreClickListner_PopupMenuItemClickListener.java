package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.PopupMenu.OnMenuItemClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMenuItemClick:(Landroid/view/MenuItem;)Z:GetOnMenuItemClick_Landroid_view_MenuItem_Handler:Android.Widget.PopupMenu/IOnMenuItemClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+PopupMenuItemClickListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener.class, __md_methods);
	}


	public GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+PopupMenuItemClickListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener (android.support.v4.app.FragmentActivity p0, long p1, long p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetEventDetails_btnMoreClickListner_PopupMenuItemClickListener.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+PopupMenuItemClickListener, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onMenuItemClick (android.view.MenuItem p0)
	{
		return n_onMenuItemClick (p0);
	}

	private native boolean n_onMenuItemClick (android.view.MenuItem p0);

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
