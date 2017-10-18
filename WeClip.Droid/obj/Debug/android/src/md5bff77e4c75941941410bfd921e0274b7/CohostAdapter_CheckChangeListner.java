package md5bff77e4c75941941410bfd921e0274b7;


public class CohostAdapter_CheckChangeListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.CompoundButton.OnCheckedChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCheckedChanged:(Landroid/widget/CompoundButton;Z)V:GetOnCheckedChanged_Landroid_widget_CompoundButton_ZHandler:Android.Widget.CompoundButton/IOnCheckedChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Adapters.CohostAdapter+CheckChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", CohostAdapter_CheckChangeListner.class, __md_methods);
	}


	public CohostAdapter_CheckChangeListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CohostAdapter_CheckChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.CohostAdapter+CheckChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCheckedChanged (android.widget.CompoundButton p0, boolean p1)
	{
		n_onCheckedChanged (p0, p1);
	}

	private native void n_onCheckedChanged (android.widget.CompoundButton p0, boolean p1);

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
