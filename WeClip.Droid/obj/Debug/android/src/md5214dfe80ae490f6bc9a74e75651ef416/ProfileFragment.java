package md5214dfe80ae490f6bc9a74e75651ef416;


public class ProfileFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer,
		android.support.design.widget.TabLayout.OnTabSelectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_onStart:()V:GetOnStartHandler\n" +
			"n_onTabReselected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabReselected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"n_onTabSelected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabSelected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"n_onTabUnselected:(Landroid/support/design/widget/TabLayout$Tab;)V:GetOnTabUnselected_Landroid_support_design_widget_TabLayout_Tab_Handler:Android.Support.Design.Widget.TabLayout/IOnTabSelectedListenerInvoker, Xamarin.Android.Support.Design\n" +
			"";
		mono.android.Runtime.register ("WeClip.Droid.ProfileFragment, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", ProfileFragment.class, __md_methods);
	}


	public ProfileFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ProfileFragment.class)
			mono.android.TypeManager.Activate ("WeClip.Droid.ProfileFragment, WeClip.Droid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void onStart ()
	{
		n_onStart ();
	}

	private native void n_onStart ();


	public void onTabReselected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabReselected (p0);
	}

	private native void n_onTabReselected (android.support.design.widget.TabLayout.Tab p0);


	public void onTabSelected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabSelected (p0);
	}

	private native void n_onTabSelected (android.support.design.widget.TabLayout.Tab p0);


	public void onTabUnselected (android.support.design.widget.TabLayout.Tab p0)
	{
		n_onTabUnselected (p0);
	}

	private native void n_onTabUnselected (android.support.design.widget.TabLayout.Tab p0);

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
