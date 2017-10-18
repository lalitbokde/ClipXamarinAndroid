package md5214dfe80ae490f6bc9a74e75651ef416;


public class HomeFragment_toolbarClickListner
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
		mono.android.Runtime.register ("WeClip.Droid.HomeFragment+toolbarClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", HomeFragment_toolbarClickListner.class, __md_methods);
	}


	public HomeFragment_toolbarClickListner () throws java.lang.Throwable
	{
		super ();
		if (getClass () == HomeFragment_toolbarClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.HomeFragment+toolbarClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public HomeFragment_toolbarClickListner (android.support.v4.app.FragmentActivity p0, android.widget.TextView p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == HomeFragment_toolbarClickListner.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.HomeFragment+toolbarClickListner, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentActivity, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
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
