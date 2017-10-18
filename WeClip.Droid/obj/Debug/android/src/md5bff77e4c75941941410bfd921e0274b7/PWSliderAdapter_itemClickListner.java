package md5bff77e4c75941941410bfd921e0274b7;


public class PWSliderAdapter_itemClickListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.Adapters.PWSliderAdapter+itemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PWSliderAdapter_itemClickListner.class, __md_methods);
	}


	public PWSliderAdapter_itemClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PWSliderAdapter_itemClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.PWSliderAdapter+itemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
