package md536df3414b75e412bc7988de6d7a26a07;


public class CameraPictureCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.PictureCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPictureTaken:([BLandroid/hardware/Camera;)V:GetOnPictureTaken_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPictureCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Helper.CameraPictureCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CameraPictureCallback.class, __md_methods);
	}


	public CameraPictureCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CameraPictureCallback.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.CameraPictureCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CameraPictureCallback (java.lang.String p0, int p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CameraPictureCallback.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Helper.CameraPictureCallback, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public void onPictureTaken (byte[] p0, android.hardware.Camera p1)
	{
		n_onPictureTaken (p0, p1);
	}

	private native void n_onPictureTaken (byte[] p0, android.hardware.Camera p1);

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
