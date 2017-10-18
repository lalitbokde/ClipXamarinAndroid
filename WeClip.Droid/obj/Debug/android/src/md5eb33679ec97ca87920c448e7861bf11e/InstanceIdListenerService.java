package md5eb33679ec97ca87920c448e7861bf11e;


public class InstanceIdListenerService
	extends com.google.android.gms.iid.InstanceIDListenerService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.GcmMain.InstanceIdListenerService, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", InstanceIdListenerService.class, __md_methods);
	}


	public InstanceIdListenerService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == InstanceIdListenerService.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.GcmMain.InstanceIdListenerService, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
