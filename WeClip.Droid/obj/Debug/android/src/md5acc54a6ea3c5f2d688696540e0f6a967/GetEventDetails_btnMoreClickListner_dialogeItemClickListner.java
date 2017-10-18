package md5acc54a6ea3c5f2d688696540e0f6a967;


public class GetEventDetails_btnMoreClickListner_dialogeItemClickListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.content.DialogInterface.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/content/DialogInterface;I)V:GetOnClick_Landroid_content_DialogInterface_IHandler:Android.Content.IDialogInterfaceOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+dialogeItemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GetEventDetails_btnMoreClickListner_dialogeItemClickListner.class, __md_methods);
	}


	public GetEventDetails_btnMoreClickListner_dialogeItemClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetEventDetails_btnMoreClickListner_dialogeItemClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+dialogeItemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GetEventDetails_btnMoreClickListner_dialogeItemClickListner (android.support.v4.app.FragmentActivity p0, md5214dfe80ae490f6bc9a74e75651ef416.EventFragment p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GetEventDetails_btnMoreClickListner_dialogeItemClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.AsyncTask.GetEventDetails+btnMoreClickListner+dialogeItemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:WeClip.Droid.EventFragment, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0, p1 });
	}


	public void onClick (android.content.DialogInterface p0, int p1)
	{
		n_onClick (p0, p1);
	}

	private native void n_onClick (android.content.DialogInterface p0, int p1);

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
