package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetProfileDetails_onClickListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetProfileDetails+onClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetProfileDetails_onClickListner.class, __md_methods);
	}


	public GetProfileDetails_onClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetProfileDetails_onClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetProfileDetails+onClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
