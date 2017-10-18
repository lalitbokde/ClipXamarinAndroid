package md5214dfe80ae490f6bc9a74e75651ef416;


public class PWSliderActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.PWSliderActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PWSliderActivity.class, __md_methods);
	}


	public PWSliderActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PWSliderActivity.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.PWSliderActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
