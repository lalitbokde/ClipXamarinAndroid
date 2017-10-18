package md5ada6d14708384ac194855915528243c6;


public class MonitoredActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_onStop:()V:GetOnStopHandler\n" +
			"n_onStart:()V:GetOnStartHandler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.ImageResizer.MonitoredActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", MonitoredActivity.class, __md_methods);
	}


	public MonitoredActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MonitoredActivity.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.ImageResizer.MonitoredActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


	public void onStop ()
	{
		n_onStop ();
	}

	private native void n_onStop ();


	public void onStart ()
	{
		n_onStart ();
	}

	private native void n_onStart ();

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
