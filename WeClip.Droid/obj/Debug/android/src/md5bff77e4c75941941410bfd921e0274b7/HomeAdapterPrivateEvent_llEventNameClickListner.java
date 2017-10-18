package md5bff77e4c75941941410bfd921e0274b7;


public class HomeAdapterPrivateEvent_llEventNameClickListner
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
		mono.android.Runtime.register ("WeClip.Droid.Adapters.HomeAdapterPrivateEvent+llEventNameClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", HomeAdapterPrivateEvent_llEventNameClickListner.class, __md_methods);
	}


	public HomeAdapterPrivateEvent_llEventNameClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == HomeAdapterPrivateEvent_llEventNameClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.HomeAdapterPrivateEvent+llEventNameClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public HomeAdapterPrivateEvent_llEventNameClickListner (android.widget.LinearLayout p0, android.support.v4.app.FragmentActivity p1, long p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == HomeAdapterPrivateEvent_llEventNameClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.Adapters.HomeAdapterPrivateEvent+llEventNameClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.LinearLayout, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
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
