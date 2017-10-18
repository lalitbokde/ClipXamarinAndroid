package md5eb33679ec97ca87920c448e7861bf11e;


public class GcmListenerServiceClass
	extends com.google.android.gms.gcm.GcmListenerService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMessageReceived:(Ljava/lang/String;Landroid/os/Bundle;)V:GetOnMessageReceived_Ljava_lang_String_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.GcmMain.GcmListenerServiceClass, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", GcmListenerServiceClass.class, __md_methods);
	}


	public GcmListenerServiceClass () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GcmListenerServiceClass.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.GcmMain.GcmListenerServiceClass, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onMessageReceived (java.lang.String p0, android.os.Bundle p1)
	{
		n_onMessageReceived (p0, p1);
	}

	private native void n_onMessageReceived (java.lang.String p0, android.os.Bundle p1);

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
