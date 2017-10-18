package md5ada6d14708384ac194855915528243c6;


public class CropImageActivity
	extends md5ada6d14708384ac194855915528243c6.MonitoredActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.ImageResizer.CropImageActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CropImageActivity.class, __md_methods);
	}


	public CropImageActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CropImageActivity.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.ImageResizer.CropImageActivity, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();

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
