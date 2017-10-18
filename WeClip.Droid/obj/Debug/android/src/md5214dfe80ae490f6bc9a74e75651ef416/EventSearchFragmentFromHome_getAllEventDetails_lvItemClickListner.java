package md5214dfe80ae490f6bc9a74e75651ef416;


public class EventSearchFragmentFromHome_getAllEventDetails_lvItemClickListner
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.AdapterView.OnItemClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onItemClick:(Landroid/widget/AdapterView;Landroid/view/View;IJ)V:GetOnItemClick_Landroid_widget_AdapterView_Landroid_view_View_IJHandler:Android.Widget.AdapterView/IOnItemClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.EventSearchFragmentFromHome+getAllEventDetails+lvItemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", EventSearchFragmentFromHome_getAllEventDetails_lvItemClickListner.class, __md_methods);
	}


	public EventSearchFragmentFromHome_getAllEventDetails_lvItemClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EventSearchFragmentFromHome_getAllEventDetails_lvItemClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.EventSearchFragmentFromHome+getAllEventDetails+lvItemClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onItemClick (android.widget.AdapterView p0, android.view.View p1, int p2, long p3)
	{
		n_onItemClick (p0, p1, p2, p3);
	}

	private native void n_onItemClick (android.widget.AdapterView p0, android.view.View p1, int p2, long p3);

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
