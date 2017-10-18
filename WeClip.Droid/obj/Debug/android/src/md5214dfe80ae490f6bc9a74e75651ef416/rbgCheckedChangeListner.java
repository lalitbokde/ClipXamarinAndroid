package md5214dfe80ae490f6bc9a74e75651ef416;


public class rbgCheckedChangeListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.RadioGroup.OnCheckedChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCheckedChanged:(Landroid/widget/RadioGroup;I)V:GetOnCheckedChanged_Landroid_widget_RadioGroup_IHandler:Android.Widget.RadioGroup/IOnCheckedChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.rbgCheckedChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", rbgCheckedChangeListner.class, __md_methods);
	}


	public rbgCheckedChangeListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == rbgCheckedChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.rbgCheckedChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public rbgCheckedChangeListner (md5214dfe80ae490f6bc9a74e75651ef416.CreateEvent p0, android.widget.RadioButton p1, android.widget.RadioButton p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == rbgCheckedChangeListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.rbgCheckedChangeListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "WeClip.Droid.CreateEvent, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Widget.RadioButton, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Widget.RadioButton, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onCheckedChanged (android.widget.RadioGroup p0, int p1)
	{
		n_onCheckedChanged (p0, p1);
	}

	private native void n_onCheckedChanged (android.widget.RadioGroup p0, int p1);

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
