package md5214dfe80ae490f6bc9a74e75651ef416;


public class SplashActivity_crManagerListner
	extends net.hockeyapp.android.CrashManagerListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldAutoUploadCrashes:()Z:GetShouldAutoUploadCrashesHandler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.SplashActivity+crManagerListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", SplashActivity_crManagerListner.class, __md_methods);
	}


	public SplashActivity_crManagerListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SplashActivity_crManagerListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.SplashActivity+crManagerListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean shouldAutoUploadCrashes ()
	{
		return n_shouldAutoUploadCrashes ();
	}

	private native boolean n_shouldAutoUploadCrashes ();

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
